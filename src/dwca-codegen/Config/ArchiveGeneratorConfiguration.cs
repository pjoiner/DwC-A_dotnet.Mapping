﻿using System.Collections.Generic;

namespace DwcaCodegen.Config
{
    public class ArchiveGeneratorConfiguration : IArchiveGeneratorConfiguration
    {
        private string @namespace = "DwC";
        private string output = ".";
        private bool pascalCase = true;
        private TermAttributeType termAttributeType = TermAttributeType.none;
        private bool mapMethod = false;
        private readonly IList<string> usings = new List<string>();
        private readonly IDictionary<string, PropertyConfiguration> properties = new Dictionary<string, PropertyConfiguration>();

        public string Namespace => @namespace;
        public string Output => output;
        public bool PascalCase => pascalCase;
        public TermAttributeType TermAttribute => termAttributeType;
        public bool MapMethod => mapMethod;
        public IList<string> Usings => usings;
        public IDictionary<string, PropertyConfiguration> Properties => properties;

        public PropertyConfiguration GetPropertyConfiguration(string term) => Properties.ContainsKey(term)
                    ? Properties[term]
                    : new PropertyConfiguration();

        public ArchiveGeneratorConfiguration AddNamespace(string namespaceName)
        {
            this.@namespace = namespaceName;
            return this;
        }

        public ArchiveGeneratorConfiguration AddPascalCase(bool pascalCase)
        {
            this.pascalCase = pascalCase;
            return this;
        }

        public ArchiveGeneratorConfiguration AddTermAttribute(TermAttributeType termAttribute)
        {
            this.termAttributeType = termAttribute;
            return this;
        }

        public ArchiveGeneratorConfiguration AddUsing(string usingNamespace)
        {
            usings.Add(usingNamespace);
            return this;
        }

        public ArchiveGeneratorConfiguration AddOutput(string output)
        {
            this.output = output;
            return this;
        }

        public ArchiveGeneratorConfiguration AddMapMethod(bool mapMethod)
        {
            this.mapMethod = mapMethod;
            return this;
        }
    }
}
