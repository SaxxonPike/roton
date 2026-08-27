using System.Collections.Generic;

namespace Roton.Emulation.Data.Impl;

public sealed class ScrollState(IState state) : IScrollState
{
    public string? Title { get; set; }
    public bool IsHelp { get; set; }
    public int Index { get; set; }
    public string? Label { get; set; }

    public bool Cancelled
    {
        get => state.CancelScroll;
        set => state.CancelScroll = value;
    }
}