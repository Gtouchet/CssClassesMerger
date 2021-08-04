using CssClassesMerger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CssCleaner
{
    class Program
    {
        

        static void Main(string[] args)
        {
            string filePath = getFilePath();

            string[] lines = GetFileContent(filePath);
            if (lines == null)
            {
                return;
            }

            WriteMergedFile(filePath, GetMergedClasses(lines));
        }

        static string getFilePath()
        {
            Console.WriteLine("Indiquez le chemin d'accès du fichier CSS à nettoyer :");
            return Console.ReadLine();
        }

        static string[] GetFileContent(string filePath)
        {
            try
            {
                return File.ReadAllLines(filePath);
            }
            catch
            {
                Console.WriteLine($"Erreur: Impossible de trouver le fichier à l'emplacement [{filePath}]");
                return null;
            }
        }

        static Merger GetMergedClasses(string[] lines)
        {
            Merger merger = new Merger();

            for (int i = 0; i < lines.Length; i += 1)
            {
                if (lines[i].Contains("{"))
                {
                    if (lines[i].Contains("@"))
                    {
                        merger.rawLines.Add(lines[i]);
                        int bracesCount = 1;
                        while (bracesCount != 0)
                        {
                            i += 1;
                            merger.rawLines.Add(lines[i]);
                            if (lines[i].Contains("{")) bracesCount += 1;
                            else if (lines[i].Contains("}")) bracesCount -= 1;
                        }
                        continue;
                    }

                    string className = lines[i];
                    i += 1;

                    List<string> properties = new List<string>();
                    while (!lines[i].Contains("}"))
                    {
                        for (int j = 0; j < properties.Count; j += 1)
                        {
                            if (properties[j].Split(":")[0] == lines[i].Split(":")[0])
                            {
                                properties[j] += " /* DUPLICATE PROPERTY */";
                            }
                        }

                        properties.Add(lines[i]);
                        i += 1;
                    }

                    if (!merger.classes.ContainsKey(className))
                    {
                        merger.classes.Add(className, properties);
                    }

                    else
                    {
                        List<string> classProperties = merger.classes[className];
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

                        merger.classes[className].AddRange(properties);
                    }
                }
            }

            return merger;
        }

        static void WriteMergedFile(string filePath,  Merger mergedClasses)
        {
            string newFilePath = Path.GetDirectoryName(filePath) + @"\new_" + Path.GetFileName(filePath);

            File.WriteAllLines(newFilePath, mergedClasses.classes.Select(className => $"{className.Key}\n{String.Join("\n", className.Value)}\n}}\n"));
            File.AppendAllLines(newFilePath, mergedClasses.rawLines);

            Console.WriteLine($"Succès: Fichier CSS mergé et enregistré sous [{newFilePath}]");
        }
    }
}
