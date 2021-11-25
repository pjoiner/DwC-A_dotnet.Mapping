using DotNetConfig;
using System;
using System.Linq;
using System.Text;

namespace DwcaCodegen.Config
{
    public class ArchiveGeneratorConfigFactory
    {
        private const string DefaultSection = "dwca-codegen";
        private const string TermAttributeNamespaceName = "DwC_A.Attributes";
        private const string ExtensionsNamespaceName = "DwC_A.Extensions";
        private const string SystemNamespaceName = "System";
        private readonly DotNetConfig.Config config;

        public ArchiveGeneratorConfigFactory(DotNetConfig.Config config)
        {
            this.config = config;
        }

        public IArchiveGeneratorConfiguration BuildConfiguration()
        {
            var @namespace = config.GetString(DefaultSection, "namespace") ?? "DwC";
            var pascalCase = config.GetBoolean(DefaultSection, "pascalCase") ?? true;
            var termAttribute = Enum.Parse<TermAttributeType>(config.GetString("default", "termAttribute") ?? "none");
            var output = config.GetString(DefaultSection, "output") ?? ".";
            var mapMethod = config.GetBoolean(DefaultSection, "mapMethod") ?? false;
            return BuildConfiguration(@namespace, pascalCase, termAttribute, output, mapMethod);
        }

        public IArchiveGeneratorConfiguration BuildConfiguration(
            string @namespace,
            bool pascalCase,
            TermAttributeType termAttribute,
            string output,
            bool mapMethod)
        {
            var archiveGeneratorConfiguration = new ArchiveGeneratorConfiguration()
                .AddNamespace(@namespace)
                .AddPascalCase(pascalCase)
                .AddTermAttribute(termAttribute)
                .AddOutput(output)
                .AddMapMethod(mapMethod);

            BuildUsings(archiveGeneratorConfiguration);

            BuildProperties(archiveGeneratorConfiguration);

            return archiveGeneratorConfiguration;
        }

        private void BuildProperties(ArchiveGeneratorConfiguration archiveGeneratorConfiguration)
        {
            var props = config.GetRegex("properties");
            var keys = props
                .Select(n => n.Subsection)
                .Distinct();
            foreach (var key in keys)
            {
                archiveGeneratorConfiguration.Properties.Add(key, new PropertyConfiguration()
                {
                    Include = config.GetBoolean("properties", key, "include") ?? true,
                    TypeName = config.GetString("properties", key, "typeName"),
                    PropertyName = config.GetString("properties", key, "propertyName")
                });
            }
        }

        private void BuildUsings(ArchiveGeneratorConfiguration archiveGeneratorConfiguration)
        {
            var usingEntries = config.GetAll(DefaultSection, "usings", "using")
                .Select(n => n.RawValue);
            foreach (var entry in usingEntries)
            {
                archiveGeneratorConfiguration.AddUsing(entry);
            }
            archiveGeneratorConfiguration.AddUsing(SystemNamespaceName);
            if (archiveGeneratorConfiguration.TermAttribute != TermAttributeType.none)
            {
                archiveGeneratorConfiguration.AddUsing(TermAttributeNamespaceName);
            }
            if( archiveGeneratorConfiguration.MapMethod)
            {
                archiveGeneratorConfiguration.AddUsing(ExtensionsNamespaceName);
            }
        }
    }
}
