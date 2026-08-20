using Roton.Emulation.Data;

namespace Roton.Emulation.Core.Impl;

public abstract class FadeMatrix(IEngineAccessor engine, int left, int top, int width, int height, int speed)
    : IFadeMatrix
{
    public int Width => width;
    public int Height => height;
    public int Left => left;
    public int Top => top;

    protected IEngine Engine => engine.Instance;

    private readonly Location[] _matrix = new Location[width * height];

    public void Initialize()
    {
        var index = 0;

        for (var x = 0; x < width; x++)
        for (var y = 0; y < height; y++)
            _matrix[index++] = new Location(x, y);
    }

    public void Randomize()
    {
        var count = _matrix.Length;

        for (var i = 0; i < count; i++)
        {
            var targetIndex = Engine.Random.GetNext(_matrix.Length);
            (_matrix[i], _matrix[targetIndex]) =
                (_matrix[targetIndex], _matrix[i]);
        }
    }

    private void FadeWait(int i)
    {
        if (i % speed == 0)
            Engine.WaitForTick();
    }

    public void FadeOut(AnsiChar ac)
    {
        var count = _matrix.Length;

        for (var i = 0; i < count; i++)
        {
            var location = _matrix[i];
            DrawAt(location.X, location.Y, ac);
            FadeWait(i);
        }
    }

    public void FadeIn()
    {
        var count = _matrix.Length;

        for (var i = 0; i < count; i++)
        {
            var location = _matrix[i];
            RedrawAt(location.X, location.Y);
            FadeWait(i);
        }
    }

    protected abstract void DrawAt(int x, int y, AnsiChar ac);

    protected abstract void RedrawAt(int x, int y);
}