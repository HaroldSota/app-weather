using AppWeather.Tests.Infrastructure;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Xunit;

namespace AppWeather.IntegrationTest.Infrastructure
{
    public abstract class ControllerTestBase : IClassFixture<TestServerFixture>
    {
        #region [ Fields ]
        protected readonly TestServerFixture _fixture;
        private string _controllPath;
         
        #endregion
        public ControllerTestBase(TestServerFixture fixture, string controllPath)
        {
            _fixture = fixture;
            _controllPath = controllPath;
        }

        public string GetQueryString(object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return string.Join("&", properties.ToArray());
        }

        protected async Task<HttpResponseMessage> GetAsync(string action)
        {
            return await _fixture.Client.GetAsync($"{_controllPath}{action}");
        }

        protected async Task<HttpResponseMessage> GetAsync(string action, object obj)
        {
            return await _fixture.Client.GetAsync($"{_controllPath}{action}?{GetQueryString(obj)}");
        }
    }
}