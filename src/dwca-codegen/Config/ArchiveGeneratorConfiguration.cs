using DwcaCodegen.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace DwcaCodegen.Config
{
    public class ArchiveGeneratorConfiguration
    {
        private const string TermAttributeNamespaceName = "DwC_A.Terms";
        private GeneratorConfiguration config = new GeneratorConfiguration();
        public string Namespace => config.Namespace;
        public string Output => config.Output;
        public bool PascalCase => config.PascalCase;
        public TermAttributeType TermAttribute => config.TermAttribute;
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

        public ArchiveGeneratorConfiguration AddPascalCase(bool pascalCase)
        {
            config.PascalCase = pascalCase;
            return this;
        }

        public ArchiveGeneratorConfiguration AddTermAttribute(TermAttributeType termAttribute)
        {
            config.TermAttribute = termAttribute;
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

        public void OverrideConfiguration(string @namespace, bool? pascalCase, TermAttributeType? termAttribute, string output)
        {
            if (!string.IsNullOrEmpty(@namespace))
            {
                AddNamespace(@namespace);
            }
            if (pascalCase.HasValue)
            {
                AddPascalCase(pascalCase.Value);
            }
            if (termAttribute.HasValue)
            {
                AddTermAttribute(termAttribute.Value);
            }
            if (!string.IsNullOrEmpty(output))
            {
                AddOutput(output);
            }
            if (config.TermAttribute != TermAttributeType.none && !config.Usings.Contains(TermAttributeNamespaceName))
            {
                config.Usings.Add(TermAttributeNamespaceName);
            }
        }
    }
}
