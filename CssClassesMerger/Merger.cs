using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CssClassesMerger
{
    class Merger
    {
        private Dictionary<string, List<string>> classes;

        public Merger()
        {
            this.classes = new Dictionary<string, List<string>>();
            this.Merge();
        }

        private void Merge()
        {
            string filePath = string.Empty;
            string[] lines = null;

            while (lines == null)
            {
                filePath = this.GetFilePath();
                lines = this.GetFileContent(filePath);
            }

            this.MergeFileClasses(lines);
            this.WriteMergedFile(filePath);
        }

        private string GetFilePath()
        {
            Console.WriteLine("Indiquez le chemin d'accès du fichier CSS dont vous voulez fusionner les classes :");
            return Console.ReadLine();
        }

        private string[] GetFileContent(string filePath)
        {
            try {
                return File.ReadAllLines(filePath);
            } catch {
                Console.WriteLine($"Erreur: Impossible de trouver le fichier à l'emplacement [{filePath}]\n");
                return null;
            }
        }

        private void MergeFileClasses(string[] lines)
        {
            for (int i = 0; i < lines.Length; i += 1)
            {
                // Commentary
                if (lines[i].Contains("/*"))
                {
                    List<string> commentary = new List<string>();

                    while (!lines[i].Contains("*/"))
                    {
                        commentary.Add(lines[i]);
                        i += 1;
                    }
                    commentary.Add(lines[i]);

                    this.classes.Add($"<commentary>{Guid.NewGuid().ToString()}", commentary);
                }

                // @ class
                else if (lines[i].Contains("@"))
                {
                    string className = lines[i];
                    List<string> classContent = new List<string>();

                    int bracesCount = 1;
                    while (bracesCount != 0)
                    {
                        i += 1;
                        classContent.Add(lines[i]);

                        if (lines[i].Contains("{"))
                        {
                            bracesCount += 1;
                        }
                        else if (lines[i].Contains("}"))
                        {
                            bracesCount -= 1;
                        }
                    }

                    this.classes.Add(className, classContent);
                }

                // Regular class
                else if (lines[i].Contains("{"))
                {
                    string className = lines[i];
                    i += 1;

                    List<string> properties = new List<string>();
                    while (!lines[i].Contains("}"))
                    {
                        for (int j = 0; j < properties.Count; j += 1)
                        {
                            if (properties[j].Split(":")[0].Trim() == lines[i].Split(":")[0].Trim())
                            {
                                properties[j] += " /* DUPLICATE PROPERTY */";
                            }
                        }

                        properties.Add(lines[i]);
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
            File.WriteAllLines(newFilePath, this.classes.Select(className =>
                $"{(!className.Key.Contains("<commentary>") ? className.Key : null)}\n" +
                $"{String.Join("\n", className.Value)}\n" +
                $"{(!className.Key.Contains("<commentary>") && !className.Key.Contains("@") ? "}" : null) + "\n"}"
            ));

            Console.WriteLine($"Succès: Fichier CSS mergé et enregistré sous [{newFilePath}]\n");
        }
    }
}
