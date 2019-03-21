using System;
using Lyon.Autofac;
using Roton.Composers.Video.Scenes;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Lyon.Presenters.Impl
{
    /// <inheritdoc />
    /// <summary>
    /// Basic SDL renderer.
    /// </summary>
    [Service]
    // ReSharper disable once UnusedMember.Global
    public sealed class ScenePresenter : IScenePresenter
    {
        private readonly Lazy<ISceneComposer> _sceneComposer;

        public ScenePresenter(Lazy<ISceneComposer> sceneComposer)
        {
            _sceneComposer = sceneComposer;
        }

        /// <summary>
        /// Renders the scene.
        /// </summary>
        public IBitmap Render()
        {
            return _sceneComposer.Value.Bitmap;
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateViewport()
        {
        }
    }
}
