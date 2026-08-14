using System;
using Roton.Composers.Video.Scenes;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Lyon.Presenters.Impl;

/// <inheritdoc />
/// <summary>
/// Basic SDL renderer.
/// </summary>
[Context(Context.Startup)]
// ReSharper disable once UnusedMember.Global
public sealed class ScenePresenter(Lazy<ISceneComposer> sceneComposer) : IScenePresenter
{
    /// <summary>
    /// Renders the scene.
    /// </summary>
    public IBitmap Render() => 
        sceneComposer.Value.Bitmap;

    /// <summary>
    /// 
    /// </summary>
    public void UpdateViewport()
    {
    }
}