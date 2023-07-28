# Esp32
## Specifications
* Windows 10
* Visual Studio 2022
* browser (for Swagger)
## Warning
If you want to test the project, you can run it, and it will create a database local.

For use docker, I must change in Program.cs the GetConnectionString parameter to myDbConnectionString, and it's configurate for run on my pc... 
## Method
* Get: return all datas in my database
* Post: take a string as parameter in the form MAC;temp and reject if the string is incorrect, but add a new line to our database if it's correct
* Delete: take either an id of a line to delete, or parameter true for destroy database, and destroy a line or the database or nothing if an error occurs
