using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    abstract internal partial class CoreBase
    {
        virtual internal bool ActorIsLocked(int index)
        {
            return Actors[index].P2 != 0;
        }

        virtual internal void ExecuteCode(int index, ICodeSeekable instructionSource, string name)
        {
            ExecuteCodeContext context = new ExecuteCodeContext(index, instructionSource, name);
        }

        virtual internal void ExecuteCode_Become(ExecuteCodeContext context)
        {
        }

        virtual internal void ExecuteCode_Bind(ExecuteCodeContext context)
        {
        }

        virtual internal void ExecuteCode_Change(ExecuteCodeContext context)
        {
        }

        virtual internal void ExecuteCode_Char(ExecuteCodeContext context)
        {
        }

        virtual internal void ExecuteCode_Clear(ExecuteCodeContext context)
        {
        }

        virtual internal void ExecuteCode_Cycle(ExecuteCodeContext context)
        {
        }

        virtual internal void ExecuteCode_Die(ExecuteCodeContext context)
        {
        }

        virtual internal void ExecuteCode_End(ExecuteCodeContext context)
        {
            context.Finished = true;
            context.Instruction = -1;
        }

        virtual internal void ExecuteCode_EndGame(ExecuteCodeContext context)
        {
            Health = 0;
        }

        virtual internal void ExecuteCode_Give(ExecuteCodeContext context)
        {
        }

        virtual internal void ExecuteCode_Go(ExecuteCodeContext context)
        {
        }

        virtual internal void ExecuteCode_Idle(ExecuteCodeContext context)
        {
            context.Moved = true;
        }

        virtual internal void ExecuteCode_If(ExecuteCodeContext context)
        {
        }

        virtual internal void ExecuteCode_Lock(ExecuteCodeContext context)
        {
            context.Actor.P2 = 1;
        }

        virtual internal void ExecuteCode_Play(ExecuteCodeContext context)
        {
        }

        virtual internal void ExecuteCode_Put(ExecuteCodeContext context)
        {
        }

        virtual internal void ExecuteCode_Restart(ExecuteCodeContext context)
        {
            context.NextLine = false;
            context.Instruction = 0;
        }

        virtual internal void ExecuteCode_Restore(ExecuteCodeContext context)
        {
        }

        virtual internal void ExecuteCode_Send(ExecuteCodeContext context)
        {
        }

        virtual internal void ExecuteCode_Set(ExecuteCodeContext context)
        {
        }

        virtual internal void ExecuteCode_Shoot(ExecuteCodeContext context)
        {
        }

        virtual internal void ExecuteCode_Take(ExecuteCodeContext context)
        {
        }

        virtual internal void ExecuteCode_ThrowStar(ExecuteCodeContext context)
        {
        }

        virtual internal void ExecuteCode_Try(ExecuteCodeContext context)
        {
        }

        virtual internal void ExecuteCode_Unlock(ExecuteCodeContext context)
        {
            context.Actor.P2 = 0;
        }

        virtual internal void ExecuteCode_Walk(ExecuteCodeContext context)
        {
        }

        virtual internal void ExecuteCode_Zap(ExecuteCodeContext context)
        {
        }

        virtual internal bool IsActorTargeted(int sender, CodeSearchInfo info, string target)
        {
            var success = false;
            switch (target.ToUpperInvariant())
            {
                case @"ALL":
                    success = info.Index <= ActorCount;
                    break;
                case @"OTHERS":
                    if (info.Index <= ActorCount)
                    {
                        if (info.Index != sender)
                        {
                            success = true;
                        }
                        else
                        {
                            info.Index++;
                            success = info.Index <= ActorCount;
                        }
                    }
                    break;
                case @"SELF":
                    if (info.Index > 0)
                    {
                        if (info.Index <= sender)
                        {
                            info.Index = sender;
                            success = true;
                        }
                    }
                    break;
                default:
                    while (true)
                    {
                        // todo: targeted labels
                        break;
                    }
                    break;
            }
            return false;
        }

        virtual internal void ReadActorCodeByte(int index, ICodeSeekable instructionSource)
        {
            var actor = Actors[index];
            if (instructionSource.Instruction < 0 || instructionSource.Instruction >= actor.Length)
            {
                OOPByte = 0;
            }
            else
            {
                var heapCode = actor.Heap[actor.Pointer];
                OOPByte = actor.Heap[actor.Pointer][instructionSource.Instruction];
                instructionSource.Instruction++;
            }
        }

        virtual internal string ReadActorCodeLine(int index, ICodeSeekable instructionSource)
        {
            StringBuilder result = new StringBuilder();
            ReadActorCodeByte(index, instructionSource);
            while (OOPByte != 0x00 && OOPByte != 0x0D)
            {
                result.Append(OOPByte.ToChar());
                ReadActorCodeByte(index, instructionSource);
            }
            return result.ToString();
        }

        virtual internal void ReadActorCodeNumber(int index, ICodeSeekable instructionSource)
        {
            StringBuilder result = new StringBuilder();
            var success = false;

            while (true)
            {
                ReadActorCodeByte(index, instructionSource);
                if (OOPByte != 0x20)
                {
                    break;
                }
            }

            OOPByte = OOPByte.ToUpperCase();
            while (OOPByte >= 0x30 && OOPByte <= 0x39)
            {
                success = true;
                result.Append(OOPByte.ToChar());
                ReadActorCodeByte(index, instructionSource);
            }

            if (instructionSource.Instruction > 0)
            {
                instructionSource.Instruction--;
            }

            if (!success)
            {
                OOPNumber = -1;
            }
            else
            {
                int resultInt = -1;
                int.TryParse(result.ToString(), out resultInt);
                OOPNumber = resultInt;
            }
        }

        virtual internal void ReadActorCodeWord(int index, ICodeSeekable instructionSource)
        {
            StringBuilder result = new StringBuilder();

            while (true)
            {
                ReadActorCodeByte(index, instructionSource);
                if (OOPByte != 0x20)
                {
                    break;
                }
            }

            OOPByte = OOPByte.ToUpperCase();

            if (!(OOPByte >= 0x30 && OOPByte <= 0x39))
            {
                while ((OOPByte >= 0x41 && OOPByte <= 0x5A) || (OOPByte >= 0x30 && OOPByte <= 0x39) || (OOPByte == 0x3A) || (OOPByte == 0x5F))
                {
                    result.Append(OOPByte.ToChar());
                    ReadActorCodeByte(index, instructionSource);
                    OOPByte = OOPByte.ToUpperCase();
                }
            }

            if (instructionSource.Instruction > 0)
            {
                instructionSource.Instruction--;
            }

            OOPWord = result.ToString();
        }

        virtual internal int SearchActorCode(int index, string term)
        {
            int result = -1;
            var termBytes = term.ToBytes();
            var actor = Actors[index];
            ByRefInstruction offset = new ByRefInstruction(0);

            while (offset.Instruction < actor.Length)
            {
                int oldOffset = offset.Instruction;
                int termOffset = 0;
                bool success = false;

                while (true)
                {
                    ReadActorCodeByte(index, offset);
                    if (termBytes[termOffset].ToUpperCase() != OOPByte.ToUpperCase())
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
                    ReadActorCodeByte(index, offset);
                    OOPByte = OOPByte.ToUpperCase();
                    if (!((OOPByte >= 0x41 && OOPByte <= 0x5A) || OOPByte == 0x5F))
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

        virtual internal bool SendLabel(int sender, string label, bool force)
        {
            string target = label;
            bool external = false;
            bool success = false;
            int index = 0;
            int offset = 0;

            if (sender < 0)
            {
                external = true;
                sender = -sender;
            }

            var info = new CodeSearchInfoProxy(
                () => { return index; },
                (int value) => { index = value; },
                () => { return target; },
                (string value) => { target = value; },
                () => { return offset; },
                (int value) => { offset = value; }
                );

            while (SendSearch(sender, info, "\x000D:"))
            {
                if (!ActorIsLocked(index) || force || (sender == index && !external))
                {
                    if (sender == index)
                    {
                        success = true;
                    }
                    Actors[index].Instruction = offset;
                }
            }

            return success;
        }

        virtual internal bool SendSearch(int sender, CodeSearchInfo search, string prefix)
        {
            string label = search.Label;
            string target = @"";
            var success = false;
            int split = label.IndexOf(':');

            if (split > 0)
            {
                target = label.Substring(0, split);
                label = label.Substring(split + 1);
                success = IsActorTargeted(sender, search, target);
            }
            else if (search.Index < sender)
            {
                search.Index = sender;
                split = 0;
                success = true;
            }
            while (true)
            {
                if (!success)
                {
                    break;
                }

                if (label.ToUpper() == @"RESTART")
                {
                    search.Offset = 0;
                }
                else
                {
                    search.Offset = SearchActorCode(search.Index, prefix + label);
                    if (search.Offset < 0 && split > 0)
                    {
                        success = IsActorTargeted(sender, search, target);
                        continue;
                    }
                }

                success = (search.Offset >= 0);
                break;
            }
            return success;
        }
    }
}
