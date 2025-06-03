// using src.DeviceManager.Repositories;
// using src.DeviceManager.Services;
// using src.DeviceProject.Repository;
//
// var builder = WebApplication.CreateBuilder(args);
//
// // Add services to the container.
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
//
// var connectionString = builder.Configuration.GetConnectionString("UniversityDatabase");
//
// builder.Services.AddSingleton<IDeviceRepository>(new DeviceRepository(connectionString));
// builder.Services.AddSingleton<IDeviceService, DeviceService>();
//
// var app = builder.Build();
//
// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
//
// app.UseHttpsRedirection();
//
//
// app.MapGet("/weatherforecast", () =>
//     {
//        
//     })
//     .WithName("GetWeatherForecast")
//     .WithOpenApi();
//
// app.Run();
//
//



using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using src.DeviceManager.Exceptions;
using src.DeviceManager.Services;
using src.DeviceManager.Repositories;
using src.DeviceManager.Services;
using src.DTO;

// FOR LOCAL TESTING ON MYSQL
// FOR LOCAL TESTING ON MYSQL
// FOR LOCAL TESTING ON MYSQL
                            // using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
                            // using Microsoft.EntityFrameworkCore;
// FOR LOCAL TESTING ON MYSQL
// FOR LOCAL TESTING ON MYSQL
// FOR LOCAL TESTING ON MYSQL

// using src.DeviceProject.Repository;


var builder = WebApplication.CreateBuilder(args);

// Swagger + Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultDatabase");
Console.WriteLine("connection string = " + connectionString);


builder.Services.AddDbContext<DeviceEmployeeContext>(opt => opt.UseSqlServer(connectionString));

// ================= FOR LOCAL TESTING ON MYSQL =================
// ================= FOR LOCAL TESTING ON MYSQL =================
// ================= FOR LOCAL TESTING ON MYSQL =================

            // builder.Services.AddDbContext<DeviceEmployeeContext>(options =>
            //         options.UseMySql(
            //                 connectionString,
            //                 new MySqlServerVersion(new Version(9, 3,0))
            //             )
            //         
            // );

// ================= FOR LOCAL TESTING ON MYSQL =================


builder.Services.AddTransient<IDeviceRepository,DeviceRepository>();
builder.Services.AddTransient<IDeviceService, DeviceService>();
builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ================= DEVICE ENDPOINTS =================

app.MapGet("/api/devices", async (IDeviceService service, CancellationToken ct) =>
{
    try
    {
        var results = await service.GetAllDevices(ct);
        return results.Count > 0 ? Results.Ok(results) : Results.NotFound("No devices found");
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message);
    }
});

app.MapGet("/api/devices/{id}", async (int id, IDeviceService service, CancellationToken ct) =>
{
    try
    {
        var device = await service.GetDeviceById(id, ct);
        return device is not null ? Results.Ok(device) : Results.NotFound("Device not found");
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message);
    }
});

app.MapPost("/api/devices", async (CreateDeviceDto dto, IDeviceService service, CancellationToken ct) =>
{
    try
    {
        await service.CreateDevice(dto, ct);
        return Results.Created("/api/devices", dto);
    }
    catch (InvalidDeviceTypeException ex)
    {
        return Results.BadRequest(ex.Message);
        
    }
});

app.MapPut("/api/devices/{id}", async (int id, UpdateDeviceDto dto, IDeviceService service, CancellationToken ct) =>
{
    try
    {
        await service.UpdateDevice(id, dto, ct);
        return Results.Ok();
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound($"No device found with id: '{id}'");
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message);
    }
});

app.MapDelete("/api/devices/{id}", async (int id, IDeviceService service, CancellationToken ct) =>
{
    try
    {
        await service.DeleteDevice(id, ct);
        return Results.Ok();
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound($"No device found with id: '{id}'");
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message);
    }
});

// ================= EMPLOYEE ENDPOINTS =================

app.MapGet("/api/employees", async (IEmployeeService service, CancellationToken ct) =>
{
    try
    {
        var list = await service.GetAllEmployees(ct);
        return list.Any() ? Results.Ok(list) : Results.NotFound("No employees found");
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message);
    }
});

app.MapGet("/api/employees/{id}", async (int id, IEmployeeService service, CancellationToken ct) =>
{
    try
    {
        var employee = await service.GetEmployeeById(id, ct);
        return employee != null ? Results.Ok(employee) : Results.NotFound("Employee not found");
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message);
    }
});


app.Run();



