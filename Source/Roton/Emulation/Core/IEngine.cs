using System;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IEngine
{
    void StepOnce();
    void Delay(int msec);
    int ResetBoardTimeHsec();
}