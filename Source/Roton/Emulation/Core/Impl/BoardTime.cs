using System;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

/// <inheritdoc />
[Context(Context.Original)]
[Context(Context.Super)]
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
        _boardTimeHundredthsSec += config.Engine.MasterClockNumerator * 100f / config.Engine.MasterClockDenominator;
    }
}