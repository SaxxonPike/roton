namespace Roton.Editing.Impl
{
    public class EditorActors : IEditorActors
    {
        public IEditorActor this[int index] => throw new System.NotImplementedException();

        public int Count { get; }
        public IEditorActor Add()
        {
            throw new System.NotImplementedException();
        }

        public IEditorActor Remove(int index)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }
    }
}