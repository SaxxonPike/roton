using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IScrollRenderer
{
    void RenderContent(ScrollState st);
    void Open();
    void Close();
}