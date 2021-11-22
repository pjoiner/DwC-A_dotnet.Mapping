using DwcaCodegen.Config;
using System.Collections.Generic;

namespace Tests
{
    public class GeneratorConfigFake : IArchiveGeneratorConfiguration
    {
        public string Namespace => "DwC";

        public string Output => ".";

        public bool PascalCase => true;

        public bool MapMethod => false;

        public IDictionary<string, PropertyConfiguration> Properties => new Dictionary<string, PropertyConfiguration>();

        public TermAttributeType TermAttribute => TermAttributeType.name;

        public IList<string> Usings => new List<string>(){ "System", "DwC_A", "DwC_A.Terms", "DwC_A.Attributes" };

        public PropertyConfiguration GetPropertyConfiguration(string term)
        {
            return new PropertyConfiguration();
        }
    }
}
