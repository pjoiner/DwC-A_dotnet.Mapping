using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DwcaCodegen.Utils
{
    public class JsonSerializer : ISerializer
    {
        private readonly JsonSerializerOptions options = new JsonSerializerOptions()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };

        public void Serialize<T>(T config, string fileName)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(config, typeof(T), options);
            File.WriteAllText(fileName, json);
        }

        public T Deserialize<T>(string fileName)
        {
            string json = File.ReadAllText(fileName);
            return System.Text.Json.JsonSerializer.Deserialize<T>(json, options);
        }

    }
}
