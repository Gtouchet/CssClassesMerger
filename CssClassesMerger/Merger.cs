using System.Collections.Generic;

namespace CssClassesMerger
{
    class Merger
    {
        public SortedDictionary<string, List<string>> classes { get; set; }
        public List<string> rawLines { get; set; }

        public Merger()
        {
            this.classes = new SortedDictionary<string, List<string>>();
            this.rawLines = new List<string>();
        }
    }
}
