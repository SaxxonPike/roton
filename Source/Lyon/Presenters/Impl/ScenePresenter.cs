using Roton;
using Roton.Composers.Video.Scenes;
using Roton.Infrastructure.Impl;

namespace Lyon.Presenters.Impl;

/// <inheritdoc />
/// <summary>
/// Basic SDL renderer.
/// </summary>
[Context(Context.Startup)]
// ReSharper disable once UnusedMember.Global
public sealed class ScenePresenter(ISceneComposer sceneComposer) : IScenePresenter
{
    /// <summary>
    /// Renders the scene.
    /// </summary>
    public IBitmap Render() => 
        sceneComposer.Bitmap;

    /// <summary>
    /// 
    /// </summary>
    public void UpdateViewport()
    {
    }
}