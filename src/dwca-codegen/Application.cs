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
            bool? pascalCase,
            TermAttributeType? termAttribute,
            string output,
            string configName)
        {
            var configFile = ConfigPath(configName);
            archiveGeneratorConfiguration.ReadFromFile(configFile, serializer);
            archiveGeneratorConfiguration.OverrideConfiguration(@namespace, pascalCase, termAttribute, output);
            Console.WriteLine($"Generating files for archive {archive} using configuration:");
            //TODO: Maybe add a verbose switch to turn this on/off
            ConfigList(archiveGeneratorConfiguration.Configuration);

            var archiveSourceGenerator = new ArchiveSourceGenerator(archiveGeneratorConfiguration.Configuration);
            var sourceFiles = archiveSourceGenerator.GenerateSource(archive);
            sourceFiles.ToList().ForEach((fileName) => Console.WriteLine($"Created {fileName}"));
        }

        public void ConfigList(string configName)
        {
            var configFile = ConfigPath(configName);
            archiveGeneratorConfiguration.ReadFromFile(configFile, serializer);
            Console.WriteLine($"Configuration File: {configFile}");
            ConfigList(archiveGeneratorConfiguration.Configuration);
        }

        private void ConfigList(GeneratorConfiguration generatorConfiguration)
        {
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

        public void ConfigNew(string configName, 
            bool empty, 
            string @namespace, 
            string output, 
            bool? pascalCase, 
            TermAttributeType? termAttribute)
        {
            var configFile = ConfigPath(configName);
            if(!empty)
            {
                archiveGeneratorConfiguration.ReadFromFile(ConfigUtils.DefaultConfig, serializer);
            }
            archiveGeneratorConfiguration.OverrideConfiguration(@namespace, pascalCase, termAttribute, output);
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

        public void ConfigAddUsing(string configName, string namespaceName)
        {
            var configFile = ConfigPath(configName);
            archiveGeneratorConfiguration.ReadFromFile(configFile, serializer);
            archiveGeneratorConfiguration.Usings.Add(namespaceName);
            archiveGeneratorConfiguration.WriteToFile(configFile, serializer);
            Console.WriteLine($"Namespace {namespaceName} added to configuration {configName}");
        }
    }
}
