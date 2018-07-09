using System.Linq;
using System.Text;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Emulation.Items;

namespace Roton.Emulation.Core.Impl
{
    public class Parser : IParser
    {
        private readonly IEngine _engine;

        public Parser(IEngine engine)
        {
            _engine = engine;
        }

        public int Search(int index, string term)
        {
            var result = -1;
            var termBytes = term.ToBytes();
            var actor = _engine.Actors[index];
            var offset = new Executable {Instruction = 0};

            while (offset.Instruction < actor.Length)
            {
                var oldOffset = offset.Instruction;
                var termOffset = 0;
                bool success;

                while (true)
                {
                    ReadByte(index, offset);
                    if (termBytes[termOffset].ToUpperCase() != _engine.State.OopByte.ToUpperCase())
                    {
                        success = false;
                        break;
                    }

                    termOffset++;
                    if (termOffset >= termBytes.Length)
                    {
                        success = true;
                        break;
                    }
                }

                if (success)
                {
                    ReadByte(index, offset);
                    _engine.State.OopByte = _engine.State.OopByte.ToUpperCase();
                    if (!((_engine.State.OopByte >= 0x41 && _engine.State.OopByte <= 0x5A) || _engine.State.OopByte == 0x5F))
                    {
                        result = oldOffset;
                        break;
                    }
                }

                oldOffset++;
                offset.Instruction = oldOffset;
            }

            return result;
        }

        public int GetNumber(IOopContext context) => ReadNumber(context.Index, context);

        public int ReadByte(int index, IExecutable instructionSource)
        {
            var actor = _engine.Actors[index];
            var value = 0;

            if (instructionSource.Instruction < 0 || instructionSource.Instruction >= actor.Length)
            {
                _engine.State.OopByte = 0;
            }
            else
            {
                value = actor.Code[instructionSource.Instruction];
                _engine.State.OopByte = value;
                instructionSource.Instruction++;
            }

            return value;
        }

        public string ReadLine(int index, IExecutable instructionSource)
        {
            var result = new StringBuilder();
            ReadByte(index, instructionSource);
            while (_engine.State.OopByte != 0x00 && _engine.State.OopByte != 0x0D)
            {
                result.Append(_engine.State.OopByte.ToChar());
                ReadByte(index, instructionSource);
            }

            return result.ToString();
        }

        public int ReadNumber(int index, IExecutable instructionSource)
        {
            var result = new StringBuilder();
            var success = false;

            while (ReadByte(index, instructionSource) == 0x20)
            {
            }

            _engine.State.OopByte = _engine.State.OopByte.ToUpperCase();
            while (_engine.State.OopByte >= 0x30 && _engine.State.OopByte <= 0x39)
            {
                success = true;
                result.Append(_engine.State.OopByte.ToChar());
                ReadByte(index, instructionSource);
            }

            if (instructionSource.Instruction > 0)
            {
                instructionSource.Instruction--;
            }

            if (!success)
            {
                _engine.State.OopNumber = -1;
            }
            else
            {
                int resultInt;
                int.TryParse(result.ToString(), out resultInt);
                _engine.State.OopNumber = resultInt;
            }

            return _engine.State.OopNumber;
        }

        public string ReadWord(int index, IExecutable instructionSource)
        {
            var result = new StringBuilder();

            while (true)
            {
                ReadByte(index, instructionSource);
                if (_engine.State.OopByte != 0x20)
                {
                    break;
                }
            }

            _engine.State.OopByte = _engine.State.OopByte.ToUpperCase();

            if (!(_engine.State.OopByte >= 0x30 && _engine.State.OopByte <= 0x39))
            {
                while ((_engine.State.OopByte >= 0x41 && _engine.State.OopByte <= 0x5A) ||
                       (_engine.State.OopByte >= 0x30 && _engine.State.OopByte <= 0x39) || (_engine.State.OopByte == 0x3A) ||
                       (_engine.State.OopByte == 0x5F))
                {
                    result.Append(_engine.State.OopByte.ToChar());
                    ReadByte(index, instructionSource);
                    _engine.State.OopByte = _engine.State.OopByte.ToUpperCase();
                }
            }

            if (instructionSource.Instruction > 0)
            {
                instructionSource.Instruction--;
            }

            _engine.State.OopWord = result.ToString();
            return _engine.State.OopWord;
        }

        public bool? GetCondition(IOopContext oopContext)
        {
            var name = ReadWord(oopContext.Index, oopContext);
            var condition = _engine.ConditionList.Get(name);
            return condition?.Execute(oopContext) ?? _engine.Flags.Contains(name);
        }

        public IXyPair GetDirection(IOopContext oopContext)
        {
            var name = ReadWord(oopContext.Index, oopContext);
            var direction = _engine.DirectionList.Get(name);
            return direction?.Execute(oopContext);
        }

        public IItem GetItem(IOopContext oopContext)
        {
            var name = ReadWord(oopContext.Index, oopContext);
            var item = _engine.ItemList.Get(name);
            return item;
        }

        public ITile GetKind(IOopContext oopContext)
        {
            var word = ReadWord(oopContext.Index, oopContext);
            var result = new Tile(0, 0);
            var success = false;

            for (var i = 1; i < 8; i++)
            {
                if (_engine.Colors[i].ToUpperInvariant() != word)
                    continue;

                result.Color = i + 8;
                word = ReadWord(oopContext.Index, oopContext);
                break;
            }

            foreach (var element in _engine.ElementList.Where(e => e != null))
            {
                if (new string(element.Name.ToUpperInvariant().Where(c => c >= 0x41 && c <= 0x5A).ToArray()) != word)
                    continue;

                success = true;
                result.Id = element.Id;
                break;
            }

            return success ? result : null;
        }

        public bool GetTarget(ISearchContext context)
        {
            context.SearchIndex++;
            var target = _engine.TargetList.Get(context.SearchTarget) ?? _engine.TargetList.GetDefault();
            return target.Execute(context);
        }
    }
}