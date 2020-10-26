using System.IO;
using Newtonsoft.Json;

namespace Pathfinder
{
    public class FilePersister : IPersister
    {
        public FilePersister()
        {
        }

        public FilePersister(string fileName)
        {
            FileName = fileName;
        }

        public void Save<T>(T serializableData)
        {
            JsonSerializer jsonSerializer = new JsonSerializer();

            using (StreamWriter streamWriter = new StreamWriter(FullPath))
            using (JsonWriter jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                jsonSerializer.Serialize(jsonTextWriter, serializableData);
            }
        }

        public T Load<T>()
        {
            JsonSerializer jsonSerializer = new JsonSerializer();

            using (StreamReader streamReader = new StreamReader(FullPath))
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

        public string DefaultExtension { get; set; } = "map";
        public string FileName { get; set; }
        public string FilePath { get; set; } = Directory.GetCurrentDirectory();
        public string FullPath => Path.Combine(FilePath, FileName);
        public ISerializer Serializer;
    }
}