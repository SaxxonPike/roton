using System.Text;
using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Execution
{
    internal abstract partial class CoreBase
    {
        public abstract IGrammar Grammar { get; }

        internal virtual bool ActorIsLocked(int index)
        {
            return Actors[index].P2 != 0;
        }

        internal virtual bool BroadcastLabel(int sender, string label, bool force)
        {
            var target = label;
            var external = false;
            var success = false;
            var index = 0;
            var offset = 0;

            if (sender < 0)
            {
                external = true;
                sender = -sender;
            }

            var info = new CodeSearchInfoProxy(
                () => index,
                value => { index = value; },
                () => target,
                value => { target = value; },
                () => offset,
                value => { offset = value; }
                );

            while (ExecuteLabel(sender, info, "\x000D:"))
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

        internal virtual void ClearFlag(string flag)
        {
            var index = Flags.IndexOf(flag);
            if (index >= 0)
            {
                Flags[index] = string.Empty;
            }
        }

        internal virtual int ColorMatch(ITile tile)
        {
            var element = Elements[tile.Id];

            if (element.Color < 0xF0)
                return (element.Color & 7);
            if (element.Color == 0xFE)
                return ((tile.Color >> 4) & 0x0F) + 8;
            return tile.Color & 0x0F;
        }

        internal virtual bool ReadCondition(OopContext context)
        {
            var actor = context.Actor;
            ReadActorCodeWord(context.Index, context);
            var word = OopWord;
            switch (word)
            {
                case "ALLIGNED":
                    return Player.Location.X == actor.Location.X || Player.Location.Y == actor.Location.Y;
                case "ENERGIZED":
                    return WorldData.EnergyCycles > 0;
                case "NOT":
                    return !ReadCondition(context);
                default:
                    return Flags.Contains(word);
            }
        }

        internal virtual IXyPair ReadDirection(OopContext context)
        {
            var actor = context.Actor;
            ReadActorCodeWord(context.Index, context);
            var word = OopWord;
            switch (word)
            {
                case "CW":
                    return ReadDirection(context)?.Clockwise();
                case "CCW":
                    return ReadDirection(context)?.CounterClockwise();
                case "E":
                case "EAST":
                    return Vector.East;
                case "FLOW":
                    return actor.Vector.Clone();
                case "I":
                case "IDLE":
                    return Vector.Idle;
                case "N":
                case "NORTH":
                    return Vector.North;
                case "OPP":
                    return ReadDirection(context)?.Opposite();
                case "RND":
                    return Rnd();
                case "RNDNE":
                    return RandomNumber(2) == 0
                        ? Vector.North
                        : Vector.East;
                case "RNDNS":
                    return RandomNumber(2) == 0
                        ? Vector.North
                        : Vector.South;
                case "RNDP":
                    var rndpDirection = ReadDirection(context);
                    if (rndpDirection != null)
                    {
                        return RandomNumber(2) == 0
                            ? rndpDirection.Clockwise()
                            : rndpDirection.CounterClockwise();
                    }
                    return null;
                case "SEEK":
                    return Seek(actor.Location);
                case "S":
                case "SOUTH":
                    return Vector.South;
                case "W":
                case "WEST":
                    return Vector.West;
                default:
                    return null;
            }
        }

        internal virtual ITile ReadKind(OopContext context)
        {
            var success = false;
            var result = new Tile(0, 0);

            while (!success)
            {
                var valid = false;
                ReadActorCodeWord(context.Index, context);
                var word = OopWord;

                if (result.Color == 0)
                {
                    for (var i = 1; i <= 8; i++)
                    {
                        if (Colors[i].ToUpperInvariant() == word)
                        {
                            result.Color = i + 8;
                            valid = true;
                            break;
                        }
                    }
                }

                if (!valid)
                {
                    foreach (var element in Elements)
                    {
                        if (string.IsNullOrEmpty(element.Name) || element.Name.ToUpperInvariant() != word)
                            continue;

                        result.Id = element.Id;
                        success = true;
                        valid = true;
                        break;
                    }
                }

                if (valid)
                    continue;

                break;
            }

            return success ? result : null;
        }

        internal virtual void ExecuteCode(int index, ICodeInstruction instructionSource, string name)
        {
            var context = new OopContext(index, instructionSource, name, this);
        }

        internal virtual void ExecuteCode_Become(OopContext context)
        {
        }

        internal virtual void ExecuteCode_Bind(OopContext context)
        {
        }

        internal virtual void ExecuteCode_Change(OopContext context)
        {
        }

        internal virtual void ExecuteCode_Char(OopContext context)
        {
            ReadActorCodeNumber(context.Index, context);
            if (OopNumber > 0x00 && OopNumber <= 0xFF)
            {
                context.Actor.P1 = OopNumber;
                UpdateBoard(context.Actor.Location);
            }
        }

        internal virtual void ExecuteCode_Clear(OopContext context)
        {
        }

        internal virtual void ExecuteCode_Cycle(OopContext context)
        {
            ReadActorCodeNumber(context.Index, context);
            if (OopNumber > 0)
            {
                context.Actor.Cycle = OopNumber;
            }
        }

        internal virtual void ExecuteCode_Die(OopContext context)
        {
            context.Died = true;
            context.DeathTile.SetTo(Elements.EmptyId, 0x0F);
        }

        internal virtual void ExecuteCode_End(OopContext context)
        {
            context.Finished = true;
            context.Instruction = -1;
        }

        internal virtual void ExecuteCode_EndGame(OopContext context)
        {
            Health = 0;
        }

        internal virtual void ExecuteCode_Give(OopContext context)
        {
        }

        internal virtual void ExecuteCode_Go(OopContext context)
        {
        }

        internal virtual void ExecuteCode_Idle(OopContext context)
        {
            context.Moved = true;
        }

        internal virtual void ExecuteCode_If(OopContext context)
        {
        }

        internal virtual void ExecuteCode_Lock(OopContext context)
        {
            context.Actor.P2 = 1;
        }

        internal virtual void ExecuteCode_Play(OopContext context)
        {
        }

        internal virtual void ExecuteCode_Put(OopContext context)
        {
        }

        internal virtual void ExecuteCode_Restart(OopContext context)
        {
            context.NextLine = false;
            context.Instruction = 0;
        }

        internal virtual void ExecuteCode_Restore(OopContext context)
        {
        }

        internal virtual void ExecuteCode_Send(OopContext context)
        {
        }

        internal virtual void ExecuteCode_Set(OopContext context)
        {
        }

        internal virtual void ExecuteCode_Shoot(OopContext context)
        {
        }

        internal virtual void ExecuteCode_Take(OopContext context)
        {
        }

        internal virtual void ExecuteCode_ThrowStar(OopContext context)
        {
        }

        internal virtual void ExecuteCode_Try(OopContext context)
        {
        }

        internal virtual void ExecuteCode_Unlock(OopContext context)
        {
            context.Actor.P2 = 0;
        }

        internal virtual void ExecuteCode_Walk(OopContext context)
        {
        }

        internal virtual void ExecuteCode_Zap(OopContext context)
        {
        }

        internal virtual bool ExecuteLabel(int sender, CodeSearchInfo search, string prefix)
        {
            var label = search.Label;
            var target = @"";
            var success = false;
            var split = label.IndexOf(':');

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

                success = search.Offset >= 0;
                break;
            }
            return success;
        }

        internal virtual bool IsActorTargeted(int sender, CodeSearchInfo info, string target)
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

        public virtual int ReadActorCodeByte(int index, ICodeInstruction instructionSource)
        {
            var actor = Actors[index];
            var value = 0;

            if (instructionSource.Instruction < 0 || instructionSource.Instruction >= actor.Length)
            {
                OopByte = 0;
            }
            else
            {
                value = actor.Code[instructionSource.Instruction];
                OopByte = value;
                instructionSource.Instruction++;
            }
            return value;
        }

        public virtual string ReadActorCodeLine(int index, ICodeInstruction instructionSource)
        {
            var result = new StringBuilder();
            ReadActorCodeByte(index, instructionSource);
            while (OopByte != 0x00 && OopByte != 0x0D)
            {
                result.Append(OopByte.ToChar());
                ReadActorCodeByte(index, instructionSource);
            }
            return result.ToString();
        }

        public virtual int ReadActorCodeNumber(int index, ICodeInstruction instructionSource)
        {
            var result = new StringBuilder();
            var success = false;

            while (ReadActorCodeByte(index, instructionSource) == 0x20)
            {
            }

            OopByte = OopByte.ToUpperCase();
            while (OopByte >= 0x30 && OopByte <= 0x39)
            {
                success = true;
                result.Append(OopByte.ToChar());
                ReadActorCodeByte(index, instructionSource);
            }

            if (instructionSource.Instruction > 0)
            {
                instructionSource.Instruction--;
            }

            if (!success)
            {
                OopNumber = -1;
            }
            else
            {
                var resultInt = -1;
                int.TryParse(result.ToString(), out resultInt);
                OopNumber = resultInt;
            }

            return OopNumber;
        }

        public virtual string ReadActorCodeWord(int index, ICodeInstruction instructionSource)
        {
            var result = new StringBuilder();

            while (true)
            {
                ReadActorCodeByte(index, instructionSource);
                if (OopByte != 0x20)
                {
                    break;
                }
            }

            OopByte = OopByte.ToUpperCase();

            if (!(OopByte >= 0x30 && OopByte <= 0x39))
            {
                while ((OopByte >= 0x41 && OopByte <= 0x5A) || (OopByte >= 0x30 && OopByte <= 0x39) || (OopByte == 0x3A) ||
                       (OopByte == 0x5F))
                {
                    result.Append(OopByte.ToChar());
                    ReadActorCodeByte(index, instructionSource);
                    OopByte = OopByte.ToUpperCase();
                }
            }

            if (instructionSource.Instruction > 0)
            {
                instructionSource.Instruction--;
            }

            OopWord = result.ToString();
            return OopWord;
        }

        internal virtual int SearchActorCode(int index, string term)
        {
            var result = -1;
            var termBytes = term.ToBytes();
            var actor = Actors[index];
            var offset = new ByRefInstruction(0);

            while (offset.Instruction < actor.Length)
            {
                var oldOffset = offset.Instruction;
                var termOffset = 0;
                var success = false;

                while (true)
                {
                    ReadActorCodeByte(index, offset);
                    if (termBytes[termOffset].ToUpperCase() != OopByte.ToUpperCase())
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
                    OopByte = OopByte.ToUpperCase();
                    if (!((OopByte >= 0x41 && OopByte <= 0x5A) || OopByte == 0x5F))
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
    }
}