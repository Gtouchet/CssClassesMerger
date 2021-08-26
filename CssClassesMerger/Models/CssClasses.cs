using System.Collections;
using System.Collections.Generic;

namespace CssClassesMerger
{
    public class CssClass
    {
        public string Name { get; set; }
        public CssProperties Properties { get; private set; } = new CssProperties();
        public CssCommentaries Commentaries { get; private set; } = new CssCommentaries();
    }

    public class CssClasses : ICollection<CssClass>
    {
        private ICollection<CssClass> classes = new List<CssClass>();

        public int Count => this.classes.Count;
        public bool IsReadOnly => false;

        public void Add(CssClass newClass)
        {
            foreach (CssClass _class in this.classes)
            {
                if (_class.Name == newClass.Name)
                {
                    _class.Properties.Add(newClass.Properties);
                    return;
                }
            }
            this.classes.Add(newClass);
        }

        public void Add(CssClasses newClasses)
        {
            foreach (CssClass newClass in newClasses)
            {
                this.Add(newClass);
            }
        }

        public void Clear()
        {
            this.classes.Clear();
        }

        public bool Contains(CssClass item)
        {
            return this.classes.Contains(item);
        }

        public void CopyTo(CssClass[] array, int arrayIndex)
        {
            this.classes.CopyTo(array, arrayIndex);
        }

        public IEnumerator<CssClass> GetEnumerator()
        {
            return this.classes.GetEnumerator();
        }

        public bool Remove(CssClass item)
        {
            return this.classes.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.classes.GetEnumerator();
        }
    }
}
