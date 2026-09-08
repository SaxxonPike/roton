namespace Roton.Editors;

public interface IBoardAccessor
{
    void EnsureBoard(int index);
}

public interface IWorldEditorFactory
{
    IWorldEditor Create(Context context);
}