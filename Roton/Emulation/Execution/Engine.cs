using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Roton.Core;
using Roton.Emulation.Mapping;
using Roton.Emulation.Timing;
using Roton.Extensions;
using Roton.FileIo;

namespace Roton.Emulation.Execution
{
    internal abstract class Engine : IEngine
    {
        private readonly IEngineConfiguration _config;
        private int _timerTick;

        protected Engine(IEngineConfiguration config)
        {
            _config = config;
            Boards = new List<IPackedBoard>();
            Memory = new Memory();
            Random = new Randomizer(new RandomState());
            SyncRandom = new Randomizer(new RandomState(config.RandomSeed));
            Disk = config.Disk;
        }

        private ITile BorderTile => State.BorderTile;

        private IKeyboard Keyboard => _config.Keyboard;

        private IRandomizer Random { get; }

        private ISpeaker Speaker => _config.Speaker;

        private IRandomizer SyncRandom { get; }

        private int TimerBase => CoreTimer.Tick & 0x7FFF;

        private Thread Thread { get; set; }

        private bool ThreadActive { get; set; }

        private int TimerTick
        {
            get { return _timerTick; }
            set { _timerTick = value & 0x7FFF; }
        }

        public int ActorIndexAt(IXyPair location)
        {
            var index = 0;
            foreach (var actor in Actors)
            {
                if (actor.Location.X == location.X && actor.Location.Y == location.Y)
                    return index;
                index++;
            }
            return -1;
        }

        public abstract IActorList Actors { get; }

        public virtual int Adjacent(IXyPair location, int id)
        {
            return (location.Y <= 1 || Tiles[location.Sum(Vector.North)].Id == id ? 1 : 0) |
                   (location.Y >= Tiles.Height || Tiles[location.Sum(Vector.South)].Id == id ? 2 : 0) |
                   (location.X <= 1 || Tiles[location.Sum(Vector.West)].Id == id ? 4 : 0) |
                   (location.X >= Tiles.Width || Tiles[location.Sum(Vector.East)].Id == id ? 8 : 0);
        }

        public IAlerts Alerts => State.Alerts;

        public void Attack(int index, IXyPair location)
        {
            if (index == 0 && World.EnergyCycles > 0)
            {
                World.Score += this.ElementAt(location).Points;
                UpdateStatus();
            }
            else
            {
                Harm(index);
            }

            if (index > 0 && index <= State.ActIndex)
            {
                State.ActIndex--;
            }

            if (Tiles[location].Id == Elements.PlayerId && World.EnergyCycles > 0)
            {
                World.Score += this.ElementAt(Actors[index].Location).Points;
                UpdateStatus();
            }
            else
            {
                Destroy(location);
                this.PlaySound(2, SoundSet.EnemySuicide);
            }
        }

        public abstract IBoard Board { get; }

        public IList<IPackedBoard> Boards { get; }

        public virtual bool BroadcastLabel(int sender, string label, bool force)
        {
            var external = false;
            var success = false;

            if (sender < 0)
            {
                external = true;
                sender = -sender;
            }

            var info = new SearchContext(this)
            {
                SearchIndex = 0,
                SearchOffset = 0,
                SearchTarget = label
            };

            while (ExecuteLabel(sender, info, "\x000D:"))
            {
                if (!ActorIsLocked(info.SearchIndex) || force || (sender == info.SearchIndex && !external))
                {
                    if (sender == info.SearchIndex)
                    {
                        success = true;
                    }
                    Actors[info.SearchIndex].Instruction = info.SearchOffset;
                }
                info.SearchTarget = label;
            }

            return success;
        }

        public void ClearSound()
        {
            State.SoundPlaying = false;
            StopSound();
        }

        public void ClearWorld()
        {
            State.BoardCount = 0;
            Boards.Clear();
            ResetAlerts();
            ClearBoard();
            Boards.Add(new PackedBoard(GameSerializer.PackBoard(Tiles)));
            World.BoardIndex = 0;
            World.Ammo = 0;
            World.Gems = 0;
            World.Health = 100;
            World.EnergyCycles = 0;
            World.Torches = 0;
            World.TorchCycles = 0;
            World.Score = 0;
            World.TimePassed = 0;
            World.Stones = -1;
            World.Keys.Clear();
            World.Flags.Clear();
            SetBoard(0);
            Board.Name = "Introduction screen";
            World.Name = string.Empty;
            State.WorldFileName = string.Empty;
        }

        public void Convey(IXyPair center, int direction)
        {
            int beginIndex;
            int endIndex;

            var surrounding = new ITile[8];

            if (direction == 1)
            {
                beginIndex = 0;
                endIndex = 8;
            }
            else
            {
                beginIndex = 7;
                endIndex = -1;
            }

            var pushable = true;
            for (var i = beginIndex; i != endIndex; i += direction)
            {
                surrounding[i] = this.TileAt(center.Sum(GetConveyorVector(i))).Clone();
                var element = Elements[surrounding[i].Id];
                if (element.Id == Elements.EmptyId)
                    pushable = true;
                else if (!element.IsPushable)
                    pushable = false;
            }

            for (var i = beginIndex; i != endIndex; i += direction)
            {
                var element = Elements[surrounding[i].Id];

                if (pushable)
                {
                    if (element.IsPushable)
                    {
                        var source = center.Sum(GetConveyorVector(i));
                        var target = center.Sum(GetConveyorVector((i + 8 - direction)%8));
                        if (element.Cycle > -1)
                        {
                            var tile = this.TileAt(source);
                            var index = ActorIndexAt(source);
                            this.TileAt(source).CopyFrom(surrounding[i]);
                            this.TileAt(target).Id = Elements.EmptyId;
                            MoveActor(index, target);
                            this.TileAt(source).CopyFrom(tile);
                        }
                        else
                        {
                            this.TileAt(target).CopyFrom(surrounding[i]);
                            UpdateBoard(target);
                        }

                        if (!Elements[surrounding[(i + 8 + direction)%8].Id].IsPushable)
                        {
                            this.TileAt(source).Id = Elements.EmptyId;
                            UpdateBoard(source);
                        }
                    }
                    else
                    {
                        pushable = false;
                    }
                }
                else
                {
                    if (element.Id == Elements.EmptyId)
                        pushable = true;
                    else if (!element.IsPushable)
                        pushable = false;
                }
            }
        }

        public void Destroy(IXyPair location)
        {
            var index = ActorIndexAt(location);
            if (index == -1)
            {
                RemoveItem(location);
            }
            else
            {
                Harm(index);
            }
        }

        public IFileSystem Disk { get; set; }

        public virtual AnsiChar Draw(IXyPair location)
        {
            if (Board.IsDark && !this.ElementAt(location).IsAlwaysVisible &&
                (World.TorchCycles <= 0 || Distance(Player.Location, location) >= 50) && !State.EditorMode)
            {
                return new AnsiChar(0xB0, 0x07);
            }

            var tile = Tiles[location];
            var element = Elements[tile.Id];
            var elementCount = Elements.Count;

            if (tile.Id == Elements.EmptyId)
            {
                return new AnsiChar(0x20, 0x0F);
            }
            if (element.HasDrawCode)
            {
                return element.Draw(this, location);
            }
            if (tile.Id < elementCount - 7)
            {
                return new AnsiChar(element.Character, tile.Color);
            }
            if (tile.Id != elementCount - 1)
            {
                return new AnsiChar(tile.Color, ((tile.Id - (elementCount - 8)) << 4) | 0x0F);
            }
            return new AnsiChar(tile.Color, 0x0F);
        }

        public abstract IDrumBank DrumBank { get; }

        public abstract IElementList Elements { get; }

        public ISound EncodeMusic(string music)
        {
            return new Sound();
        }

        public virtual void EnterBoard()
        {
            Board.Entrance.CopyFrom(Player.Location);
            if (Board.IsDark && Alerts.Dark)
            {
                SetMessage(0xC8, Alerts.DarkMessage);
                Alerts.Dark = false;
            }
            World.TimePassed = 0;
            UpdateStatus();
        }

        public virtual void ExecuteCode(int index, IExecutable instructionSource, string name)
        {
            var context = new OopContext(index, instructionSource, name, this);

            context.PreviousInstruction = context.Instruction;
            context.Moved = false;
            context.Repeat = false;
            context.Died = false;
            context.Finished = false;
            context.CommandsExecuted = 0;

            while (true)
            {
                if (context.Instruction < 0)
                    break;

                context.NextLine = true;
                context.PreviousInstruction = context.Instruction;

                var command = ReadActorCodeByte(index, context);
                switch (command)
                {
                    case 0x3A: // :
                    case 0x27: // '
                    case 0x40: // @
                        ReadActorCodeLine(index, context);
                        break;
                    case 0x2F: // /
                    case 0x3F: // ?
                        if (command == 0x2F)
                            context.Repeat = true;

                        var vector = Grammar.GetDirection(context);
                        if (vector == null)
                        {
                            RaiseError("Bad direction");
                        }
                        else
                        {
                            ExecuteDirection(context, vector);
                            ReadActorCodeByte(index, context);
                            if (State.OopByte != 0x0D)
                                context.Instruction--;
                            context.Moved = true;
                        }
                        break;
                    case 0x23: // #
                        Grammar.Execute(context);
                        break;
                    case 0x0D: // enter
                        if (context.Message.Count > 0)
                            context.Message.Add(string.Empty);
                        break;
                    case 0x00:
                        context.Finished = true;
                        break;
                    default:
                        context.Message.Add(command.ToStringValue() + ReadActorCodeLine(context.Index, context));
                        break;
                }

                if (context.Finished ||
                    context.Moved ||
                    context.Repeat ||
                    context.Died ||
                    context.CommandsExecuted > 32)
                    break;
            }

            if (context.Repeat)
                context.Instruction = context.PreviousInstruction;

            if (State.OopByte == 0)
                context.Instruction = -1;

            if (context.Message.Count > 0)
                ExecuteMessage(context);

            if (context.Died)
                ExecuteDeath(context);
        }

        public bool ExecuteLabel(int sender, ISearchContext context, string prefix)
        {
            var label = context.SearchTarget;
            var target = string.Empty;
            var success = false;
            var split = label.IndexOf(':');

            if (split > 0)
            {
                target = label.Substring(0, split);
                label = label.Substring(split + 1);
                context.SearchTarget = target;
                success = Grammar.GetTarget(context);
            }
            else if (context.SearchIndex < sender)
            {
                context.SearchIndex = sender;
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
                    context.SearchOffset = 0;
                }
                else
                {
                    context.SearchOffset = SearchActorCode(context.SearchIndex, prefix + label);
                    if (context.SearchOffset < 0 && split > 0)
                    {
                        success = Grammar.GetTarget(context);
                        continue;
                    }
                }

                success = context.SearchOffset >= 0;
                break;
            }
            return success;
        }

        public void FadePurple()
        {
            FadeBoard(new AnsiChar(0xDB, 0x05));
            RedrawBoard();
        }

        public bool FindTile(ITile kind, IXyPair location)
        {
            location.X++;
            while (location.Y <= Tiles.Height)
            {
                while (location.X <= Tiles.Width)
                {
                    var tile = this.TileAt(location);
                    if (tile.Id == kind.Id)
                    {
                        if (kind.Color == 0 || ColorMatch(this.TileAt(location)) == kind.Color)
                        {
                            return true;
                        }
                    }
                    location.X++;
                }
                location.X = 1;
                location.Y++;
            }

            return false;
        }

        public virtual void ForcePlayerColor(int index)
        {
            var actor = Actors[index];
            var playerElement = Elements[Elements.PlayerId];
            if (this.TileAt(actor.Location).Color != playerElement.Color ||
                playerElement.Character != 0x02)
            {
                playerElement.Character = 2;
                this.TileAt(actor.Location).Color = playerElement.Color;
                UpdateBoard(actor.Location);
            }
        }

        public abstract IGameSerializer GameSerializer { get; }

        public IXyPair GetCardinalVector(int index)
        {
            return new Vector(State.Vector4[index], State.Vector4[index + 4]);
        }

        public bool GetPlayerTimeElapsed(int interval)
        {
            var result = false;
            while (GetTimeDifference(TimerTick, State.PlayerTime) > 0)
            {
                result = true;
                State.PlayerTime = (State.PlayerTime + interval) & 0x7FFF;
            }
            return result;
        }

        public abstract IGrammar Grammar { get; }

        public abstract void HandlePlayerInput(IActor actor, int hotkey);

        public void Harm(int index)
        {
            var actor = Actors[index];
            if (index == 0)
            {
                if (World.Health > 0)
                {
                    World.Health -= 10;
                    UpdateStatus();
                    SetMessage(0x64, Alerts.OuchMessage);
                    var color = this.TileAt(actor.Location).Color;
                    color &= 0x0F;
                    color |= 0x70;
                    this.TileAt(actor.Location).Color = color;
                    if (World.Health > 0)
                    {
                        World.TimePassed = 0;
                        if (Board.RestartOnZap)
                        {
                            this.PlaySound(4, SoundSet.TimeOut);
                            this.TileAt(actor.Location).Id = Elements.EmptyId;
                            UpdateBoard(actor.Location);
                            var oldLocation = actor.Location.Clone();
                            actor.Location.CopyFrom(Board.Entrance);
                            UpdateRadius(oldLocation, 0);
                            UpdateRadius(actor.Location, 0);
                            State.GamePaused = true;
                        }
                        this.PlaySound(4, SoundSet.Ouch);
                    }
                    else
                    {
                        this.PlaySound(5, SoundSet.GameOver);
                    }
                }
            }
            else
            {
                var element = this.TileAt(actor.Location).Id;
                if (element == Elements.BulletId)
                {
                    this.PlaySound(3, SoundSet.BulletDie);
                }
                else if (element != Elements.ObjectId)
                {
                    this.PlaySound(3, SoundSet.EnemyDie);
                }
                RemoveActor(index);
            }
        }

        public abstract IHud Hud { get; }

        public IMemory Memory { get; }

        public void MoveActor(int index, IXyPair target)
        {
            var actor = Actors[index];
            var sourceLocation = actor.Location.Clone();
            var sourceTile = this.TileAt(actor.Location);
            var targetTile = this.TileAt(target);
            var underTile = actor.UnderTile.Clone();

            actor.UnderTile.CopyFrom(targetTile);
            if (targetTile.Id == Elements.EmptyId)
            {
                targetTile.SetTo(sourceTile.Id, sourceTile.Color & 0x0F);
            }
            else
            {
                targetTile.SetTo(sourceTile.Id, (targetTile.Color & 0x70) | (sourceTile.Color & 0x0F));
            }
            sourceTile.CopyFrom(underTile);
            actor.Location.CopyFrom(target);
            if (targetTile.Id == Elements.PlayerId)
            {
                ForcePlayerColor(index);
            }
            UpdateBoard(target);
            UpdateBoard(sourceLocation);
            if (index == 0 && Board.IsDark)
            {
                var squareDistanceX = (target.X - sourceLocation.X).Square();
                var squareDistanceY = (target.Y - sourceLocation.Y).Square();
                if (squareDistanceX + squareDistanceY == 1)
                {
                    var glowLocation = new Location();
                    for (var x = target.X - 11; x <= target.X + 11; x++)
                    {
                        for (var y = target.Y - 8; y <= target.Y + 8; y++)
                        {
                            glowLocation.SetTo(x, y);
                            if (glowLocation.X >= 1 && glowLocation.X <= Tiles.Width && glowLocation.Y >= 1 &&
                                glowLocation.Y <= Tiles.Height)
                            {
                                if ((Distance(sourceLocation, glowLocation) < 50) ^
                                    (Distance(target, glowLocation) < 50))
                                {
                                    UpdateBoard(glowLocation);
                                }
                            }
                        }
                    }
                }
            }
            if (index == 0)
            {
                Hud.UpdateCamera();
            }
        }

        public void MoveActorOnRiver(int index)
        {
            var actor = Actors[index];
            var vector = new Vector();
            var underId = actor.UnderTile.Id;

            if (underId == Elements.RiverNId)
            {
                vector.SetTo(0, -1);
            }
            else if (underId == Elements.RiverSId)
            {
                vector.SetTo(0, 1);
            }
            else if (underId == Elements.RiverWId)
            {
                vector.SetTo(-1, 0);
            }
            else if (underId == Elements.RiverEId)
            {
                vector.SetTo(1, 0);
            }

            if (this.ElementAt(actor.Location).Id == Elements.PlayerId)
            {
                this.ElementAt(actor.Location.Sum(vector)).Interact(this, actor.Location.Sum(vector), 0, vector);
            }

            if (vector.IsNonZero())
            {
                var target = actor.Location.Sum(vector);
                if (this.ElementAt(target).IsFloor)
                {
                    MoveActor(index, target);
                }
            }
        }

        public void PackBoard()
        {
            var board = new PackedBoard(GameSerializer.PackBoard(Tiles));
            Boards[World.BoardIndex] = board;
        }

        public virtual IActor Player => Actors[0];

        public void PlaySound(int priority, ISound sound, int offset, int length)
        {
        }

        public void PlotTile(IXyPair location, ITile tile)
        {
            if (this.ElementAt(location).Id == Elements.PlayerId)
                return;

            var targetElement = Elements[tile.Id];
            var existingTile = Tiles[location];
            var targetColor = tile.Color;
            if (targetElement.Color >= 0xF0)
            {
                if (targetColor == 0)
                    targetColor = existingTile.Color;
                if (targetColor == 0)
                    targetColor = 0x0F;
                if (targetElement.Color == 0xFE)
                    targetColor = ((targetColor - 8) << 4) + 0x0F;
            }
            else
            {
                targetColor = targetElement.Color;
            }

            if (targetElement.Id == existingTile.Id)
            {
                existingTile.Color = targetColor;
            }
            else
            {
                Destroy(location);
                if (targetElement.Cycle < 0)
                {
                    existingTile.SetTo(targetElement.Id, targetColor);
                }
                else
                {
                    SpawnActor(location, new Tile(targetElement.Id, targetColor), targetElement.Cycle,
                        State.DefaultActor);
                }
            }
            UpdateBoard(location);
        }

        public void Push(IXyPair location, IXyPair vector)
        {
            // this is here to prevent endless push loops
            // but doesn't exist in the original code
            if (vector.IsZero())
            {
                throw Exceptions.PushStackOverflow;
            }

            var tile = this.TileAt(location);
            if ((tile.Id == Elements.SliderEwId && vector.Y == 0) || (tile.Id == Elements.SliderNsId && vector.X == 0) ||
                Elements[tile.Id].IsPushable)
            {
                var furtherTile = this.TileAt(location.Sum(vector));
                if (furtherTile.Id == Elements.TransporterId)
                {
                    PushThroughTransporter(location, vector);
                }
                else if (furtherTile.Id != Elements.EmptyId)
                {
                    Push(location.Sum(vector), vector);
                }

                var furtherElement = Elements[furtherTile.Id];
                if (!furtherElement.IsFloor && furtherElement.IsDestructible && furtherTile.Id != Elements.PlayerId)
                {
                    Destroy(location.Sum(vector));
                }

                furtherElement = Elements[furtherTile.Id];
                if (furtherElement.IsFloor)
                {
                    MoveTile(location, location.Sum(vector));
                }
            }
        }

        public void PushThroughTransporter(IXyPair location, IXyPair vector)
        {
            var actor = this.ActorAt(location.Sum(vector));

            if (actor.Vector.Matches(vector))
            {
                var search = actor.Location.Clone();
                var target = new Location();
                var ended = false;
                var success = true;

                while (!ended)
                {
                    search.Add(vector);
                    var element = this.ElementAt(search);
                    if (element.Id == Elements.BoardEdgeId)
                    {
                        ended = true;
                    }
                    else
                    {
                        if (success)
                        {
                            success = false;
                            if (!element.IsFloor)
                            {
                                Push(search, vector);
                                element = this.ElementAt(search);
                            }
                            if (element.IsFloor)
                            {
                                ended = true;
                                target.CopyFrom(search);
                            }
                            else
                            {
                                target.X = 0;
                            }
                        }
                    }

                    if (element.Id == Elements.TransporterId)
                    {
                        if (this.ActorAt(search).Vector.Matches(vector.Opposite()))
                        {
                            success = true;
                        }
                    }
                }

                if (target.X > 0)
                {
                    MoveTile(actor.Location.Difference(vector), target);
                    this.PlaySound(3, SoundSet.Transporter);
                }
            }
        }

        public void RaiseError(string error)
        {
            SetMessage(0xC8, Alerts.ErrorMessage(error));
            this.PlaySound(5, SoundSet.Error);
        }

        public int RandomNumber(int max)
        {
            return Random.GetNext(max);
        }

        public int ReadActorCodeByte(int index, IExecutable instructionSource)
        {
            var actor = Actors[index];
            var value = 0;

            if (instructionSource.Instruction < 0 || instructionSource.Instruction >= actor.Length)
            {
                State.OopByte = 0;
            }
            else
            {
                Debug.Assert(actor.Length == actor.Code.Length, @"Actor length and actual code length mismatch.");
                value = actor.Code[instructionSource.Instruction];
                State.OopByte = value;
                instructionSource.Instruction++;
            }
            return value;
        }

        public string ReadActorCodeLine(int index, IExecutable instructionSource)
        {
            var result = new StringBuilder();
            ReadActorCodeByte(index, instructionSource);
            while (State.OopByte != 0x00 && State.OopByte != 0x0D)
            {
                result.Append(State.OopByte.ToChar());
                ReadActorCodeByte(index, instructionSource);
            }
            return result.ToString();
        }

        public int ReadActorCodeNumber(int index, IExecutable instructionSource)
        {
            var result = new StringBuilder();
            var success = false;

            while (ReadActorCodeByte(index, instructionSource) == 0x20)
            {
            }

            State.OopByte = State.OopByte.ToUpperCase();
            while (State.OopByte >= 0x30 && State.OopByte <= 0x39)
            {
                success = true;
                result.Append(State.OopByte.ToChar());
                ReadActorCodeByte(index, instructionSource);
            }

            if (instructionSource.Instruction > 0)
            {
                instructionSource.Instruction--;
            }

            if (!success)
            {
                State.OopNumber = -1;
            }
            else
            {
                int resultInt;
                int.TryParse(result.ToString(), out resultInt);
                State.OopNumber = resultInt;
            }

            return State.OopNumber;
        }

        public string ReadActorCodeWord(int index, IExecutable instructionSource)
        {
            var result = new StringBuilder();

            while (true)
            {
                ReadActorCodeByte(index, instructionSource);
                if (State.OopByte != 0x20)
                {
                    break;
                }
            }

            State.OopByte = State.OopByte.ToUpperCase();

            if (!(State.OopByte >= 0x30 && State.OopByte <= 0x39))
            {
                while ((State.OopByte >= 0x41 && State.OopByte <= 0x5A) ||
                       (State.OopByte >= 0x30 && State.OopByte <= 0x39) || (State.OopByte == 0x3A) ||
                       (State.OopByte == 0x5F))
                {
                    result.Append(State.OopByte.ToChar());
                    ReadActorCodeByte(index, instructionSource);
                    State.OopByte = State.OopByte.ToUpperCase();
                }
            }

            if (instructionSource.Instruction > 0)
            {
                instructionSource.Instruction--;
            }

            State.OopWord = result.ToString();
            return State.OopWord;
        }

        public virtual int ReadKey()
        {
            var key = Keyboard.GetKey();
            State.KeyPressed = key > 0 ? key : 0;
            return State.KeyPressed;
        }

        public void RedrawBoard()
        {
            Hud.RedrawBoard();
        }

        public void RemoveActor(int index)
        {
            var actor = Actors[index];
            if (index < State.ActIndex)
            {
                State.ActIndex--;
            }

            this.TileAt(actor.Location).CopyFrom(actor.UnderTile);
            if (actor.Location.Y > 0)
            {
                UpdateBoard(actor.Location);
            }

            for (var i = 1; i <= State.ActorCount; i++)
            {
                var a = Actors[i];
                if (a.Follower >= index)
                {
                    if (a.Follower == index)
                    {
                        a.Follower = -1;
                    }
                    else
                    {
                        a.Follower--;
                    }
                }

                if (a.Leader >= index)
                {
                    if (a.Leader == index)
                    {
                        a.Leader = -1;
                    }
                    else
                    {
                        a.Leader--;
                    }
                }
            }

            if (index < State.ActorCount)
            {
                for (var i = index; i < State.ActorCount; i++)
                {
                    Actors[i].CopyFrom(Actors[i + 1]);
                }
            }

            State.ActorCount--;
        }

        public virtual void RemoveItem(IXyPair location)
        {
            this.TileAt(location).Id = Elements.EmptyId;
            UpdateBoard(location);

        }

        public IXyPair Rnd()
        {
            var result = new Vector();
            Rnd(result);
            return result;
        }

        public IXyPair RndP(IXyPair vector)
        {
            var result = new Vector();
            result.CopyFrom(
                SyncRandomNumber(2) == 0
                    ? vector.Clockwise()
                    : vector.CounterClockwise());
            return result;
        }

        public virtual int SearchActorCode(int index, string term)
        {
            var result = -1;
            var termBytes = term.ToBytes();
            var actor = Actors[index];
            var offset = new Executable {Instruction = 0};

            while (offset.Instruction < actor.Length)
            {
                var oldOffset = offset.Instruction;
                var termOffset = 0;
                bool success;

                while (true)
                {
                    ReadActorCodeByte(index, offset);
                    if (termBytes[termOffset].ToUpperCase() != State.OopByte.ToUpperCase())
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
                    State.OopByte = State.OopByte.ToUpperCase();
                    if (!((State.OopByte >= 0x41 && State.OopByte <= 0x5A) || State.OopByte == 0x5F))
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

        public IXyPair Seek(IXyPair location)
        {
            var result = new Vector();
            if (SyncRandomNumber(2) == 0 || Player.Location.Y == location.Y)
            {
                result.X = (Player.Location.X - location.X).Polarity();
            }
            if (result.X == 0)
            {
                result.Y = (Player.Location.Y - location.Y).Polarity();
            }
            if (World.EnergyCycles > 0)
            {
                result.SetOpposite();
            }
            return result;
        }

        public void SetBoard(int boardIndex)
        {
            var element = Elements[Elements.PlayerId];
            this.TileAt(Player.Location).SetTo(element.Id, element.Color);
            PackBoard();
            UnpackBoard(boardIndex);
        }

        public void SetMessage(int duration, IMessage message)
        {
            var index = ActorIndexAt(new Location(0, 0));
            if (index >= 0)
            {
                RemoveActor(index);
                Hud.UpdateBorder();
            }

            var topMessage = message.Text[0];
            var bottomMessage = message.Text.Length > 1 ? message.Text[1] : string.Empty;

            SpawnActor(new Location(0, 0), new Tile(Elements.MessengerId, 0), 1, State.DefaultActor);
            Actors[State.ActorCount].P2 = duration/(State.GameWaitTime + 1);
            State.Message = topMessage;
            State.Message2 = bottomMessage;
        }

        public virtual void ShowInGameHelp()
        {
            ShowHelp("GAME");
        }

        public abstract ISoundSet SoundSet { get; }

        public void SpawnActor(IXyPair location, ITile tile, int cycle, IActor source)
        {
            // must reserve one actor for player, and one for messenger
            if (State.ActorCount < Actors.Capacity - 2)
            {
                State.ActorCount++;
                var actor = Actors[State.ActorCount];

                if (source == null)
                {
                    source = State.DefaultActor;
                }
                actor.CopyFrom(source);
                actor.Location.CopyFrom(location);
                actor.Cycle = cycle;
                actor.UnderTile.CopyFrom(this.TileAt(location));
                if (this.ElementAt(actor.Location).IsEditorFloor)
                {
                    var newColor = this.TileAt(actor.Location).Color & 0x70;
                    newColor |= tile.Color & 0x0F;
                    this.TileAt(actor.Location).Color = newColor;
                }
                else
                {
                    this.TileAt(actor.Location).Color = tile.Color;
                }
                this.TileAt(actor.Location).Id = tile.Id;
                if (actor.Location.Y > 0)
                {
                    UpdateBoard(actor.Location);
                }
            }
        }

        public bool SpawnProjectile(int id, IXyPair location, IXyPair vector, bool enemyOwned)
        {
            var target = location.Sum(vector);
            var element = this.ElementAt(target);

            if (element.IsFloor || element.Id == Elements.WaterId)
            {
                SpawnActor(target, new Tile(id, Elements[id].Color), 1, State.DefaultActor);
                var actor = Actors[State.ActorCount];
                actor.P1 = enemyOwned ? 1 : 0;
                actor.Vector.CopyFrom(vector);
                actor.P2 = 0x64;
                return true;
            }

            if ((element.Id != Elements.BreakableId) &&
                (!element.IsDestructible ||
                 (((element.Id != Elements.PlayerId) || (World.EnergyCycles != 0)) && enemyOwned)))
            {
                return false;
            }

            Destroy(target);
            this.PlaySound(2, SoundSet.BulletDie);
            return true;
        }

        public void Start()
        {
            if (!ThreadActive)
            {
                ThreadActive = true;
                Thread = new Thread(StartMain);
                TimerTick = CoreTimer.Tick;
                Thread.Start();
            }
        }

        public abstract IState State { get; }

        public string StoneText
        {
            get
            {
                foreach (var flag in World.Flags.Select(f => f.ToUpperInvariant()))
                {
                    if (flag.Length > 0 && flag.StartsWith("Z"))
                    {
                        return flag.Substring(1);
                    }
                }
                return string.Empty;
            }
        }

        public void Stop()
        {
            if (ThreadActive)
            {
                ThreadActive = false;
            }
        }

        public int SyncRandomNumber(int max)
        {
            return SyncRandom.GetNext(max);
        }

        public abstract ITileGrid Tiles { get; }

        public bool TitleScreen => State.PlayerElement != Elements.PlayerId;

        public void UnpackBoard(int boardIndex)
        {
            GameSerializer.UnpackBoard(Tiles, Boards[boardIndex].Data);
            World.BoardIndex = boardIndex;
        }

        public void UpdateBoard(IXyPair location)
        {
            DrawTile(location, Draw(location));
        }

        public void UpdateRadius(IXyPair location, RadiusMode mode)
        {
            var source = location.Clone();
            var left = source.X - 9;
            var right = source.X + 9;
            var top = source.Y - 6;
            var bottom = source.Y + 6;
            for (var x = left; x <= right; x++)
            {
                for (var y = top; y <= bottom; y++)
                {
                    if (x >= 1 && x <= Tiles.Width && y >= 1 && y <= Tiles.Height)
                    {
                        var target = new Location(x, y);
                        if (mode != RadiusMode.Update)
                        {
                            if (Distance(source, target) < 50)
                            {
                                var element = this.ElementAt(target);
                                if (mode == RadiusMode.Explode)
                                {
                                    if (element.CodeEditText.Length > 0)
                                    {
                                        var actorIndex = ActorIndexAt(target);
                                        if (actorIndex > 0)
                                        {
                                            BroadcastLabel(-actorIndex, @"BOMBED", false);
                                        }
                                    }
                                    if (element.IsDestructible || element.Id == Elements.StarId)
                                    {
                                        Destroy(target);
                                    }
                                    if (element.Id == Elements.EmptyId || element.Id == Elements.BreakableId)
                                    {
                                        this.TileAt(target).SetTo(Elements.BreakableId, SyncRandomNumber(7) + 9);
                                    }
                                }
                                else
                                {
                                    if (this.TileAt(target).Id == Elements.BreakableId)
                                    {
                                        this.TileAt(target).Id = Elements.EmptyId;
                                    }
                                }
                            }
                        }
                        UpdateBoard(target);
                    }
                }
            }
        }

        public void UpdateStatus()
        {
            Hud.UpdateStatus();
        }

        public virtual void WaitForTick()
        {
            while (TimerTick == TimerBase && ThreadActive)
            {
                Thread.Sleep(1);
            }
            TimerTick++;
        }

        public abstract IWorld World { get; }

        protected virtual bool ActorIsLocked(int index)
        {
            return Actors[index].P2 != 0;
        }

        private void ClearBoard()
        {
            var emptyId = Elements.EmptyId;
            var boardEdgeId = State.EdgeTile.Id;
            var boardBorderId = BorderTile.Id;
            var boardBorderColor = BorderTile.Color;

            // board properties
            Board.Name = string.Empty;
            State.Message = string.Empty;
            Board.MaximumShots = 0xFF;
            Board.IsDark = false;
            Board.RestartOnZap = false;
            Board.TimeLimit = 0;
            Board.ExitEast = 0;
            Board.ExitNorth = 0;
            Board.ExitSouth = 0;
            Board.ExitWest = 0;

            // build board edges
            for (var y = 0; y <= Tiles.Height + 1; y++)
            {
                this.TileAt(0, y).Id = boardEdgeId;
                this.TileAt(Tiles.Width + 1, y).Id = boardEdgeId;
            }
            for (var x = 0; x <= Tiles.Width + 1; x++)
            {
                this.TileAt(x, 0).Id = boardEdgeId;
                this.TileAt(x, Tiles.Height + 1).Id = boardEdgeId;
            }

            // clear out board
            for (var x = 1; x <= Tiles.Width; x++)
            {
                for (var y = 1; y <= Tiles.Height; y++)
                {
                    this.TileAt(x, y).SetTo(emptyId, 0);
                }
            }

            // build border
            for (var y = 1; y <= Tiles.Height; y++)
            {
                this.TileAt(1, y).SetTo(boardBorderId, boardBorderColor);
                this.TileAt(Tiles.Width, y).SetTo(boardBorderId, boardBorderColor);
            }
            for (var x = 1; x <= Tiles.Width; x++)
            {
                this.TileAt(x, 1).SetTo(boardBorderId, boardBorderColor);
                this.TileAt(x, Tiles.Height).SetTo(boardBorderId, boardBorderColor);
            }

            // generate player actor
            var element = Elements[Elements.PlayerId];
            State.ActorCount = 0;
            Player.Location.SetTo(Tiles.Width/2, Tiles.Height/2);
            this.TileAt(Player.Location).SetTo(element.Id, element.Color);
            Player.Cycle = 1;
            Player.UnderTile.SetTo(0, 0);
            Player.Pointer = 0;
            Player.Length = 0;
        }

        private int ColorMatch(ITile tile)
        {
            var element = Elements[tile.Id];

            if (element.Color < 0xF0)
                return element.Color & 7;
            if (element.Color == 0xFE)
                return ((tile.Color >> 4) & 0x0F) + 8;
            return tile.Color & 0x0F;
        }

        private int Distance(IXyPair a, IXyPair b)
        {
            return (a.Y - b.Y).Square()*2 + (a.X - b.X).Square();
        }

        private void DrawTile(IXyPair location, AnsiChar ac)
        {
            Hud.DrawTile(location.X - 1, location.Y - 1, ac);
        }

        private void EnterHighScore(int score)
        {
        }

        protected virtual void ExecuteDeath(IOopContext context)
        {
            var location = context.Actor.Location.Clone();
            Harm(context.Index);
            PlotTile(location, context.DeathTile);
        }

        protected virtual void ExecuteDirection(IOopContext context, IXyPair vector)
        {
            if (vector.IsZero())
            {
                context.Repeat = false;
            }
            else
            {
                var target = context.Actor.Location.Sum(vector);
                if (!this.ElementAt(target).IsFloor)
                {
                    Push(target, vector);
                }
                if (this.ElementAt(target).IsFloor)
                {
                    MoveActor(context.Index, target);
                    context.Repeat = false;
                }
            }
        }

        protected virtual void ExecuteMessage(IOopContext context)
        {
            if (context.Message.Count == 1)
            {
                SetMessage(0xC8, new Message(context.Message));
            }
            else
            {
                context.Engine.State.KeyVector.SetTo(0, 0);
                Hud.ShowScroll(context.Message);
            }
        }

        private void FadeBoard(AnsiChar ac)
        {
            Hud.FadeBoard(ac);
        }

        public void FadeRed()
        {
            FadeBoard(new AnsiChar(0xDB, 0x04));
            RedrawBoard();
        }

        private IXyPair GetConveyorVector(int index)
        {
            return new Vector(State.Vector8[index], State.Vector8[index + 8]);
        }

        private int GetTimeDifference(int now, int then)
        {
            now &= 0x7FFF;
            then &= 0x7FFF;
            if (now < 0x4000 && then >= 0x4000)
            {
                now += 0x8000;
            }
            return now - then;
        }

        public abstract bool HandleTitleInput(int hotkey);

        protected virtual void InitializeElements(bool showInvisibles)
        {
            // this isn't all the initializations.
            // todo: replace this with the ability to completely reinitialize engine default memory
            Elements[Elements.InvisibleId].Character = showInvisibles ? 0xB0 : 0x20;
            Elements[Elements.InvisibleId].Color = 0xFF;
            Elements[Elements.PlayerId].Character = 0x02;
        }

        protected virtual byte[] LoadFile(string filename)
        {
            try
            {
                return Disk.GetFile(filename);
            }
            catch (Exception)
            {
                // TODO: This kind of error handling is bad.
                return null;
            }
        }

        private void MainLoop(bool gameIsActive)
        {
            var alternating = false;

            Hud.CreateStatusText();
            Hud.UpdateStatus();

            if (State.Init)
            {
                if (!State.AboutShown)
                {
                    ShowAbout();
                }
                if (State.DefaultWorldName.Length <= 0)
                {
                    // normally we would load the world here,
                    // however it will have already been loaded in the context
                }
                State.StartBoard = World.BoardIndex;
                SetBoard(0);
                State.Init = false;
            }

            var element = Elements[State.PlayerElement];
            this.TileAt(Player.Location).SetTo(element.Id, element.Color);
            if (State.PlayerElement == Elements.MonitorId)
            {
                SetMessage(0, new Message());
                Hud.DrawTitleStatus();
            }

            if (gameIsActive)
            {
                FadePurple();
            }

            State.GameWaitTime = State.GameSpeed << 1;
            State.BreakGameLoop = false;
            State.GameCycle = SyncRandomNumber(0x64);
            State.ActIndex = State.ActorCount + 1;

            while (ThreadActive)
            {
                if (!State.GamePaused)
                {
                    if (State.ActIndex <= State.ActorCount)
                    {
                        var actorData = Actors[State.ActIndex];
                        if (actorData.Cycle != 0)
                        {
                            if (State.ActIndex%actorData.Cycle == State.GameCycle%actorData.Cycle)
                            {
                                Elements[this.TileAt(actorData.Location).Id].Act(this, State.ActIndex);
                            }
                        }
                        State.ActIndex++;
                    }
                }
                else
                {
                    State.ActIndex = State.ActorCount + 1;
                    if (State.PlayerTimer.Clock(25))
                    {
                        alternating = !alternating;
                    }
                    if (alternating)
                    {
                        var playerElement = Elements[Elements.PlayerId];
                        DrawTile(Player.Location, new AnsiChar(playerElement.Character, playerElement.Color));
                    }
                    else
                    {
                        if (this.TileAt(Player.Location).Id == Elements.PlayerId)
                        {
                            DrawTile(Player.Location, new AnsiChar(0x20, 0x0F));
                        }
                        else
                        {
                            UpdateBoard(Player.Location);
                        }
                    }
                    Hud.DrawPausing();
                    ReadInput();
                    if (State.KeyPressed == 0x1B)
                    {
                        if (World.Health > 0)
                        {
                            State.BreakGameLoop = Hud.EndGameConfirmation();
                        }
                        else
                        {
                            State.BreakGameLoop = true;
                            Hud.UpdateBorder();
                        }
                        State.KeyPressed = 0;
                    }
                    if (!State.KeyVector.IsZero())
                    {
                        var target = Player.Location.Sum(State.KeyVector);
                        this.ElementAt(target).Interact(this, target, 0, State.KeyVector);
                    }
                    if (!State.KeyVector.IsZero())
                    {
                        var target = Player.Location.Sum(State.KeyVector);
                        if (this.ElementAt(target).IsFloor)
                        {
                            if (this.ElementAt(Player.Location).Id == Elements.PlayerId)
                            {
                                MoveActor(0, target);
                            }
                            else
                            {
                                UpdateBoard(Player.Location);
                                Player.Location.Add(State.KeyVector);
                                this.TileAt(Player.Location).SetTo(Elements.PlayerId, Elements[Elements.PlayerId].Color);
                                UpdateBoard(Player.Location);
                                UpdateRadius(Player.Location, RadiusMode.Update);
                                UpdateRadius(Player.Location.Difference(State.KeyVector), RadiusMode.Update);
                            }
                            State.GamePaused = false;
                            Hud.ClearPausing();
                            State.GameCycle = SyncRandomNumber(100);
                            World.IsLocked = true;
                        }
                    }
                }

                if (State.ActIndex > State.ActorCount)
                {
                    if (!State.BreakGameLoop && !State.GamePaused)
                    {
                        if (State.GameWaitTime <= 0 || State.PlayerTimer.Clock(State.GameWaitTime))
                        {
                            State.GameCycle++;
                            if (State.GameCycle > 420)
                            {
                                State.GameCycle = 1;
                            }
                            State.ActIndex = 0;
                            ReadInput();
                        }
                    }
                    WaitForTick();
                }

                if (State.BreakGameLoop)
                {
                    ClearSound();
                    if (State.PlayerElement == Elements.PlayerId)
                    {
                        if (World.Health <= 0)
                        {
                            EnterHighScore(World.Score);
                        }
                    }
                    else if (State.PlayerElement == Elements.MonitorId)
                    {
                        Hud.ClearTitleStatus();
                    }
                    element = Elements[Elements.PlayerId];
                    this.TileAt(Player.Location).SetTo(element.Id, element.Color);
                    State.GameOver = false;
                    break;
                }
            }
        }

        protected void MoveTile(IXyPair source, IXyPair target)
        {
            var sourceIndex = ActorIndexAt(source);
            if (sourceIndex >= 0)
            {
                MoveActor(sourceIndex, target);
            }
            else
            {
                this.TileAt(target).CopyFrom(this.TileAt(source));
                UpdateBoard(target);
                RemoveItem(source);
                UpdateBoard(source);
            }
        }

        public bool PlayWorld()
        {
            bool gameIsActive;

            if (World.IsLocked)
            {
                // reload world here
                gameIsActive = State.WorldLoaded;
                State.StartBoard = World.BoardIndex;
            }
            else
            {
                gameIsActive = true;
            }

            if (gameIsActive)
            {
                SetBoard(State.StartBoard);
                EnterBoard();
                State.PlayerElement = Elements.PlayerId;
                State.GamePaused = true;
                MainLoop(true);
            }

            return gameIsActive;
        }

        protected virtual void ReadInput()
        {
            State.KeyShift = false;
            State.KeyArrow = false;
            State.KeyPressed = 0;
            State.KeyVector.SetTo(0, 0);

            var key = Keyboard.GetKey();
            if (key >= 0)
            {
                State.KeyPressed = key;
                State.KeyShift = Keyboard.Shift;
                switch (key)
                {
                    case 0xCB:
                        State.KeyVector.CopyFrom(Vector.West);
                        State.KeyArrow = true;
                        break;
                    case 0xCD:
                        State.KeyVector.CopyFrom(Vector.East);
                        State.KeyArrow = true;
                        break;
                    case 0xC8:
                        State.KeyVector.CopyFrom(Vector.North);
                        State.KeyArrow = true;
                        break;
                    case 0xD0:
                        State.KeyVector.CopyFrom(Vector.South);
                        State.KeyArrow = true;
                        break;
                }
            }
        }

        private void ResetAlerts()
        {
            Alerts.AmmoPickup = true;
            Alerts.Dark = true;
            Alerts.EnergizerPickup = true;
            Alerts.FakeWall = true;
            Alerts.Forest = true;
            Alerts.GemPickup = true;
            Alerts.OutOfAmmo = true;
            Alerts.CantShootHere = true;
            Alerts.NotDark = true;
            Alerts.NoTorches = true;
            Alerts.TorchPickup = true;
        }

        private void Rnd(IXyPair result)
        {
            result.X = SyncRandomNumber(3) - 1;
            if (result.X == 0)
            {
                result.Y = (SyncRandomNumber(2) << 1) - 1;
            }
            else
            {
                result.Y = 0;
            }
        }

        protected void SetEditorMode()
        {
            InitializeElements(true);
            State.EditorMode = true;
        }

        protected void SetGameMode()
        {
            InitializeElements(false);
            State.EditorMode = false;
        }

        private void ShowAbout()
        {
            ShowHelp("ABOUT");
        }

        private void ShowHelp(string filename)
        {
        }

        protected virtual void StartMain()
        {
            State.GameSpeed = 4;
            State.DefaultSaveName = "SAVED";
            State.DefaultBoardName = "TEMP";
            State.DefaultWorldName = "TOWN";
            if (!State.WorldLoaded)
            {
                ClearWorld();
            }

            if (State.EditorMode)
                SetEditorMode();
            else
                SetGameMode();

            TitleScreenLoop();
        }

        private void StopSound()
        {
        }

        protected void TitleScreenLoop()
        {
            State.QuitZzt = false;
            State.Init = true;
            State.StartBoard = 0;
            var gameEnded = true;
            Hud.Initialize();
            while (ThreadActive)
            {
                if (!State.Init)
                {
                    SetBoard(0);
                }
                while (ThreadActive)
                {
                    State.PlayerElement = Elements.MonitorId;
                    State.GamePaused = false;
                    MainLoop(gameEnded);
                    if (!ThreadActive)
                    {
                        // escape if the thread is supposed to shut down
                        break;
                    }

                    var hotkey = State.KeyPressed.ToUpperCase();
                    var startPlaying = HandleTitleInput(hotkey);
                    if (startPlaying)
                    {
                        gameEnded = PlayWorld();
                    }

                    if (gameEnded || State.QuitZzt)
                    {
                        break;
                    }
                }
                if (State.QuitZzt)
                {
                    break;
                }
            }
        }
    }
}