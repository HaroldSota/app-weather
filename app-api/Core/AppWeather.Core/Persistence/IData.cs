using AppWeather.Core.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppWeather.Core.Persistence
{
    public interface IData :IEntity
    {
        Guid Id { get; }
    }
}