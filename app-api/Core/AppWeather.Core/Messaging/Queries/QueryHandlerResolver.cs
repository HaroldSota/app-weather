using AppWeather.Core;
using AppWeather.Core.Infrastructure.TypeFinder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppWeather.Core.Messaging.Queries
{
    internal sealed class QueryHandlerResolver : IHandlerResolver
    {
        private readonly Dictionary<Type, Type> _handlers;

        internal QueryHandlerResolver()
        {
            var typeFinder = EngineContext.Current.Resolve<ITypeFinder>();
            var handlers = typeFinder.FindClassesOfType(typeof(IQueryHandler<,>));

            _handlers = handlers.ToDictionary(
                type => type.GetInterfaces()
                    .First(i => i.IsGenericType)
                    .GetGenericArguments()[0]
            );
        }

        public Type Get(Type type)
        {
            return _handlers[type];
        }
    }
}
