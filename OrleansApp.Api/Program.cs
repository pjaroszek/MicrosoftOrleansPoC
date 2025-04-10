using System.Net;
using Microsoft.OpenApi.Models;
using Orleans.Configuration;
using OrleansApp.Api.Middlewares;
using OrleansApp.Common.DTOs;
using OrleansApp.Common.Exceptions;
using OrleansApp.Orleans.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5080);
});

// Konfiguracja Serilog - PRZED budowaniem aplikacji
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
    .CreateLogger();

builder.Host.UseSerilog();

builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder.UseLocalhostClustering();
    // Konfiguracja magazynu stanu w pamięci (dla PoC)
    siloBuilder.AddMemoryGrainStorage("SqlStateStore");
    
    // Dodanie dashboardu Orleans
    siloBuilder.UseDashboard(options =>
    {
        options.Username = "admin";    // Opcjonalnie: Nazwa użytkownika do logowania
        options.Password = "admin";  // Opcjonalnie: Hasło do logowania
        options.Host = "*";           // Pozwala na dostęp z dowolnego adresu
        options.Port = 5081;          // Port, na którym będzie dostępny dashboard
        options.HostSelf = true;      // Orleans sam obsługuje hosting dashboardu
    });
    
    siloBuilder.Configure<EndpointOptions>(options =>
    {
        options.AdvertisedIPAddress = IPAddress.Parse("0.0.0.0");
        options.SiloPort = 11111;
        options.GatewayPort = 30000;
    });
    
    // Konfiguracja magazynu stanu w pamięci (dla PoC)
    siloBuilder.AddMemoryGrainStorage("SqlStateStore");
});

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Orleans User API",
        Version = "v1",
        Description = "API obsługujące użytkowników za pomocą Orleans"
    });
});

// Dodanie middleware dla CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", corsBuilder =>
    {
        corsBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.MapOpenApi();
//}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Orleans User API v1");
    c.RoutePrefix = "swagger"; // Możesz również ustawić na pusty string, by Swagger był na stronie głównej
});

// Obsługa wyjątków - Middleware
app.UseGlobalExceptionHandler();

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Zdefiniowanie endpointów Minimal API
var usersGroup = app.MapGroup("/api/users");

// Pobierz użytkownika po ID
usersGroup.MapGet("/{id}", async (string id, IGrainFactory grainFactory) =>
{
    var userGrain = grainFactory.GetGrain<IUserGrain>(id);
    
    try
    {
        var user = await userGrain.GetUserAsync();
        return Results.Ok(user);
    }
    catch (NotFoundException ex)
    {
        return Results.NotFound(new OrleansApp.Common.Models.ErrorResponse
        {
            StatusCode = StatusCodes.Status404NotFound,
            Message = ex.Message
        });
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

// Utwórz nowego użytkownika
usersGroup.MapPost("/", async (UserDto user, IGrainFactory grainFactory) =>
{
    // Generowanie ID jeśli nie zostało podane
    if (string.IsNullOrEmpty(user.Id))
    {
        user.Id = Guid.NewGuid().ToString();
    }
    
    var userGrain = grainFactory.GetGrain<IUserGrain>(user.Id);
    await userGrain.UpdateUserAsync(user);
    
    return Results.Created($"/api/users/{user.Id}", user);
});

// Aktualizuj użytkownika
usersGroup.MapPut("/{id}", async (string id, UserDto userUpdate, IGrainFactory grainFactory) =>
{
    var userGrain = grainFactory.GetGrain<IUserGrain>(id);
    
    try
    {
        // Upewnij się, że użytkownik istnieje
        var existingUser = await userGrain.GetUserAsync();
        
        // Uaktualnij dane
        userUpdate.Id = id; // Upewnij się, że ID się zgadza
        await userGrain.UpdateUserAsync(userUpdate);
        
        return Results.Ok(userUpdate);
    }
    catch (NotFoundException ex)
    {
        return Results.NotFound(new OrleansApp.Common.Models.ErrorResponse
        {
            StatusCode = StatusCodes.Status404NotFound,
            Message = ex.Message
        });
    }
});

// Usuń użytkownika
usersGroup.MapDelete("/{id}", async (string id, IGrainFactory grainFactory) =>
{
    var userGrain = grainFactory.GetGrain<IUserGrain>(id);
    
    try
    {
        // Upewnij się, że użytkownik istnieje
        var existingUser = await userGrain.GetUserAsync();
        
        // Usuń użytkownika
        await userGrain.DeleteUserAsync();
        
        return Results.NoContent();
    }
    catch (NotFoundException ex)
    {
        return Results.NotFound(new OrleansApp.Common.Models.ErrorResponse
        {
            StatusCode = StatusCodes.Status404NotFound,
            Message = ex.Message
        });
    }
});

app.Run();
