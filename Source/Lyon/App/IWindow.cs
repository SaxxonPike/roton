namespace Lyon.App;

/// <summary>
/// Represents the game window.
/// </summary>
public interface IWindow
{
    /// <summary>
    /// Opens the game window and starts the game loop.
    /// </summary>
    void Start();
    
    /// <summary>
    /// Stops execution and closes the game window.
    /// </summary>
    void Close();
}