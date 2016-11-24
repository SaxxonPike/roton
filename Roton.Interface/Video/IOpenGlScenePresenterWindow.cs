namespace Roton.Interface.Video
{
    public interface IOpenGlScenePresenterWindow
    {
        void MakeCurrent();
        void SwapBuffers();
        int Width { get; }
        int Height { get; }
    }
}
