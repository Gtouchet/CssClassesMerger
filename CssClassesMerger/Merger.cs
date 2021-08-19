using System;
using System.Collections.Generic;
using System.Linq;

namespace CssClassesMerger
{
    class Merger
    {
        private CssContent cssContent;
        private int index;
        private int ruleIndex;

        public Merger()
        {
            this.cssContent = new CssContent();
        }

        public void Merge(string filePath, string[] lines)
        {
            for (index = 0; index < lines.Length; index += 1)
            {
                if (lines[index].Contains("/*"))
                {
                    this.cssContent.cssClasses.Add(this.GetRawCommentary(lines, index));
                }
                else if (lines[index].Trim().EndsWith("{"))
                {
                    if (lines[index].Trim().StartsWith("@"))
                    {
                        this.cssContent.AddRule(this.GetRule(this.GetRuleLines(lines)));
                    }
                    else
                    {
                        this.cssContent.AddClass(this.GetClass(lines, index));
                    }
                }
            }

            this.WriteMergedFile(filePath);
        }

        private CssClass GetClass(string[] lines, int i, bool isRuleClass = false)
        {
            CssClass cssClass = new CssClass() { name = lines[i] };

            i += 1;
            while (!lines[i].Trim().EndsWith("}"))
            {
                if (lines[i].Contains("/*"))
                {
                    cssClass.properties.Add(this.GetClassCommentary(lines, i, isRuleClass));
                }
                else if (lines[i].Contains(":"))
                {
                    cssClass.AddProperties(new List<CssProperty>()
                    {
                        new CssProperty()
                        {
                            name = lines[i].Split(":")[0],
                            content = lines[i].Split(":")[1],
                        },
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

        private CssRule GetRule(string[] lines)
        {
            CssRule cssRule = new CssRule() { name = lines[0] };

            for (ruleIndex = 1; ruleIndex < lines.Length; ruleIndex += 1)
            {
                if (lines[ruleIndex].Contains("/*"))
                {
                    cssRule.cssClasses.Add(this.GetRawCommentary(lines, ruleIndex, true));
                }
                else if (lines[ruleIndex].Trim().EndsWith("{"))
                {
                    cssRule.AddClass(this.GetClass(lines, ruleIndex, true));
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

        private CssClass GetRawCommentary(string[] lines, int i, bool isRuleRawCommentary = false)
        {
            CssProperty rawCommentary = new CssProperty();

            rawCommentary.commentary.Add(lines[i]);
            while (!lines[i].Contains("*/"))
            {
                i += 1;
                rawCommentary.commentary.Add(lines[i]);
            }

            if (!isRuleRawCommentary)
            {
                this.index = i;
            }
            else
            {
                this.ruleIndex = i;
            }

            return new CssClass()
            {
                name = "<commentary>",
                properties = new List<CssProperty>() { rawCommentary },
            };
        }

        private CssProperty GetClassCommentary(string[] lines, int i, bool isRuleCommentaryClass = false)
        {
            List<string> classCommentary = new List<string>();

            classCommentary.Add(lines[i]);
            while (!lines[i].Contains("*/"))
            {
                i += 1;
                classCommentary.Add(lines[i]);
            }

            if (!isRuleCommentaryClass)
            {
                this.index = i;
            }
            else
            {
                this.ruleIndex = i;
            }

            return new CssProperty()
            {
                name = "<commentary>",
                commentary = classCommentary,
            };
        }

        private void WriteMergedFile(string filePath)
        {
            // todo: write in file

            foreach (CssClass cssClass in this.cssContent.cssClasses)
            {
                this.Display(cssClass);
            }

            foreach (CssRule cssRule in this.cssContent.cssRules)
            {
                Console.WriteLine(cssRule.name);
                foreach (CssClass cssClass in cssRule.cssClasses)
                {
                    this.Display(cssClass, true);
                }
                Console.WriteLine("}\n");
            }
        }

        private void Display(CssClass cssClass, bool isRuleClass = false)
        {
            if (cssClass.name == "<commentary>")
            {
                foreach (string commentaryLine in cssClass.properties[0].commentary)
                {
                    Console.WriteLine(commentaryLine);
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine(cssClass.name);
                foreach (CssProperty property in cssClass.properties)
                {
                    if (property.name == "<commentary>")
                    {
                        foreach (string commentaryLine in property.commentary)
                        {
                            Console.WriteLine(commentaryLine);
                        }
                    }
                    else
                    {
                        Console.WriteLine(property.name + ":" + property.content);
                    }
                }
                Console.WriteLine(isRuleClass ? "\t}": "}\n");
            }
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