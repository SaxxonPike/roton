﻿using System;
using Roton.Composers.Video.Scenes;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Lyon.Presenters.Impl
{
    /// <summary>
    /// Basic SDL renderer.
    /// </summary>
    [Context(Context.Startup)]
    public sealed class ScenePresenter : IScenePresenter
    {
        private readonly Lazy<ISceneComposer> _sceneComposer;

        public ScenePresenter(Lazy<ISceneComposer> sceneComposer)
        {
            _sceneComposer = sceneComposer;
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
