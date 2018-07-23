namespace Lyon.Presenters
{
    public interface IOpenGlScenePresenterWindow
    {
        void MakeCurrent();
        void SwapBuffers();
        int Width { get; }
        int Height { get; }
    }
}
