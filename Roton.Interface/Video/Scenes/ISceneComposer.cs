using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;

namespace Roton.Interface.Video.Scenes
{
    public interface ISceneComposer
    {
        AnsiChar GetChar(int x, int y);
        void SetChar(int x, int y, AnsiChar ac);
        void SetSize(int width, int height, int scaleX, int scaleY);
    }
}
