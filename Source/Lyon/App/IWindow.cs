namespace Lyon.App;

public interface IWindow
{
    void Start(float updateRate);
    void Close();
}