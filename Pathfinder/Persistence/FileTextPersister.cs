using System;
using System.IO;

namespace Pathfinder.Persistence
{
    public class FileTextPersister
    {
        public string MapName { get; set; }

        public string DefaultExtension { get; set; } = "txt";
        public string FileName { get; set; }
        public string FilePath { get; set; } = Directory.GetCurrentDirectory();
        public string FullPath => Path.Combine(FilePath, FileName + "." + DefaultExtension);

        public void Save<T>(T text)
        {
            if (string.IsNullOrEmpty(FileName))
                throw new Exception("Must set Property FileName before trying to write text to file.");

            if (text.GetType().Name != "String")
                throw new Exception("Must be type string, no conversion implemented yet");

            var splitted = text.ToString().Split(new[] {Environment.NewLine},
                StringSplitOptions.None);

            using (var file =
                new StreamWriter(FullPath))
            {
                foreach (string line in splitted) file.WriteLine(line);
            }
        }

        public string Load<T>()
        {
            var returnType = typeof(T);
            if (returnType != typeof(string)) throw new NotSupportedException("Only Supports string return types");

            string text = File.ReadAllText(FullPath);
            text = RemoveExtraLineEndingAddedToFile<T>(text);
            return text;
        }

        private static string RemoveExtraLineEndingAddedToFile<T>(string text)
        {
            return text.Remove(text.Length - 1, 1);
        }

        public void Delete()
        {
            File.Delete(FullPath);
        }

        public bool Exists()
        {
            bool fileExists = File.Exists(FullPath);
            return fileExists;
        }
    }
}