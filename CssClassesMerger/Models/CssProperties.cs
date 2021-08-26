using System.Collections;
using System.Collections.Generic;

namespace CssClassesMerger
{
    public class CssProperty
    {
        public string Name { get; set; }
        public string Content { get; set; }
    }

    public class CssProperties : ICollection<CssProperty>
    {
        private ICollection<CssProperty> properties = new List<CssProperty>();

        public int Count => this.properties.Count;
        public bool IsReadOnly => false;

        public void Add(CssProperty newProperty)
        {
            if (this.IsDuplicateProperty(newProperty))
            {
                newProperty.Content += " /* DUPLICATE PROPERTY */";
            }
            this.properties.Add(newProperty);
        }

        public void Add(CssProperties newProperties)
        {
            foreach (CssProperty newProperty in newProperties)
            {
                this.Add(newProperty);
            }
        }

        public void Clear()
        {
            this.properties.Clear();
        }

        public bool Contains(CssProperty item)
        {
            return this.properties.Contains(item);
        }

        public void CopyTo(CssProperty[] array, int arrayIndex)
        {
            this.properties.CopyTo(array, arrayIndex);
        }

        public IEnumerator<CssProperty> GetEnumerator()
        {
            return this.properties.GetEnumerator();
        }

        public bool Remove(CssProperty item)
        {
            return this.properties.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.properties.GetEnumerator();
        }

        private bool IsDuplicateProperty(CssProperty newProperty)
        {
            foreach (CssProperty property in this.properties)
            {
                if (newProperty.Name == property.Name && !newProperty.Content.Contains("DUPLICATE PROPERTY"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
