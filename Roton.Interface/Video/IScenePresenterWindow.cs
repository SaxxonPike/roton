namespace Roton.Interface.Video
{
    public interface IScenePresenterWindow
    {
        void MakeCurrent();
        void SwapBuffers();
        int Width { get; }
        int Height { get; }
    }
}
