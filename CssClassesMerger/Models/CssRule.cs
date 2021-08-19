using System.Collections.Generic;
using System.Linq;

namespace CssClassesMerger
{
    class CssRule
    {
        public string name { get; set; }
        public List<CssClass> cssClasses { get; set; }

        public CssRule()
        {
            this.cssClasses = new List<CssClass>();
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

        public void AddClasses(List<CssClass> newClasses)
        {
            foreach (CssClass newClass in newClasses)
            {
                this.AddClass(newClass);
            }
        }
    }
}
