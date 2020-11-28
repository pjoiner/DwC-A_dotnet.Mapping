using DwcaCodegen.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace DwcaCodegen.Config
{
    public class ArchiveGeneratorConfiguration
    {
        private GeneratorConfiguration config = new GeneratorConfiguration();
        public bool AddAttributes => config.AddAttributes;
        public string Namespace => config.Namespace;
        public string Output => config.Output;
        public bool Capitalize => config.Capitalize;
        public IList<string> Usings => config.Usings;
        public IDictionary<string, PropertyConfiguration> Properties => config.Properties;

        public void WriteToFile(string fileName, ISerializer serializer)
        {
            var path = Path.GetDirectoryName(fileName);
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            serializer.Serialize(config, fileName);
        }

        public void ReadFromFile(string fileName, ISerializer serializer)
        {
            if (!File.Exists(fileName))
            {
                config = new GeneratorConfiguration();
            }
            else
            {
                config = serializer.Deserialize<GeneratorConfiguration>(fileName);
            }
        }

        public PropertyConfiguration GetPropertyConfiguration(string term)
        {
            if (config.Properties.ContainsKey(term))
            {
                return config.Properties[term];
            }
            else
            {
                return new PropertyConfiguration();
            }
        }

        public ArchiveGeneratorConfiguration AddPropertyConfig(string term,
            Action<PropertyConfiguration> configFunc)
        {
            if (configFunc != null)
            {
                var propertyConfig = new PropertyConfiguration();
                configFunc(propertyConfig);
                config.Properties.Add(term, propertyConfig);
            }
            return this;
        }

        public ArchiveGeneratorConfiguration AddNamespace(string namespaceName)
        {
            config.Namespace = namespaceName;
            return this;
        }

        public ArchiveGeneratorConfiguration AddCapitalize(bool capitalize)
        {
            config.Capitalize = capitalize;
            return this;
        }

        public ArchiveGeneratorConfiguration AddUsing(string usingNamespace)
        {
            config.Usings.Add(usingNamespace);
            return this;
        }

        public ArchiveGeneratorConfiguration AddOutput(string output)
        {
            config.Output = output;
            return this;
        }

        public ArchiveGeneratorConfiguration UseAttributes(bool addAttributes)
        {
            config.AddAttributes = addAttributes;
            return this;
        }

    }
}
