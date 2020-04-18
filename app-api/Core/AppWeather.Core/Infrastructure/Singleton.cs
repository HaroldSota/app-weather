using System;
using System.Collections.Generic;

namespace AppWeather.Core.Infrastructure
{
    /// <summary>
    ///    Generic singleton dictionary
    /// </summary>
    /// <typeparam name="T">The type of object to store</typeparam>
    public class Singleton<T> 
    {
        #region [ Fields ]

        private static T instance;

        private static IDictionary<Type, object> AllSingletons { get; }

        #endregion
     
        #region [ Ctor ]

        static Singleton()
        {
            AllSingletons = new Dictionary<Type, object>();
        }

        #endregion

        #region [ Properties ]
        
        public static T Instance
        {
            get => instance;
            set
            {
                instance = value;
                AllSingletons[typeof(T)] = value;
            }
        }

        #endregion
    }
}