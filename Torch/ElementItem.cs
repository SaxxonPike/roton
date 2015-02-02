using Roton;

namespace Torch
{
    public class ElementItem
    {
        public ElementItem(Element element)
        {
            this.Element = element;
        }

        public Element Element
        {
            get;
            private set;
        }

        public int Index
        {
            get { return Element.Index; }
        }

        public override string ToString()
        {
            string name = Element.ToString();
            if (string.IsNullOrWhiteSpace(name))
            {
                name = "(Element " + Element.Index.ToString() + ")";
            }
            return name;
        }
    }
}
