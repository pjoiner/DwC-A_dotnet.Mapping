namespace DwcaCodegen.Utils
{
    public interface ISerializer
    {
        T Deserialize<T>(string fileName);
        void Serialize<T>(T config, string fileName);
    }
}