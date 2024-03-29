
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PrsBackEnd.Models;

var builder = WebApplication.CreateBuilder(args);

//Program.cs builds/runs program

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
 {
     options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
 });


//  .AddXmlSerializerFormatters()

builder.Services.AddDbContext<PrsDbContext>(
        //lambda
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("PrsConnectionString")) 
    );



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseCors("*");

app.UseAuthorization();

app.MapControllers();

app.Run();
