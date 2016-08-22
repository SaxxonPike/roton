using Roton.Core;
using Roton.Interface.Video.Scenes;
using Roton.Interface.Video.Scenes.Composition;

namespace Roton.Interface.Extensions
{
    public static class SceneComposerExtensions
    {
        public static void Clear(this ISceneComposer composer)
        {
            for (var y = 0; y < composer.Rows; y++)
            {
                for (var x = 0; x < composer.Columns; x++)
                {
                    composer.SetChar(x, y, new AnsiChar());
                }
            }
        }
    }
}
