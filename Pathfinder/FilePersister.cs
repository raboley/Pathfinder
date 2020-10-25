using System.IO;
using System.Numerics;
using System.Runtime.Serialization.Formatters.Binary;

namespace Pathfinder.Pathfinder
{
    public class FilePersister : IPersister
    {
        public FilePersister(string fileName)
        {
            FileName = fileName;
        }

        public void Save(GridNode gridNode)
        {
            Stream saveFileStream = File.Create(FileName);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(saveFileStream, gridNode);
            saveFileStream.Close(); 
        }

        public GridNode Load()
        {
            Stream openFileStream = File.OpenRead(FileName);
            BinaryFormatter deserializer = new BinaryFormatter();
            var gridNode = (GridNode) deserializer.Deserialize(openFileStream);
            openFileStream.Close();
            
            gridNode.WorldPosition = new Vector3(gridNode.X, gridNode.Y, gridNode.Z);

            return gridNode;
        }

        public string FileName { get; set; }
    }
}