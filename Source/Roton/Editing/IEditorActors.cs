namespace Roton.Editing
{
    public interface IEditorActors
    {
        IEditorActor this[int index] { get; }
        int Count { get; }
        IEditorActor Add();
        IEditorActor Remove(int index);
        void Clear();
    }
}