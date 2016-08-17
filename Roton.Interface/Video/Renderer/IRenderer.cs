using OpenTK;

namespace Roton.Interface.Video.Renderer
{
    /// <summary>
    /// The OpenGL rendering interface.
    /// </summary>
    public interface IRenderer
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
        /// <param name="gameBitmap">A reference to the frame that should be rendered.</param>
        void Render(IFastBitmap gameBitmap);

        /// <summary>
        /// Updates the viewport of the renderer. This should be called when
        /// the size of the renderer changes.
        /// </summary>
        void UpdateViewport();
    }
}
