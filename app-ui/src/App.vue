<template>
  <div id="app">
    <div class="row">
      <searchbar  v-on:selected="showWather" :serverUrl="serverUrl"/>
    </div>
    <div class="row">
     <weathermap :apiKey="'AIzaSyDl7u3Zsj3y1Xr9snQzBW2etDUslw73yMU'" 
                 :mapConfig="{ center: {lat: 39.7, lng: -96.2}, zoom: 4}"
                 :location="location"
                 :serverUrl="serverUrl"/>
      </div>
  </div>
</template>

<script>
import searchbar from './components/search-box'
import weathermap from './components/weather-map'

export default {
  name: 'App',
  components: {
    searchbar,
    weathermap
   },
   data(){
     return {
       location : {},
       serverUrl :'https://localhost:5001/api/'
     }
   },
   methods:{
     showWather(l)
     {
      this.location = l;
     }
   }
}
</script>

<style>
body
{
  background-color: #ebebeb;
}
#app{
  width:100%;
  overflow: hidden;
}

.row-first {
  height:30%;
  overflow: hidden;
}
</style>
