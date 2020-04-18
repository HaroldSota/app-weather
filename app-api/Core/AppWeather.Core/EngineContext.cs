using AppWeather.Core.Infrastructure;
using System;
using System.Runtime.CompilerServices;

namespace AppWeather.Core
{
    /// <summary>
    ///     Provides access to the singleton instance of the "AppWeather" engine.
    /// </summary>
    public class EngineContext
    {
        #region [ Fields ]

        private static EngineContext _context;

        private readonly IEngine _installedEngine;

        #endregion

        #region [ Ctor]
        private EngineContext(IEngine installedEngine)
        {
            _installedEngine = installedEngine;
        }

        #endregion

        #region [ Properties ]

        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    Create();
                }

                return Singleton<IEngine>.Instance;
            }
        }

        #endregion

        #region [ Methods ]

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Create<T>()
               where T : IEngine
        {
            if (_context == null)
            {
                Singleton<IEngine>.Instance = Activator.CreateInstance<T>();
                _context = new EngineContext(Singleton<IEngine>.Instance);
            }

            return (T)_context._installedEngine;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Create()
        {
            return Singleton<IEngine>.Instance ?? (Singleton<IEngine>.Instance = new AppWeatherEngine());
        }

        #endregion
    }
}