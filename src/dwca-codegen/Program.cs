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
    class Program
    {
        //TODO: Need to find a more elegant way of doing this
        //TODO: Also need to recover the ConfigUtils to select the correct config file location
        static DotNetConfig.Config config = DotNetConfig.Config.Build(".config/.dwca-generator");

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
            //TODO: Register any Options
            //services.Configure<SettingsType>(configuration.GetSection("sectionName"));

            services.AddSingleton<IGenerator, Application>();
            services.AddSingleton<IArchiveSourceGenerator, ArchiveSourceGenerator>();
            services.AddSingleton<ClassGenerator>();
            services.AddSingleton<ArchiveGeneratorConfigFactory>();
            services.AddSingleton<DotNetConfig.Config>(config);
            services.AddCliCommands();
        }

        public static Parser BuildParser(ServiceProvider serviceProvider)
        {
            var rootCommand = new RootCommand();

            foreach (var command in serviceProvider.GetServices<Command>())
            {
                rootCommand.AddCommand(command);
            }

            rootCommand = rootCommand.WithConfigurableDefaults("default", config);
            
            var commandLineBuilder = new CommandLineBuilder(rootCommand);

            return commandLineBuilder
                .UseDefaults()
                .Build();
        }
    }
}
