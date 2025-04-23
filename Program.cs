using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using MormorDagnysDel2.Data;
using MormorDagnysDel2.Interfaces;
using MormorDagnysDel2.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
    {
        policy.WithOrigins("*")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// var serverVersion = new MySqlServerVersion(new Version(9, 1, 0));
builder.Services.AddDbContext<DataContext>(options =>
{
    //options.UseMySql(builder.Configuration.GetConnectionString("MySQL"), serverVersion);
    options.UseSqlite(builder.Configuration.GetConnectionString("DevConnection"));
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();