namespace Lyon.App;

public interface IWindow
{
    void SetSize(int width, int height);
    void Start(float updateRate);
    void Close();
}