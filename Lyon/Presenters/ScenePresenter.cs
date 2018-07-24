using System;
using DotSDL.Graphics;
using Roton.Interface.Video;
using Roton.Interface.Video.Scenes;

namespace Lyon.Presenters
{
    /// <summary>
    /// Basic SDL renderer.
    /// </summary>
    public class ScenePresenter : IScenePresenter
    {
        private readonly Func<IBitmapSceneComposer> _getBitmapSceneComposer;
        private readonly Func<IScenePresenterWindow> _getWindow;

        public ScenePresenter(Func<IBitmapSceneComposer> getBitmapSceneComposer, Func<IScenePresenterWindow> getWindow)
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
