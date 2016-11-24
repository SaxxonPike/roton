using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Interface.Video
{
    public interface IOpenGlScenePresenterWindow
    {
        void MakeCurrent();
        void SwapBuffers();
        int Width { get; }
        int Height { get; }
    }
}
