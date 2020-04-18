<template>
    <div class="search-content">
        <div class="search-content-inner" ref="search-content-inner">
            <div class="dropdown center-box query-box">
                <span class="query-icon fa fa-search" aria-hidden="true"></span>
                <input placeholder="Search location, zip..." class="query-input" 
                       ref="query-input"
                       v-model="queryText"
                       v-on:keydown.enter.stop='enter'
                       v-on:keydown.down.stop='down'
                       v-on:keydown.up.stop='up'
                       v-on:input='change' />
                <ul v-bind:class="{'dropdown-menu ': true, 'show': queryText.length >0}" style="width:100%;">
                    <li class="dropdown-item" v-on:click="suggestionClick(0)">
                        <span class="item-icon fa fa-location-arrow" /> Use your current location
                    </li>
                    <li v-for="(suggestion, index) in suggestions" :key="suggestion"
                        class="dropdown-item"
                        v-on:click="suggestionClick(index+1)">
                        <span class="item-icon fa fa-location-arrow" /> {{ suggestion }}
                    </li>
                </ul>
            </div>
            <div class="search-history center-box"  v-bind:style="searchHistory.length == 0? 'display:none': ''">
                <div v-for="(item, index) in searchHistory" :key="index"  class="history-item" href="#">
                    <div class="history-item-part item-left">{{item.CityName}}</div>
                    <div class="item-right">
                         <img class='history-item-part-img'  src="@/assets/images/thermometer.png"/>
                         <div class="history-item-part-txt">{{Math.trunc(item.Temperature -273.15)}}° </div>/
                        <img class='history-item-part-img'  src="@/assets/images/humidity.png"/>
                       {{item.Humidity}}%
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import axios from 'axios';

    export default {
        name: 'searchbar',
        props :{
            serverUrl : String
        },
        data() {
            return {
                queryText: '',
                suggestions: [],
                suggestionList: null,
                currenIndex: 0,
                searchHistory : []
            }
        },
        mounted() {
            this.suggestionList =this.$refs["query-input"].nextSibling;
            this.refreshHistory();
        },

        methods: {
            change: function () {
                this.showSugestion();
                this.currenIndex = -1;
                if (this.queryText.length >= 3) {
                
                axios.get(this.serverUrl +'places/Get?cityname=' + this.queryText, {withCredentials: true})
                  .then(response => {
                      console.log('Suggestions retlengthurned: '+response.data.Suggestions.length);
                      this.suggestions = response.data.Suggestions;
                  })
                  .catch(e => {
                    console.log(e);
                    this.suggestions = [];
                  });
                }
            },

            suggestionClick(index) {
                this.currenIndex = index;
                if (index > 0)
                    this.queryText = this.suggestions[this.currenIndex - 1];
                this.changeSugestionIndex();
                this.selected();
            },

            enter: function () {
                if (this.currenIndex - 1 >= 0) {
                    this.queryText = this.suggestions[this.currenIndex - 1];
                }
                this.selected();
            },

            up: function () {
                if (this.currenIndex == 0) return;
                this.currenIndex--;
                this.changeSugestionIndex();
            },
            down: function () {
                //console.log('length: ' +this.suggestions.length + ', index: '+ this.currenIndex)
                if (this.currenIndex < this.suggestions.length) {
                    this.currenIndex++;
                    this.changeSugestionIndex();
                }
            },

            showSugestion: function () {
                this.suggestionList.classList.add("show");
            },
            hideSugestion: function () {
                this.suggestions = [];
                this.suggestionList.classList.remove("show");
            },
            changeSugestionIndex: function () {
                for (var i = 0; i < this.suggestionList.children.length; i++) {
                    if (i == this.currenIndex)
                        this.suggestionList.children[i].classList.add('active')
                    else
                        this.suggestionList.children[i].classList.remove('active')
                }

            },

            selected: function () {
                if (this.currenIndex == 0) {
                    navigator.geolocation.getCurrentPosition(pos => {
                        this.$emit("selected", { lat: pos.coords.latitude, lon: pos.coords.longitude });
                        this.hideSugestion();
                        setTimeout(this.refreshHistory, 1000)
                        
                    });
                } else {
                   
                    this.$emit("selected", { query : (this.currenIndex > 0) ? this.suggestions[this.currenIndex - 1] : this.queryText});
                    this.hideSugestion();
                    setTimeout(this.refreshHistory, 1000)
                }
            },
            refreshHistory()
            {
                console.log('refreshHistory');
                axios.get(this.serverUrl +'searchhistory/GetLast?count=5',
                {withCredentials: true})
                  .then(response => {
                      console.log('History length:' + response.data.result);
                      this.searchHistory=response.data.result;
                  })
                  .catch(e => {
                    console.log(e);
                  });
            }
        }
    }
</script>

<style scoped>
    .search-content {
        width: 100%;
        background-color: orange;
    }

    .search-content-inner {
        display: list-item;
        min-height: 350px;
        background-image: url('~@/assets/images/1.jpg');
        background-size: 100% 100%;
        background-color: #ebebeb;
        margin-bottom: 3px;
        padding-top: 110px;
    }

    .center-box {
        margin: auto auto 30px;
        max-width: 750px;
    }

    .query-box {
        background-color: white;
        border-radius: 3px;
        padding: 10px;
        min-height: 58px;
    }

    .query-icon {
        padding-left: 5px;
        font-size: 25px;
        color: #708090;
    }

    .query-input {
        color: #708090;
        font-size: 24px;
        border: none;
        background: none;
        width: 85%;
        padding-left: 10px;
        margin-right: 12px;
        outline-width: 0;
    }

    .dropdown, .dropdown-item {
        color: #708090;
        font-size: 20px;
        font-family: Solis,Arial,Helvetica,sans-serif;
    }

    .dropdown-item {
        cursor: pointer;
    }

        .dropdown-item.active, .dropdown-item:active {
            color: #191919;
            text-decoration: none;
            background-color: #a6b2be;
        }

    .item-icon {
        padding-right: 10px;
        font-size: 15px;
    }

      .search-history, .history-item
      {
        margin-left: 100px;
        margin-right: 200px;
        display: flex;
        flex-wrap: wrap;
        margin-left: auto;
        margin-right: auto;
        box-sizing: border-box;
        position: relative;
       
      }
      
    .history-item
    {
      background: rgba(0,0,0,.7);
      opacity: 0.8;
      margin-top: 6px;
      padding: 4px;
      border-radius: 3px;
      width: 230px;
      color: #a6b2be;
      text-decoration: none;
      font-size: 13px;
    }
    .history-item-part
    {
       margin: 3px;
       display: inline-block;
       white-space: nowrap;
       height: 70%;
       overflow: hidden;
       position: relative;
       right : 4px !important;
       
    }
    .item-left
     {
        margin-left: 4px;
        overflow: hidden;
        margin-left: 7px;
     }

    .item-right
    {
      min-width: 60px;
      position: absolute;
      display: inline-block !important;
      right : 4px !important;
    }

  .history-item-part-txt
  {
      display: inline-block;
      min-width: 25px;
      text-align: right;
      
  }
     .history-item-part-img
    {
        width: 19px;
        height: 90%;
        margin-left: 5px;
        border: none;
        right : 4px !important;
    }

    @media only screen and (max-width: 800px) {
        .center-box {
            margin-left: 40px;
            margin-right: 40px;
        }
    }

    @media only screen and (max-width: 600px) {
        .search-content-inner {
            min-height: 200px;
            height: 430px;
            background-size: cover;
           
        }
        .center-box {
            margin-left: 40px;
            margin-right: 40px;
        }

        .query-input, .input {
            width: 70%;
        }

        .item-icon .query-icon {
            font-size: 1wv;
        }
    }

    @media only screen and (max-width: 600px) {
          .history-item
          {
            width: 100%;
          }
    }
    
</style>