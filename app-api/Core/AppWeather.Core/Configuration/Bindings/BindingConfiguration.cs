using Microsoft.Extensions.Configuration;
using System.Linq;

namespace AppWeather.Core.Configuration.Bindings
{
    public abstract class BindingConfiguration : BaseConfigurationProvider, IBindingConfiguration
    {
        public BindingConfiguration(IConfiguration configuration, string prefix)
            : base(configuration, prefix)
        {
        }
        #region [ Properties ]

        private string _name = null;
        public string Name => _name??= GetConfiguration<string>("Name");

        private string _endPoint = null;
        public string Endpoint => _endPoint ??= GetConfiguration<string>("Endpoint");

        private string _apiKeyName = null;
        public string ApiKeyName => _apiKeyName ??= GetConfiguration<string>("ApiKeyName");

        private string _apiKeyValue = null;
        public string ApiKeyValue => _apiKeyValue ??= GetConfiguration<string>("ApiKeyValue");

        private IBindingResourceConfiguration[] _resources;
        public IBindingResourceConfiguration[] Resources
        {
            get
            {
                if (_resources == null)
                    _resources = GetArrayConfiguration("Resources", (config, prefix) => new BindingResourceConfiguration(config, prefix));

                return _resources;
            }
        }

        #endregion


    }
}
