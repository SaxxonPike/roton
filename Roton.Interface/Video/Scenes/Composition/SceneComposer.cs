using System.Text;
using Roton.Core;
using Roton.Interface.Events;

namespace Roton.Interface.Video.Scenes.Composition
{
    public class SceneComposer : ISceneComposer
    {
        public event SetSizeEventHandler AfterSetSize;

        protected AnsiChar[] Chars;
        private readonly AnsiChar _blankCharacter;
        private readonly Encoding _codePage437 = Encoding.GetEncoding(437);

        public SceneComposer(int columns, int rows)
        {
            _blankCharacter = new AnsiChar();
            SetSize(columns, rows, false);
        }

        public virtual void SetSize(int width, int height, bool wide)
        {
            Rows = height;
            Columns = width;

            var charTotal = Columns * Rows;
            Chars = new AnsiChar[charTotal];

            AfterSetSize?.Invoke(this, new SetSizeEventArgs { Width = width, Height = height, Wide = wide });
        }

        public virtual void Write(int x, int y, string value, int color)
        {
            foreach (var b in _codePage437.GetBytes(value ?? string.Empty))
            {
                if (y >= Rows)
                    break;

                while (x >= Columns)
                {
                    x -= Columns;
                    y++;
                }

                Plot(x++, y, new AnsiChar(b, color));
            }
        }

        protected virtual void OnGlyphUpdated(int index, AnsiChar ac)
        {
        }

        public virtual AnsiChar Read(int x, int y)
        {
            return IsOutOfBounds(x, y)
                ? _blankCharacter
                : Chars[GetBufferOffset(x, y)];
        }

        public virtual void Update(int x, int y)
        {
            var index = GetBufferOffset(x, y);
            OnGlyphUpdated(index, Chars[index]);
        }

        public void Clear()
        {
            for (var y = 0; y < Rows; y++)
                for (var x = 0; x < Columns; x++)
                    Plot(x, y, _blankCharacter);
        }

        public virtual void Plot(int x, int y, AnsiChar ac)
        {
            if (IsOutOfBounds(x, y))
                return;

            var index = GetBufferOffset(x, y);
            Chars[index] = ac;
            OnGlyphUpdated(index, ac);
        }

        private bool IsOutOfBounds(int x, int y)
        {
            return (x < 0 || x >= Columns || y < 0 || y >= Rows);
        }

        protected int GetBufferOffset(int x, int y)
        {
            return x + y * Columns;
        }

        public int Rows { get; protected set; }

        public int Columns { get; protected set; }
    }
}
