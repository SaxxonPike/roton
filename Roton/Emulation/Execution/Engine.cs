using System;
using System.Collections.Generic;
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
            Random = new Random();
            RandomDeterministic = new Random(0);
        }

        private Random Random { get; }

        private Random RandomDeterministic { get; }

        private int TimerBase => CoreTimer.Tick & 0x7FFF;

        private Thread Thread { get; set; }

        private bool ThreadActive { get; set; }

        private int TimerTick
        {
            get { return _timerTick; }
            set { _timerTick = value & 0x7FFF; }
        }

        private bool AboutShown
        {
            get { return StateData.AboutShown; }
            set { StateData.AboutShown = value; }
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

        public IAlerts Alerts => StateData.Alerts;

        public void Attack(int index, IXyPair location)
        {
            if (index == 0 && WorldData.EnergyCycles > 0)
            {
                WorldData.Score += this.ElementAt(location).Points;
                UpdateStatus();
            }
            else
            {
                Harm(index);
            }

            if (index > 0 && index <= StateData.ActIndex)
            {
                StateData.ActIndex--;
            }

            if (Tiles[location].Id == Elements.PlayerId && WorldData.EnergyCycles > 0)
            {
                WorldData.Score += this.ElementAt(Actors[index].Location).Points;
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

        private ITile BorderTile => StateData.BorderTile;

        private void ClearBoard()
        {
            var emptyId = Elements.EmptyId;
            var boardEdgeId = StateData.EdgeTile.Id;
            var boardBorderId = BorderTile.Id;
            var boardBorderColor = BorderTile.Color;

            // board properties
            Board.Name = string.Empty;
            StateData.Message = string.Empty;
            Board.Shots = 0xFF;
            Board.Dark = false;
            Board.RestartOnZap = false;
            Board.TimeLimit = 0;
            Board.ExitEast = 0;
            Board.ExitNorth = 0;
            Board.ExitSouth = 0;
            Board.ExitWest = 0;

            // build board edges
            for (var y = 0; y <= Tiles.Height + 1; y++)
            {
                TileAt(0, y).Id = boardEdgeId;
                TileAt(Tiles.Width + 1, y).Id = boardEdgeId;
            }
            for (var x = 0; x <= Tiles.Width + 1; x++)
            {
                TileAt(x, 0).Id = boardEdgeId;
                TileAt(x, Tiles.Height + 1).Id = boardEdgeId;
            }

            // clear out board
            for (var x = 1; x <= Tiles.Width; x++)
            {
                for (var y = 1; y <= Tiles.Height; y++)
                {
                    TileAt(x, y).SetTo(emptyId, 0);
                }
            }

            // build border
            for (var y = 1; y <= Tiles.Height; y++)
            {
                TileAt(1, y).SetTo(boardBorderId, boardBorderColor);
                TileAt(Tiles.Width, y).SetTo(boardBorderId, boardBorderColor);
            }
            for (var x = 1; x <= Tiles.Width; x++)
            {
                TileAt(x, 1).SetTo(boardBorderId, boardBorderColor);
                TileAt(x, Tiles.Height).SetTo(boardBorderId, boardBorderColor);
            }

            // generate player actor
            var element = Elements.PlayerElement;
            StateData.ActorCount = 0;
            Player.Location.SetTo(Tiles.Width /2, Tiles.Height /2);
            TileAt(Player.Location).SetTo(element.Id, element.Color);
            Player.Cycle = 1;
            Player.UnderTile.SetTo(0, 0);
            Player.Pointer = 0;
            Player.Length = 0;
        }

        public void ClearSound()
        {
            StateData.SoundPlaying = false;
            StopSound();
        }

        public void ClearWorld()
        {
            StateData.BoardCount = 0;
            Boards.Clear();
            ResetAlerts();
            ClearBoard();
            Boards.Add(new PackedBoard(GameSerializer.PackBoard(Tiles)));
            WorldData.BoardIndex = 0;
            WorldData.Ammo = 0;
            WorldData.Gems = 0;
            WorldData.Health = 100;
            WorldData.EnergyCycles = 0;
            WorldData.Torches = 0;
            WorldData.TorchCycles = 0;
            WorldData.Score = 0;
            WorldData.TimePassed = 0;
            WorldData.Stones = -1;
            WorldData.Keys.Clear();
            WorldData.Flags.Clear();
            SetBoard(0);
            Board.Name = "Introduction screen";
            WorldData.Name = string.Empty;
            StateData.WorldFileName = string.Empty;
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
                surrounding[i] = TileAt(center.Sum(GetConveyorVector(i))).Clone();
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
                            var tile = TileAt(source);
                            var index = ActorIndexAt(source);
                            TileAt(source).CopyFrom(surrounding[i]);
                            TileAt(target).Id = Elements.EmptyId;
                            MoveActor(index, target);
                            TileAt(source).CopyFrom(tile);
                        }
                        else
                        {
                            TileAt(target).CopyFrom(surrounding[i]);
                            UpdateBoard(target);
                        }

                        if (!Elements[surrounding[(i + 8 + direction)%8].Id].IsPushable)
                        {
                            TileAt(source).Id = Elements.EmptyId;
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

        private IXyPair Enter
        {
            get { return Board.Enter; }
            set { Board.Enter.CopyFrom(value); }
        }

        public IXyPair GetCardinalVector(int index)
        {
            return new Vector(StateData.Vector4[index], StateData.Vector4[index + 4]);
        }

        private IXyPair GetConveyorVector(int index)
        {
            return new Vector(StateData.Vector8[index], StateData.Vector8[index + 8]);
        }

        public abstract IGrammar Grammar { get; }

        public abstract IHud Hud { get; }

        private IKeyboard Keyboard => _config.Keyboard;

        public IMemory Memory { get; }

        public void MoveActor(int index, IXyPair target)
        {
            var actor = Actors[index];
            var sourceLocation = actor.Location.Clone();
            var sourceTile = TileAt(actor.Location);
            var targetTile = TileAt(target);
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
            if (index == 0 && Board.Dark)
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
            Boards[WorldData.BoardIndex] = board;
        }

        public void PlaySound(int priority, ISound sound, int offset, int length)
        {
        }

        public void Push(IXyPair location, IXyPair vector)
        {
            // this is here to prevent endless push loops
            // but doesn't exist in the original code
            if (vector.IsZero())
            {
                throw Exceptions.PushStackOverflow;
            }

            var tile = TileAt(location);
            if ((tile.Id == Elements.SliderEwId && vector.Y == 0) || (tile.Id == Elements.SliderNsId && vector.X == 0) ||
                Elements[tile.Id].IsPushable)
            {
                var furtherTile = TileAt(location.Sum(vector));
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

        public void RaiseError(string error)
        {
            SetMessage(0xC8, Alerts.ErrorMessage(error));
            this.PlaySound(5, SoundSet.Error);
        }

        public int RandomNumber(int max)
        {
            return Random.Next(max);
        }

        public int RandomNumberDeterministic(int max)
        {
            return RandomDeterministic.Next(max);
        }

        private int ReadActorCodeByte(int index, ICodeInstruction instructionSource)
        {
            var actor = Actors[index];
            var value = 0;

            if (instructionSource.Instruction < 0 || instructionSource.Instruction >= actor.Length)
            {
                StateData.OopByte = 0;
            }
            else
            {
                value = actor.Code[instructionSource.Instruction];
                StateData.OopByte = value;
                instructionSource.Instruction++;
            }
            return value;
        }

        public string ReadActorCodeLine(int index, ICodeInstruction instructionSource)
        {
            var result = new StringBuilder();
            ReadActorCodeByte(index, instructionSource);
            while (StateData.OopByte != 0x00 && StateData.OopByte != 0x0D)
            {
                result.Append(StateData.OopByte.ToChar());
                ReadActorCodeByte(index, instructionSource);
            }
            return result.ToString();
        }

        public int ReadActorCodeNumber(int index, ICodeInstruction instructionSource)
        {
            var result = new StringBuilder();
            var success = false;

            while (ReadActorCodeByte(index, instructionSource) == 0x20)
            {
            }

            StateData.OopByte = StateData.OopByte.ToUpperCase();
            while (StateData.OopByte >= 0x30 && StateData.OopByte <= 0x39)
            {
                success = true;
                result.Append(StateData.OopByte.ToChar());
                ReadActorCodeByte(index, instructionSource);
            }

            if (instructionSource.Instruction > 0)
            {
                instructionSource.Instruction--;
            }

            if (!success)
            {
                StateData.OopNumber = -1;
            }
            else
            {
                var resultInt = -1;
                int.TryParse(result.ToString(), out resultInt);
                StateData.OopNumber = resultInt;
            }

            return StateData.OopNumber;
        }

        public string ReadActorCodeWord(int index, ICodeInstruction instructionSource)
        {
            var result = new StringBuilder();

            while (true)
            {
                ReadActorCodeByte(index, instructionSource);
                if (StateData.OopByte != 0x20)
                {
                    break;
                }
            }

            StateData.OopByte = StateData.OopByte.ToUpperCase();

            if (!(StateData.OopByte >= 0x30 && StateData.OopByte <= 0x39))
            {
                while ((StateData.OopByte >= 0x41 && StateData.OopByte <= 0x5A) || (StateData.OopByte >= 0x30 && StateData.OopByte <= 0x39) || (StateData.OopByte == 0x3A) ||
                       (StateData.OopByte == 0x5F))
                {
                    result.Append(StateData.OopByte.ToChar());
                    ReadActorCodeByte(index, instructionSource);
                    StateData.OopByte = StateData.OopByte.ToUpperCase();
                }
            }

            if (instructionSource.Instruction > 0)
            {
                instructionSource.Instruction--;
            }

            StateData.OopWord = result.ToString();
            return StateData.OopWord;
        }

        public void RedrawBoard()
        {
            Hud.RedrawBoard();
        }

        public virtual void RemoveItem(IXyPair location)
        {
            TileAt(location).Id = Elements.EmptyId;
        }

        private bool RestartOnZap
        {
            get { return Board.RestartOnZap; }
            set { Board.RestartOnZap = value; }
        }

        public IXyPair Rnd()
        {
            var result = new Vector();
            Rnd(result);
            return result;
        }

        public IXyPair Seek(IXyPair location)
        {
            var result = new Vector();
            if (RandomNumberDeterministic(2) == 0 || Player.Location.Y == location.Y)
            {
                result.X = (Player.Location.X - location.X).Polarity();
            }
            if (result.X == 0)
            {
                result.Y = (Player.Location.Y - location.Y).Polarity();
            }
            if (WorldData.EnergyCycles > 0)
            {
                result.SetOpposite();
            }
            return result;
        }

        public abstract IGameSerializer GameSerializer { get; }

        public void SetBoard(int boardIndex)
        {
            var element = Elements.PlayerElement;
            TileAt(Player.Location).SetTo(element.Id, element.Color);
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

            SpawnActor(new Location(0, 0), new Tile(Elements.MessengerId, 0), 1, StateData.DefaultActor);
            Actors[StateData.ActorCount].P2 = duration / (StateData.GameWaitTime + 1);
            StateData.Message = topMessage;
            StateData.Message2 = bottomMessage;
        }

        public abstract ISoundSet SoundSet { get; }

        private ISpeaker Speaker => _config.Speaker;

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

        public abstract IState StateData { get; }

        public void Stop()
        {
            if (ThreadActive)
            {
                ThreadActive = false;
            }
        }

        public abstract ITileGrid Tiles { get; }

        public abstract bool TorchesEnabled { get; }

        public void UnpackBoard(int boardIndex)
        {
            GameSerializer.UnpackBoard(Tiles, Boards[boardIndex].Data);
            WorldData.BoardIndex = boardIndex;
        }

        public void UpdateBoard(IXyPair location)
        {
            DrawTile(location, Draw(location));
        }

        public abstract IWorld WorldData { get; }

        public virtual AnsiChar Draw(IXyPair location)
        {
            if (Board.Dark && !this.ElementAt(location).IsAlwaysVisible &&
                (WorldData.TorchCycles <= 0 || Distance(Player.Location, location) >= 50) && !StateData.EditorMode)
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

        public abstract IElementList Elements { get; }

        public virtual IActor Player => Actors[0];

        protected virtual void ReadInput()
        {
            StateData.KeyShift = false;
            StateData.KeyArrow = false;
            StateData.KeyPressed = 0;
            StateData.KeyVector.SetTo(0, 0);

            var key = Keyboard.GetKey();
            if (key >= 0)
            {
                StateData.KeyPressed = key;
                StateData.KeyShift = Keyboard.Shift;
                switch (key)
                {
                    case 0xCB:
                        StateData.KeyVector.CopyFrom(Vector.West);
                        StateData.KeyArrow = true;
                        break;
                    case 0xCD:
                        StateData.KeyVector.CopyFrom(Vector.East);
                        StateData.KeyArrow = true;
                        break;
                    case 0xC8:
                        StateData.KeyVector.CopyFrom(Vector.North);
                        StateData.KeyArrow = true;
                        break;
                    case 0xD0:
                        StateData.KeyVector.CopyFrom(Vector.South);
                        StateData.KeyArrow = true;
                        break;
                }
            }
        }

        public virtual int ReadKey()
        {
            var key = Keyboard.GetKey();
            StateData.KeyPressed = key > 0 ? key : 0;
            return StateData.KeyPressed;
        }

        public string StoneText
        {
            get
            {
                foreach (var flag in WorldData.Flags.Select(f => f.ToUpperInvariant()))
                {
                    if (flag.Length > 0 && flag.StartsWith("Z"))
                    {
                        return flag.Substring(1);
                    }
                }
                return string.Empty;
            }
        }

        public bool TitleScreen => StateData.PlayerElement != Elements.PlayerId;

        public virtual void WaitForTick()
        {
            while (TimerTick == TimerBase && ThreadActive)
            {
                Thread.Sleep(1);
            }
            TimerTick++;
        }

        protected virtual bool ActorIsLocked(int index)
        {
            return Actors[index].P2 != 0;
        }

        public virtual int Adjacent(IXyPair location, int id)
        {
            return (location.Y <= 1 || Tiles[location.Sum(Vector.North)].Id == id ? 1 : 0) |
                   (location.Y >= Tiles.Height || Tiles[location.Sum(Vector.South)].Id == id ? 2 : 0) |
                   (location.X <= 1 || Tiles[location.Sum(Vector.West)].Id == id ? 4 : 0) |
                   (location.X >= Tiles.Width || Tiles[location.Sum(Vector.East)].Id == id ? 8 : 0);
        }

        public virtual bool BroadcastLabel(int sender, string label, bool force)
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

        internal virtual int ColorMatch(ITile tile)
        {
            var element = Elements[tile.Id];

            if (element.Color < 0xF0)
                return element.Color & 7;
            if (element.Color == 0xFE)
                return ((tile.Color >> 4) & 0x0F) + 8;
            return tile.Color & 0x0F;
        }

        protected int Distance(IXyPair a, IXyPair b)
        {
            return (a.Y - b.Y).Square()*2 + (a.X - b.X).Square();
        }

        private void DrawTile(IXyPair location, AnsiChar ac)
        {
            Hud.DrawTile(location.X - 1, location.Y - 1, ac);
        }

        public virtual void EnterBoard()
        {
            Enter.CopyFrom(Player.Location);
            if (Board.Dark && Alerts.Dark)
            {
                SetMessage(0xC8, Alerts.DarkMessage);
                Alerts.Dark = false;
            }
            WorldData.TimePassed = 0;
            UpdateStatus();
        }

        private void EnterHighScore(int score)
        {
        }

        public virtual void ExecuteCode(int index, ICodeInstruction instructionSource, string name)
        {
            var context = new OopContext(index, instructionSource, name, this);
        }

        protected virtual bool ExecuteLabel(int sender, CodeSearchInfo search, string prefix)
        {
            var label = search.Label;
            var target = string.Empty;
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

        private void FadeBoard(AnsiChar ac)
        {
            Hud.FadeBoard(ac);
        }

        public void FadePurple()
        {
            FadeBoard(new AnsiChar(0xDB, 0x05));
            RedrawBoard();
        }

        public void FadeRed()
        {
            FadeBoard(new AnsiChar(0xDB, 0x04));
            RedrawBoard();
        }

        public virtual void ForcePlayerColor(int index)
        {
            var actor = Actors[index];
            if (TileAt(actor.Location).Color != Elements.PlayerElement.Color ||
                Elements.PlayerElement.Character != 0x02)
            {
                Elements.PlayerElement.Character = 2;
                TileAt(actor.Location).Color = Elements.PlayerElement.Color;
                UpdateBoard(actor.Location);
            }
        }

        private bool GetMainTimeElapsed(int interval)
        {
            return TimerTick%interval == 0;
            // TODO: Fix this for real.
            // This should say if it's been [interval] ticks since last checked
            // We could run into issues if TimerTick overflows
        }

        public bool GetPlayerTimeElapsed(int interval)
        {
            var result = false;
            while (GetTimeDifference(TimerTick, StateData.PlayerTime) > 0)
            {
                result = true;
                StateData.PlayerTime = (StateData.PlayerTime + interval) & 0x7FFF;
            }
            return result;
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

        public void Harm(int index)
        {
            var actor = Actors[index];
            if (index == 0)
            {
                if (WorldData.Health > 0)
                {
                    WorldData.Health -= 10;
                    UpdateStatus();
                    SetMessage(0x64, Alerts.OuchMessage);
                    var color = TileAt(actor.Location).Color;
                    color &= 0x0F;
                    color |= 0x70;
                    TileAt(actor.Location).Color = color;
                    if (WorldData.Health > 0)
                    {
                        WorldData.TimePassed = 0;
                        if (RestartOnZap)
                        {
                            this.PlaySound(4, SoundSet.TimeOut);
                            TileAt(actor.Location).Id = Elements.EmptyId;
                            UpdateBoard(actor.Location);
                            var oldLocation = actor.Location.Clone();
                            actor.Location.CopyFrom(Enter);
                            UpdateRadius(oldLocation, 0);
                            UpdateRadius(actor.Location, 0);
                            StateData.GamePaused = true;
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
                var element = TileAt(actor.Location).Id;
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

        protected virtual void InitializeElementDelegates()
        {
        }

        protected virtual void InitializeElements(bool showInvisibles)
        {
            // this isn't all the initializations.
            // todo: replace this with the ability to completely reinitialize engine default memory
            Elements.InvisibleElement.Character = showInvisibles ? 0xB0 : 0x20;
            Elements.InvisibleElement.Color = 0xFF;
            Elements.PlayerElement.Character = 0x02;
        }

        internal virtual bool IsActorTargeted(int sender, CodeSearchInfo info, string target)
        {
            var success = false;
            switch (target.ToUpperInvariant())
            {
                case @"ALL":
                    success = info.Index <= StateData.ActorCount;
                    break;
                case @"OTHERS":
                    if (info.Index <= StateData.ActorCount)
                    {
                        if (info.Index != sender)
                        {
                            success = true;
                        }
                        else
                        {
                            info.Index++;
                            success = info.Index <= StateData.ActorCount;
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

            if (StateData.Init)
            {
                if (!AboutShown)
                {
                    ShowAbout();
                }
                if (StateData.DefaultWorldName.Length <= 0)
                {
                    // normally we would load the world here,
                    // however it will have already been loaded in the context
                }
                StateData.StartBoard = WorldData.BoardIndex;
                SetBoard(0);
                StateData.Init = false;
            }

            var element = Elements[StateData.PlayerElement];
            TileAt(Player.Location).SetTo(element.Id, element.Color);
            if (StateData.PlayerElement == Elements.MonitorId)
            {
                SetMessage(0, new Message());
                Hud.DrawTitleStatus();
            }

            if (gameIsActive)
            {
                FadePurple();
            }

            StateData.GameWaitTime = StateData.GameSpeed << 1;
            StateData.BreakGameLoop = false;
            StateData.GameCycle = RandomNumberDeterministic(0x64);
            StateData.ActIndex = StateData.ActorCount + 1;

            while (ThreadActive)
            {
                if (!StateData.GamePaused)
                {
                    if (StateData.ActIndex <= StateData.ActorCount)
                    {
                        var actorData = Actors[StateData.ActIndex];
                        if (actorData.Cycle != 0)
                        {
                            if (StateData.ActIndex % actorData.Cycle == StateData.GameCycle %actorData.Cycle)
                            {
                                Elements[TileAt(actorData.Location).Id].Act(this, StateData.ActIndex);
                            }
                        }
                        StateData.ActIndex++;
                    }
                }
                else
                {
                    StateData.ActIndex = StateData.ActorCount + 1;
                    if (GetMainTimeElapsed(25))
                    {
                        alternating = !alternating;
                    }
                    if (alternating)
                    {
                        var playerElement = Elements.PlayerElement;
                        DrawTile(Player.Location, new AnsiChar(playerElement.Character, playerElement.Color));
                    }
                    else
                    {
                        if (TileAt(Player.Location).Id == Elements.PlayerId)
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
                    if (StateData.KeyPressed == 0x1B)
                    {
                        if (WorldData.Health > 0)
                        {
                            StateData.BreakGameLoop = Hud.EndGameConfirmation();
                        }
                        else
                        {
                            StateData.BreakGameLoop = true;
                            Hud.UpdateBorder();
                        }
                        StateData.KeyPressed = 0;
                    }
                    if (!StateData.KeyVector.IsZero())
                    {
                        var target = Player.Location.Sum(StateData.KeyVector);
                        this.ElementAt(target).Interact(this, target, 0, StateData.KeyVector);
                    }
                    if (!StateData.KeyVector.IsZero())
                    {
                        var target = Player.Location.Sum(StateData.KeyVector);
                        if (this.ElementAt(target).IsFloor)
                        {
                            if (this.ElementAt(Player.Location).Id == Elements.PlayerId)
                            {
                                MoveActor(0, target);
                            }
                            else
                            {
                                UpdateBoard(Player.Location);
                                Player.Location.Add(StateData.KeyVector);
                                TileAt(Player.Location).SetTo(Elements.PlayerId, Elements.PlayerElement.Color);
                                UpdateBoard(Player.Location);
                                UpdateRadius(Player.Location, RadiusMode.Update);
                                UpdateRadius(Player.Location.Difference(StateData.KeyVector), RadiusMode.Update);
                            }
                            StateData.GamePaused = false;
                            Hud.ClearPausing();
                            StateData.GameCycle = RandomNumberDeterministic(100);
                            WorldData.Locked = true;
                        }
                    }
                }

                if (StateData.ActIndex > StateData.ActorCount)
                {
                    if (!StateData.BreakGameLoop)
                    {
                        if (StateData.GameWaitTime <= 0 || GetMainTimeElapsed(StateData.GameWaitTime))
                        {
                            StateData.GameCycle++;
                            if (StateData.GameCycle > 420)
                            {
                                StateData.GameCycle = 1;
                            }
                            StateData.ActIndex = 0;
                            ReadInput();
                        }
                    }
                    WaitForTick();
                }

                if (StateData.BreakGameLoop)
                {
                    ClearSound();
                    if (StateData.PlayerElement == Elements.PlayerId)
                    {
                        if (WorldData.Health <= 0)
                        {
                            EnterHighScore(WorldData.Score);
                        }
                    }
                    else if (StateData.PlayerElement == Elements.MonitorId)
                    {
                        Hud.ClearTitleStatus();
                    }
                    element = Elements.PlayerElement;
                    TileAt(Player.Location).SetTo(element.Id, element.Color);
                    StateData.GameOver = false;
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
                TileAt(target).CopyFrom(TileAt(source));
                UpdateBoard(target);
                RemoveItem(source);
                UpdateBoard(source);
            }
        }

        public ISound EncodeMusic(string music)
        {
            return new Sound();
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

        public void RemoveActor(int index)
        {
            var actor = Actors[index];
            if (index < StateData.ActIndex)
            {
                StateData.ActIndex--;
            }

            TileAt(actor.Location).CopyFrom(actor.UnderTile);
            if (actor.Location.Y > 0)
            {
                UpdateBoard(actor.Location);
            }

            for (var i = 1; i <= StateData.ActorCount; i++)
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

            if (index < StateData.ActorCount)
            {
                for (var i = index; i < StateData.ActorCount; i++)
                {
                    Actors[i].CopyFrom(Actors[i + 1]);
                }
            }

            StateData.ActorCount--;
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
            result.X = RandomNumberDeterministic(3) - 1;
            if (result.X == 0)
            {
                result.Y = (RandomNumberDeterministic(2) << 1) - 1;
            }
            else
            {
                result.Y = 0;
            }
        }

        public IXyPair RndP(IXyPair vector)
        {
            var result = new Vector();
            result.CopyFrom(
                RandomNumberDeterministic(2) == 0
                    ? vector.Clockwise()
                    : vector.CounterClockwise());
            return result;
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
                    if (termBytes[termOffset].ToUpperCase() != StateData.OopByte.ToUpperCase())
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
                    StateData.OopByte = StateData.OopByte.ToUpperCase();
                    if (!((StateData.OopByte >= 0x41 && StateData.OopByte <= 0x5A) || StateData.OopByte == 0x5F))
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

        protected void SetEditorMode()
        {
            InitializeElements(true);
            StateData.EditorMode = true;
        }

        protected void SetGameMode()
        {
            InitializeElements(false);
            StateData.EditorMode = false;
        }

        private void ShowAbout()
        {
            ShowHelp("ABOUT");
        }

        private void ShowHelp(string filename)
        {
        }

        public virtual void ShowInGameHelp()
        {
            ShowHelp("GAME");
        }

        public void SpawnActor(IXyPair location, ITile tile, int cycle, IActor source)
        {
            // must reserve one actor for player, and one for messenger
            if (StateData.ActorCount < Actors.Capacity - 2)
            {
                StateData.ActorCount++;
                var actor = Actors[StateData.ActorCount];

                if (source == null)
                {
                    source = StateData.DefaultActor;
                }
                actor.CopyFrom(source);
                actor.Location.CopyFrom(location);
                actor.Cycle = cycle;
                actor.UnderTile.CopyFrom(TileAt(location));
                if (this.ElementAt(actor.Location).IsEditorFloor)
                {
                    var newColor = TileAt(actor.Location).Color & 0x70;
                    newColor |= tile.Color & 0x0F;
                    TileAt(actor.Location).Color = newColor;
                }
                else
                {
                    TileAt(actor.Location).Color = tile.Color;
                }
                TileAt(actor.Location).Id = tile.Id;
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
                SpawnActor(target, new Tile(id, Elements[id].Color), 1, StateData.DefaultActor);
                var actor = Actors[StateData.ActorCount];
                actor.P1 = enemyOwned ? 1 : 0;
                actor.Vector.CopyFrom(vector);
                actor.P2 = 0x64;
                return true;
            }
            if (element.Id != Elements.BreakableId)
            {
                if (element.IsDestructible)
                {
                    if (enemyOwned != (element.Id == Elements.PlayerId) || WorldData.EnergyCycles > 0)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                Destroy(target);
                this.PlaySound(2, SoundSet.BulletDie);
                return true;
            }
            return false;
        }

        protected virtual void StartMain()
        {
            StateData.GameSpeed = 4;
            StateData.DefaultSaveName = "SAVED";
            StateData.DefaultBoardName = "TEMP";
            StateData.DefaultWorldName = "TOWN";
            Hud.GenerateFadeMatrix();
            if (!StateData.WorldLoaded)
            {
                ClearWorld();
            }
            SetGameMode();
            TitleScreenLoop();
        }

        private void StopSound()
        {
        }

        public ITile TileAt(IXyPair l)
        {
            return Tiles[l];
        }

        private ITile TileAt(int x, int y)
        {
            return Tiles[new Location(x, y)];
        }

        internal virtual void TitleScreenLoop()
        {
            StateData.QuitZzt = false;
            StateData.Init = true;
            StateData.StartBoard = 0;
            var gameEnded = true;
            Hud.Initialize();
            while (ThreadActive)
            {
                if (!StateData.Init)
                {
                    SetBoard(0);
                }
                while (ThreadActive)
                {
                    StateData.PlayerElement = Elements.MonitorId;
                    var gameIsActive = false;
                    StateData.GamePaused = false;
                    MainLoop(gameEnded);
                    if (!ThreadActive)
                    {
                        // escape if the thread is supposed to shut down
                        break;
                    }

                    switch (StateData.KeyPressed.ToUpperCase())
                    {
                        case 0x57: // W
                            break;
                        case 0x50: // P
                            if (WorldData.Locked)
                            {
                                // reload world here
                                gameIsActive = StateData.WorldLoaded;
                                StateData.StartBoard = WorldData.BoardIndex;
                            }
                            else
                            {
                                gameIsActive = true;
                            }
                            if (gameIsActive)
                            {
                                SetBoard(StateData.StartBoard);
                                EnterBoard();
                            }
                            break;
                        case 0x41: // A
                            break;
                        case 0x45: // E
                            break;
                        case 0x53: // S
                            break;
                        case 0x52: // R
                            break;
                        case 0x48: // H
                            break;
                        case 0x7C: // ?
                            break;
                        case 0x1B: // esc
                        case 0x51: // Q
                            break;
                    }

                    if (gameIsActive)
                    {
                        StateData.PlayerElement = Elements.PlayerId;
                        StateData.GamePaused = true;
                        MainLoop(true);
                        gameEnded = true;
                    }

                    if (gameEnded || StateData.QuitZzt)
                    {
                        break;
                    }
                }
                if (StateData.QuitZzt)
                {
                    break;
                }
            }
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
                                        TileAt(target).SetTo(Elements.BreakableId, RandomNumberDeterministic(7) + 9);
                                    }
                                }
                                else
                                {
                                    if (TileAt(target).Id == Elements.BreakableId)
                                    {
                                        TileAt(target).Id = Elements.EmptyId;
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
    }
}