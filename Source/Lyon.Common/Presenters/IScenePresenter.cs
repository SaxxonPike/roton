using Roton.Composers.Video.Scenes;

namespace Lyon.Presenters.Presenters;

/// <summary>
/// The rendering interface.
/// </summary>
public interface IScenePresenter
{
    /// <summary>
    /// Renders the scene. If the scene has not been updated since the last render, returns null.
    /// </summary>
    Bitmap? Render();
}