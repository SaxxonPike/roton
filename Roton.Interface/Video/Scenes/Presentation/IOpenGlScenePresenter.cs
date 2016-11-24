using OpenTK;
using OpenTK.Platform;
using Roton.Interface.Video.Scenes.Composition;

namespace Roton.Interface.Video.Scenes.Presentation
{
    /// <summary>
    /// The OpenGL rendering interface.
    /// </summary>
    public interface IOpenGlScenePresenter
    {
        /// <summary>
        /// Initializes the OpenGL renderer.
        /// </summary>
        void Init();

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
