using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AppWeather.Core.Infrastructure.TypeFinder
{
    /// <summary>
    /// Classes implementing this interface provide information about types
    /// to various services in the AppWeather engine.
    /// </summary>
    public interface ITypeFinder
    {
        IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true);

        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true);

        IList<Assembly> GetAssemblies();
    }
}