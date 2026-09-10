using System;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

/// <inheritdoc />
[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class TimeConverter(IConfig config)
    : ITimeConverter
{
    /// <inheritdoc />
    public int HsecToTicks(int hsec) =>
        Math.Max(1, hsec * (config.Engine.MasterClockDenominator / config.Engine.MasterClockNumerator + 50) / 100);
}