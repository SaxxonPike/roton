﻿using Roton.Emulation.Core;

namespace Lyon.App
{
    public interface ILauncher
    {
        void Launch(IEngine context);
    }
}