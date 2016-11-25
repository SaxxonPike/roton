namespace Roton.Interface.Video.Scenes.Presentation
{
    /// <summary>
    /// The OpenGL rendering interface.
    /// </summary>
    public interface IOpenGlScenePresenter
    {
        /// <summary>
        /// Renders the scene.
        /// </summary>
        void Render();

        /// <summary>
        /// Updates the viewport of the renderer. This should be called when
        /// the size of the renderer changes.
        /// </summary>
        void UpdateViewport();
    }
}
