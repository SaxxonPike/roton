using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;

namespace Roton.Interface.Video.Scenes.Composition
{
    public class CallbackSceneComposer : SceneComposer
    {
        private readonly Action<int, int, AnsiChar> _callback;

        public CallbackSceneComposer(int rows, int columns, Action<int, int, AnsiChar> callback) : base(rows, columns)
        {
            _callback = callback;
        }

        protected override void OnGlyphUpdated(int index, AnsiChar ac)
        {
            base.OnGlyphUpdated(index, ac);
            _callback(index%Columns, index/Columns, ac);
        }
    }
}
