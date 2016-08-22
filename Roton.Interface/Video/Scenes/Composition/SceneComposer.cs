using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;

namespace Roton.Interface.Video.Scenes.Composition
{
    public class SceneComposer : ISceneComposer
    {
        protected readonly AnsiChar[] Chars;
        private readonly AnsiChar _blankCharacter;

        public SceneComposer(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;

            var charTotal = Columns * Rows;
            _blankCharacter = new AnsiChar();
            Chars = new AnsiChar[charTotal];
        }

        protected virtual void OnGlyphUpdated(int index, AnsiChar ac)
        {
        }

        public virtual AnsiChar GetChar(int x, int y)
        {
            return IsOutOfBounds(x, y)
                ? _blankCharacter
                : Chars[GetBufferOffset(x, y)];
        }

        public virtual void RefreshChar(int x, int y)
        {
            var index = GetBufferOffset(x, y);
            OnGlyphUpdated(index, Chars[index]);
        }

        public virtual void SetChar(int x, int y, AnsiChar ac)
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

        public int Rows { get; }

        public int Columns { get; }
    }
}
