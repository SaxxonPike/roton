using System.Linq;
using System.Text;
using Roton.Core;
using Roton.Emulation.Commands;
using Roton.Emulation.Items;
using Roton.Emulation.Mapping;
using Roton.Emulation.Targets;

namespace Roton.Emulation.Execution
{
    public class Parser : IParser
    {
        private readonly IActors _actors;
        private readonly IState _state;
        private readonly IConditions _conditions;
        private readonly IDirections _directions;
        private readonly IFlags _flags;
        private readonly IParser _parser;
        private readonly IElements _elements;
        private readonly IItems _items;
        private readonly IColors _colors;
        private readonly ITargets _targets;

        public Parser(IActors actors, IState state, IConditions conditions, IDirections directions, IFlags flags,
            IParser parser, IElements elements, IItems items, IColors colors, ITargets targets)
        {
            _actors = actors;
            _state = state;
            _conditions = conditions;
            _directions = directions;
            _flags = flags;
            _parser = parser;
            _elements = elements;
            _items = items;
            _colors = colors;
            _targets = targets;
        }

        public int Search(int index, string term)
        {
            var result = -1;
            var termBytes = term.ToBytes();
            var actor = _actors[index];
            var offset = new Executable {Instruction = 0};

            while (offset.Instruction < actor.Length)
            {
                var oldOffset = offset.Instruction;
                var termOffset = 0;
                bool success;

                while (true)
                {
                    ReadByte(index, offset);
                    if (termBytes[termOffset].ToUpperCase() != _state.OopByte.ToUpperCase())
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
                    _state.OopByte = _state.OopByte.ToUpperCase();
                    if (!((_state.OopByte >= 0x41 && _state.OopByte <= 0x5A) || _state.OopByte == 0x5F))
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

        public int ReadByte(int index, IExecutable instructionSource)
        {
            var actor = _actors[index];
            var value = 0;

            if (instructionSource.Instruction < 0 || instructionSource.Instruction >= actor.Length)
            {
                _state.OopByte = 0;
            }
            else
            {
                value = actor.Code[instructionSource.Instruction];
                _state.OopByte = value;
                instructionSource.Instruction++;
            }

            return value;
        }

        public string ReadLine(int index, IExecutable instructionSource)
        {
            var result = new StringBuilder();
            ReadByte(index, instructionSource);
            while (_state.OopByte != 0x00 && _state.OopByte != 0x0D)
            {
                result.Append(_state.OopByte.ToChar());
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

            _state.OopByte = _state.OopByte.ToUpperCase();
            while (_state.OopByte >= 0x30 && _state.OopByte <= 0x39)
            {
                success = true;
                result.Append(_state.OopByte.ToChar());
                ReadByte(index, instructionSource);
            }

            if (instructionSource.Instruction > 0)
            {
                instructionSource.Instruction--;
            }

            if (!success)
            {
                _state.OopNumber = -1;
            }
            else
            {
                int resultInt;
                int.TryParse(result.ToString(), out resultInt);
                _state.OopNumber = resultInt;
            }

            return _state.OopNumber;
        }

        public string ReadWord(int index, IExecutable instructionSource)
        {
            var result = new StringBuilder();

            while (true)
            {
                ReadByte(index, instructionSource);
                if (_state.OopByte != 0x20)
                {
                    break;
                }
            }

            _state.OopByte = _state.OopByte.ToUpperCase();

            if (!(_state.OopByte >= 0x30 && _state.OopByte <= 0x39))
            {
                while ((_state.OopByte >= 0x41 && _state.OopByte <= 0x5A) ||
                       (_state.OopByte >= 0x30 && _state.OopByte <= 0x39) || (_state.OopByte == 0x3A) ||
                       (_state.OopByte == 0x5F))
                {
                    result.Append(_state.OopByte.ToChar());
                    ReadByte(index, instructionSource);
                    _state.OopByte = _state.OopByte.ToUpperCase();
                }
            }

            if (instructionSource.Instruction > 0)
            {
                instructionSource.Instruction--;
            }

            _state.OopWord = result.ToString();
            return _state.OopWord;
        }

        public bool? GetCondition(IOopContext oopContext)
        {
            var name = ReadWord(oopContext.Index, oopContext);
            var condition = _conditions.Get(name);
            return condition?.Execute(oopContext) ?? _flags.Contains(name);
        }

        public IXyPair GetDirection(IOopContext oopContext)
        {
            var name = _parser.ReadWord(oopContext.Index, oopContext);
            var direction = _directions.Get(name);
            return direction?.Execute(oopContext);
        }

        public IOopItem GetItem(IOopContext oopContext)
        {
            var name = _parser.ReadWord(oopContext.Index, oopContext);
            var item = _items.Get(name);
            return item?.Execute(oopContext);
        }

        public ITile GetKind(IOopContext oopContext)
        {
            var word = _parser.ReadWord(oopContext.Index, oopContext);
            var result = new Tile(0, 0);
            var success = false;

            for (var i = 1; i < 8; i++)
            {
                if (_colors[i].ToUpperInvariant() != word)
                    continue;

                result.Color = i + 8;
                word = _parser.ReadWord(oopContext.Index, oopContext);
                break;
            }

            foreach (var element in _elements.Where(e => e != null))
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
            var target = _targets.Get(context.SearchTarget) ?? _targets.GetDefault();
            return target.Execute(context);
        }
    }

    public interface IParser
    {
        bool? GetCondition(IOopContext oopContext);
        IXyPair GetDirection(IOopContext oopContext);
        IOopItem GetItem(IOopContext oopContext);
        ITile GetKind(IOopContext oopContext);
        bool GetTarget(ISearchContext context);
        int ReadByte(int index, IExecutable instructionSource);
        string ReadLine(int index, IExecutable instructionSource);
        int ReadNumber(int index, IExecutable instructionSource);
        string ReadWord(int index, IExecutable instructionSource);
        int Search(int index, string term);
    }
}