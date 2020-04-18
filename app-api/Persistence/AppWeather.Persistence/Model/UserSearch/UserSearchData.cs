﻿using AppWeather.Core.Domain.BaseModel;
using AppWeather.Core.Persistence;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppWeather.Persistence.Model
{
    [Table("UserSearch")]
    public class UserSearchData : IData
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string CityName { get; set; }
        public float? Temperature { get; set; }
        public int? Humidity { get; set; }
        public DateTime SearchTime { get; set; }
    }
}