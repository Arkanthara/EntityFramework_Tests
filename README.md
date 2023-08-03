# Tests on Entity Framework
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
* Branch esp32 with the implementation of the server for retrieve datas from esp32. Not functionnal for now... But in progress ! 
