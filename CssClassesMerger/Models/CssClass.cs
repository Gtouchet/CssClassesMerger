using System.Collections.Generic;

namespace CssClassesMerger
{
    class CssClass
    {
        public string name { get; set; }
        public List<CssProperty> properties { get; set; }

        public CssClass()
        {
            this.properties = new List<CssProperty>();
        }

        public void AddProperties(List<CssProperty> newProperties)
        {
            for (int i = 0; i < newProperties.Count; i += 1)
            {
                newProperties[i] = this.CheckForDuplicates(newProperties[i]);
            }
            this.properties.AddRange(newProperties);
        }

        private CssProperty CheckForDuplicates(CssProperty newProperty)
        {
            foreach (CssProperty property in this.properties)
            {
                if (newProperty.name == property.name && !newProperty.content.Contains("/* DUPLICATE PROPERTY */"))
                {
                    newProperty.content += " /* DUPLICATE PROPERTY */";
                }
            }
            return newProperty;
        }
    }
}
