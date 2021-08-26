using System.Collections.Generic;

namespace CssClassesMerger
{
    class CssContent
    {
        public CssClasses Classes { get; private set; } = new CssClasses();
        public CssRules Rules { get; private set; } = new CssRules();
        public CssCommentaries Commentaries { get; private set; } = new CssCommentaries();

        public void AddClass(CssClass newClass)
        {
            this.Classes.Add(newClass);
        }

        public void AddRule(CssRule newRule)
        {
            this.Rules.Add(newRule);
        }

        public void AddCommentary(CssCommentaries newCommentary)
        {
            this.Commentaries.Add(newCommentary);
        }
    }
}
