using System;
using System.IO;

namespace CssClassesMerger
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = string.Empty;
            string[] lines = null;

            while (lines == null)
            {
                filePath = GetFilePath();
                lines = GetFileContent(filePath);
            }

            new Merger().Merge(filePath, lines);
        }

        static string GetFilePath()
        {
            Console.WriteLine("Indiquez le chemin d'accès du fichier CSS dont vous voulez fusionner les classes :");
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
                Console.WriteLine($"Erreur: Impossible de lire le fichier à l'emplacement [{filePath}]\n");
                return null;
            }
        }
    }
}
