using System;
using System.Collections.Generic;
using System.Reflection;

namespace AppWeather.Core.Infrastructure.TypeFinder
{
    /// <inheritdoc />
    public class AppWeatherTypeFinder: AppDomainTypeFinder
    {
        #region [ Fields ]

        private bool _binFolderAssembliesLoaded;

        #endregion

        #region [ Ctor ]

        public AppWeatherTypeFinder(IAppWeatherFileProvider fileProvider = null) : base(fileProvider)
        {
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        ///     Get assemblies
        /// </summary>
        /// <returns>Result</returns>
        public override IList<Assembly> GetAssemblies()
        {
            if (_binFolderAssembliesLoaded)
                return base.GetAssemblies();

            _binFolderAssembliesLoaded = true;
            var binPath = AppContext.BaseDirectory;
            LoadMatchingAssemblies(binPath);

            return base.GetAssemblies();
        }

        #endregion 
    }
}