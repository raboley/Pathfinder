using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Pathfinder.Pathfinder
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
            Stream saveFileStream = File.Create(FullPath);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(saveFileStream, serializableData);
            saveFileStream.Close();
        }

        public T Load<T>()
        {
            Stream openFileStream = File.OpenRead(FullPath);
            BinaryFormatter deserializer = new BinaryFormatter();
            var gridNode = (T) deserializer.Deserialize(openFileStream);
            openFileStream.Close();

            // gridNode.WorldPosition = new Vector3(gridNode.X, gridNode.Y, gridNode.Z);

            return gridNode;
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
    }
}