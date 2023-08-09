# Tests on Entity Framework and creation of a server for retrieve datas from esp32
## Specifications
* Windows 10
* Visual Studio 2022
* Browser (for Swagger)
## Objective
* Retrieve datas from esp32. Esp32 send data with https, so I use Web API.
## Tests
First, I tested EntityFramework and Microsoft.EntityFramework.Core:
|Tests|Status|
|:-:|:-:|
|EntityFramework|Success|
|Microsoft.EntityFramework.Core|Success|
## Structure
* Branch entityframework with tests on EntityFramework with local db. Functionnal, but I have had to create an App.config file manually...
* Branch microsoft.entityframework.core with tests on Microsoft.EntityFramework.Core. Functionnal. The goal was to create a database and make some basic operations on it...
* The main branch has the implementation of the server
## Server
* The server is in C-sharp
* There are get methods for see datas, a post method, and a delete method for destroy either the database or a row
* Connection to the database is made in Program.cs with connectionStrings which is in appsettings.json
