using AppWeather.Core.Domain.BaseModel;

namespace AppWeather.Core.Persistence
{
    public interface IRepository<T> where T : BaseEntity
    {
    }
}
