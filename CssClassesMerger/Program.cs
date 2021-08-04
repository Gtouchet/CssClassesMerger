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
            Console.WriteLine("Indiquez le chemin d'accès du fichier CSS à nettoyer :");
            string filePath = Console.ReadLine();

            string[] lines;
            try
            {
                lines = File.ReadAllLines(filePath);
            }
            catch
            {
                Console.WriteLine($"Erreur: Impossible de trouver le fichier à l'emplacement [{filePath}]");
                return;
            }

            SortedDictionary<string, List<string>> classes = new SortedDictionary<string, List<string>>();
            List<string> rawLines = new List<string>();
            List<string> duplicatesWarnings = new List<string>();

            for (int i = 0; i < lines.Length; i += 1)
            {
                if (lines[i].Contains("{"))
                {
                    // @classes
                    if (lines[i].Contains("@"))
                    {
                        rawLines.Add(lines[i]);
                        int bracesCount = 1;
                        while (bracesCount != 0)
                        {
                            i += 1;
                            rawLines.Add(lines[i]);
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

                    if (!classes.ContainsKey(className))
                    {
                        classes.Add(className, properties);
                    }

                    else
                    {
                        List<string> classProperties = classes[className];
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

                        classes[className].AddRange(properties);
                    }
                }
            }

            string newFilePath = Path.GetDirectoryName(filePath) + @"\new_" + Path.GetFileName(filePath);
            File.WriteAllLines(newFilePath, classes.Select(className => $"{className.Key}\n{String.Join("\n", className.Value)}\n}}\n"));
            File.AppendAllLines(newFilePath, rawLines);

            Console.WriteLine($"Succès: Fichier CSS nettoyé et enregistré sous [{newFilePath}].");
        }
    }
}
