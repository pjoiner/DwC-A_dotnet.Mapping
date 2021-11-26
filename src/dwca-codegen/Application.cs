using DwcaCodegen.Config;
using DwcaCodegen.Generator;
using System;
using System.Linq;

namespace DwcaCodegen
{
    public class Application : IGenerator, IConfigApp
    {
        private readonly IArchiveSourceGenerator archiveSourceGenerator;
        private readonly ArchiveGeneratorConfigFactory archiveGeneratorConfigFactory;
        private readonly DefaultConfigurationBuilder defaultConfigurationBuilder;

        public Application(IArchiveSourceGenerator archiveSourceGenerator,
            ArchiveGeneratorConfigFactory archiveGeneratorConfigFactory,
            DefaultConfigurationBuilder defaultConfigurationBuilder)
        {
            this.archiveSourceGenerator = archiveSourceGenerator;
            this.archiveGeneratorConfigFactory = archiveGeneratorConfigFactory;
            this.defaultConfigurationBuilder = defaultConfigurationBuilder;
        }

        public void Generate(string archive,
            string @namespace,
            bool pascalCase,
            TermAttributeType termAttribute,
            string output,
            bool mapMethod)
        {
            Console.WriteLine($"Generating files for archive {archive} using configuration:");

            var archiveGeneratorConfiguration = archiveGeneratorConfigFactory
                .BuildConfiguration(@namespace, pascalCase, termAttribute, output, mapMethod);
            //TODO: Maybe add a verbose switch to turn this on/off
            ConfigList(archiveGeneratorConfiguration);
            var sourceFiles = archiveSourceGenerator.GenerateSource(archive, archiveGeneratorConfiguration);
            sourceFiles.ToList().ForEach((fileName) => Console.WriteLine($"Created {fileName}"));
        }

        public void ConfigList()
        {
            var archiveGeneratorConfiguration = archiveGeneratorConfigFactory
                .BuildConfiguration();
            ConfigList(archiveGeneratorConfiguration);
        }

        private void ConfigList(IArchiveGeneratorConfiguration generatorConfiguration)
        {
            Console.WriteLine($"Config File: {ConfigUtils.FullConfigFilePath}");
            Console.WriteLine($"Namespace:  {generatorConfiguration.Namespace}");
            Console.WriteLine($"PascalCase: {generatorConfiguration.PascalCase}");
            Console.WriteLine($"Term Attribute: {generatorConfiguration.TermAttribute}");
            Console.WriteLine($"Output:     {generatorConfiguration.Output}");
            Console.WriteLine($"Map method: {generatorConfiguration.MapMethod}");
            Console.WriteLine($"Usings:");
            generatorConfiguration.Usings.ToList().ForEach(n => Console.WriteLine($"          {n}"));
            Console.WriteLine();
            Console.WriteLine($"Name      Type      Include Term");
            Console.WriteLine(new string('-', 80));
            foreach (var property in generatorConfiguration.Properties)
            {
                var propertyConfig = generatorConfiguration.GetPropertyConfiguration(property.Key);
                Console.WriteLine($"{propertyConfig.PropertyName,-10}{propertyConfig.TypeName,-10}{propertyConfig.Include,-8}{property.Key}");
            }
        }

        public void ConfigInit()
        {
            defaultConfigurationBuilder.Init();
            Console.WriteLine($"Configuration file {ConfigUtils.FullConfigFilePath} created");
        }

    }
}
