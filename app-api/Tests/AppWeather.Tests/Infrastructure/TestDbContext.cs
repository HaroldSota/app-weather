using AppWeather.Core;
using AppWeather.Persistence;

namespace AppWeather.Tests.Infrastructure
{
    public static class TestDbContext
    {

        public static IDbContext DbContext { get; private set; }

        static TestDbContext()
        {
            DbContext = EngineContext.Current.Resolve<IDbContext>();
        }


    }
}
