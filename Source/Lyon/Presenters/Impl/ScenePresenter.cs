using Roton;
using Roton.Composers.Video.Scenes;
using Roton.Infrastructure;

namespace Lyon.Presenters.Impl;

/// <inheritdoc />
[Context(Context.Original)]
[Context(Context.Super)]
// ReSharper disable once UnusedMember.Global
public sealed class ScenePresenter(ISceneComposer sceneComposer) : IScenePresenter
{
    /// <inheritdoc />
    public Bitmap? Render() => 
        sceneComposer.GetBitmap(true);
}