using Microsoft.Extensions.DependencyInjection;
using OrleansApp.Application.Common.Handlers;
using OrleansApp.Application.User.CommandHandlers;
using OrleansApp.Application.User.Commands;
using OrleansApp.Common.DTOs;

namespace OrleansApp.Application.Common;

public static class MediatorExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator>();
        //services.AddTransient<ICommandWithResultHandler<AddNewUserCommand, UserDto>, AddNewUserCommandHandler>();
        
        services.Scan(scan => scan
            .FromAssemblies(typeof(IMediator).Assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandWithResultHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            
            .AddClasses(classes => classes.AssignableTo(typeof(IEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            
            .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        
        // services.Scan(scan => scan
        //     .FromAssemblies(
        //         typeof(ICommandWithResultHandler<,>).Assembly, // Application assembly
        //         typeof(AddNewUserCommandHandler).Assembly) // Upewnij się, że to assembly jest skanowane
        //     .AddClasses(classes => classes.AssignableTo(typeof(ICommandWithResultHandler<,>)))
        //     .AsImplementedInterfaces()
        //     .WithTransientLifetime());
        return services;
    }
}