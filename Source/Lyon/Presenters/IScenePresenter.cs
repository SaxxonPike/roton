using Roton.Composers.Video.Scenes;

namespace Lyon.Presenters
{
    /// <summary>
    /// The rendering interface.
    /// </summary>
    public interface IScenePresenter
    {
        /// <summary>
        /// Renders the scene.
        /// </summary>
        IBitmap Render();

        /// <summary>
        /// Updates the viewport of the renderer. This should be called when
        /// the size of the renderer changes.
        /// </summary>
        void UpdateViewport();
    }
}
