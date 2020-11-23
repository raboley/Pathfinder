using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Pathfinder.Persistence
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

        public string DefaultExtension { get; set; } = "json";
        public string FileName { get; set; }
        public string FilePath { get; set; } = Directory.GetCurrentDirectory();
        public string FullPath => Path.Combine(FilePath, FileName + "." + DefaultExtension);

        public void Save<T>(T serializableData)
        {
            var jsonSerializer = new JsonSerializer();

            using (var streamWriter = new StreamWriter(FullPath))
            using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                jsonTextWriter.QuoteChar = '\'';
                jsonTextWriter.Formatting = Formatting.Indented;
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
            set => FileName = value;
        }

        public bool Exists()
        {
            bool fileExists = File.Exists(FullPath);
            return fileExists;
        }

        public List<T> LoadAllOfType<T>()
        {
            List<T> allOfType = new List<T>();
            var allFiles = Directory.GetFiles(FilePath);
            var beginningMapName = MapName;

            try
            {
                foreach (var file in allFiles)
                {
                    var mapName = file.Split('.')[0];
                    MapName = mapName;
                    var thingOfType = Load<T>();
                    allOfType.Add(thingOfType);

                    // if (IsEnumerableType(thingOfType.GetType()))
                    // {
                    //     allOfType.AddRange((IEnumerable<T>) thingOfType);
                    // }
                    // else
                    // {
                    //     allOfType.Add(thingOfType);
                    // }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                MapName = beginningMapName;
            }


            return allOfType;
        }

        bool IsEnumerableType(Type type)
        {
            return (type.GetInterface(nameof(IEnumerable)) != null);
        }
    }
}