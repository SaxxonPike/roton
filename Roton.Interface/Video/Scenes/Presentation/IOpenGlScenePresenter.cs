using OpenTK;
using Roton.Interface.Video.Scenes.Composition;

namespace Roton.Interface.Video.Scenes.Presentation
{
    /// <summary>
    /// The OpenGL rendering interface.
    /// </summary>
    public interface IOpenGlScenePresenter
    {
        /// <summary>
        /// The <see cref="GLControl" /> that the renderer should use.
        /// </summary>
        GLControl FormControl { get; set; }

        /// <summary>
        /// The height (in characters) of the terminal.
        /// </summary>
        double TerminalHeight { get; set; }

        /// <summary>
        /// The width (in characters) of the terminal.
        /// </summary>
        double TerminalWidth { get; set; } 

        /// <summary>
        /// Initializes the OpenGL renderer.
        /// </summary>
        void Init();

        /// <summary>
        /// Renders the scene.
        /// </summary>
        /// <param name="composer">The composer to pull bitmap data from.</param>
        void Render(IBitmapSceneComposer composer);

        /// <summary>
        /// Updates the viewport of the renderer. This should be called when
        /// the size of the renderer changes.
        /// </summary>
        void UpdateViewport();
    }
}
