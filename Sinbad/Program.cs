using Sinbad.Context;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Here we add new services: it's a dbcontext. So we can use dependency injection because when we use a service,
// the DbContext will be given to our service.
builder.Services.AddDbContext<EspContext>(
   options => options.UseSqlServer(builder.Configuration.GetConnectionString("myDbConnectionString")));




var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();




