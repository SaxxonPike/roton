using OpenTK;

namespace Roton.Interface.Video.Renderer
{
    /// <summary>
    /// A base renderer class. This class must be inherited.
    /// </summary>
    public abstract class Renderer : IRenderer
    {
        private bool _glReady;

        /// <summary>
        /// The <see cref="OpenTK.GLControl" /> that the renderer should use.
        /// </summary>
        public GLControl FormControl { get; set; }

        /// <summary>
        /// The height (in characters) of the terminal.
        /// </summary>
        public double TerminalHeight { get; set; }

        /// <summary>
        /// The width (in characters) of the terminal.
        /// </summary>
        public double TerminalWidth { get; set; }

        /// <summary>
        /// Initializes the OpenGL renderer.
        /// </summary>
        public void Init()
        {
            // If a GLControl is assigned, it must be set as the current context
            // before anything happens.
            FormControl?.MakeCurrent();

            InitImplementation();
            _glReady = true;
        }

        /// <summary>
        /// Initializes the OpenGL renderer implementation. This method must be
        /// implemented.
        /// </summary>
        protected abstract void InitImplementation();

        /// <summary>
        /// Renders the scene. If <see cref="FormControl" /> is assigned, its
        /// buffer will be swapped on each render call.
        /// </summary>
        /// <param name="gameBitmap">A reference to the frame that should be rendered.</param>
        public virtual void Render(IFastBitmap gameBitmap)
        {
            if (!_glReady) return;

            RenderImplementation(gameBitmap);
            FormControl?.SwapBuffers();
        }

        /// <summary>
        /// Renders the scene. This method must be implemented.
        /// </summary>
        protected abstract void RenderImplementation(IFastBitmap gameBitmap);

        /// <summary>
        /// 
        /// </summary>
        public void UpdateViewport()
        {
            if(!_glReady) return;

            UpdateViewportImplementation();
        }

        /// <summary>
        /// Updates the viewport of the renderer.
        /// </summary>
        protected abstract void UpdateViewportImplementation();
    }
}
