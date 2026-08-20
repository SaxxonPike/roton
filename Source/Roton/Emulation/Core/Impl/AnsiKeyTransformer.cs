using System;
using System.Collections.Generic;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Core.Impl;
// Source: http://www.lagmonster.org/docs/DOS7/v-ansi-keys.html

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class AnsiKeyTransformer : IAnsiKeyTransformer
{
    private class AnsiKeyMap(byte[] natural, byte[] shift, byte[] ctrl, byte[] alt)
    {
        public byte[] Natural { get; } = natural;
        public byte[] Shift { get; } = shift ?? natural;
        public byte[] Ctrl { get; } = ctrl ?? natural;
        public byte[] Alt { get; } = alt ?? natural;
    }

    private static readonly IDictionary<AnsiKey, AnsiKeyMap> Map = new Dictionary<AnsiKey, AnsiKeyMap>
    {
        {AnsiKey.None, new AnsiKeyMap([], [], [], [])},
        {AnsiKey.A, new AnsiKeyMap([97], [65], [1], [0, 30])},
        {AnsiKey.B, new AnsiKeyMap([98], [66], [2], [0, 48])},
        {AnsiKey.C, new AnsiKeyMap([99], [67], [3], [0, 46])},
        {AnsiKey.D, new AnsiKeyMap([100], [68], [4], [0, 32])},
        {AnsiKey.E, new AnsiKeyMap([101], [69], [5], [0, 18])},
        {AnsiKey.F, new AnsiKeyMap([102], [70], [6], [0, 33])},
        {AnsiKey.G, new AnsiKeyMap([103], [71], [7], [0, 34])},
        {AnsiKey.H, new AnsiKeyMap([104], [72], [8], [0, 35])},
        {AnsiKey.I, new AnsiKeyMap([105], [73], [9], [0, 23])},
        {AnsiKey.J, new AnsiKeyMap([106], [74], [10], [0, 36])},
        {AnsiKey.K, new AnsiKeyMap([107], [75], [11], [0, 37])},
        {AnsiKey.L, new AnsiKeyMap([108], [76], [12], [0, 38])},
        {AnsiKey.M, new AnsiKeyMap([109], [77], [13], [0, 50])},
        {AnsiKey.N, new AnsiKeyMap([110], [78], [14], [0, 49])},
        {AnsiKey.O, new AnsiKeyMap([111], [79], [15], [0, 24])},
        {AnsiKey.P, new AnsiKeyMap([112], [80], [16], [0, 25])},
        {AnsiKey.Q, new AnsiKeyMap([113], [81], [17], [0, 16])},
        {AnsiKey.R, new AnsiKeyMap([114], [82], [18], [0, 19])},
        {AnsiKey.S, new AnsiKeyMap([115], [83], [19], [0, 31])},
        {AnsiKey.T, new AnsiKeyMap([116], [84], [20], [0, 20])},
        {AnsiKey.U, new AnsiKeyMap([117], [85], [21], [0, 22])},
        {AnsiKey.V, new AnsiKeyMap([118], [86], [22], [0, 47])},
        {AnsiKey.W, new AnsiKeyMap([119], [87], [23], [0, 17])},
        {AnsiKey.X, new AnsiKeyMap([120], [88], [24], [0, 45])},
        {AnsiKey.Y, new AnsiKeyMap([121], [89], [25], [0, 21])},
        {AnsiKey.Z, new AnsiKeyMap([122], [90], [26], [0, 44])},
        {AnsiKey.D0, new AnsiKeyMap([48], [41], [], [0, 129])},
        {AnsiKey.D1, new AnsiKeyMap([49], [33], [], [0, 120])},
        {AnsiKey.D2, new AnsiKeyMap([50], [64], [], [0, 121])},
        {AnsiKey.D3, new AnsiKeyMap([51], [35], [], [0, 122])},
        {AnsiKey.D4, new AnsiKeyMap([52], [36], [], [0, 123])},
        {AnsiKey.D5, new AnsiKeyMap([53], [37], [], [0, 124])},
        {AnsiKey.D6, new AnsiKeyMap([54], [94], [], [0, 125])},
        {AnsiKey.D7, new AnsiKeyMap([55], [38], [], [0, 126])},
        {AnsiKey.D8, new AnsiKeyMap([56], [42], [], [0, 127])},
        {AnsiKey.D9, new AnsiKeyMap([57], [40], [], [0, 128])},
        {AnsiKey.Apostophe, new AnsiKeyMap([39], [34], [], [0, 40])},
        {AnsiKey.Comma, new AnsiKeyMap([44], [60], [], [0, 51])},
        {AnsiKey.Minus, new AnsiKeyMap([45], [95], [31], [0, 130])},
        {AnsiKey.Period, new AnsiKeyMap([46], [62], [], [0, 52])},
        {AnsiKey.Slash, new AnsiKeyMap([47], [63], [], [0, 53])},
        {AnsiKey.Semicolon, new AnsiKeyMap([59], [58], [], [0, 39])},
        {AnsiKey.Equals, new AnsiKeyMap([61], [43], [], [0, 131])},
        {AnsiKey.LeftSquare, new AnsiKeyMap([91], [123], [27], [0, 26])},
        {AnsiKey.Backslash, new AnsiKeyMap([92], [124], [28], [0, 43])},
        {AnsiKey.RightSquare, new AnsiKeyMap([93], [125], [29], [0, 27])},
        {AnsiKey.Grave, new AnsiKeyMap([96], [126], [], [0, 41])},
        {AnsiKey.F1, new AnsiKeyMap([0, 59], [0, 84], [0, 94], [0, 104])},
        {AnsiKey.F2, new AnsiKeyMap([0, 60], [0, 85], [0, 95], [0, 105])},
        {AnsiKey.F3, new AnsiKeyMap([0, 61], [0, 86], [0, 96], [0, 106])},
        {AnsiKey.F4, new AnsiKeyMap([0, 62], [0, 87], [0, 97], [0, 107])},
        {AnsiKey.F5, new AnsiKeyMap([0, 63], [0, 88], [0, 98], [0, 108])},
        {AnsiKey.F6, new AnsiKeyMap([0, 64], [0, 89], [0, 99], [0, 109])},
        {AnsiKey.F7, new AnsiKeyMap([0, 65], [0, 90], [0, 100], [0, 110])},
        {AnsiKey.F8, new AnsiKeyMap([0, 66], [0, 91], [0, 101], [0, 111])},
        {AnsiKey.F9, new AnsiKeyMap([0, 67], [0, 92], [0, 102], [0, 112])},
        {AnsiKey.F10, new AnsiKeyMap([0, 68], [0, 93], [0, 103], [0, 113])},
        {AnsiKey.F11, new AnsiKeyMap([0, 133], [0, 135], [0, 137], [0, 139])},
        {AnsiKey.F12, new AnsiKeyMap([0, 134], [0, 136], [0, 138], [0, 140])},
        {AnsiKey.Num1, new AnsiKeyMap([0, 79], [49], [0, 117], [])},
        {AnsiKey.Num2, new AnsiKeyMap([0, 80], [50], [0, 145], [])},
        {AnsiKey.Num3, new AnsiKeyMap([0, 81], [51], [0, 118], [])},
        {AnsiKey.Num4, new AnsiKeyMap([0, 75], [52], [0, 115], [])},
        {AnsiKey.Num5, new AnsiKeyMap([0, 76], [53], [0, 143], [])},
        {AnsiKey.Num6, new AnsiKeyMap([0, 77], [54], [0, 116], [])},
        {AnsiKey.Num7, new AnsiKeyMap([0, 71], [55], [0, 119], [])},
        {AnsiKey.Num8, new AnsiKeyMap([0, 72], [56], [0, 141], [])},
        {AnsiKey.Num9, new AnsiKeyMap([0, 73], [57], [0, 132], [])},
        {AnsiKey.NumPeriod, new AnsiKeyMap([0, 83], [46], [0, 147], [])},
        {AnsiKey.Num0, new AnsiKeyMap([0, 82], [48], [0, 146], [])},
        {AnsiKey.NumEnter, new AnsiKeyMap([13], [], [10], [0, 166])},
        {AnsiKey.NumSlash, new AnsiKeyMap([47], [47], [0, 142], [0, 74])},
        {AnsiKey.NumStar, new AnsiKeyMap([42], [0, 144], [0, 78], [])},
        {AnsiKey.NumMinus, new AnsiKeyMap([45], [45], [0, 149], [0, 164])},
        {AnsiKey.NumPlus, new AnsiKeyMap([43], [43], [0, 150], [0, 55])},
        {AnsiKey.Insert, new AnsiKeyMap([224, 82], [224, 82], [224, 146], [224, 162])},
        {AnsiKey.Delete, new AnsiKeyMap([224, 83], [224, 83], [224, 147], [224, 163])},
        {AnsiKey.Home, new AnsiKeyMap([224, 71], [224, 71], [224, 119], [224, 151])},
        {AnsiKey.End, new AnsiKeyMap([224, 79], [224, 79], [224, 117], [224, 159])},
        {AnsiKey.PageUp, new AnsiKeyMap([224, 73], [224, 73], [224, 132], [224, 153])},
        {AnsiKey.PageDown, new AnsiKeyMap([224, 81], [224, 81], [224, 118], [224, 161])},
        {AnsiKey.Up, new AnsiKeyMap([224, 72], [224, 72], [224, 141], [224, 152])},
        {AnsiKey.Left, new AnsiKeyMap([224, 75], [224, 75], [224, 115], [224, 155])},
        {AnsiKey.Right, new AnsiKeyMap([224, 77], [224, 77], [224, 116], [224, 157])},
        {AnsiKey.Down, new AnsiKeyMap([224, 80], [224, 80], [224, 145], [224, 154])},
        {AnsiKey.Escape, new AnsiKeyMap([27], [27], [27], [])},
        {AnsiKey.Backspace, new AnsiKeyMap([8], [8], [127], [])},
        {AnsiKey.Enter, new AnsiKeyMap([13], [], [10], [0, 28])},
        {AnsiKey.Tab, new AnsiKeyMap([9], [0, 15], [0, 148], [0, 165])},
        {AnsiKey.Space, new AnsiKeyMap([32], [32], [32], [32])},
        {AnsiKey.PrintScreen, new AnsiKeyMap([], [], [0, 114], [])},
        {AnsiKey.Pause, new AnsiKeyMap([], [], [0, 0], [])}
    };

    public ReadOnlySpan<byte> GetBytes(IKeyPress keyPress)
    {
        var map = Map.TryGetValue(keyPress.Key, out var value) ? value : Map[AnsiKey.None];
        return keyPress.Mod.HasFlag(KeyMod.Shift) ? map.Shift :
            keyPress.Mod.HasFlag(KeyMod.Control) ? map.Ctrl :
            keyPress.Mod.HasFlag(KeyMod.Alt) ? map.Alt :
            map.Natural;
    }
}