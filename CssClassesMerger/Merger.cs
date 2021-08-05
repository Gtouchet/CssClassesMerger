using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CssClassesMerger
{
    class Merger
    {
        public string[] lines;
        private SortedDictionary<string, List<string>> classes;
        private List<string> rawLines;

        public Merger()
        {
            this.lines = null;
            this.classes = new SortedDictionary<string, List<string>>();
            this.rawLines = new List<string>();

            this.Merge();
        }

        private void Merge()
        {
            string filePath = string.Empty;
            while (this.lines == null)
            {
                filePath = this.GetFilePath();
                this.GetFileContent(filePath);
            }
            this.MergeFileClasses();
            this.WriteMergedFile(filePath);
        }

        private string GetFilePath()
        {
            Console.WriteLine("Indiquez le chemin d'accès du fichier CSS dont vous voulez fusionner les classes :");
            return Console.ReadLine();
        }

        private void GetFileContent(string filePath)
        {
            try
            {
                this.lines = File.ReadAllLines(filePath);
            }
            catch
            {
                Console.WriteLine($"Erreur: Impossible de trouver le fichier à l'emplacement [{filePath}]\n");
            }
        }

        private void MergeFileClasses()
        {
            for (int i = 0; i < this.lines.Length; i += 1)
            {
                if (this.lines[i].Contains("{"))
                {
                    if (this.lines[i].Contains("@"))
                    {
                        this.rawLines.Add(this.lines[i]);
                        int bracesCount = 1;
                        while (bracesCount != 0)
                        {
                            i += 1;
                            this.rawLines.Add(this.lines[i]);
                            if (this.lines[i].Contains("{")) bracesCount += 1;
                            else if (this.lines[i].Contains("}")) bracesCount -= 1;
                        }
                        continue;
                    }

                    string className = this.lines[i];
                    i += 1;

                    List<string> properties = new List<string>();
                    while (!this.lines[i].Contains("}"))
                    {
                        for (int j = 0; j < properties.Count; j += 1)
                        {
                            if (properties[j].Split(":")[0] == this.lines[i].Split(":")[0])
                            {
                                properties[j] += " /* DUPLICATE PROPERTY */";
                            }
                        }

                        properties.Add(this.lines[i]);
                        i += 1;
                    }

                    if (!this.classes.ContainsKey(className))
                    {
                        this.classes.Add(className, properties);
                    }

                    else
                    {
                        List<string> classProperties = this.classes[className];
                        foreach (string classProperty in classProperties)
                        {
                            for (int j = 0; j < properties.Count; j += 1)
                            {
                                if (properties[j].Split(":")[0].Trim() == classProperty.Split(":")[0].Trim())
                                {
                                    properties[j] += " /* DUPLICATE PROPERTY */";
                                }
                            }
                        }

                        this.classes[className].AddRange(properties);
                    }
                }
            }
        }

        private void WriteMergedFile(string filePath)
        {
            string newFilePath = Path.GetDirectoryName(filePath) + @"\new_" + Path.GetFileName(filePath);

            File.WriteAllLines(newFilePath, this.classes.Select(className => $"{className.Key}\n{String.Join("\n", className.Value)}\n}}\n"));
            File.AppendAllLines(newFilePath, this.rawLines);

            Console.WriteLine($"Succès: Fichier CSS mergé et enregistré sous [{newFilePath}]\n");
        }
    }
}
