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

        public Application(IArchiveSourceGenerator archiveSourceGenerator,
            ArchiveGeneratorConfigFactory archiveGeneratorConfigFactory)
        {
            this.archiveSourceGenerator = archiveSourceGenerator;
            this.archiveGeneratorConfigFactory = archiveGeneratorConfigFactory;
        }

        public void ConfigList()
        {
            var archiveGeneratorConfiguration = archiveGeneratorConfigFactory
                .BuildConfiguration();
            ConfigList(archiveGeneratorConfiguration);
        }

        public void Generate(string archive,
            string @namespace,
            bool? pascalCase,
            TermAttributeType? termAttribute,
            string output,
            string configName)
        {
            Console.WriteLine($"Generating files for archive {archive} using configuration:");

            var archiveGeneratorConfiguration = archiveGeneratorConfigFactory
                .BuildConfiguration(@namespace, pascalCase, termAttribute, output);
            //TODO: Maybe add a verbose switch to turn this on/off
            ConfigList(archiveGeneratorConfiguration);
            var sourceFiles = archiveSourceGenerator.GenerateSource(archive, archiveGeneratorConfiguration);
            sourceFiles.ToList().ForEach((fileName) => Console.WriteLine($"Created {fileName}"));
        }

        private void ConfigList(IArchiveGeneratorConfiguration generatorConfiguration)
        {
            Console.WriteLine($"Config File: {ConfigUtils.FullConfigFilePath}");
            Console.WriteLine($"Namespace:  {generatorConfiguration.Namespace}");
            Console.WriteLine($"PascalCase: {generatorConfiguration.PascalCase}");
            Console.WriteLine($"Term Attribute: {generatorConfiguration.TermAttribute}");
            Console.WriteLine($"Output:     {generatorConfiguration.Output}");
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

    }
}
