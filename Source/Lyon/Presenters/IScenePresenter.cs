using Roton.Composers.Video.Scenes;

namespace Lyon.Presenters;

/// <summary>
/// The rendering interface.
/// </summary>
public interface IScenePresenter
{
    /// <summary>
    /// Renders the scene.
    /// </summary>
    IBitmap Render();
}