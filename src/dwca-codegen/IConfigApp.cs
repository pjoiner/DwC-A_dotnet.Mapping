namespace DwcaCodegen
{
    public interface IConfigApp
    {
        void ConfigAdd(string configName, string term, string name, bool include, string type);
        void ConfigList(string configName);
        void ConfigDelete(string configName, string term);
        public void ConfigNew(string configName, bool empty);
        public string ConfigPath(string configName);
    }
}