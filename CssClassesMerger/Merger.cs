using System;
using System.Collections.Generic;

namespace CssClassesMerger
{
    class Merger
    {
        private CssContent Content = new CssContent();
        private int index;
        private int ruleIndex;

        public void Merge(string filePath, string[] lines)
        {
            for (index = 0; index < lines.Length; index += 1)
            {
                if (lines[index].Contains("/*"))
                {
                    this.Content.AddCommentary(this.GetCommentary(lines, index));
                }
                else if (lines[index].Trim().EndsWith("{"))
                {
                    if (lines[index].Trim().StartsWith("@"))
                    {
                        this.Content.AddRule(this.GetRule(this.GetRuleLines(lines)));
                    }
                    else
                    {
                        this.Content.AddClass(this.GetClass(lines, index));
                    }
                }
            }
            this.WriteMergedFile(filePath);
        }

        private CssClass GetClass(string[] lines, int i, bool isRuleClass = false)
        {
            CssClass cssClass = new CssClass() { Name = lines[i] };

            i += 1;
            while (!lines[i].Trim().EndsWith("}"))
            {
                if (lines[i].Contains("/*"))
                {
                    cssClass.Commentaries.Add(this.GetCommentary(lines, i, isRuleClass));
                }
                else if (lines[i].Contains(":"))
                {
                    cssClass.Properties.Add(new CssProperty()
                    {
                        Name = lines[i].Split(":")[0],
                        Content = lines[i].Split(":")[1],
                    });
                }
                i += 1;
            }

            if (!isRuleClass)
            {
                this.index = i;
            }

            return cssClass;
        }

        private CssCommentaries GetCommentary(string[] lines, int i, bool isRuleCommentary = false)
        {
            CssCommentaries commentary = new CssCommentaries();
            commentary.Add(lines[i]);

            while (!lines[i].Contains("*/"))
            {
                i += 1;
                commentary.Add(lines[i]);
            }

            if (!isRuleCommentary)
            {
                this.index = i;
            }

            return commentary;
        }

        private CssRule GetRule(string[] lines)
        {
            CssRule cssRule = new CssRule() { Name = lines[0] };

            for (ruleIndex = 1; ruleIndex < lines.Length; ruleIndex += 1)
            {
                if (lines[ruleIndex].Contains("/*"))
                {
                    cssRule.Commentaries.Add(this.GetCommentary(lines, ruleIndex, true));
                }
                else if (lines[ruleIndex].Trim().EndsWith("{"))
                {
                    cssRule.Classes.Add(this.GetClass(lines, ruleIndex, true));
                }
            }

            return cssRule;
        }

        private string[] GetRuleLines(string[] lines)
        {
            List<string> ruleLines = new List<string>();

            int bracet = 1;
            while (bracet != 0)
            {
                ruleLines.Add(lines[index]);
                index += 1;
                bracet += lines[index].Contains("{") ? 1 : lines[index].Contains("}") ? -1 : 0;
            }

            return ruleLines.ToArray();
        }

        private void WriteMergedFile(string filePath)
        {
            foreach (CssClass cssClass in this.Content.Classes)
            {
                this.Display(cssClass);
            }
            foreach (CssRule cssRule in this.Content.Rules)
            {
                this.Display(cssRule);
            }
            foreach (string commentaryLine in this.Content.Commentaries)
            {
                Console.WriteLine(commentaryLine);
            }
        }

        private void Display(CssClass cssClass, bool isRuleClass = false)
        {
            Console.WriteLine(cssClass.Name);
            foreach (CssProperty cssProperty in cssClass.Properties)
            {
                Console.WriteLine($"{cssProperty.Name}:{cssProperty.Content}");
            }
            foreach (string commentaryLine in cssClass.Commentaries)
            {
                Console.WriteLine(commentaryLine);
            }
            Console.WriteLine(isRuleClass ? "\t}\n" : "}\n");
        }

        private void Display(CssRule cssRule)
        {
            Console.WriteLine(cssRule.Name);
            foreach (CssClass cssClass in cssRule.Classes)
            {
                this.Display(cssClass, true);
            }
            foreach (string commentaryLine in cssRule.Commentaries)
            {
                Console.WriteLine(commentaryLine);
            }
            Console.WriteLine("}\n");
        }
    }
}

/*
string newFilePath = Path.GetDirectoryName(filePath) + @"\new_" + Path.GetFileName(filePath);
File.WriteAllLines(newFilePath, classes.Select(className =>
    $"{(!className.Key.Contains("<commentary>") ? className.Key : null)}\n" +
    $"{String.Join("\n", className.Value)}\n" +
    $"{(!className.Key.Contains("<commentary>") && !className.Key.Contains("@") ? "}" : null) + "\n"}"
));

Console.WriteLine($"Succès: Fichier CSS mergé et enregistré sous [{newFilePath}]\n");
*/