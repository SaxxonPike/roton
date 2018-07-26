using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Core.Impl
{
    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public sealed class Engine : IEngine, IDisposable
    {
        private readonly Lazy<IActionList> _actionList;
        private readonly Lazy<IActors> _actors;
        private readonly Lazy<IAlerts> _alerts;
        private readonly Lazy<IBoard> _board;
        private readonly Lazy<IBoards> _boards;
        private readonly Lazy<ICheatList> _cheats;
        private readonly Lazy<IClock> _clock;
        private readonly Lazy<IColors> _colors;
        private readonly Lazy<ICommandList> _commands;
        private readonly Lazy<IConditionList> _conditions;
        private readonly Lazy<IConfig> _config;
        private readonly Lazy<IDirectionList> _directions;
        private readonly Lazy<IDrawList> _drawList;
        private readonly Lazy<IElementList> _elements;
        private readonly Lazy<IFacts> _facts;
        private readonly Lazy<IMemory> _memory;
        private readonly Lazy<IHeap> _heap;
        private readonly Lazy<IAnsiKeyTransformer> _ansiKeyTransformer;
        private readonly Lazy<IScrollFormatter> _scrollFormatter;
        private readonly Lazy<ISpeaker> _speaker;
        private readonly Lazy<IDrumBank> _drumBank;
        private readonly Lazy<IFeatures> _features;
        private readonly Lazy<IFileSystem> _fileSystem;
        private readonly Lazy<IGameSerializer> _gameSerializer;
        private readonly Lazy<IHud> _hud;
        private readonly Lazy<IInteractionList> _interactionList;
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

        private int _ticksToRun;

        public Engine(Lazy<IClockFactory> clockFactory, Lazy<IActors> actors, Lazy<IAlerts> alerts, Lazy<IBoard> board,
            Lazy<IFileSystem> fileSystem, Lazy<IElementList> elements,
            Lazy<IInterpreter> interpreter, Lazy<IRandom> random, Lazy<IKeyboard> keyboard,
            Lazy<ITiles> tiles, Lazy<ISounds> sounds, Lazy<ITimers> timers, Lazy<IParser> parser,
            Lazy<IConfig> config, Lazy<IConditionList> conditions, Lazy<IDirectionList> directions,
            Lazy<IColors> colors, Lazy<ICheatList> cheats, Lazy<ICommandList> commands, Lazy<ITargetList> targets,
            Lazy<IFeatures> features, Lazy<IGameSerializer> gameSerializer, Lazy<IHud> hud, Lazy<IState> state,
            Lazy<IWorld> world, Lazy<IItemList> items, Lazy<IBoards> boards, Lazy<IActionList> actionList,
            Lazy<IDrawList> drawList, Lazy<IInteractionList> interactionList, Lazy<IFacts> facts, Lazy<IMemory> memory,
            Lazy<IHeap> heap, Lazy<IAnsiKeyTransformer> ansiKeyTransformer, Lazy<IScrollFormatter> scrollFormatter,
            Lazy<ISpeaker> speaker, Lazy<IDrumBank> drumBank)
        {
            _clock = new Lazy<IClock>(() =>
            {
                var clock = clockFactory.Value.Create(10, 718);
                
                if (clock != null)
                    clock.OnTick += ClockTick;
                
                return clock;
            });

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
            _facts = facts;
            _memory = memory;
            _heap = heap;
            _ansiKeyTransformer = ansiKeyTransformer;
            _scrollFormatter = scrollFormatter;
            _speaker = speaker;
            _drumBank = drumBank;
        }

        private void ClockTick(object sender, EventArgs args)
        {
            if (_ticksToRun < 3) _ticksToRun++;
            if (!ThreadActive) Clock.Stop();
        }
        
        private IClock Clock => _clock.Value;

        private IBoards Boards => _boards.Value;

        private ITile BorderTile => State.BorderTile;

        private IFileSystem Disk => _fileSystem.Value;

        private IFeatures Features => _features.Value;

        private ISpeaker Speaker => _speaker.Value;

        private IGameSerializer GameSerializer => _gameSerializer.Value;

        private IInterpreter Interpreter => _interpreter.Value;

        private IKeyboard Keyboard => _keyboard.Value;

        private IScrollFormatter ScrollFormatter => _scrollFormatter.Value;

        public ITimers Timers => _timers.Value;
        
        public IDrumBank DrumBank => _drumBank.Value;

        private Thread Thread { get; set; }

        public bool ThreadActive { get; set; }

        public IActionList ActionList
            => _actionList.Value;

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

        public event EventHandler Tick;
        
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

        public void CleanUpPassageMovement()
        {
            Features.CleanUpPassageMovement();
        }

        public void ClearForest(IXyPair location)
        {
            Features.ClearForest(location);
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
            World.Ammo = Facts.DefaultAmmo;
            World.Gems = Facts.DefaultGems;
            World.Health = Facts.DefaultHealth;
            World.EnergyCycles = Facts.DefaultEnergyCycles;
            World.Torches = Facts.DefaultTorches;
            World.TorchCycles = Facts.DefaultTorchCycles;
            World.Score = Facts.DefaultScore;
            World.TimePassed = Facts.DefaultTimePassed;
            World.Stones = Facts.DefaultStones;
            World.Keys.Clear();
            World.Flags.Clear();
            SetBoard(0);
            Board.Name = Facts.DefaultBoardTitle;
            World.Name = Facts.DefaultWorldTitle;
            State.WorldFileName = string.Empty;
        }

        public IColors Colors => _colors.Value;

        public ICommandList CommandList => _commands.Value;

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
                (World.TorchCycles <= 0 || Distance(Player.Location, location) >= Facts.TorchRadius) &&
                !State.EditorMode)
                return Facts.DarknessTile;

            var tile = Tiles[location];
            var element = ElementList[tile.Id];
            var elementCount = ElementList.Count;

            if (tile.Id == ElementList.EmptyId)
                return Facts.EmptyTile;

            if (element.HasDrawCode)
                return DrawList.Get(tile.Id).Draw(location);

            if (tile.Id < elementCount - 7) return new AnsiChar(element.Character, tile.Color);

            return tile.Id != elementCount - 1
                ? new AnsiChar(tile.Color, ((tile.Id - (elementCount - 8)) << 4) | 0x0F)
                : new AnsiChar(tile.Color, 0x0F);
        }

        public IDrawList DrawList => _drawList.Value;

        public IElement ElementAt(IXyPair location)
        {
            return ElementList[Tiles[location].Id];
        }

        public IElementList ElementList => _elements.Value;

        public ISound EncodeMusic(string music)
        {
            var speed = 1;
            var octave = 3;
            var result = new List<int>();
            var isNote = false;
            var note = -1;

            foreach (var c in music.ToUpperInvariant())
            {
                if (!isNote)
                {
                    note = -1;
                }
                else
                {
                    switch (c)
                    {
                        case '!':
                            note--;
                            break;
                        case '#':
                            note++;
                            break;
                    }

                    isNote = false;
                    result.Add(note + (octave << 4));
                    result.Add(speed);
                }

                switch (c)
                {
                    case 'T':
                        speed = 1;
                        break;
                    case 'S':
                        speed = 2;
                        break;
                    case 'I':
                        speed = 4;
                        break;
                    case 'Q':
                        speed = 8;
                        break;
                    case 'H':
                        speed = 16;
                        break;
                    case 'W':
                        speed = 32;
                        break;
                    case '.':
                        speed = speed * 3 / 2;
                        break;
                    case '3':
                        speed = speed / 3;
                        break;
                    case '+':
                        if (octave < 6)
                            octave++;
                        break;
                    case '-':
                        if (octave > 1)
                            octave--;
                        break;
                    case 'C':
                        note = 0;
                        isNote = true;
                        break;
                    case 'D':
                        note = 2;
                        isNote = true;
                        break;
                    case 'E':
                        note = 4;
                        isNote = true;
                        break;
                    case 'F':
                        note = 5;
                        isNote = true;
                        break;
                    case 'G':
                        note = 7;
                        isNote = true;
                        break;
                    case 'A':
                        note = 9;
                        isNote = true;
                        break;
                    case 'B':
                        note = 11;
                        isNote = true;
                        break;
                    case 'X':
                        result.Add(0);
                        result.Add(speed);
                        break;
                    case '0':
                    case '1':
                    case '2':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        result.Add(0xF0 | (c - 0x30));
                        result.Add(speed);
                        break;
                }
            }

            if (isNote)
            {
                result.Add(note + (octave << 4));
                result.Add(speed);
            }

            return new Sound(result.Take(254).ToArray());
        }

        public void EnterBoard()
        {
            Features.EnterBoard();
        }

        public void ExecuteCode(int index, IExecutable instructionSource, string name)
        {
            var context = new OopContext(index, instructionSource, name, this)
            {
                Moved = false,
                Repeat = false,
                Died = false,
                Finished = false,
                CommandsExecuted = 0
            };

            context.PreviousInstruction = context.Instruction;

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

                if (label.ToUpper() == Facts.RestartLabel)
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

        public IFacts Facts => _facts.Value;

        public IHeap Heap => _heap.Value;

        public IMemory Memory => _memory.Value;

        public void StepOnce()
        {
            var oldThreadActive = ThreadActive;
            ThreadActive = true;
            MainLoop(true, true);
            ThreadActive = oldThreadActive;
        }

        public string[] GetMessageLines() => Features.GetMessageLines();

        public void FadePurple()
        {
            FadeBoard(Facts.FadeTile);
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

        public void ForcePlayerColor(int index)
        {
            Features.ForcePlayerColor(index);
        }

        public IXyPair GetCardinalVector(int index)
        {
            return new Vector(State.Vector4[index], State.Vector4[index + 4]);
        }

        public void HandlePlayerInput(IActor actor)
        {
            Features.HandlePlayerInput(actor);
        }

        public void Harm(int index)
        {
            var actor = Actors[index];
            if (index == 0)
            {
                if (World.Health > 0)
                {
                    World.Health -= Facts.HealthLostPerHit;
                    UpdateStatus();
                    SetMessage(Facts.ShortMessageDuration, Alerts.OuchMessage);
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

        public IHud Hud => _hud.Value;

        public IInteractionList InteractionList
            => _interactionList.Value;

        public IItemList ItemList => _items.Value;

        private void ShowFormattedScroll(string error)
        {
            Hud.ShowScroll("Roton Error", ScrollFormatter.Format(error));
        }

        public void LoadWorld(string name)
        {
            byte[] TryLoadWorld()
            {
                try
                {
                    return Disk.GetFile(Features.GetWorldName(name));
                }
                catch (IOException e)
                {
                    ShowFormattedScroll(e.ToString());
                    return new byte[0];
                }
            }

            var worldData = TryLoadWorld();

            if (worldData == null || worldData.Length == 0)
                return;

            using (var stream = new MemoryStream(worldData))
            {
                if (stream.Length == 0)
                    return;

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
                        .Select(i => new PackedBoard(GameSerializer.LoadBoardData(stream)))
                        .ToList();

                    Boards.Clear();

                    foreach (var rawBoard in newBoards)
                        Boards.Add(rawBoard);
                }
            }

            UnpackBoard(World.BoardIndex);
            State.WorldLoaded = true;
        }

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
            if (targetTile.Id == ElementList.PlayerId)
                ForcePlayerColor(index);

            UpdateBoard(target);
            UpdateBoard(sourceLocation);
            if (index == 0 && Board.IsDark)
            {
                var squareDistanceX = (target.X - sourceLocation.X).Square();
                var squareDistanceY = (target.Y - sourceLocation.Y).Square();
                if (squareDistanceX + squareDistanceY == 1)
                {
                    var glowLocation = new Location();
                    for (var x = target.X - Facts.TorchDrawBoxVerticalSize;
                        x <= target.X + Facts.TorchDrawBoxVerticalSize;
                        x++)
                    for (var y = target.Y - Facts.TorchDrawBoxHorizontalSize;
                        y <= target.Y + Facts.TorchDrawBoxHorizontalSize;
                        y++)
                    {
                        glowLocation.SetTo(x, y);
                        if (glowLocation.X >= 1 && glowLocation.X <= Tiles.Width && glowLocation.Y >= 1 &&
                            glowLocation.Y <= Tiles.Height)
                            if ((Distance(sourceLocation, glowLocation) < Facts.TorchRadius) ^
                                (Distance(target, glowLocation) < Facts.TorchRadius))
                                UpdateBoard(glowLocation);
                    }
                }
            }

            if (index == 0)
                Hud.UpdateCamera();
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
            if (State.GameOver)
                return;

            var soundIsNotPlaying = !State.SoundPlaying;
            var soundIsMusic = priority == -1;
            var soundIsHigherPriority = State.SoundPriority != -1 && priority >= State.SoundPriority;

            if (!(soundIsNotPlaying || soundIsMusic || soundIsHigherPriority))
                return;

            if (!soundIsMusic)
                State.SoundBuffer.Clear();

            State.SoundBuffer.Enqueue(sound);
            State.SoundPlaying = true;
            State.SoundPriority = priority;
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
            if (vector.IsZero())
                throw Exceptions.PushStackOverflow;

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
            SetMessage(Facts.LongMessageDuration, Alerts.ErrorMessage(error));
            PlaySound(5, Sounds.Error);
        }

        public IRandom Random => _random.Value;

//        private EngineKeyCode GetKeyCode()
//        {
//            if (!Keyboard.KeyIsAvailable)
//                return EngineKeyCode.None;
//            
//            var keyPress = Keyboard.GetKey();
//            if (keyPress >= 0x80 && Keyboard.KeyIsAvailable)
//            {
//                keyPress = Keyboard.GetKey();
//                keyPress |= 0x80;
//            }
//
//            State.KeyShift = Keyboard.Shift;
//            
//            return (EngineKeyCode) keyPress;
//        }
//        
//        public EngineKeyCode ReadKey()
//        {
//            var key = GetKeyCode();
//            State.KeyPressed = key;
//            return State.KeyPressed;
//        }

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
            Features.RemoveItem(location);
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
            try
            {
                var text = Disk
                    .GetFile($"{filename}.HLP")
                    .ToStringValue()
                    .Split('\xD');
                Hud.ShowScroll(filename, text);
            }
            catch (Exception e)
            {
                ShowFormattedScroll(e.ToString());
            }
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
                _ticksToRun = 0;
                ThreadActive = true;
                Thread = new Thread(StartMain);
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
                        if (Distance(source, target) < Facts.TorchRadius)
                        {
                            var element = ElementAt(target);
                            if (mode == RadiusMode.Explode)
                            {
                                if (element.CodeEditText.Length > 0)
                                {
                                    var actorIndex = ActorIndexAt(target);
                                    if (actorIndex > 0) BroadcastLabel(-actorIndex, Facts.BombedLabel, false);
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
            if (Clock == null)
                return;
            
            if (State.SoundPlaying)
            {
                if (State.SoundTicks <= 0)
                {
                    if (State.SoundBuffer.Count > 0)
                    {
                        var sound = State.SoundBuffer.Dequeue();
                        State.SoundTicks = sound.Duration << 2;
                        if (sound.Note >= 0xF0)
                        {
                            Speaker.PlayDrum(sound.Note - 0xF0);                                
                        }
                        else if (sound.Note > 0x00)
                        {
                            var actualNote = (sound.Note & 0xF) + (sound.Note >> 4) * 12;
                            Speaker.PlayNote(actualNote);                                
                        }
                        else
                        {
                            Speaker.StopNote();                                
                        }
                    }
                    else
                    {
                        State.SoundPlaying = false;
                        State.SoundPriority = 0;
                        Speaker.StopNote();
                    }
                }

                if (State.SoundPlaying)
                    State.SoundTicks--;
            }
                
            Tick?.Invoke(this, EventArgs.Empty);

            while (_ticksToRun <= 0 && ThreadActive)
                Thread.Sleep(1);

            _ticksToRun--;
        }

        public IWorld World
            => _world.Value;

        private bool ActorIsLocked(int index)
        {
            return Features.IsActorLocked(index);
        }

        public void ClearBoard()
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

        private void FadeBoard(AnsiChar ac)
        {
            Hud.FadeBoard(ac);
        }

        public void FadeRed()
        {
            FadeBoard(Facts.ErrorFadeTile);
            Hud.RedrawBoard();
        }

        private IXyPair GetConveyorVector(int index)
        {
            return new Vector(State.Vector8[index], State.Vector8[index + 8]);
        }

        private string GetWorldName(string baseName)
        {
            return Features.GetWorldName(baseName);
        }

        private void InitializeElements(bool showInvisibles)
        {
            ElementList.Reset();

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

        private void MainLoop(bool doFade, bool step)
        {
            var alternating = false;

            if (!step)
            {
                Hud.CreateStatusText();
                Hud.UpdateStatus();
                MainLoopInit(doFade);
            }

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

                    if (Timers.Player.Clock(Facts.PauseFlashInterval))
                        alternating = !alternating;

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
                    if (State.KeyPressed == EngineKeyCode.Escape)
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
                                Tiles[Player.Location].SetTo(ElementList.PlayerId,
                                    ElementList[ElementList.PlayerId].Color);
                                UpdateBoard(Player.Location);
                                UpdateRadius(Player.Location, RadiusMode.Update);
                                UpdateRadius(Player.Location.Difference(State.KeyVector), RadiusMode.Update);
                            }

                            State.GamePaused = false;
                            Hud.ClearPausing();
                            State.GameCycle = Random.Synced(Facts.MainLoopRandomCycleRange);
                            World.IsLocked = true;
                        }
                    }
                }

                if (State.ActIndex > State.ActorCount)
                {
                    if (!State.BreakGameLoop && !State.GamePaused)
                        if (State.GameWaitTime <= 0 || Timers.Player.Clock(State.GameWaitTime))
                        {
                            State.GameCycle++;
                            if (State.GameCycle > Facts.MaxGameCycle) State.GameCycle = 1;

                            State.ActIndex = 0;
                            ReadInput();
                        }

                    if (step)
                        break;

                    WaitForTick();
                }

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

                    var element = ElementList[ElementList.PlayerId];
                    Tiles[Player.Location].SetTo(element.Id, element.Color);
                    State.GameOver = false;
                    break;
                }
            }
        }

        private void MainLoopInit(bool doFade)
        {
            if (State.Init)
            {
                if (!State.AboutShown)
                    ShowAbout();

                if (State.DefaultWorldName.Length > 0)
                    LoadWorld(State.DefaultWorldName);

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

            if (doFade)
                FadePurple();

            State.GameWaitTime = State.GameSpeed << 1;
            State.BreakGameLoop = false;
            State.GameCycle = Random.Synced(Facts.MainLoopRandomCycleRange);
            State.ActIndex = State.ActorCount + 1;
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
                LoadWorld(World.Name);
                
                if (State.WorldLoaded)
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
                MainLoop(true, false);
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

        private IAnsiKeyTransformer AnsiKeyTransformer => _ansiKeyTransformer.Value;

        private EngineKeyCode ConvertKey(IKeyPress keyPress)
        {
            var bytes = AnsiKeyTransformer.GetBytes(keyPress)?.ToList();

            if (bytes == null || bytes.Count == 0)
                return EngineKeyCode.None;

            if (bytes.Count > 1 && (bytes[0] == 0 || bytes[0] >= 0x80))
                return (EngineKeyCode) (bytes[1] | 0x80);

            return (EngineKeyCode) bytes[0];
        }

        public void ReadInput()
        {
            //State.KeyShift = false;
            State.KeyArrow = false;
            State.KeyPressed = 0;
            State.KeyVector.SetTo(0, 0);

            var key = Keyboard.GetKey();
            if (key == null || key.Key == AnsiKey.None)
                return;

            State.KeyPressed = ConvertKey(key);
            State.KeyShift = key.Shift;

            switch (State.KeyPressed)
            {
                case EngineKeyCode.Left:
                    State.KeyVector.CopyFrom(Vector.West);
                    State.KeyArrow = true;
                    break;
                case EngineKeyCode.Right:
                    State.KeyVector.CopyFrom(Vector.East);
                    State.KeyArrow = true;
                    break;
                case EngineKeyCode.Up:
                    State.KeyVector.CopyFrom(Vector.North);
                    State.KeyArrow = true;
                    break;
                case EngineKeyCode.Down:
                    State.KeyVector.CopyFrom(Vector.South);
                    State.KeyArrow = true;
                    break;
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

        private void ShowAbout() => Features.ShowAbout();

        private void StartInit()
        {
            State.GameSpeed = Facts.DefaultGameSpeed;
            State.DefaultSaveName = Facts.DefaultSavedGameName;
            State.DefaultBoardName = Facts.DefaultBoardName;
            State.DefaultWorldName = Config.DefaultWorld ?? Facts.DefaultWorldName;
            State.Init = true;

            ClearWorld();

            if (Config.DefaultWorld != null)
            {
                State.DefaultWorldName = Config.DefaultWorld;
            }
            else if (Facts.ConfigFileName != null)
            {
                var zztCfg = Disk.GetFile(Facts.ConfigFileName);
                if (zztCfg != null && zztCfg.Length > 0)
                {
                    using (var stream = new MemoryStream(zztCfg))
                    using (var reader = new StreamReader(stream))
                    {
                        State.DefaultWorldName = reader.ReadLine();
                    }
                }
            }

            SetGameMode();
            Clock.Start();
        }

        private void StartMain()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            ValidateDependencies();
            StartInit();
            TitleScreenLoop();
        }

        private void ValidateDependencies()
        {
            if (ActionList == null) throw new Exception($"{nameof(ActionList)} cannot be null.");
            if (Actors == null) throw new Exception($"{nameof(Actors)} cannot be null.");
            if (Alerts == null) throw new Exception($"{nameof(Alerts)} cannot be null.");
            if (Board == null) throw new Exception($"{nameof(Board)} cannot be null.");
            if (CheatList == null) throw new Exception($"{nameof(CheatList)} cannot be null.");
            if (Colors == null) throw new Exception($"{nameof(Colors)} cannot be null.");
            if (CommandList == null) throw new Exception($"{nameof(CommandList)} cannot be null.");
            if (ConditionList == null) throw new Exception($"{nameof(ConditionList)} cannot be null.");
            if (Config == null) throw new Exception($"{nameof(Config)} cannot be null.");
            if (DirectionList == null) throw new Exception($"{nameof(DirectionList)} cannot be null.");
            if (ElementList == null) throw new Exception($"{nameof(ElementList)} cannot be null.");
            if (Hud == null) throw new Exception($"{nameof(Hud)} cannot be null.");
            if (ItemList == null) throw new Exception($"{nameof(ItemList)} cannot be null.");
            if (Parser == null) throw new Exception($"{nameof(Parser)} cannot be null.");
            if (Random == null) throw new Exception($"{nameof(Random)} cannot be null.");
            if (Sounds == null) throw new Exception($"{nameof(Sounds)} cannot be null.");
            if (State == null) throw new Exception($"{nameof(State)} cannot be null.");
            if (TargetList == null) throw new Exception($"{nameof(TargetList)} cannot be null.");
            if (Tiles == null) throw new Exception($"{nameof(Tiles)} cannot be null.");
            if (World == null) throw new Exception($"{nameof(World)} cannot be null.");
            if (DrawList == null) throw new Exception($"{nameof(DrawList)} cannot be null.");
            if (InteractionList == null) throw new Exception($"{nameof(InteractionList)} cannot be null.");
            if (Facts == null) throw new Exception($"{nameof(Facts)} cannot be null.");
        }

        private void StopSound()
        {
        }

        private void TitleScreenLoop()
        {
            State.QuitEngine = false;
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
                    MainLoop(gameEnded, false);

                    if (!ThreadActive)
                        break;

                    var startPlaying = Features.HandleTitleInput();
                    if (startPlaying)
                        gameEnded = PlayWorld();

                    if (gameEnded || State.QuitEngine)
                        break;
                }

                if (State.QuitEngine) break;
            }
        }

        private void UnpackBoard(int boardIndex)
        {
            GameSerializer.UnpackBoard(Tiles, Boards[boardIndex].Data);
            World.BoardIndex = boardIndex;
        }

        public void Dispose()
        {
            Clock?.Dispose();
        }
    }
}