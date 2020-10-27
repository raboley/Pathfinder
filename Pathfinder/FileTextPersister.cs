using System;
using System.IO;

namespace Pathfinder
{
    public class FileTextPersister
    {
        public void Save<T>(T text)
        {
            if (string.IsNullOrEmpty(FileName))
            {
                throw new Exception("Must set Property FileName before trying to write text to file.");
            }

            if (text.GetType().Name != "String")
            {
                throw new Exception("Must be type string, no conversion implemented yet");
            }

            string[] splitted = text.ToString().Split(new string[] {System.Environment.NewLine},
                StringSplitOptions.None);

            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(FullPath))
            {
                foreach (string line in splitted)
                {
                    file.WriteLine(line);
                }
            }
        }

        public string Load<T>()
        {
            Type returnType = typeof(T);
            if (returnType != typeof(string))
            {
                throw new NotSupportedException("Only Supports string return types");
            }

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

        public string MapName { get; set; }

        public bool Exists()
        {
            bool fileExists = File.Exists(FullPath);
            return fileExists;
        }

        public string DefaultExtension { get; set; } = "txt";
        public string FileName { get; set; }
        public string FilePath { get; set; } = Directory.GetCurrentDirectory();
        public string FullPath => Path.Combine(FilePath, FileName + "." + DefaultExtension);
    }
}