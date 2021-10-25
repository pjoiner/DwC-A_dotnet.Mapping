using DwcaCodegen.Config;

namespace DwcaCodegen
{
    public interface IConfigApp
    {
        void ConfigAdd(string configName, string term, string name, bool include, string type);
        void ConfigList(string configName);
        void ConfigDelete(string configName, string term);
        void ConfigNew(string configName, bool empty, string @namespace, string output, bool? pascalCase, TermAttributeType? termAttribute);
        string ConfigPath(string configName);
        void ConfigAddUsing(string configName, string namespaceName);
    }
}