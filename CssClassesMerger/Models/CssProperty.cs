using System.Collections.Generic;

namespace CssClassesMerger
{
    class CssProperty
    {
        public string name { get; set; }
        public string content { get; set; }
        public List<string> commentary { get; set; }

        public CssProperty()
        {
            this.commentary = new List<string>();
        }
    }
}
