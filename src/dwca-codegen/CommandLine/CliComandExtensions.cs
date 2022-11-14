using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Reflection;

namespace DwcaCodegen.CommandLine;

public static class CliCommandExtensions
{
    public static IServiceCollection AddCliCommands(this IServiceCollection services)
    {
        Type commandType = typeof(Command);

        IEnumerable<Type> commands = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(x => commandType.IsAssignableFrom(x));

        foreach (Type command in commands)
        {
            services.AddSingleton(commandType, command);
        }

        return services;
    }
}
