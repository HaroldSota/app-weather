using AppWeather.Core.Domain.BaseModel;
using AppWeather.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppWeather.Core.Domain.UserSearchModel
{
    public interface IUserSearchRepository : IRepository<UserSearch>
    {
        void Add(UserSearch folder);
    }
}
