using DwcaCodegen.CommandLine;
using DwcaCodegen.Config;
using DwcaCodegen.Generator;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Threading.Tasks;

namespace DwcaCodegen
{
    public class Program
    {
        static async Task<int> Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();
            var parser = BuildParser(serviceProvider);

            return await parser.InvokeAsync(args).ConfigureAwait(false);
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IGenerator, Application>();
            services.AddTransient<IConfigApp, Application>();
            services.AddTransient<IArchiveSourceGenerator, ArchiveSourceGenerator>();
            services.AddTransient<ArchiveGeneratorConfigFactory>();
            services.AddTransient<DefaultConfigurationBuilder>();
            services.AddTransient<DotNetConfig.Config>((c) =>
            {
                return DotNetConfig.Config.Build(ConfigUtils.FullConfigFilePath);
            });
            services.AddTransient<OptionBuilder>();
            services.AddCliCommands();
        }

        public static Parser BuildParser(ServiceProvider serviceProvider)
        {
            var rootCommand = new RootCommand();

            foreach (var command in serviceProvider.GetServices<Command>())
            {
                rootCommand.AddCommand(command);
            }

            var config = serviceProvider.GetService<DotNetConfig.Config>();
            rootCommand = rootCommand.WithConfigurableDefaults("dwca-codegen", config);

            var commandLineBuilder = new CommandLineBuilder(rootCommand);

            return commandLineBuilder
                .UseDefaults()
                .Build();
        }
    }
}
