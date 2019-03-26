using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Emulation.Items;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Core.Impl
{
    [Context(Context.Original)]
    [Context(Context.Super)]
    public sealed class Parser : IParser
    {
        private readonly Lazy<IEngine> _engine;

        public Parser(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        private IEngine Engine
        {
            [DebuggerStepThrough] get => _engine.Value;
        }

        public int Search(int index, int offset, string term)
        {
            var result = -1;
            var termBytes = term.ToBytes();
            var actor = Engine.Actors[index];
            var offs = new Executable {Instruction = offset};

            while (offs.Instruction < actor.Length)
            {
                var oldOffset = offs.Instruction;
                var termOffset = 0;
                bool success;

                while (true)
                {
                    ReadByte(index, offs);
                    if (termBytes[termOffset].ToUpperCase() != Engine.State.OopByte.ToUpperCase())
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
                    ReadByte(index, offs);
                    Engine.State.OopByte = Engine.State.OopByte.ToUpperCase();
                    if (!(Engine.State.OopByte >= 0x41 && Engine.State.OopByte <= 0x5A ||
                          Engine.State.OopByte == 0x5F))
                    {
                        result = oldOffset;
                        break;
                    }
                }

                oldOffset++;
                offs.Instruction = oldOffset;
            }

            return result;
        }

        public int GetNumber(IOopContext context) => ReadNumber(context.Index, context);

        public int ReadByte(int index, IExecutable instructionSource)
        {
            var actor = Engine.Actors[index];
            var value = 0;

            if (instructionSource.Instruction < 0 || instructionSource.Instruction >= actor.Length)
            {
                Engine.State.OopByte = 0;
            }
            else
            {
                value = actor.Code[instructionSource.Instruction];
                Engine.State.OopByte = value;
                instructionSource.Instruction++;
            }

            return value;
        }

        public string ReadLine(int index, IExecutable instructionSource)
        {
            var result = new StringBuilder();
            ReadByte(index, instructionSource);
            while (Engine.State.OopByte != 0x00 && Engine.State.OopByte != 0x0D)
            {
                result.Append(Engine.State.OopByte.ToChar());
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

            Engine.State.OopByte = Engine.State.OopByte.ToUpperCase();
            while (Engine.State.OopByte >= 0x30 && Engine.State.OopByte <= 0x39)
            {
                success = true;
                result.Append(Engine.State.OopByte.ToChar());
                ReadByte(index, instructionSource);
            }

            if (instructionSource.Instruction > 0)
            {
                instructionSource.Instruction--;
            }

            if (!success)
            {
                Engine.State.OopNumber = -1;
            }
            else
            {
                int.TryParse(result.ToString(), out var resultInt);
                Engine.State.OopNumber = resultInt;
            }

            return Engine.State.OopNumber;
        }

        public string ReadWord(int index, IExecutable instructionSource)
        {
            var result = new StringBuilder();

            while (true)
            {
                ReadByte(index, instructionSource);
                if (Engine.State.OopByte != 0x20)
                {
                    break;
                }
            }

            Engine.State.OopByte = Engine.State.OopByte.ToUpperCase();

            if (!(Engine.State.OopByte >= 0x30 && Engine.State.OopByte <= 0x39))
            {
                while (Engine.State.OopByte >= 0x41 && Engine.State.OopByte <= 0x5A ||
                       Engine.State.OopByte >= 0x30 && Engine.State.OopByte <= 0x39 ||
                       Engine.State.OopByte == 0x3A ||
                       Engine.State.OopByte == 0x5F)
                {
                    result.Append(Engine.State.OopByte.ToChar());
                    ReadByte(index, instructionSource);
                    Engine.State.OopByte = Engine.State.OopByte.ToUpperCase();
                }
            }

            if (instructionSource.Instruction > 0)
            {
                instructionSource.Instruction--;
            }

            Engine.State.OopWord = result.ToString();
            return Engine.State.OopWord;
        }

        public bool? GetCondition(IOopContext oopContext)
        {
            var name = ReadWord(oopContext.Index, oopContext);
            var condition = Engine.ConditionList.Get(name);
            return condition?.Execute(oopContext) ?? Engine.World.Flags.Contains(name);
        }

        public IXyPair GetDirection(IOopContext oopContext)
        {
            var name = ReadWord(oopContext.Index, oopContext);
            var direction = Engine.DirectionList.Get(name);
            return direction?.Execute(oopContext);
        }

        public IItem GetItem(IOopContext oopContext)
        {
            var name = ReadWord(oopContext.Index, oopContext);
            var item = Engine.ItemList.Get(name);
            return item;
        }

        public ITile GetKind(IOopContext oopContext)
        {
            var word = ReadWord(oopContext.Index, oopContext);
            var result = new Tile(0, 0);
            var success = false;

            for (var i = 1; i < 8; i++)
            {
                if (Engine.Colors[i].ToUpperInvariant() != word)
                    continue;

                result.Color = i + 8;
                word = ReadWord(oopContext.Index, oopContext);
                break;
            }

            foreach (var element in Engine.ElementList.Where(e => e != null))
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
            var target = Engine.TargetList.Get(context.SearchTarget) ?? Engine.TargetList.Get(string.Empty);
            return target.Execute(context);
        }
    }
}