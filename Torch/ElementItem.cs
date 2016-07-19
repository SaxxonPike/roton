using Roton;
using Roton.Core;

namespace Torch
{
    public class ElementItem
    {
        public ElementItem(IElement element)
        {
            Element = element;
        }

        public IElement Element { get; }

        public int Index => Element.Id;

        public override string ToString()
        {
            var name = Element.ToString();
            if (string.IsNullOrWhiteSpace(name))
            {
                name = "(Element " + Element.Id + ")";
            }
            return name;
        }
    }
}