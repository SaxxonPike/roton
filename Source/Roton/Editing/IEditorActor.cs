using Roton.Emulation.Data;

namespace Roton.Editing
{
    public interface IEditorActor : IActor
    {
        int Index { get; }
    }
}