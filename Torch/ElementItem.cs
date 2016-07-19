using Roton;

namespace Torch
{
    public class ElementItem
    {
        public ElementItem(Element element)
        {
            Element = element;
        }

        public Element Element { get; }

        public int Index => Element.Index;

        public override string ToString()
        {
            var name = Element.ToString();
            if (string.IsNullOrWhiteSpace(name))
            {
                name = "(Element " + Element.Index + ")";
            }
            return name;
        }
    }
}