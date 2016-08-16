using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;
using Roton.Interface.Video.Glyphs;

namespace Roton.Interface.Video.Scenes.Composers
{
    public class BitmapSceneComposer : ISceneComposer
    {
        private readonly IGlyphComposer _glyphComposer;
        private readonly int _glyphWidth;
        private readonly int _glyphHeight;
        private readonly int _columns;
        private readonly int _rows;
        private readonly int _scaleX;
        private readonly int _scaleY;
        private readonly AnsiChar[] _chars;
        private readonly int _charTotal;

        public BitmapSceneComposer(IGlyphComposer glyphComposer, int glyphWidth, int glyphHeight, int columns, int rows, int scaleX, int scaleY)
        {
            _glyphComposer = glyphComposer;
            _glyphWidth = glyphWidth;
            _glyphHeight = glyphHeight;
            _columns = columns;
            _rows = rows;
            _scaleX = scaleX;
            _scaleY = scaleY;
            _charTotal = columns*rows;
            _chars = new AnsiChar[_charTotal];
        }

        public AnsiChar GetChar(int x, int y)
        {
            var index = x + (y*_columns);
            throw new NotImplementedException();
        }

        public void SetChar(int x, int y, AnsiChar ac)
        {
            throw new NotImplementedException();
        }

        public void SetSize(int width, int height, int scaleX, int scaleY)
        {
            throw new NotImplementedException();
        }
    }
}
