<template>
    <div class="map-container">
        <div class="warning" ref="warningBox">
            <span class="closebtn" onclick="this.parentElement.style.display='none';">x</span>
            <strong>Warning! </strong>
            <span ref="warningMessage"></span>
        </div>
        <div class="map-title">
            {{currentCity}} Weather Radar
        </div>
        
        <div class="map" v-bind:style="forecastList.length == 0? 'width:100%': ''">
            <div id="google-map" style="min-height:300px;"></div>
        </div>
        <div class="forecast" v-bind:style="forecastList.length == 0? 'display:none': ''">
            <div v-for="item in forecastList" :key="item.Date" 
                  class="forecast-list-item">
                <div class="forecast-date">
                    <span class="day">{{item.Day.substring(0, 3)}}</span>
                    <span class="date">{{item.Date}}</span>
                </div>
                <div class="forecast-temps">
                    <span class="high">{{Math.trunc(item.MaxTemp -273.15)}}°</span>
                    <span class="low">/ {{Math.trunc(item.MinTemp -273.15)}}°</span>
                </div>
                <div class="forecast-wind">
                    <span class="forecast-wind-item">Wind</span>
                    <span class="forecast-wind-item"> {{item.AvgWindSpeed}}</span>
                </div>
                <div class="forecast-humidity">
                    <span class="forecast-humidity-item">Precip</span>
                    <span class="forecast-humidity-item"> {{item.AvgHumidity}}%</span>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
import GoogleMapsApiLoader from 'google-maps-api-loader'
import axios from 'axios';

export default {
  name : 'weathermap',
  props: {
    mapConfig: Object,
    apiKey: String,
    location: Object,
    serverUrl : String
  },

  watch :  {
    'location' : function(){
      this.onLocationChange();
    }
  },

  data() {
    return {
      google: null,
      map: null,
      currentCity : '',
      forecastList : [],
      currentMarker : null
    }
  },

  async mounted() {
    this.google = await GoogleMapsApiLoader({ apiKey: this.apiKey });
    this.initializeMap()
  },

  methods: {
    initializeMap() {
      this.map = new this.google.maps.Map(document.getElementById('google-map'), {
        center: {lat: 52.52, lng: 13.14}, zoom: 7
      });
     var weatherLayer = new this.google.maps.weather.WeatherLayer({
                temperatureUnits: this.google.maps.weather.TemperatureUnit.FAHRENHEIT
              });
     weatherLayer.setMap(this.map);
    },

    onLocationChange()
    {
  
        var querystring = this.location.lat 
        ?  'coord=' + this.location.lat + ','+ this.location.lon
        :   (isNaN(this.location.query) 
              ?'cityName=' 
              : 'zipCode=') + this.location.query;

        axios.get(this.serverUrl +'weather/forecast?'+ querystring, {withCredentials: true})
          .then(response => {
            if(response.data)
            {
              var data =response.data;
              var geoLocation = new this.google.maps.LatLng(data.Locality.Lat, data.Locality.Lon);
              this.map.setCenter(geoLocation);

              this.forecastList = data.Forecasts;
              this.currentCity = data.Locality.CityName;


             if(this.currentMarker) this.currentMarker.setMap(null);
              
              var infowindow = new this.google.maps.InfoWindow({
                    content: '<div class="container" style="width:110px;><div class="row">' +
    '<div class="col-sm">Min: ' + Math.trunc(this.forecastList[0].MinTemp -273.15) +'°</div>' +
    '<div class="col-sm">Max: ' + Math.trunc(this.forecastList[0].MaxTemp-273.15) +'°</div>' +
  '</div></div>',
       position: geoLocation});
              this.currentMarker= new this.google.maps.Marker({
                  position: geoLocation,
                  title:"Forecast",
                  map : this.map,
                  icon : 'http://maps.google.com/mapfiles/ms/icons/blue-dot.png',
                  animation: this.google.maps.Animation.DROP
              });

              this.currentMarker.addListener('click', function() { infowindow.open(this.map, this.marker); });

            }else{
              this.$refs.warningBox.style.display='block';
              this.$refs.warningMessage.innerHTML = response.data.Message;
            }
          })
          .catch(e => {
            console.log(e);
          });
    }
  }
}
</script>

<style scoped>
    .map-container {
        width: 100%;
        min-height: 350px;
        margin: 40px;
        border-radius: 3px;
        background-color: white;
        padding: 0px 30px 30px 30px;
        box-sizing: border-box;
    }

    .map-title {
        padding: 12px 0;
        font-size: 18px;
        font-weight: 500;
        text-transform: uppercase;
    }

    .map {
        float: left;
        width: 65%;
        max-height: 400px !important;
        min-width: 200px;
        position: relative;
        margin-right: 20px;
    }

    .forecast {
        float: left;
        width: 30%;
        vertical-align: baseline;
        min-width: 200px;
        background-color: #ebebeb;
        padding-bottom: 1px;
    }

    .forecast-list-item {
        width: 100%;
        margin-bottom: 2px;
        padding: 0 5px 5px 0px;
        display: flex;
        background-color: white;
        position: relative;
        text-align: center;
    }

    .forecast-date, .forecast-temps {
        margin-right: 5px;
        font-size: 14px;
        padding-right: 0px;
        text-align: center;
        width: 65px;
    }

    .day,.forecast-humidity-item, .forecast-wind-item {
        color: #1f1f1f;
        display: block;
        font-size: 12px;
        text-align: center;
    }

    .forecast-humidity
    {
      position: absolute;
      right : 0px;
      width: 50px;
    }
    .date {
        color: #878787;
        display: block;
        font-size: 12px;
        text-align: center;
    }

    .high {
        font-size: 20px;
        display: inline-block;
    }

    .low {
        color: #878787;
        font-size: 12px;
        display: inline-block;
    }

    .warning {
        margin: 10px auto auto;
        padding: 20px;
        width: 80%;
        min-width: 200px;
        background-color: #ff9800;
        color: white;
        display: none;
    }

    .closebtn {
        margin-left: 15px;
        color: white;
        font-weight: bold;
        float: right;
        font-size: 22px;
        line-height: 20px;
        cursor: pointer;
        transition: 0.3s;
    }

        .closebtn:hover {
            color: black;
        }

         .currentWeather {
    text-align: center;
    height: auto;
    margin-top: 10rem;
    background-image: -webkit-linear-gradient(45deg, #424242, #141414);
    border-radius: 10px;
    color: #EEE;
    opacity: 0.75;
    box-shadow: 0px 0px 100px 1px #EEE;
}
    @media only screen and (max-width:1000px) {
       .map {
           width: 60%;
        }

        .forecast {
            width: 35%;
        }
    }
    @media only screen and (max-width:800px) {
        .forecast, .map, #google-map {
            width: 100%;
        }

        .forecast {
            margin-top: 20px;
        }
    }
</style>