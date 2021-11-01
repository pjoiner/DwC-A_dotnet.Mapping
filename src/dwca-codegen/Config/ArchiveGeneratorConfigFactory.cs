﻿using DotNetConfig;
using System;
using System.Linq;
using System.Text;

namespace DwcaCodegen.Config
{
    public class ArchiveGeneratorConfigFactory
    {
        private const string TermAttributeNamespaceName = "DwC_A.Terms";
        private readonly DotNetConfig.Config config;

        public ArchiveGeneratorConfigFactory(DotNetConfig.Config config)
        {
            this.config = config;
        }

        public IArchiveGeneratorConfiguration BuildConfiguration()
        {
            var @namespace = config.GetString("default", "namespace") ?? "DwC";
            var pascalCase = config.GetBoolean("default", "pascalCase") ?? true;
            var termAttribute = Enum.Parse<TermAttributeType>(config.GetString("default", "termAttribute") ?? "none");
            var output = config.GetString("default", "output") ?? ".";
            return BuildConfiguration(@namespace, pascalCase, termAttribute, output);
        }

        public IArchiveGeneratorConfiguration BuildConfiguration(
            string @namespace,
            bool pascalCase,
            TermAttributeType termAttribute,
            string output)
        {
            var archiveGeneratorConfiguration = new ArchiveGeneratorConfiguration()
                .AddNamespace(@namespace)
                .AddPascalCase(pascalCase)
                .AddTermAttribute(termAttribute)
                .AddOutput(output);

            var usingEntries = config.GetAll("default", "usings", "using")
                .Select(n => n.RawValue);
            foreach (var entry in usingEntries)
            {
                archiveGeneratorConfiguration.AddUsing(entry);
            }
            if (archiveGeneratorConfiguration.TermAttribute != TermAttributeType.none 
                && !archiveGeneratorConfiguration.Usings.Contains(TermAttributeNamespaceName))
            {
                archiveGeneratorConfiguration.AddUsing(TermAttributeNamespaceName);
            }

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

            return archiveGeneratorConfiguration;
        }
    }
}