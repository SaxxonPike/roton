using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Roton.Core;
using Roton.Interface.Extensions;
using Roton.Interface.Input;
using Roton.Interface.Video.Scenes.Composition;

namespace Roton.Interface.Video.Terminals
{
    public class Vt100Terminal : ITerminal
    {
        // TODO: This is WIP. -saxxon

        private readonly Stream _outputStream;
        private readonly Stream _inputStream;
        private readonly KeysBuffer _keysBuffer;
        private ISceneComposer _sceneComposer;
        private readonly Encoding _encoding = Encoding.GetEncoding(437);

        private bool _currentCharacterSet;
        private int _currentForegroundColor;
        private int _currentBackgroundColor;
        private bool _currentBlink;
        private bool _currentBright;
        private int _currentX;
        private int _currentY;
        private int _terminalWidth;
        private int _terminalHeight;

        public Vt100Terminal(Stream outputStream, Stream inputStream)
        {
            _outputStream = outputStream;
            _inputStream = inputStream;
            _keysBuffer = new KeysBuffer();
            _sceneComposer = new CallbackSceneComposer(80, 24, OutputCharacterCallback);
            _currentForegroundColor = 0x07;
            _currentCharacterSet = false;
            _currentX = 0;
            _currentY = 0;
            _terminalWidth = 80;
            _terminalHeight = 25;
        }

        public IKeyboard Keyboard => _keysBuffer;

        private static int TranslateColor(int color)
        {
            return ((color & 1) << 2) |
                (color & 2) |
                ((color & 4) >> 2) |
                (color & 8);
        }

        private void OutputCharacterCallback(int x, int y, AnsiChar ac)
        {
            // Do we need to write a position control code?

            if (x != _currentX || y != _currentY)
            {
                WriteEscapeCode($"[{y};{x}H");
                _currentX = x;
                _currentY = y;
            }

            // Do we need to write any attribute control codes?

            var attributes = new List<string>();
            var desiredForegroundColor = ac.Color & 0x7;
            var desiredBackgroundColor = (ac.Color >> 4) & 0x7;
            var desiredBlink = (ac.Color & 0x80) != 0;
            var desiredBright = (ac.Color & 0x08) != 0;
            var desiredCharacterSet = (ac.Char & 0x80) != 0;

            if (desiredForegroundColor != _currentForegroundColor)
            {
                attributes.Add($"3{TranslateColor(desiredForegroundColor)}");
                _currentForegroundColor = desiredForegroundColor;
            }

            if (desiredBackgroundColor != _currentBackgroundColor)
            {
                attributes.Add($"4{TranslateColor(desiredBackgroundColor)}");
                _currentBackgroundColor = desiredBackgroundColor;
            }

            if (desiredBlink != _currentBlink)
            {
                attributes.Add(desiredBlink ? "5" : "25");
                _currentBlink = desiredBlink;
            }

            if (desiredBright != _currentBright)
            {
                attributes.Add(desiredBright ? "1" : "21");
                _currentBright = desiredBright;
            }

            if (attributes.Any())
                WriteEscapeCode($"[{string.Join(";", attributes)}m");

            // Do we need to write a character set control code?

            if (desiredCharacterSet != _currentCharacterSet)
            {
                WriteEscapeCode(desiredCharacterSet ? "(0" : "(B");
                _currentCharacterSet = desiredCharacterSet;
            }


        }

        public void Clear() => _sceneComposer.Clear();

        public int GetKey()
        {
            return -1;
        }

        public void Plot(int x, int y, AnsiChar ac) => _sceneComposer.SetChar(x, y, ac);

        public AnsiChar Read(int x, int y) => _sceneComposer.GetChar(x, y);

        public void SetSize(int width, int height, bool wide)
        {
            WriteEscapeCode($"[4;{height};{width}");
            _terminalWidth = width;
            _sceneComposer = new CallbackSceneComposer(width, height, OutputCharacterCallback);
        }

        public void Write(int x, int y, string value, int color)
        {
            var output = _encoding.GetBytes(value);
            var ansiChars = output.Select(c => new AnsiChar(c, color));
            foreach (var ac in ansiChars)
            {
                while (x >= _terminalWidth)
                {
                    x -= _terminalWidth;
                    y++;
                }
                Plot(x, y, ac);
            }
        }

        private void WriteEscapeCode(string code)
        {
            // escape character
            _outputStream.WriteByte(27);
            var bytes = _encoding.GetBytes(code);
            _outputStream.Write(bytes, 0, bytes.Length);
        }
    }
}
