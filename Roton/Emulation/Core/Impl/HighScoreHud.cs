using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Core.Impl
{
    [Context(Context.Original)]
    [Context(Context.Super)]
    public sealed class HighScoreHud : IHighScoreHud
    {
        public string Show(int x, int y, int width, int height)
        {
            // TODO: implement
            return null;
        }
    }
}