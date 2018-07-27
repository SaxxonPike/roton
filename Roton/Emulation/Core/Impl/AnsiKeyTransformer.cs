using System.Collections.Generic;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Core.Impl
{
    // Source: http://www.lagmonster.org/docs/DOS7/v-ansi-keys.html

    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public sealed class AnsiKeyTransformer : IAnsiKeyTransformer
    {
        private class AnsiKeyMap
        {
            public byte[] Natural { get; }
            public byte[] Shift { get; }
            public byte[] Ctrl { get; }
            public byte[] Alt { get; }

            public AnsiKeyMap(byte[] natural, byte[] shift, byte[] ctrl, byte[] alt)
            {
                Natural = natural;
                Shift = shift ?? natural;
                Ctrl = ctrl ?? natural;
                Alt = alt ?? natural;
            }
        }

        private static readonly IDictionary<AnsiKey, AnsiKeyMap> Map = new Dictionary<AnsiKey, AnsiKeyMap>
        {
            {AnsiKey.None, new AnsiKeyMap(new byte[] {}, new byte[] {}, new byte[] {}, new byte[] {})},
            {AnsiKey.A, new AnsiKeyMap(new byte[] {97}, new byte[] {65}, new byte[] {1}, new byte[] {0, 30})},
            {AnsiKey.B, new AnsiKeyMap(new byte[] {98}, new byte[] {66}, new byte[] {2}, new byte[] {0, 48})},
            {AnsiKey.C, new AnsiKeyMap(new byte[] {99}, new byte[] {67}, new byte[] {3}, new byte[] {0, 46})},
            {AnsiKey.D, new AnsiKeyMap(new byte[] {100}, new byte[] {68}, new byte[] {4}, new byte[] {0, 32})},
            {AnsiKey.E, new AnsiKeyMap(new byte[] {101}, new byte[] {69}, new byte[] {5}, new byte[] {0, 18})},
            {AnsiKey.F, new AnsiKeyMap(new byte[] {102}, new byte[] {70}, new byte[] {6}, new byte[] {0, 33})},
            {AnsiKey.G, new AnsiKeyMap(new byte[] {103}, new byte[] {71}, new byte[] {7}, new byte[] {0, 34})},
            {AnsiKey.H, new AnsiKeyMap(new byte[] {104}, new byte[] {72}, new byte[] {8}, new byte[] {0, 35})},
            {AnsiKey.I, new AnsiKeyMap(new byte[] {105}, new byte[] {73}, new byte[] {9}, new byte[] {0, 23})},
            {AnsiKey.J, new AnsiKeyMap(new byte[] {106}, new byte[] {74}, new byte[] {10}, new byte[] {0, 36})},
            {AnsiKey.K, new AnsiKeyMap(new byte[] {107}, new byte[] {75}, new byte[] {11}, new byte[] {0, 37})},
            {AnsiKey.L, new AnsiKeyMap(new byte[] {108}, new byte[] {76}, new byte[] {12}, new byte[] {0, 38})},
            {AnsiKey.M, new AnsiKeyMap(new byte[] {109}, new byte[] {77}, new byte[] {13}, new byte[] {0, 50})},
            {AnsiKey.N, new AnsiKeyMap(new byte[] {110}, new byte[] {78}, new byte[] {14}, new byte[] {0, 49})},
            {AnsiKey.O, new AnsiKeyMap(new byte[] {111}, new byte[] {79}, new byte[] {15}, new byte[] {0, 24})},
            {AnsiKey.P, new AnsiKeyMap(new byte[] {112}, new byte[] {80}, new byte[] {16}, new byte[] {0, 25})},
            {AnsiKey.Q, new AnsiKeyMap(new byte[] {113}, new byte[] {81}, new byte[] {17}, new byte[] {0, 16})},
            {AnsiKey.R, new AnsiKeyMap(new byte[] {114}, new byte[] {82}, new byte[] {18}, new byte[] {0, 19})},
            {AnsiKey.S, new AnsiKeyMap(new byte[] {115}, new byte[] {83}, new byte[] {19}, new byte[] {0, 31})},
            {AnsiKey.T, new AnsiKeyMap(new byte[] {116}, new byte[] {84}, new byte[] {20}, new byte[] {0, 20})},
            {AnsiKey.U, new AnsiKeyMap(new byte[] {117}, new byte[] {85}, new byte[] {21}, new byte[] {0, 22})},
            {AnsiKey.V, new AnsiKeyMap(new byte[] {118}, new byte[] {86}, new byte[] {22}, new byte[] {0, 47})},
            {AnsiKey.W, new AnsiKeyMap(new byte[] {119}, new byte[] {87}, new byte[] {23}, new byte[] {0, 17})},
            {AnsiKey.X, new AnsiKeyMap(new byte[] {120}, new byte[] {88}, new byte[] {24}, new byte[] {0, 45})},
            {AnsiKey.Y, new AnsiKeyMap(new byte[] {121}, new byte[] {89}, new byte[] {25}, new byte[] {0, 21})},
            {AnsiKey.Z, new AnsiKeyMap(new byte[] {122}, new byte[] {90}, new byte[] {26}, new byte[] {0, 44})},
            {AnsiKey.D0, new AnsiKeyMap(new byte[] {48}, new byte[] {41}, new byte[] {}, new byte[] {0, 129})},
            {AnsiKey.D1, new AnsiKeyMap(new byte[] {49}, new byte[] {33}, new byte[] {}, new byte[] {0, 120})},
            {AnsiKey.D2, new AnsiKeyMap(new byte[] {50}, new byte[] {64}, new byte[] {}, new byte[] {0, 121})},
            {AnsiKey.D3, new AnsiKeyMap(new byte[] {51}, new byte[] {35}, new byte[] {}, new byte[] {0, 122})},
            {AnsiKey.D4, new AnsiKeyMap(new byte[] {52}, new byte[] {36}, new byte[] {}, new byte[] {0, 123})},
            {AnsiKey.D5, new AnsiKeyMap(new byte[] {53}, new byte[] {37}, new byte[] {}, new byte[] {0, 124})},
            {AnsiKey.D6, new AnsiKeyMap(new byte[] {54}, new byte[] {94}, new byte[] {}, new byte[] {0, 125})},
            {AnsiKey.D7, new AnsiKeyMap(new byte[] {55}, new byte[] {38}, new byte[] {}, new byte[] {0, 126})},
            {AnsiKey.D8, new AnsiKeyMap(new byte[] {56}, new byte[] {42}, new byte[] {}, new byte[] {0, 127})},
            {AnsiKey.D9, new AnsiKeyMap(new byte[] {57}, new byte[] {40}, new byte[] {}, new byte[] {0, 128})},
            {AnsiKey.Apostophe, new AnsiKeyMap(new byte[] {39}, new byte[] {34}, new byte[] {}, new byte[] {0, 40})},
            {AnsiKey.Comma, new AnsiKeyMap(new byte[] {44}, new byte[] {60}, new byte[] {}, new byte[] {0, 51})},
            {AnsiKey.Minus, new AnsiKeyMap(new byte[] {45}, new byte[] {95}, new byte[] {31}, new byte[] {0, 130})},
            {AnsiKey.Period, new AnsiKeyMap(new byte[] {46}, new byte[] {62}, new byte[] {}, new byte[] {0, 52})},
            {AnsiKey.Slash, new AnsiKeyMap(new byte[] {47}, new byte[] {63}, new byte[] {}, new byte[] {0, 53})},
            {AnsiKey.Semicolon, new AnsiKeyMap(new byte[] {59}, new byte[] {58}, new byte[] {}, new byte[] {0, 39})},
            {AnsiKey.Equals, new AnsiKeyMap(new byte[] {61}, new byte[] {43}, new byte[] {}, new byte[] {0, 131})},
            {AnsiKey.LeftSquare, new AnsiKeyMap(new byte[] {91}, new byte[] {123}, new byte[] {27}, new byte[] {0, 26})},
            {AnsiKey.Backslash, new AnsiKeyMap(new byte[] {92}, new byte[] {124}, new byte[] {28}, new byte[] {0, 43})},
            {AnsiKey.RightSquare, new AnsiKeyMap(new byte[] {93}, new byte[] {125}, new byte[] {29}, new byte[] {0, 27})},
            {AnsiKey.Grave, new AnsiKeyMap(new byte[] {96}, new byte[] {126}, new byte[] {}, new byte[] {0, 41})},
            {AnsiKey.F1, new AnsiKeyMap(new byte[] {0, 59}, new byte[] {0, 84}, new byte[] {0, 94}, new byte[] {0, 104})},
            {AnsiKey.F2, new AnsiKeyMap(new byte[] {0, 60}, new byte[] {0, 85}, new byte[] {0, 95}, new byte[] {0, 105})},
            {AnsiKey.F3, new AnsiKeyMap(new byte[] {0, 61}, new byte[] {0, 86}, new byte[] {0, 96}, new byte[] {0, 106})},
            {AnsiKey.F4, new AnsiKeyMap(new byte[] {0, 62}, new byte[] {0, 87}, new byte[] {0, 97}, new byte[] {0, 107})},
            {AnsiKey.F5, new AnsiKeyMap(new byte[] {0, 63}, new byte[] {0, 88}, new byte[] {0, 98}, new byte[] {0, 108})},
            {AnsiKey.F6, new AnsiKeyMap(new byte[] {0, 64}, new byte[] {0, 89}, new byte[] {0, 99}, new byte[] {0, 109})},
            {AnsiKey.F7, new AnsiKeyMap(new byte[] {0, 65}, new byte[] {0, 90}, new byte[] {0, 100}, new byte[] {0, 110})},
            {AnsiKey.F8, new AnsiKeyMap(new byte[] {0, 66}, new byte[] {0, 91}, new byte[] {0, 101}, new byte[] {0, 111})},
            {AnsiKey.F9, new AnsiKeyMap(new byte[] {0, 67}, new byte[] {0, 92}, new byte[] {0, 102}, new byte[] {0, 112})},
            {AnsiKey.F10, new AnsiKeyMap(new byte[] {0, 68}, new byte[] {0, 93}, new byte[] {0, 103}, new byte[] {0, 113})},
            {AnsiKey.F11, new AnsiKeyMap(new byte[] {0, 133}, new byte[] {0, 135}, new byte[] {0, 137}, new byte[] {0, 139})},
            {AnsiKey.F12, new AnsiKeyMap(new byte[] {0, 134}, new byte[] {0, 136}, new byte[] {0, 138}, new byte[] {0, 140})},
            {AnsiKey.Num1, new AnsiKeyMap(new byte[] {0, 79}, new byte[] {49}, new byte[] {0, 117}, new byte[] {})},
            {AnsiKey.Num2, new AnsiKeyMap(new byte[] {0, 80}, new byte[] {50}, new byte[] {0, 145}, new byte[] {})},
            {AnsiKey.Num3, new AnsiKeyMap(new byte[] {0, 81}, new byte[] {51}, new byte[] {0, 118}, new byte[] {})},
            {AnsiKey.Num4, new AnsiKeyMap(new byte[] {0, 75}, new byte[] {52}, new byte[] {0, 115}, new byte[] {})},
            {AnsiKey.Num5, new AnsiKeyMap(new byte[] {0, 76}, new byte[] {53}, new byte[] {0, 143}, new byte[] {})},
            {AnsiKey.Num6, new AnsiKeyMap(new byte[] {0, 77}, new byte[] {54}, new byte[] {0, 116}, new byte[] {})},
            {AnsiKey.Num7, new AnsiKeyMap(new byte[] {0, 71}, new byte[] {55}, new byte[] {0, 119}, new byte[] {})},
            {AnsiKey.Num8, new AnsiKeyMap(new byte[] {0, 72}, new byte[] {56}, new byte[] {0, 141}, new byte[] {})},
            {AnsiKey.Num9, new AnsiKeyMap(new byte[] {0, 73}, new byte[] {57}, new byte[] {0, 132}, new byte[] {})},
            {AnsiKey.NumPeriod, new AnsiKeyMap(new byte[] {0, 83}, new byte[] {46}, new byte[] {0, 147}, new byte[] {})},
            {AnsiKey.Num0, new AnsiKeyMap(new byte[] {0, 82}, new byte[] {48}, new byte[] {0, 146}, new byte[] {})},
            {AnsiKey.NumEnter, new AnsiKeyMap(new byte[] {13}, new byte[] {}, new byte[] {10}, new byte[] {0, 166})},
            {AnsiKey.NumSlash, new AnsiKeyMap(new byte[] {47}, new byte[] {47}, new byte[] {0, 142}, new byte[] {0, 74})},
            {AnsiKey.NumStar, new AnsiKeyMap(new byte[] {42}, new byte[] {0, 144}, new byte[] {0, 78}, new byte[] {})},
            {AnsiKey.NumMinus, new AnsiKeyMap(new byte[] {45}, new byte[] {45}, new byte[] {0, 149}, new byte[] {0, 164})},
            {AnsiKey.NumPlus, new AnsiKeyMap(new byte[] {43}, new byte[] {43}, new byte[] {0, 150}, new byte[] {0, 55})},
            {AnsiKey.Insert, new AnsiKeyMap(new byte[] {224, 82}, new byte[] {224, 82}, new byte[] {224, 146}, new byte[] {224, 162})},
            {AnsiKey.Delete, new AnsiKeyMap(new byte[] {224, 83}, new byte[] {224, 83}, new byte[] {224, 147}, new byte[] {224, 163})},
            {AnsiKey.Home, new AnsiKeyMap(new byte[] {224, 71}, new byte[] {224, 71}, new byte[] {224, 119}, new byte[] {224, 151})},
            {AnsiKey.End, new AnsiKeyMap(new byte[] {224, 79}, new byte[] {224, 79}, new byte[] {224, 117}, new byte[] {224, 159})},
            {AnsiKey.PageUp, new AnsiKeyMap(new byte[] {224, 73}, new byte[] {224, 73}, new byte[] {224, 132}, new byte[] {224, 153})},
            {AnsiKey.PageDown, new AnsiKeyMap(new byte[] {224, 81}, new byte[] {224, 81}, new byte[] {224, 118}, new byte[] {224, 161})},
            {AnsiKey.Up, new AnsiKeyMap(new byte[] {224, 72}, new byte[] {224, 72}, new byte[] {224, 141}, new byte[] {224, 152})},
            {AnsiKey.Left, new AnsiKeyMap(new byte[] {224, 75}, new byte[] {224, 75}, new byte[] {224, 115}, new byte[] {224, 155})},
            {AnsiKey.Right, new AnsiKeyMap(new byte[] {224, 77}, new byte[] {224, 77}, new byte[] {224, 116}, new byte[] {224, 157})},
            {AnsiKey.Down, new AnsiKeyMap(new byte[] {224, 80}, new byte[] {224, 80}, new byte[] {224, 145}, new byte[] {224, 154})},
            {AnsiKey.Escape, new AnsiKeyMap(new byte[] {27}, new byte[] {27}, new byte[] {27}, new byte[] {})},
            {AnsiKey.Backspace, new AnsiKeyMap(new byte[] {8}, new byte[] {8}, new byte[] {127}, new byte[] {})},
            {AnsiKey.Enter, new AnsiKeyMap(new byte[] {13}, new byte[] {}, new byte[] {10}, new byte[] {0, 28})},
            {AnsiKey.Tab, new AnsiKeyMap(new byte[] {9}, new byte[] {0, 15}, new byte[] {0, 148}, new byte[] {0, 165})},
            {AnsiKey.Space, new AnsiKeyMap(new byte[] {32}, new byte[] {32}, new byte[] {32}, new byte[] {32})},
            {AnsiKey.PrintScreen, new AnsiKeyMap(new byte[] {}, new byte[] {}, new byte[] {0, 114}, new byte[] {})},
            {AnsiKey.Pause, new AnsiKeyMap(new byte[] {}, new byte[] {}, new byte[] {0, 0}, new byte[] {})}
        };

        public IEnumerable<byte> GetBytes(IKeyPress keyPress)
        {
            var map = Map.ContainsKey(keyPress.Key) ? Map[keyPress.Key] : Map[AnsiKey.None];
            return keyPress.Shift ? map.Shift :
                keyPress.Control ? map.Ctrl :
                keyPress.Alt ? map.Alt :
                map.Natural;
        }
    }
}