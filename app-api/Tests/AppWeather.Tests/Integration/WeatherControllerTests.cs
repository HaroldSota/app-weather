using AppWeather.Api.Messaging.Model.Weather;
using AppWeather.Core.Messaging.Queries;
using AppWeather.IntegrationTest.Infrastructure;
using AppWeather.Persistence.Model;
using AppWeather.Tests.Infrastructure;
using FluentAssertions;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace AppWeather.Tests.Integration
{

    [CollectionDefinition("WeatherControllerTests", DisableParallelization = true)]
    public class WeatherControllerTests : ControllerTestBase
    {
        #region [ Ctor ]

        public WeatherControllerTests()
            : base(new TestServerFixture(), "api/weather/")
        {
            Seed();
        }

        #endregion

        #region [ Test forecast by city name ]

        [Fact]
        public async Task Get_Forecast_Provides_Locality_Weather_forecast()
        {
            //Act
            var response = await GetAsync("forecast?cityName=Berlin");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
           
            var result = response.GetObject<GetForecastByCityNameResponse>();
            
            result.Should().NotBeNull();
            result.Locality.CityName.Should().Equals("Berlin");
            result.Forecasts.Should().NotBeNull();
            result.Forecasts.Count.Should().BeInRange(5, 6, "Because it depends on the time of the testing, some times the openweather map returns 6 days forecast");
            GetLastSearch().CityName.Should().Be("Berlin");
        }

        [Fact]
        public async Task Get_Forecast_Provides_City_Name_Contain_Space()
        {
            //Act
            var response = await GetAsync("forecast?cityName=Frankfurt am Main");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = response.GetObject<GetForecastByCityNameResponse>();
            result.Locality.CityName.Should().Be("Frankfurt am Main");
            result.Forecasts.Should().NotBeNull();
            result.Forecasts.Count.Should().BeInRange(5, 6, "Because it depends on the time of the testing, some times the openweather map returns 6 days forecast");
            GetLastSearch().CityName.Should().Be("Frankfurt am Main");
        }

        [Fact]
        public async Task Get_Forecast_Provides_City_Name_All_German_Character()
        {
            //Act
            var response = await GetAsync("forecast?cityName=�������");

            //Assert
            var result = response.GetObject<QueryResponseError>();

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            result.Should().NotBeNull();
            result.Message.Should().Be("city not found");
            GetLastSearch().CityName.Should().NotBe("�������");
        }

        [Fact]
        public async Task Get_Forecast_Provides_City_Name_That_Exists_German_Character()
        {
            //Act
            var response = await GetAsync("forecast?cityName=M�nchen");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = response.GetObject<GetForecastByCityNameResponse>();
            result.Should().NotBeNull();
            result.Locality.CityName.Should().Be("Munich");
            result.Forecasts.Should().NotBeNull();
            result.Forecasts.Count.Should().BeInRange(5, 6, "Because it depends on the time of the testing, some times the openweather map returns 6 days forecast");
            GetLastSearch().CityName.Should().Be("Munich");
        }

        [Fact]
        public async Task Get_Forecast_Negative_City_Name_With_No_Alphabetic_Letter()
        {
            //Act
            var response = await GetAsync("forecast?cityName=@AK47");

            //Assert
            var result = response.GetObject<QueryResponseError>();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            result.Should().NotBeNull();
            result.Message.Should().Equals("The cityname can contain only alphabetic letters and space!");
            GetLastSearch().CityName.Should().NotBe("@AK47");
        }

        [Fact]
        public async Task Get_Forecast_Negative_With_Empty_Locality()
        {
            //Act
            var response = await GetAsync("forecast?cityName=");

            //Assert
            var result = response.GetObject<QueryResponseError>();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Should().NotBeNull();
            result.Message.Should().Be("Error: CityName is empty!");
            GetLastSearch().CityName.Should().NotBeNullOrEmpty();
              
        }

        [Fact]
        public async Task Get_Forecast_Negative_With_two_letters_Locality()
        {
            //Act
            var response = await GetAsync("forecast?cityName=Be");

            //Assert
            var result = response.GetObject<QueryResponseError>();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            result.Should().NotBeNull();
            result.Message.Should().Be("Error: CityName must be at least 3 alphabetic letters!");
            GetLastSearch().Should().NotBe("Be");
        }

        [Fact]
        public async Task Get_Forecast_Negative_City_Name_That_Does_Not_Exist()
        {
            //Act
            var response = await GetAsync("forecast?cityName=aaabc");
          

            var result = response.GetObject<QueryResponseError>();

            //Assert

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            result.Should().NotBeNull();
            result.Message.Should().Be("city not found");
            GetLastSearch().CityName.Should().NotBe("aaabc");
        }

        #endregion

        #region [ Test forecast by zip code ]

        [Fact]
        public async Task Get_Forecast_Provides_ZipCode_Weather_Forecast()
        {
            //Act
            var response = await GetAsync("forecast?zipCode=10115");

            //Assert
            var result = response.GetObject<GetForecastByCityNameResponse>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();

            result.Locality.CityName.Should().Be("Berlin");
            result.Forecasts.Should().NotBeNull();
            result.Forecasts.Count.Should().BeInRange(5, 6, "Because it depends on the time of the testing, some times the openweather map returns 6 days forecast");
            GetLastSearch().CityName.Should().Be("Berlin");
        }


        [Fact]
        public async Task Get_Forecast_Negative_Not_Valid_Zip_Code_Number()
        {
            //Act
            var zipCode = "00015";
            var response = await GetAsync($"forecast?zipCode={zipCode}");

            //Assert
            var result = response.GetObject<QueryResponseError>();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            result.Should().NotBeNull();
            result.Message.Should().Be($"{zipCode} is not a valid German zip code! Germany has 5 digits always from 01001 up to 99998.");

        }

        [Fact]
        public async Task Get_Forecast_Negative_Not_Valid_Zip_Code_Not_Number()
        {
            //Act
            var zipCode = "B0015";
            var response = await GetAsync($"forecast?zipCode={zipCode}");

            //Assert
            var result = response.GetObject<QueryResponseError>();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            result.Should().NotBeNull();
            result.Message.Should().Be($"{zipCode} is not a valid German zip code! Germany has 5 digits always from 01001 up to 99998.");
        }

        [Fact]
        public async Task Get_Forecast_Negative_Empty_Zip_Code()
        {
            //Act
            var response = await GetAsync($"forecast?zipCode=");

            //Assert
            var result = response.GetObject<QueryResponseError>();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            result.Should().NotBeNull();
            result.Message.Should().Be("Error:Zip Code is empty!");

        }

        #endregion

        #region [ Test forecast by GeoCode ]

        [Fact]
        public async Task Get_Forecast_Provides_Coordinate_Weather_Forecast()
        {
            //Act
            var response = await GetAsync("forecast?coord=52.516920,13.400171");

            //Assert
            response.EnsureSuccessStatusCode();
            var result = response.GetObject<GetForecastByCioordinateResponse>();

            result.Should().NotBeNull();
            result.Locality.CityName.Should().Be("Mitte");
            result.Forecasts.Should().NotBeNull();
            result.Forecasts.Count.Should().BeInRange(5, 6, "Because it depends on the time of the testing, some times the openweather map returns 6 days forecast");
            GetLastSearch().CityName.Should().Be("Mitte");
        }

        [Fact]
        public async Task Get_Forecast_Negative_Not_Valid_Coordinate_Number()
        {
            //Act
            var response = await GetAsync("forecast?coord=5250,1314");

            //Assert
            var result = response.GetObject<QueryResponseError>();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            result.Should().NotBeNull();
            result.Message.Should().Be("5250 is not a float");

        }

        [Fact]
        public async Task Get_Forecast_Negative_Not_Valid_Coordinate_Not_Number()
        {
            //Act
            var response = await GetAsync("forecast?coord=52.5O,13.14");

            //Assert
            var result = response.GetObject<QueryResponseError>();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            result.Should().NotBeNull();
            result.Message.Should().Be("The given GeoCode is not in the correct fomat e.g. lat,lon!");
        }

        [Fact]
        public async Task Get_Forecast_Negative_Empty_Coordinate()
        {
            //Act
            var response = await GetAsync("forecast?coord=");

            //Assert
            var result = response.GetObject<QueryResponseError>();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            result.Should().NotBeNull();
            result.Message.Should().Be("Error: GeoCode is null or empty!");
        }

        #endregion

        #region [ Utilities ]

        private UserSearchData GetLastSearch()=> 
            TestDbContext.DbContext.Set<UserSearchData>().OrderByDescending(item => item.SearchTime).FirstOrDefault();
        private void Seed()
        {
            var set = TestDbContext.DbContext.Set<UserSearchData>();

            //paralel testing

            set.RemoveRange(set.ToList());

            set.AddRange(new[] {
                    new UserSearchData() { Id = Guid.NewGuid(), UserId = "TestUserId", CityName = "Berlin", Temperature = 13, Humidity = 20, SearchTime = DateTime.Now.AddSeconds(-1) },
                    new UserSearchData() { Id = Guid.NewGuid(), UserId = "TestUserId", CityName = "Hamburg", Temperature = 15, Humidity = 65, SearchTime = DateTime.Now.AddSeconds(-2) },
                    new UserSearchData() { Id = Guid.NewGuid(), UserId = "TestUserId", CityName = "M�nchen", Temperature = 21, Humidity = 71, SearchTime = DateTime.Now.AddSeconds(-3) },
                    new UserSearchData() { Id = Guid.NewGuid(), UserId = "TestUserId", CityName = "K�ln", Temperature = 7, Humidity = 33, SearchTime = DateTime.Now.AddSeconds(-4) },
                    new UserSearchData() { Id = Guid.NewGuid(), UserId = "TestUserId", CityName = "Stuttgart", Temperature = -9, Humidity = 22, SearchTime = DateTime.Now.AddSeconds(-5) },
                    new UserSearchData() { Id = Guid.NewGuid(), UserId = "TestUserId", CityName = "D�sseldorf", Temperature = 8, Humidity = 85, SearchTime = DateTime.Now.AddSeconds(-6) },
            });

            TestDbContext.DbContext.SaveChanges();
        }
        #endregion
    }
}