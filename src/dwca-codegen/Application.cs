using DwcaCodegen.Config;
using DwcaCodegen.Generator;
using DwcaCodegen.Utils;
using System;
using System.IO;
using System.Linq;

namespace DwcaCodegen
{
    public class Application : IGenerator, IConfigApp
    {
        private readonly ArchiveGeneratorConfiguration archiveGeneratorConfiguration;
        private readonly ISerializer serializer;

        public Application()
        {
            archiveGeneratorConfiguration = new ArchiveGeneratorConfiguration();
            serializer = new JsonSerializer();
        }

        public void Generate(string archive,
            string @namespace,
            bool capitalize,
            string output,
            string configName)
        {
            var configFile = ConfigPath(configName);
            Console.WriteLine($"Generating files for archive {archive} for namespace {@namespace} in {output}");
            archiveGeneratorConfiguration.ReadFromFile(configFile, serializer);
            archiveGeneratorConfiguration.AddNamespace(@namespace)
                .AddCapitalize(capitalize)
                .AddOutput(output);
            var archiveSourceGenerator = new ArchiveSourceGenerator(archiveGeneratorConfiguration);
            var sourceFiles = archiveSourceGenerator.GenerateSource(archive);
            sourceFiles.ToList().ForEach((fileName) => Console.WriteLine($"Created {fileName}"));
        }

        public void ConfigList(string configName)
        {
            var configFile = ConfigPath(configName);
            archiveGeneratorConfiguration.ReadFromFile(configFile, serializer);
            Console.WriteLine($"Configuration File: {configFile}");
            Console.WriteLine($"Namespace:  {archiveGeneratorConfiguration.Namespace}");
            Console.WriteLine($"Capitalize: {archiveGeneratorConfiguration.Capitalize}");
            Console.WriteLine($"Output:     {archiveGeneratorConfiguration.Output}");
            Console.WriteLine($"Usings:");
            archiveGeneratorConfiguration.Usings.ToList().ForEach(n => Console.WriteLine($"          {n}"));
            Console.WriteLine();
            Console.WriteLine($"Name      Type      Include Term");
            Console.WriteLine(new string('-', 80));
            foreach (var property in archiveGeneratorConfiguration.Properties)
            {
                var propertyConfig = archiveGeneratorConfiguration.GetPropertyConfiguration(property.Key);
                Console.WriteLine($"{propertyConfig.PropertyName,-10}{propertyConfig.TypeName,-10}{propertyConfig.Include,-8}{property.Key}");
            }
        }

        public void ConfigAdd(string configName,
            string term,
            string name,
            bool include,
            string type)
        {
            var configFile = ConfigPath(configName);
            archiveGeneratorConfiguration.ReadFromFile(configFile, serializer);
            if (archiveGeneratorConfiguration.Properties.ContainsKey(term))
            {
                Console.WriteLine($"Configuration for term {term} already exists.  Delete the term first before adding.");
                return;
            }
            var propertyConfiguration = new PropertyConfiguration()
            {
                Include = include,
                PropertyName = name,
                TypeName = type
            };
            archiveGeneratorConfiguration.Properties.Add(term, propertyConfiguration);
            archiveGeneratorConfiguration.WriteToFile(configFile, serializer);
            Console.WriteLine($"Configuration for term {term} added to file {configFile}");
        }

        public void ConfigDelete(string configName, string term)
        {
            var configFile = ConfigPath(configName);
            archiveGeneratorConfiguration.ReadFromFile(configFile, serializer);
            if (archiveGeneratorConfiguration.Properties.ContainsKey(term))
            {
                archiveGeneratorConfiguration.Properties.Remove(term);
            }
            archiveGeneratorConfiguration.WriteToFile(configFile, serializer);
            Console.WriteLine($"Configuration for term {term} deleted from file {configFile}");
        }

        public void ConfigNew(string configName, bool empty)
        {
            var configFile = ConfigPath(configName);
            if(!empty)
            {
                archiveGeneratorConfiguration.ReadFromFile(ConfigUtils.DefaultConfig, serializer);
            }
            archiveGeneratorConfiguration.WriteToFile(configFile, serializer);
            Console.WriteLine($"Configuration file {configFile} created");
        }

        public string ConfigPath(string configName)
        {
            if(string.IsNullOrEmpty(configName))
            {
                configName = "default";
            }
            if(!Path.HasExtension(configName))
            {
                configName = Path.ChangeExtension(configName, ".json");
            }
            var rootConfigPath = ConfigUtils.ConfigLocation;
            return Path.Combine(rootConfigPath, configName);
        }
    }
}
