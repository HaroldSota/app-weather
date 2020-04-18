# app-weather
 .NetCore + VueJS weather app

There are two folders 'app-api' containing the aplication api, in .NET Core 3.1 that uses MS SQL as db, and app-ui containing the VueJS
app created with '@vue/cli 4.3.1' 

To run the application, you need:
1) Create a MS SQL database schema
2) Set the connection string in the config file 'app-api/Presentation/AppWeather.Api/appsettings.json'.
3) In Package Manager Console chose the AppWeather.Persistence.Migrations project and run the commands:
     >Add-Migration InitialCreate
     
     >Update-Database 
  
4) Change the post execution (not mandatory), but you will need it to configure the ui for step 5
5) In app-ui/src/App.vue change the serverUrl :'https://localhost:5001/api/' you your local api location.

To run unit testing you can use the EF Core in-memory database provider or your testing db, 
the default configuration is to use in-memory database, but you can change it at 'app-api/Tests/AppWeather.Tests/appsettings.Tests.json'
by changing IsTesting to false.

