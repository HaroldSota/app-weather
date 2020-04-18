﻿using AppWeather.Core.Domain.BaseModel;
using System;

namespace AppWeather.Core.Domain.UserSearchModel
{
    public sealed class UserSearch: BaseEntity
    {
        #region [ Ctor ]
        public UserSearch(Guid id)
             : base(id)
        {
        }

        public UserSearch(string userId, string cityName, float? temperature, int? humidity, DateTime searchTime)
            : this(Guid.NewGuid(), userId, cityName,  temperature,  humidity, searchTime)
        {
        }

        public UserSearch(Guid id, string userId, string cityName, float? temperature, int? humidity, DateTime searchTime)
            : base(id)
        {
            UserId = userId;
            CityName = cityName;
            Temperature = temperature;
            Humidity = humidity;
            SearchTime = searchTime;
        }

        #endregion

        #region [ Properties ]
        public string UserId { get; private set; }

        public string CityName { get; private set; }

        public float? Temperature { get; private set; }

        public int? Humidity { get; private set; }
        public DateTime SearchTime { get; private set; }

        #endregion
    }
}