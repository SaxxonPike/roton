using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Roton.Emulation.Actions;
using Roton.Emulation.Cheats;
using Roton.Emulation.Commands;
using Roton.Emulation.Conditions;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Directions;
using Roton.Emulation.Draws;
using Roton.Emulation.Infrastructure;
using Roton.Emulation.Interactions;
using Roton.Emulation.Items;
using Roton.Emulation.Targets;

namespace Roton.Emulation.Core.Impl
{
    public sealed class Engine : IEngine
    {
        private readonly Lazy<IActors> _actors;
        private readonly Lazy<IAlerts> _alerts;
        private readonly Lazy<IBoard> _board;
        private readonly Lazy<IBoards> _boards;
        private readonly Lazy<IActionList> _actionList;
        private readonly Lazy<IDrawList> _drawList;
        private readonly Lazy<IInteractionList> _interactionList;
        private readonly Lazy<ICheatList> _cheats;
        private readonly Lazy<IClock> _clock;
        private readonly Lazy<IColors> _colors;
        private readonly Lazy<ICommands> _commands;
        private readonly Lazy<IConditionList> _conditions;
        private readonly Lazy<IConfig> _config;
        private readonly Lazy<IDirectionList> _directions;
        private readonly Lazy<IElementList> _elements;
        private readonly Lazy<IFeatures> _features;
        private readonly Lazy<IFileSystem> _fileSystem;
        private readonly Lazy<IFlags> _flags;
        private readonly Lazy<IGameSerializer> _gameSerializer;
        private readonly Lazy<IHud> _hud;
        private readonly Lazy<IInterpreter> _interpreter;
        private readonly Lazy<IItemList> _items;
        private readonly Lazy<IKeyboard> _keyboard;
        private readonly Lazy<IParser> _parser;
        private readonly Lazy<IRandom> _random;
        private readonly Lazy<ISounds> _sounds;
        private readonly Lazy<IState> _state;
        private readonly Lazy<ITargetList> _targets;
        private readonly Lazy<ITiles> _tiles;
        private readonly Lazy<ITimers> _timers;
        private readonly Lazy<IWorld> _world;

        private int _clockTick;

        public Engine(Lazy<IClock> clock, Lazy<IActors> actors, Lazy<IAlerts> alerts, Lazy<IBoard> board,
            Lazy<IFileSystem> fileSystem, Lazy<IElementList> elements,
            Lazy<IInterpreter> interpreter, Lazy<IRandom> random, Lazy<IKeyboard> keyboard,
            Lazy<ITiles> tiles, Lazy<ISounds> sounds, Lazy<ITimers> timers, Lazy<IParser> parser,
            Lazy<IConfig> config, Lazy<IFlags> flags, Lazy<IConditionList> conditions, Lazy<IDirectionList> directions,
            Lazy<IColors> colors, Lazy<ICheatList> cheats, Lazy<ICommands> commands, Lazy<ITargetList> targets,
            Lazy<IFeatures> features, Lazy<IGameSerializer> gameSerializer, Lazy<IHud> hud, Lazy<IState> state,
            Lazy<IWorld> world, Lazy<IItemList> items, Lazy<IBoards> boards, Lazy<IActionList> actionList,
            Lazy<IDrawList> drawList, Lazy<IInteractionList> interactionList)
        {
            _clock = clock;
            _actors = actors;
            _alerts = alerts;
            _board = board;
            _fileSystem = fileSystem;
            _elements = elements;
            _interpreter = interpreter;
            _random = random;
            _keyboard = keyboard;
            _tiles = tiles;
            _sounds = sounds;
            _timers = timers;
            _parser = parser;
            _config = config;
            _flags = flags;
            _conditions = conditions;
            _directions = directions;
            _colors = colors;
            _cheats = cheats;
            _commands = commands;
            _targets = targets;
            _features = features;
            _gameSerializer = gameSerializer;
            _hud = hud;
            _state = state;
            _world = world;
            _items = items;
            _boards = boards;
            _actionList = actionList;
            _drawList = drawList;
            _interactionList = interactionList;
        }

        private IBoards Boards => _boards.Value;

        private ITile BorderTile => State.BorderTile;

        private IClock Clock => _clock.Value;

        private int ClockBase => Clock.Tick & 0x7FFF;

        private IFileSystem Disk => _fileSystem.Value;

        private IFeatures Features => _features.Value;

        private IGameSerializer GameSerializer => _gameSerializer.Value;

        private IInterpreter Interpreter => _interpreter.Value;

        private IKeyboard Keyboard => _keyboard.Value;

        private ITimers Timers => _timers.Value;

        private int ClockTick
        {
            get { return _clockTick; }
            set { _clockTick = value & 0x7FFF; }
        }

        private Thread Thread { get; set; }

        private bool ThreadActive { get; set; }

        public IActor ActorAt(IXyPair location)
        {
            return Actors
                .FirstOrDefault(actor => actor.Location.X == location.X && actor.Location.Y == location.Y);
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

        public IActors Actors => _actors.Value;

        public int Adjacent(IXyPair location, int id)
        {
            return (location.Y <= 1 || Tiles[location.Sum(Vector.North)].Id == id ? 1 : 0) |
                   (location.Y >= Tiles.Height || Tiles[location.Sum(Vector.South)].Id == id ? 2 : 0) |
                   (location.X <= 1 || Tiles[location.Sum(Vector.West)].Id == id ? 4 : 0) |
                   (location.X >= Tiles.Width || Tiles[location.Sum(Vector.East)].Id == id ? 8 : 0);
        }

        public IAlerts Alerts => _alerts.Value;

        public void Attack(int index, IXyPair location)
        {
            if (index == 0 && World.EnergyCycles > 0)
            {
                World.Score += ElementAt(location).Points;
                UpdateStatus();
            }
            else
            {
                Harm(index);
            }

            if (index > 0 && index <= State.ActIndex) State.ActIndex--;

            if (Tiles[location].Id == ElementList.PlayerId && World.EnergyCycles > 0)
            {
                World.Score += ElementAt(Actors[index].Location).Points;
                UpdateStatus();
            }
            else
            {
                Destroy(location);
                PlaySound(2, Sounds.EnemySuicide);
            }
        }

        public IBoard Board => _board.Value;

        public bool BroadcastLabel(int sender, string label, bool force)
        {
            var external = false;
            var success = false;

            if (sender < 0)
            {
                external = true;
                sender = -sender;
            }

            var info = new SearchContext
            {
                SearchIndex = 0,
                SearchOffset = 0,
                SearchTarget = label
            };

            while (ExecuteLabel(sender, info, "\x000D:"))
            {
                if (!ActorIsLocked(info.SearchIndex) || force || sender == info.SearchIndex && !external)
                {
                    if (sender == info.SearchIndex) success = true;

                    Actors[info.SearchIndex].Instruction = info.SearchOffset;
                }

                info.SearchTarget = label;
            }

            return success;
        }

        public ICheatList CheatList => _cheats.Value;

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
            Flags.Clear();
            SetBoard(0);
            Board.Name = "Introduction screen";
            World.Name = string.Empty;
            State.WorldFileName = string.Empty;
        }

        public IColors Colors => _colors.Value;

        public ICommands Commands => _commands.Value;

        public IConditionList ConditionList => _conditions.Value;

        public IConfig Config => _config.Value;

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
                surrounding[i] = Tiles[center.Sum(GetConveyorVector(i))].Clone();
                var element = ElementList[surrounding[i].Id];
                if (element.Id == ElementList.EmptyId)
                    pushable = true;
                else if (!element.IsPushable)
                    pushable = false;
            }

            for (var i = beginIndex; i != endIndex; i += direction)
            {
                var element = ElementList[surrounding[i].Id];

                if (pushable)
                {
                    if (element.IsPushable)
                    {
                        var source = center.Sum(GetConveyorVector(i));
                        var target = center.Sum(GetConveyorVector((i + 8 - direction) % 8));
                        if (element.Cycle > -1)
                        {
                            var tile = Tiles[source];
                            var index = ActorIndexAt(source);
                            Tiles[source].CopyFrom(surrounding[i]);
                            Tiles[target].Id = ElementList.EmptyId;
                            MoveActor(index, target);
                            Tiles[source].CopyFrom(tile);
                        }
                        else
                        {
                            Tiles[target].CopyFrom(surrounding[i]);
                            UpdateBoard(target);
                        }

                        if (!ElementList[surrounding[(i + 8 + direction) % 8].Id].IsPushable)
                        {
                            Tiles[source].Id = ElementList.EmptyId;
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
                    if (element.Id == ElementList.EmptyId)
                        pushable = true;
                }
            }
        }

        public void Destroy(IXyPair location)
        {
            var index = ActorIndexAt(location);
            if (index == -1)
                RemoveItem(location);
            else
                Harm(index);
        }

        public IDirectionList DirectionList => _directions.Value;

        public AnsiChar Draw(IXyPair location)
        {
            if (Board.IsDark && !ElementAt(location).IsAlwaysVisible &&
                (World.TorchCycles <= 0 || Distance(Player.Location, location) >= 50) && !State.EditorMode)
                return new AnsiChar(0xB0, 0x07);

            var tile = Tiles[location];
            var element = ElementList[tile.Id];
            var elementCount = ElementList.Count;

            if (tile.Id == ElementList.EmptyId) return new AnsiChar(0x20, 0x0F);

            if (element.HasDrawCode)
                return DrawList.Get(tile.Id).Draw(location);

            if (tile.Id < elementCount - 7) return new AnsiChar(element.Character, tile.Color);

            if (tile.Id != elementCount - 1)
                return new AnsiChar(tile.Color, ((tile.Id - (elementCount - 8)) << 4) | 0x0F);

            return new AnsiChar(tile.Color, 0x0F);
        }

        public IDrawList DrawList => _drawList.Value;

        public IElement ElementAt(IXyPair location)
        {
            return ElementList[Tiles[location].Id];
        }

        public IElementList ElementList => _elements.Value;

        public ISound EncodeMusic(string music)
        {
            return new Sound();
        }

        public void EnterBoard()
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

        public void ExecuteCode(int index, IExecutable instructionSource, string name)
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
                        Parser.ReadLine(index, context);
                        break;
                    case 0x2F: // /
                    case 0x3F: // ?
                        if (command == 0x2F)
                            context.Repeat = true;

                        var vector = Parser.GetDirection(context);
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
                        Interpreter.Execute(context);
                        break;
                    case 0x0D: // enter
                        if (context.Message.Count > 0)
                            context.Message.Add(string.Empty);
                        break;
                    case 0x00:
                        context.Finished = true;
                        break;
                    default:
                        context.Message.Add(command.ToStringValue() + Parser.ReadLine(context.Index, context));
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
            var success = false;
            var split = label.IndexOf(':');

            if (split > 0)
            {
                var target = label.Substring(0, split);
                label = label.Substring(split + 1);
                context.SearchTarget = target;
                success = Parser.GetTarget(context);
            }
            else if (context.SearchIndex < sender)
            {
                context.SearchIndex = sender;
                split = 0;
                success = true;
            }

            while (true)
            {
                if (!success) break;

                if (label.ToUpper() == @"RESTART")
                {
                    context.SearchOffset = 0;
                }
                else
                {
                    context.SearchOffset = Parser.Search(context.SearchIndex, prefix + label);
                    if (context.SearchOffset < 0 && split > 0)
                    {
                        success = Parser.GetTarget(context);
                        continue;
                    }
                }

                success = context.SearchOffset >= 0;
                break;
            }

            return success;
        }

        public bool ExecuteTransaction(IOopContext context, bool take)
        {
            // Does the item exist?
            var item = Parser.GetItem(context);
            if (item == null)
                return false;

            // Do we have a valid amount?
            var amount = Parser.ReadNumber(context.Index, context);
            if (amount <= 0)
                return true;

            // Modify value if we are taking.
            if (take)
                State.OopNumber = -State.OopNumber;

            // Determine if the result will be in range.
            var pendingAmount = item.Value + State.OopNumber;
            if ((pendingAmount & 0xFFFF) >= 0x8000)
                return true;

            // Successful transaction.
            item.Value = pendingAmount;
            return false;
        }

        public void FadePurple()
        {
            FadeBoard(new AnsiChar(0xDB, 0x05));
            Hud.RedrawBoard();
        }

        public bool FindTile(ITile kind, IXyPair location)
        {
            location.X++;
            while (location.Y <= Tiles.Height)
            {
                while (location.X <= Tiles.Width)
                {
                    var tile = Tiles[location];
                    if (tile.Id == kind.Id)
                        if (kind.Color == 0 || ColorMatch(Tiles[location]) == kind.Color)
                            return true;

                    location.X++;
                }

                location.X = 1;
                location.Y++;
            }

            return false;
        }

        public IFlags Flags => _flags.Value;

        public void ForcePlayerColor(int index)
        {
            var actor = Actors[index];
            var playerElement = ElementList[ElementList.PlayerId];
            if (Tiles[actor.Location].Color != playerElement.Color ||
                playerElement.Character != 0x02)
            {
                playerElement.Character = 2;
                Tiles[actor.Location].Color = playerElement.Color;
                UpdateBoard(actor.Location);
            }
        }

        public IXyPair GetCardinalVector(int index)
        {
            return new Vector(State.Vector4[index], State.Vector4[index + 4]);
        }

        public bool GetPlayerTimeElapsed(int interval)
        {
            var result = false;
            while (GetTimeDifference(ClockTick, State.PlayerTime) > 0)
            {
                result = true;
                State.PlayerTime = (State.PlayerTime + interval) & 0x7FFF;
            }

            return result;
        }

        public void HandlePlayerInput(IActor actor, int hotkey)
        {
            Features.HandlePlayerInput(actor, hotkey);
        }

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
                    var color = Tiles[actor.Location].Color;
                    color &= 0x0F;
                    color |= 0x70;
                    Tiles[actor.Location].Color = color;
                    if (World.Health > 0)
                    {
                        World.TimePassed = 0;
                        if (Board.RestartOnZap)
                        {
                            PlaySound(4, Sounds.TimeOut);
                            Tiles[actor.Location].Id = ElementList.EmptyId;
                            UpdateBoard(actor.Location);
                            var oldLocation = actor.Location.Clone();
                            actor.Location.CopyFrom(Board.Entrance);
                            UpdateRadius(oldLocation, 0);
                            UpdateRadius(actor.Location, 0);
                            State.GamePaused = true;
                        }

                        PlaySound(4, Sounds.Ouch);
                    }
                    else
                    {
                        PlaySound(5, Sounds.GameOver);
                    }
                }
            }
            else
            {
                var element = Tiles[actor.Location].Id;
                if (element == ElementList.BulletId)
                    PlaySound(3, Sounds.BulletDie);
                else if (element != ElementList.ObjectId) PlaySound(3, Sounds.EnemyDie);

                RemoveActor(index);
            }
        }

        public void LoadWorld(string name)
        {
            using (var stream = new MemoryStream(Disk.GetFile(Features.GetWorldName(name))))
            using (var reader = new BinaryReader(stream))
            {
                var type = reader.ReadInt16();
                if (type != World.WorldType)
                    throw new Exception("Incompatible world for this engine.");

                var numBoards = reader.ReadInt16();
                if (numBoards < 0)
                    throw new Exception("Board count must be zero or greater.");

                GameSerializer.LoadWorld(stream);

                var newBoards = Enumerable
                    .Range(0, numBoards + 1)
                    .Select(i => new PackedBoard(GameSerializer.LoadBoardData(stream)));
                
                Boards.Clear();
                
                foreach (var rawBoard in newBoards)
                    Boards.Add(rawBoard);
            }
            
            UnpackBoard(World.BoardIndex);
            State.WorldLoaded = true;
        }

        public IHud Hud => _hud.Value;

        public IItemList ItemList => _items.Value;

        public void LockActor(int index)
        {
            Features.LockActor(index);
        }

        public void MoveActor(int index, IXyPair target)
        {
            var actor = Actors[index];
            var sourceLocation = actor.Location.Clone();
            var sourceTile = Tiles[actor.Location];
            var targetTile = Tiles[target];
            var underTile = actor.UnderTile.Clone();

            actor.UnderTile.CopyFrom(targetTile);
            if (targetTile.Id == ElementList.EmptyId)
                targetTile.SetTo(sourceTile.Id, sourceTile.Color & 0x0F);
            else
                targetTile.SetTo(sourceTile.Id, (targetTile.Color & 0x70) | (sourceTile.Color & 0x0F));

            sourceTile.CopyFrom(underTile);
            actor.Location.CopyFrom(target);
            if (targetTile.Id == ElementList.PlayerId) ForcePlayerColor(index);

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
                    for (var y = target.Y - 8; y <= target.Y + 8; y++)
                    {
                        glowLocation.SetTo(x, y);
                        if (glowLocation.X >= 1 && glowLocation.X <= Tiles.Width && glowLocation.Y >= 1 &&
                            glowLocation.Y <= Tiles.Height)
                            if ((Distance(sourceLocation, glowLocation) < 50) ^
                                (Distance(target, glowLocation) < 50))
                                UpdateBoard(glowLocation);
                    }
                }
            }

            if (index == 0) Hud.UpdateCamera();
        }

        public void MoveActorOnRiver(int index)
        {
            var actor = Actors[index];
            var vector = new Vector();
            var underId = actor.UnderTile.Id;

            if (underId == ElementList.RiverNId)
                vector.SetTo(0, -1);
            else if (underId == ElementList.RiverSId)
                vector.SetTo(0, 1);
            else if (underId == ElementList.RiverWId)
                vector.SetTo(-1, 0);
            else if (underId == ElementList.RiverEId) vector.SetTo(1, 0);

            var actorTile = Tiles[actor.Location];
            if (actorTile.Id == ElementList.PlayerId)
                InteractionList.Get(actorTile.Id).Interact(actor.Location.Sum(vector), 0, vector);

            if (vector.IsNonZero())
            {
                var target = actor.Location.Sum(vector);
                if (ElementAt(target).IsFloor) MoveActor(index, target);
            }
        }

        public IParser Parser => _parser.Value;

        public IActor Player => Actors[0];

        public void PlaySound(int priority, ISound sound)
        {
            PlaySound(priority, sound, 0, sound.Length);
        }

        public void PlaySound(int priority, ISound sound, int offset, int length)
        {
        }

        public void PlotTile(IXyPair location, ITile tile)
        {
            if (ElementAt(location).Id == ElementList.PlayerId)
                return;

            var targetElement = ElementList[tile.Id];
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
                    existingTile.SetTo(targetElement.Id, targetColor);
                else
                    SpawnActor(location, new Tile(targetElement.Id, targetColor), targetElement.Cycle,
                        State.DefaultActor);
            }

            UpdateBoard(location);
        }

        public void Push(IXyPair location, IXyPair vector)
        {
            // this is here to prevent endless push loops
            // but doesn't exist in the original code
            if (vector.IsZero()) throw Exceptions.PushStackOverflow;

            var tile = Tiles[location];
            if (tile.Id == ElementList.SliderEwId && vector.Y == 0 ||
                tile.Id == ElementList.SliderNsId && vector.X == 0 ||
                ElementList[tile.Id].IsPushable)
            {
                var furtherTile = Tiles[location.Sum(vector)];
                if (furtherTile.Id == ElementList.TransporterId)
                    PushThroughTransporter(location, vector);
                else if (furtherTile.Id != ElementList.EmptyId) Push(location.Sum(vector), vector);

                var furtherElement = ElementList[furtherTile.Id];
                if (!furtherElement.IsFloor && furtherElement.IsDestructible && furtherTile.Id != ElementList.PlayerId)
                    Destroy(location.Sum(vector));

                furtherElement = ElementList[furtherTile.Id];
                if (furtherElement.IsFloor) MoveTile(location, location.Sum(vector));
            }
        }

        public void PushThroughTransporter(IXyPair location, IXyPair vector)
        {
            var actor = ActorAt(location.Sum(vector));

            if (actor.Vector.Matches(vector))
            {
                var search = actor.Location.Clone();
                var target = new Location();
                var ended = false;
                var success = true;

                while (!ended)
                {
                    search.Add(vector);
                    var element = ElementAt(search);
                    if (element.Id == ElementList.BoardEdgeId)
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
                                element = ElementAt(search);
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

                    if (element.Id == ElementList.TransporterId)
                        if (ActorAt(search).Vector.Matches(vector.Opposite()))
                            success = true;
                }

                if (target.X > 0)
                {
                    MoveTile(actor.Location.Difference(vector), target);
                    PlaySound(3, Sounds.Transporter);
                }
            }
        }

        public void PutTile(IXyPair location, IXyPair vector, ITile kind)
        {
            if (!Features.CanPutTile(location))
                return;

            if (location.X >= 1 && location.X <= Tiles.Width && location.Y >= 1 &&
                location.Y <= Tiles.Height)
            {
                if (!ElementAt(location).IsFloor) Push(location, vector);
                PlotTile(location, kind);
            }
        }

        public void RaiseError(string error)
        {
            SetMessage(0xC8, Alerts.ErrorMessage(error));
            PlaySound(5, Sounds.Error);
        }

        public IRandom Random => _random.Value;

        public int ReadKey()
        {
            var key = Keyboard.GetKey();
            State.KeyPressed = key > 0 ? key : 0;
            return State.KeyPressed;
        }

        public void RemoveActor(int index)
        {
            var actor = Actors[index];
            if (index < State.ActIndex) State.ActIndex--;

            Tiles[actor.Location].CopyFrom(actor.UnderTile);
            if (actor.Location.Y > 0) UpdateBoard(actor.Location);

            for (var i = 1; i <= State.ActorCount; i++)
            {
                var a = Actors[i];
                if (a.Follower >= index)
                {
                    if (a.Follower == index)
                        a.Follower = -1;
                    else
                        a.Follower--;
                }

                if (a.Leader >= index)
                {
                    if (a.Leader == index)
                        a.Leader = -1;
                    else
                        a.Leader--;
                }
            }

            if (index < State.ActorCount)
                for (var i = index; i < State.ActorCount; i++)
                    Actors[i].CopyFrom(Actors[i + 1]);

            State.ActorCount--;
        }

        public void RemoveItem(IXyPair location)
        {
            Tiles[location].Id = ElementList.EmptyId;
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
                Random.Synced(2) == 0
                    ? vector.Clockwise()
                    : vector.CounterClockwise());
            return result;
        }

        public IXyPair Seek(IXyPair location)
        {
            var result = new Vector();
            if (Random.Synced(2) == 0 || Player.Location.Y == location.Y)
                result.X = (Player.Location.X - location.X).Polarity();

            if (result.X == 0) result.Y = (Player.Location.Y - location.Y).Polarity();

            if (World.EnergyCycles > 0) result.SetOpposite();

            return result;
        }

        public void SetBoard(int boardIndex)
        {
            var element = ElementList[ElementList.PlayerId];
            Tiles[Player.Location].SetTo(element.Id, element.Color);
            PackBoard();
            UnpackBoard(boardIndex);
        }

        public void SetEditorMode()
        {
            InitializeElements(true);
            State.EditorMode = true;
        }

        public void SetGameMode()
        {
            InitializeElements(false);
            State.EditorMode = false;
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

            SpawnActor(new Location(0, 0), new Tile(ElementList.MessengerId, 0), 1, State.DefaultActor);
            Actors[State.ActorCount].P2 = duration / (State.GameWaitTime + 1);
            State.Message = topMessage;
            State.Message2 = bottomMessage;
        }

        public void ShowHelp(string filename)
        {
        }

        public void ShowInGameHelp()
        {
            ShowHelp("GAME");
        }

        public ISounds Sounds => _sounds.Value;

        public void SpawnActor(IXyPair location, ITile tile, int cycle, IActor source)
        {
            // must reserve one actor for player, and one for messenger
            if (State.ActorCount < Actors.Capacity - 2)
            {
                State.ActorCount++;
                var actor = Actors[State.ActorCount];

                if (source == null) source = State.DefaultActor;

                actor.CopyFrom(source);
                actor.Location.CopyFrom(location);
                actor.Cycle = cycle;
                actor.UnderTile.CopyFrom(Tiles[location]);
                if (ElementAt(actor.Location).IsEditorFloor)
                {
                    var newColor = Tiles[actor.Location].Color & 0x70;
                    newColor |= tile.Color & 0x0F;
                    Tiles[actor.Location].Color = newColor;
                }
                else
                {
                    Tiles[actor.Location].Color = tile.Color;
                }

                Tiles[actor.Location].Id = tile.Id;
                if (actor.Location.Y > 0) UpdateBoard(actor.Location);
            }
        }

        public bool SpawnProjectile(int id, IXyPair location, IXyPair vector, bool enemyOwned)
        {
            var target = location.Sum(vector);
            var element = ElementAt(target);

            if (element.IsFloor || element.Id == ElementList.WaterId)
            {
                SpawnActor(target, new Tile(id, ElementList[id].Color), 1, State.DefaultActor);
                var actor = Actors[State.ActorCount];
                actor.P1 = enemyOwned ? 1 : 0;
                actor.Vector.CopyFrom(vector);
                actor.P2 = 0x64;
                return true;
            }

            if (element.Id != ElementList.BreakableId &&
                (!element.IsDestructible ||
                 (element.Id != ElementList.PlayerId || World.EnergyCycles != 0) && enemyOwned))
                return false;

            Destroy(target);
            PlaySound(2, Sounds.BulletDie);
            return true;
        }

        public void Start()
        {
            if (!ThreadActive)
            {
                ThreadActive = true;
                Thread = new Thread(StartMain);
                ClockTick = Clock.Tick;
                Thread.Start();
            }
        }

        public IState State => _state.Value;

        public void Stop()
        {
            if (ThreadActive) ThreadActive = false;
        }

        public ITargetList TargetList => _targets.Value;

        public ITiles Tiles => _tiles.Value;

        public bool TitleScreen => State.PlayerElement != ElementList.PlayerId;

        public void UnlockActor(int index)
        {
            Features.UnlockActor(index);
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
            for (var y = top; y <= bottom; y++)
                if (x >= 1 && x <= Tiles.Width && y >= 1 && y <= Tiles.Height)
                {
                    var target = new Location(x, y);
                    if (mode != RadiusMode.Update)
                        if (Distance(source, target) < 50)
                        {
                            var element = ElementAt(target);
                            if (mode == RadiusMode.Explode)
                            {
                                if (element.CodeEditText.Length > 0)
                                {
                                    var actorIndex = ActorIndexAt(target);
                                    if (actorIndex > 0) BroadcastLabel(-actorIndex, KnownLabels.Bombed, false);
                                }

                                if (element.IsDestructible || element.Id == ElementList.StarId) Destroy(target);

                                if (element.Id == ElementList.EmptyId || element.Id == ElementList.BreakableId)
                                    Tiles[target].SetTo(ElementList.BreakableId, Random.Synced(7) + 9);
                            }
                            else
                            {
                                if (Tiles[target].Id == ElementList.BreakableId) Tiles[target].Id = ElementList.EmptyId;
                            }
                        }

                    UpdateBoard(target);
                }
        }

        public void UpdateStatus()
        {
            Hud.UpdateStatus();
        }

        public void WaitForTick()
        {
            while (ClockTick == ClockBase && ThreadActive) Thread.Sleep(1);

            ClockTick++;
        }

        public void ClearForest(IXyPair location) => Features.ClearForest(location);

        public void CleanUpPassageMovement() => Features.CleanUpPassageMovement();

        public IActionList ActionList 
            => _actionList.Value;
        
        public IInteractionList InteractionList 
            => _interactionList.Value;
        
        public IWorld World 
            => _world.Value;

        private bool ActorIsLocked(int index) 
            => Features.IsActorLocked(index);

        private void ClearBoard()
        {
            var emptyId = ElementList.EmptyId;
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
                Tiles[new Location(0, y)].Id = boardEdgeId;
                Tiles[new Location(Tiles.Width + 1, y)].Id = boardEdgeId;
            }

            for (var x = 0; x <= Tiles.Width + 1; x++)
            {
                Tiles[new Location(x, 0)].Id = boardEdgeId;
                Tiles[new Location(x, Tiles.Height + 1)].Id = boardEdgeId;
            }

            // clear out board
            for (var x = 1; x <= Tiles.Width; x++)
            for (var y = 1; y <= Tiles.Height; y++)
                Tiles[new Location(x, y)].SetTo(emptyId, 0);

            // build border
            for (var y = 1; y <= Tiles.Height; y++)
            {
                Tiles[new Location(1, y)].SetTo(boardBorderId, boardBorderColor);
                Tiles[new Location(Tiles.Width, y)].SetTo(boardBorderId, boardBorderColor);
            }

            for (var x = 1; x <= Tiles.Width; x++)
            {
                Tiles[new Location(x, 1)].SetTo(boardBorderId, boardBorderColor);
                Tiles[new Location(x, Tiles.Height)].SetTo(boardBorderId, boardBorderColor);
            }

            // generate player actor
            var element = ElementList[ElementList.PlayerId];
            State.ActorCount = 0;
            Player.Location.SetTo(Tiles.Width / 2, Tiles.Height / 2);
            Tiles[Player.Location].SetTo(element.Id, element.Color);
            Player.Cycle = 1;
            Player.UnderTile.SetTo(0, 0);
            Player.Pointer = 0;
            Player.Length = 0;
        }

        private int ColorMatch(ITile tile)
        {
            var element = ElementList[tile.Id];

            if (element.Color < 0xF0)
                return element.Color & 7;
            if (element.Color == 0xFE)
                return ((tile.Color >> 4) & 0x0F) + 8;
            return tile.Color & 0x0F;
        }

        private int Distance(IXyPair a, IXyPair b)
        {
            return (a.Y - b.Y).Square() * 2 + (a.X - b.X).Square();
        }

        private void DrawTile(IXyPair location, AnsiChar ac)
        {
            Hud.DrawTile(location.X - 1, location.Y - 1, ac);
        }

        private void EnterHighScore(int score)
        {
        }

        private void ExecuteDeath(IOopContext context)
        {
            var location = context.Actor.Location.Clone();
            Harm(context.Index);
            PlotTile(location, context.DeathTile);
        }

        private void ExecuteDirection(IOopContext context, IXyPair vector)
        {
            if (vector.IsZero())
            {
                context.Repeat = false;
            }
            else
            {
                var target = context.Actor.Location.Sum(vector);
                if (!ElementAt(target).IsFloor) Push(target, vector);

                if (ElementAt(target).IsFloor)
                {
                    MoveActor(context.Index, target);
                    context.Repeat = false;
                }
            }
        }

        private void ExecuteMessage(IOopContext context)
        {
            Features.ExecuteMessage(context);
        }

        private void ExecuteOnce()
        {
            if (State.ActIndex > State.ActorCount)
            {
                if (!State.BreakGameLoop && !State.GamePaused)
                    if (State.GameWaitTime <= 0 || Timers.Player.Clock(State.GameWaitTime))
                    {
                        State.GameCycle++;
                        if (State.GameCycle > 420) State.GameCycle = 1;

                        State.ActIndex = 0;
                        ReadInput();
                    }

                WaitForTick();
            }
        }

        private void FadeBoard(AnsiChar ac)
        {
            Hud.FadeBoard(ac);
        }

        public void FadeRed()
        {
            FadeBoard(new AnsiChar(0xDB, 0x04));
            Hud.RedrawBoard();
        }

        private IXyPair GetConveyorVector(int index)
        {
            return new Vector(State.Vector8[index], State.Vector8[index + 8]);
        }

        private int GetTimeDifference(int now, int then)
        {
            now &= 0x7FFF;
            then &= 0x7FFF;
            if (now < 0x4000 && then >= 0x4000) now += 0x8000;

            return now - then;
        }

        private string GetWorldName(string baseName)
        {
            return Features.GetWorldName(baseName);
        }

        private void InitializeElements(bool showInvisibles)
        {
            // this isn't all the initializations.
            // todo: replace this with the ability to completely reinitialize engine default memory
            ElementList[ElementList.InvisibleId].Character = showInvisibles ? 0xB0 : 0x20;
            ElementList[ElementList.InvisibleId].Color = 0xFF;
            ElementList[ElementList.PlayerId].Character = 0x02;
        }

        private byte[] LoadFile(string filename)
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
                if (!State.AboutShown) ShowAbout();

                if (State.DefaultWorldName.Length <= 0)
                {
                    // normally we would load the world here,
                    // however it will have already been loaded in the context
                }

                State.StartBoard = World.BoardIndex;
                SetBoard(0);
                State.Init = false;
            }

            var element = ElementList[State.PlayerElement];
            Tiles[Player.Location].SetTo(element.Id, element.Color);
            if (State.PlayerElement == ElementList.MonitorId)
            {
                SetMessage(0, new Message());
                Hud.DrawTitleStatus();
            }

            if (gameIsActive) FadePurple();

            State.GameWaitTime = State.GameSpeed << 1;
            State.BreakGameLoop = false;
            State.GameCycle = Random.Synced(0x64);
            State.ActIndex = State.ActorCount + 1;

            while (ThreadActive)
            {
                if (!State.GamePaused)
                {
                    if (State.ActIndex <= State.ActorCount)
                    {
                        var actorData = Actors[State.ActIndex];
                        if (actorData.Cycle != 0)
                            if (State.ActIndex % actorData.Cycle == State.GameCycle % actorData.Cycle)
                                ActionList.Get(Tiles[actorData.Location].Id).Act(State.ActIndex);

                        State.ActIndex++;
                    }
                }
                else
                {
                    State.ActIndex = State.ActorCount + 1;
                    if (Timers.Player.Clock(25)) alternating = !alternating;

                    if (alternating)
                    {
                        var playerElement = ElementList[ElementList.PlayerId];
                        DrawTile(Player.Location, new AnsiChar(playerElement.Character, playerElement.Color));
                    }
                    else
                    {
                        if (Tiles[Player.Location].Id == ElementList.PlayerId)
                            DrawTile(Player.Location, new AnsiChar(0x20, 0x0F));
                        else
                            UpdateBoard(Player.Location);
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
                        InteractionList.Get(ElementAt(target).Id).Interact(target, 0, State.KeyVector);
                    }

                    if (!State.KeyVector.IsZero())
                    {
                        var target = Player.Location.Sum(State.KeyVector);
                        if (ElementAt(target).IsFloor)
                        {
                            if (ElementAt(Player.Location).Id == ElementList.PlayerId)
                            {
                                MoveActor(0, target);
                            }
                            else
                            {
                                UpdateBoard(Player.Location);
                                Player.Location.Add(State.KeyVector);
                                Tiles[Player.Location].SetTo(ElementList.PlayerId, ElementList[ElementList.PlayerId].Color);
                                UpdateBoard(Player.Location);
                                UpdateRadius(Player.Location, RadiusMode.Update);
                                UpdateRadius(Player.Location.Difference(State.KeyVector), RadiusMode.Update);
                            }

                            State.GamePaused = false;
                            Hud.ClearPausing();
                            State.GameCycle = Random.Synced(100);
                            World.IsLocked = true;
                        }
                    }
                }

                ExecuteOnce();

                if (State.BreakGameLoop)
                {
                    ClearSound();
                    if (State.PlayerElement == ElementList.PlayerId)
                    {
                        if (World.Health <= 0) EnterHighScore(World.Score);
                    }
                    else if (State.PlayerElement == ElementList.MonitorId)
                    {
                        Hud.ClearTitleStatus();
                    }

                    element = ElementList[ElementList.PlayerId];
                    Tiles[Player.Location].SetTo(element.Id, element.Color);
                    State.GameOver = false;
                    break;
                }
            }
        }

        private void MoveTile(IXyPair source, IXyPair target)
        {
            var sourceIndex = ActorIndexAt(source);
            if (sourceIndex >= 0)
            {
                MoveActor(sourceIndex, target);
            }
            else
            {
                Tiles[target].CopyFrom(Tiles[source]);
                UpdateBoard(target);
                RemoveItem(source);
                UpdateBoard(source);
            }
        }

        private void PackBoard()
        {
            var board = new PackedBoard(GameSerializer.PackBoard(Tiles));
            Boards[World.BoardIndex] = board;
        }

        private bool PlayWorld()
        {
            var gameIsActive = false;

            if (World.IsLocked)
            {
                var file = LoadFile(GetWorldName(string.IsNullOrWhiteSpace(World.Name)
                    ? State.WorldFileName
                    : World.Name));
                if (file != null)
                {
                    gameIsActive = State.WorldLoaded;
                    State.StartBoard = World.BoardIndex;
                }
            }
            else
            {
                gameIsActive = true;
            }

            if (gameIsActive)
            {
                SetBoard(State.StartBoard);
                EnterBoard();
                State.PlayerElement = ElementList.PlayerId;
                State.GamePaused = true;
                MainLoop(true);
            }

            return gameIsActive;
        }

        private int ReadActorCodeByte(int index, IExecutable instructionSource)
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

        private void ReadInput()
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
            result.X = Random.Synced(3) - 1;
            if (result.X == 0)
                result.Y = (Random.Synced(2) << 1) - 1;
            else
                result.Y = 0;
        }

        private void ShowAbout()
        {
            ShowHelp("ABOUT");
        }

        private void StartInit()
        {
            State.GameSpeed = 4;
            State.DefaultSaveName = "SAVED";
            State.DefaultBoardName = "TEMP";
            State.DefaultWorldName = "TOWN";
            
            ClearWorld();

            var worldToLoad = State.DefaultWorldName;

            var zztCfg = Disk.GetFile("ZZT.CFG");
            if (zztCfg != null && zztCfg.Length > 0)
            {
                using (var stream = new MemoryStream(zztCfg))
                using (var reader = new StreamReader(stream))
                {
                    worldToLoad = reader.ReadLine();
                }
            }

            LoadWorld(worldToLoad);

            if (!State.WorldLoaded)
                ClearWorld();

            if (State.EditorMode)
                SetEditorMode();
            else
                SetGameMode();
        }

        private void StartMain()
        {
            StartInit();
            TitleScreenLoop();
        }

        private void StopSound()
        {
        }

        private void TitleScreenLoop()
        {
            State.QuitZzt = false;
            State.Init = true;
            State.StartBoard = 0;
            var gameEnded = true;
            Hud.Initialize();
            while (ThreadActive)
            {
                if (!State.Init) SetBoard(0);

                while (ThreadActive)
                {
                    State.PlayerElement = ElementList.MonitorId;
                    State.GamePaused = false;
                    MainLoop(gameEnded);
                    if (!ThreadActive) break;

                    var hotkey = State.KeyPressed.ToUpperCase();
                    var startPlaying = Features.HandleTitleInput(hotkey);
                    if (startPlaying) gameEnded = PlayWorld();

                    if (gameEnded || State.QuitZzt) break;
                }

                if (State.QuitZzt) break;
            }
        }

        private void UnpackBoard(int boardIndex)
        {
            GameSerializer.UnpackBoard(Tiles, Boards[boardIndex].Data);
            World.BoardIndex = boardIndex;
        }
    }
}