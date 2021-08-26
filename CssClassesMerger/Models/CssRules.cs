using System.Collections;
using System.Collections.Generic;

namespace CssClassesMerger
{
    public class CssRule
    {
        public string Name { get; set; }
        public CssClasses Classes { get; private set; } = new CssClasses();
        public CssCommentaries Commentaries { get; private set; } = new CssCommentaries();
    }

    public class CssRules : ICollection<CssRule>
    {
        private ICollection<CssRule> rules = new List<CssRule>();

        public int Count => this.rules.Count;
        public bool IsReadOnly => false;

        public void Add(CssRule newRule)
        {
            foreach (CssRule rule in this.rules)
            {
                if (rule.Name == newRule.Name)
                {
                    rule.Classes.Add(newRule.Classes);
                    return;
                }
            }
            this.rules.Add(newRule);
        }

        public void Clear()
        {
            this.rules.Clear();
        }

        public bool Contains(CssRule item)
        {
            return this.rules.Contains(item);
        }

        public void CopyTo(CssRule[] array, int arrayIndex)
        {
            this.rules.CopyTo(array, arrayIndex);
        }

        public IEnumerator<CssRule> GetEnumerator()
        {
            return this.rules.GetEnumerator();
        }

        public bool Remove(CssRule item)
        {
            return this.rules.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.rules.GetEnumerator();
        }
    }
}
