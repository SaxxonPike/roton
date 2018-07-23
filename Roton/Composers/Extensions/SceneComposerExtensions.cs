using Roton.Composers.Video.Scenes;
using Roton.Emulation.Data.Impl;

namespace Roton.Composers.Extensions
{
    public static class SceneComposerExtensions
    {
        public static void Clear(this ISceneComposer composer)
        {
            for (var y = 0; y < composer.Rows; y++)
            {
                for (var x = 0; x < composer.Columns; x++)
                {
                    composer.Plot(x, y, new AnsiChar());
                }
            }
        }
    }
}
