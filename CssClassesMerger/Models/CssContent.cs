using System.Collections.Generic;
using System.Linq;

namespace CssClassesMerger
{
    class CssContent
    {
        public List<CssClass> cssClasses { get; set; }
        public List<CssRule> cssRules { get; set; } 

        public CssContent()
        {
            this.cssClasses = new List<CssClass>();
            this.cssRules = new List<CssRule>();
        }

        public void AddClass(CssClass newClass)
        {
            CssClass existingClass = this.cssClasses.FirstOrDefault(cssClass => cssClass.name == newClass.name);

            if (existingClass != null)
            {
                existingClass.AddProperties(newClass.properties);
            }
            else
            {
                this.cssClasses.Add(newClass);
            }
        }

        public void AddRule(CssRule newRule)
        {
            CssRule existingRule = this.cssRules.FirstOrDefault(cssRule => cssRule.name == newRule.name);

            if (existingRule != null)
            {
                existingRule.AddClasses(newRule.cssClasses);
            }
            else
            {
                this.cssRules.Add(newRule);
            }
        }
    }
}
