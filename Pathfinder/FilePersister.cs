using System.IO;
using Newtonsoft.Json;

namespace Pathfinder
{
    public class FilePersister : IPersister
    {
        public ISerializer Serializer;

        public FilePersister()
        {
        }

        public FilePersister(string fileName)
        {
            FileName = fileName;
        }

        public string DefaultExtension { get; set; } = "map";
        public string FileName { get; set; }
        public string FilePath { get; set; } = Directory.GetCurrentDirectory();
        public string FullPath => Path.Combine(FilePath, FileName);

        public void Save<T>(T serializableData)
        {
            var jsonSerializer = new JsonSerializer();

            using (var streamWriter = new StreamWriter(FullPath))
            using (JsonWriter jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                jsonSerializer.Serialize(jsonTextWriter, serializableData);
            }
        }

        public T Load<T>()
        {
            var jsonSerializer = new JsonSerializer();

            using (var streamReader = new StreamReader(FullPath))
            using (JsonReader jsonReader = new JsonTextReader(streamReader))
            {
                return jsonSerializer.Deserialize<T>(jsonReader);
            }
        }

        public void Delete()
        {
            File.Delete(FullPath);
        }

        public string MapName
        {
            get => Path.GetFileNameWithoutExtension(FileName);
            set => FileName = value + "." + DefaultExtension;
        }

        public bool Exists()
        {
            bool fileExists = File.Exists(FullPath);
            return fileExists;
        }
    }
}