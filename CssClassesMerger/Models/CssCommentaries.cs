using System.Collections;
using System.Collections.Generic;

namespace CssClassesMerger
{
    public class CssCommentaries : ICollection<string>
    {
        private ICollection<string> commentary = new List<string>();

        public int Count => this.commentary.Count;
        public bool IsReadOnly => false;

        public void Add(CssCommentaries commentary)
        {
            foreach (string line in commentary)
            {
                this.commentary.Add(line);
            }
        }

        public void Add(string item)
        {
            this.commentary.Add(item);
        }

        public void Clear()
        {
            this.commentary.Clear();
        }

        public bool Contains(string item)
        {
            return this.commentary.Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            this.commentary.CopyTo(array, arrayIndex);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return this.commentary.GetEnumerator();
        }

        public bool Remove(string item)
        {
            return this.commentary.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.commentary.GetEnumerator();
        }
    }
}
