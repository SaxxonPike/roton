using System;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

/// <inheritdoc />
[Context(Context.Startup)]
internal sealed class BoardTime(
    IConfig config) 
    : IBoardTime
{
    private float _boardTimeHundredthsSec;

    /// <inheritdoc />
    public void Reset()
    {
        _boardTimeHundredthsSec = 0;
    }

    /// <inheritdoc />
    public int Elapse()
    {
        var result = (int)Math.Truncate(_boardTimeHundredthsSec);
        _boardTimeHundredthsSec -= result;
        return result;
    }

    /// <inheritdoc />
    public void Advance()
    {
        _boardTimeHundredthsSec += config.MasterClockNumerator * 100f / config.MasterClockDenominator;
    }
}