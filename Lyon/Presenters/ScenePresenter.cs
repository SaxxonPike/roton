using System;
using DotSDL.Graphics;
using Lyon.App;
using Roton.Composers.Video.Scenes;

namespace Lyon.Presenters
{
    /// <summary>
    /// Basic SDL renderer.
    /// </summary>
    public class ScenePresenter : IScenePresenter
    {
        private readonly Func<IBitmapSceneComposer> _getBitmapSceneComposer;
        private readonly Func<IWindow> _getWindow;

        public ScenePresenter(Func<IBitmapSceneComposer> getBitmapSceneComposer, Func<IWindow> getWindow)
        {
            _getBitmapSceneComposer = getBitmapSceneComposer;
            _getWindow = getWindow;
        }

        /// <summary>
        /// Initializes the SDL renderer.
        /// </summary>
        private void Init()
        {
        }

        /// <summary>
        /// Renders the scene.
        /// </summary>
        public IDirectAccessBitmap Render()
        {
            var composer = _getBitmapSceneComposer();
            return composer?.DirectAccessBitmap;
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateViewport()
        {
        }
    }
}
