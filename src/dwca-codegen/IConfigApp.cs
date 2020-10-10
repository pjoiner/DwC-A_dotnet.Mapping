namespace DwcaCodegen
{
    public interface IConfigApp
    {
        void ConfigAdd(string configFile, string term, string name, bool include, string type);
        void ConfigList(string configFile);
        void ConfigDelete(string configFile, string term);
        public void ConfigNew(string configName);
    }
}