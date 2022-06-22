using System.Collections.Generic;

namespace Roton.Emulation.Data;

public interface ITextContent : IList<string>
{
    void SetText(IEnumerable<string> content);
}