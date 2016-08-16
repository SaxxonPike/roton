﻿using Roton.Core;

namespace Roton.Interface.Video.Scenes
{
    public interface ISceneComposer
    {
        AnsiChar GetChar(int x, int y);
        void SetChar(int x, int y, AnsiChar ac);
    }
}
