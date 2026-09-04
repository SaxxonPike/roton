using Roton.Emulation.Data;

namespace Roton.Emulation.Kinds;

public interface IKind
{
    void Initialize(IElement element);
}

public interface IKindList
{
    void InitializeAll();
}