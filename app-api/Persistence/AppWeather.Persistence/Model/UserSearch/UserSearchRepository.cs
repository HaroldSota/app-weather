using AppWeather.Core.Domain.UserSearchModel;
using AppWeather.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppWeather.Persistence.Model
{
    public sealed class UserSearchRepository : IUserSearchRepository
    {
        #region [ Fields ]

        private readonly IBaseRepository<UserSearch, UserSearchData> _baseRepository;

        #endregion

        #region [ Ctor ]

        public UserSearchRepository(IBaseRepository<UserSearch, UserSearchData> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        #endregion

        #region [ CRUD Methods ]
        public void Add(UserSearch userSearch)
        {
            _baseRepository.Add(userSearch);
        }

        #endregion
    }
}
