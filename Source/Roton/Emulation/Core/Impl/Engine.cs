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
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Core.Impl
{
    [Context(Context.Original)]
    [Context(Context.Super)]
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
        private readonly Lazy<IObjectMover> _objectMover;
        private readonly Lazy<IMusicEncoder> _musicEncoder;
        private readonly Lazy<IHighScoreListFactory> _highScoreListFactory;
        private readonly Lazy<IConfigFileService> _configFileService;
        private readonly Lazy<IFileDialog> _fileDialog;
        private readonly Lazy<ITracer> _tracer;
        private readonly Lazy<IFeatures> _features;
        private readonly Lazy<IFileSystem> _fileSystem;
        private readonly Lazy<IGameSerializer> _gameSerializer;
        private readonly Lazy<IHud> _hud;
        private readonly Lazy<IInteractionList> _interactionList;
        private readonly Lazy<IInterpreter> _interpreter;
        private readonly Lazy<IItemList> _items;
        private readonly Lazy<IKeyboard> _keyboard;
        private readonly Lazy<IParser> _parser;
        private readonly Lazy<IRandomizer> _randomizer;
        private readonly Lazy<ISounds> _sounds;
        private readonly Lazy<IState> _state;
        private readonly Lazy<ITargetList> _targets;
        private readonly Lazy<ITiles> _tiles;
        private readonly Lazy<ITimers> _timers;
        private readonly Lazy<IWorld> _world;

        private int _ticksToRun;
        private bool _step;

        public Engine(Lazy<IClockFactory> clockFactory, Lazy<IActors> actors, Lazy<IAlerts> alerts, Lazy<IBoard> board,
            Lazy<IFileSystem> fileSystem, Lazy<IElementList> elements,
            Lazy<IInterpreter> interpreter, Lazy<IRandomizer> randomizer, Lazy<IKeyboard> keyboard,
            Lazy<ITiles> tiles, Lazy<ISounds> sounds, Lazy<ITimers> timers, Lazy<IParser> parser,
            Lazy<IConfig> config, Lazy<IConditionList> conditions, Lazy<IDirectionList> directions,
            Lazy<IColors> colors, Lazy<ICheatList> cheats, Lazy<ICommandList> commands, Lazy<ITargetList> targets,
            Lazy<IFeatures> features, Lazy<IGameSerializer> gameSerializer, Lazy<IHud> hud, Lazy<IState> state,
            Lazy<IWorld> world, Lazy<IItemList> items, Lazy<IBoards> boards, Lazy<IActionList> actionList,
            Lazy<IDrawList> drawList, Lazy<IInteractionList> interactionList, Lazy<IFacts> facts, Lazy<IMemory> memory,
            Lazy<IHeap> heap, Lazy<IAnsiKeyTransformer> ansiKeyTransformer, Lazy<IScrollFormatter> scrollFormatter,
            Lazy<ISpeaker> speaker, Lazy<IDrumBank> drumBank, Lazy<IObjectMover> objectMover, Lazy<IMusicEncoder> musicEncoder,
            Lazy<IHighScoreListFactory> highScoreListFactory, Lazy<IConfigFileService> configFileService,
            Lazy<IFileDialog> fileDialog, Lazy<ITracer> tracer)
        {
            _clock = new Lazy<IClock>(() =>
            {
                var clock = clockFactory.Value.Create(
                    _config.Value.MasterClockNumerator, 
                    _config.Value.MasterClockDenominator);
                
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
            _randomizer = randomizer;
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
            _objectMover = objectMover;
            _musicEncoder = musicEncoder;
            _highScoreListFactory = highScoreListFactory;
            _configFileService = configFileService;
            _fileDialog = fileDialog;
            _tracer = tracer;
        }

        private void ClockTick(object sender, EventArgs args)
        {
            if (_ticksToRun < 3) _ticksToRun++;
            if (!ThreadActive) 
                Clock.Stop();
        }

        private IHighScoreListFactory HighScoreListFactory => _highScoreListFactory.Value;

        private IObjectMover ObjectMover => _objectMover.Value;

        public IMusicEncoder MusicEncoder => _musicEncoder.Value;
        
        private IClock Clock => _clock.Value;

        private IBoards Boards => _boards.Value;

        private ITile BorderTile => State.BorderTile;

        public IFileSystem Disk => _fileSystem.Value;

        private IFeatures Features => _features.Value;

        private ISpeaker Speaker => _speaker.Value;

        public IGameSerializer GameSerializer => _gameSerializer.Value;

        private IInterpreter Interpreter => _interpreter.Value;

        private IKeyboard Keyboard => _keyboard.Value;

        private IScrollFormatter ScrollFormatter => _scrollFormatter.Value;

        public ITimers Timers => _timers.Value;
        
        public IDrumBank DrumBank => _drumBank.Value;

        private ITracer Tracer => _tracer.Value;

        private Thread Thread { get; set; }

        public bool ThreadActive => Thread != null || _step;

        public int MemoryUsage => Features.BaseMemoryUsage + Heap.Size + Boards.Sum(b => b.Data.Length);
        
        public void Cheat()
        {
            var cheatText = Hud.EnterCheat().ToUpper();
            var clear = false;

            if (!ThreadActive)
                return;

            if (!string.IsNullOrEmpty(cheatText))
            {
                if (cheatText[0] == '-')
                {
                    cheatText = cheatText.Substring(1);
                    while (World.Flags.Contains(cheatText))
                        World.Flags.Remove(cheatText);
                    clear = true;
                }
                else if (cheatText[0] == '+')
                {
                    cheatText = cheatText.Substring(1);
                    World.Flags.Add(cheatText);
                }
            }
            
            var cheat = CheatList.Get(cheatText);
            cheat?.Execute(cheatText, clear);
            Hud.UpdateStatus();
            
            // TODO: figure out the actual priority of this sound
            PlaySound(3, Sounds.Cheat);
        }

        public void PlayStep()
        {
            if (State.GameOver || State.GameQuiet || State.SoundPlaying)
                return;

            Speaker.PlayStep();
        }

        public string GetHighScoreName(string fileName) => Features.GetHighScoreName(fileName);
        
        public void ShowHighScores()
        {
            var list = HighScoreListFactory.Load();
            if (list == null) 
                return;
            
            Hud.ShowHighScores(list);
        }

        public IActionList ActionList
            => _actionList.Value;

        public IActor ActorAt(IXyPair location)
        {
            return Actors
                .FirstOrDefault(actor => actor.Location.X == location.X && actor.Location.Y == location.Y) ??
                Actors[-1];
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

        public event EventHandler Exited;
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
                SearchTarget = label,
                Index = sender
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

        public void CleanUpPassageMovement() => Features.CleanUpPassageMovement();

        public void ClearForest(IXyPair location) => Features.ClearForest(location);

        public void ClearSound()
        {
            State.SoundPlaying = false;
            Speaker.StopNote();
        }

        public void ClearWorld()
        {
            State.BoardCount = 0;
            Boards.Clear();
            Alerts.Reset();
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

        public IElement ElementAt(IXyPair location) => ElementList[Tiles[location].Id];

        public IElementList ElementList => _elements.Value;

        public void EnterBoard() => Features.EnterBoard();

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

                Tracer?.TraceOop(context);
                
                context.NextLine = true;
                context.PreviousInstruction = context.Instruction;
                context.Command = ReadActorCodeByte(index, context);
                switch (context.Command)
                {
                    case 0x3A: // :
                    case 0x27: // '
                    case 0x40: // @
                        Parser.ReadLine(index, context);
                        break;
                    case 0x2F: // /
                    case 0x3F: // ?
                        if (context.Command == 0x2F)
                            context.Repeat = true;

                        var vector = Parser.GetDirection(context);
                        if (vector == null)
                        {
                            RaiseError("Bad direction");
                            break;
                        }
                        
                        ObjectMover.ExecuteDirection(context, vector);

                        ReadActorCodeByte(index, context);
                        if (State.OopByte != 0x0D)
                            context.Instruction--;
                        context.Moved = true;

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
                        context.Message.Add($"{context.Command.ToStringValue()}{Parser.ReadLine(context.Index, context)}");
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
                    context.SearchOffset = Parser.Search(context.SearchIndex, 0, prefix + label);
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
            _step = true;
            MainLoop(true);
            _step = false;
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

        public void ForcePlayerColor(int index) => Features.ForcePlayerColor(index);

        public IXyPair GetCardinalVector(int index) => new Vector(State.Vector4[index], State.Vector4[index + 4]);

        public void HandlePlayerInput(IActor actor) => Features.HandlePlayerInput(actor);

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

        public IInteractionList InteractionList => _interactionList.Value;

        public IItemList ItemList => _items.Value;

        private void ShowFormattedScroll(string error) => Hud.ShowScroll(false, "Roton Error", ScrollFormatter.Format(error));

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
            {
                ShowDosError();
                return;
            }
            
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

            Hud.CreateStatusWorld();
            UnpackBoard(World.BoardIndex);
            State.WorldLoaded = true;
        }

        private void ShowDosError()
        {
            Hud.ShowScroll(false, "Error",
                new[]
                {
                    "$DOS Error:",
                    string.Empty,
                    "This may be caused by missing",
                    "files or a bad disk. If you",
                    "are trying to save a game,",
                    "your disk may be full -- try",
                    "using a blank, formatted disk",
                    "for saving the game!"
                }
            );
        }

        public void LockActor(int index) => Features.LockActor(index);

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
            else if (underId == ElementList.RiverEId) 
                vector.SetTo(1, 0);

            if (vector.IsNonZero())
            {
                var actorTile = Tiles[actor.Location];
                if (actorTile.Id == ElementList.PlayerId)
                {
                    var targetLocation = actor.Location.Sum(vector);
                    InteractionList.Get(Tiles[targetLocation].Id).Interact(targetLocation, 0, vector);                
                }
            }
            
            if (vector.IsNonZero())
            {
                var target = actor.Location.Sum(vector);
                if (ElementAt(target).IsFloor) 
                    MoveActor(index, target);
            }
        }

        public IParser Parser => _parser.Value;

        public IActor Player => Actors[0];

        public void PlaySound(int priority, ISound sound, int? offset = null, int? length = null)
        {
            if (State.GameOver || State.GameQuiet)
                return;

            var soundIsNotPlaying = !State.SoundPlaying;
            var soundIsMusic = priority == -1;
            var soundIsHigherPriority = State.SoundPriority != -1 && priority >= State.SoundPriority;

            if (!(soundIsNotPlaying || soundIsMusic || soundIsHigherPriority))
                return;

            if (!soundIsMusic)
                State.SoundBuffer.Clear();

            State.SoundBuffer.Enqueue(sound, offset, length);
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

        public IRandomizer Random => _randomizer.Value;

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

        public void RemoveItem(IXyPair location) => Features.RemoveItem(location);

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
                Random.GetNext(2) == 0
                    ? vector.Clockwise()
                    : vector.CounterClockwise());
            return result;
        }

        public IXyPair Seek(IXyPair location)
        {
            var result = new Vector();
            if (Random.GetNext(2) == 0 || Player.Location.Y == location.Y)
                result.X = (Player.Location.X - location.X).Polarity();

            if (result.X == 0) result.Y = (Player.Location.Y - location.Y).Polarity();

            if (World.EnergyCycles > 0) result.SetOpposite();

            return result;
        }

        public void SetBoard(int boardIndex)
        {
            var element = ElementList.Player();
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

        public void ShowHelp(string title, string filename) => Hud.ShowHelp(title, filename);

        public void ShowInGameHelp() => Features.ShowInGameHelp();

        public void OpenWorld()
        {
            var name = Features.OpenWorld();
            if (!string.IsNullOrEmpty(name))
            {
                LoadWorld(name);
                State.StartBoard = World.BoardIndex;
                SetBoard(0);
                FadePurple();
            }
        }

        public string ShowLoad(string title, string extension)
        {
            return _fileDialog.Value.Open(title, extension);
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
            if (Thread == null)
            {
                _ticksToRun = 0;
                Thread = new Thread(StartMain);
                Thread.Start();
            }
        }

        public IState State => _state.Value;

        public void Stop()
        {
            Thread = null;
        }

        public ITargetList TargetList => _targets.Value;

        public ITiles Tiles => _tiles.Value;

        public bool TitleScreen => State.PlayerElement != ElementList.PlayerId;

        public void UnlockActor(int index) => Features.UnlockActor(index);

        public void UpdateBoard(IXyPair location) => DrawTile(location, Draw(location));

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
                                    Tiles[target].SetTo(ElementList.BreakableId, Random.GetNext(7) + 9);
                            }
                            else
                            {
                                if (Tiles[target].Id == ElementList.BreakableId) Tiles[target].Id = ElementList.EmptyId;
                            }
                        }

                    UpdateBoard(target);
                }
        }

        public void UpdateStatus() => Hud.UpdateStatus();

        private void UpdateSound()
        {
            if (!State.SoundPlaying) 
                return;

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

        public void WaitForTick()
        {
            var isFast = State.GameWaitTime <= 0 && Config.FastMode;

            if (isFast)
            {
                while (_ticksToRun > 0)
                {
                    UpdateSound();
                    if (Clock != null)
                        Tick?.Invoke(this, EventArgs.Empty);
                    _ticksToRun--;
                }
            }
            else
            {
                UpdateSound();
                
                if (Clock == null)
                    return;
            
                Tick?.Invoke(this, EventArgs.Empty);
            
                while (_ticksToRun <= 0 && ThreadActive)
                    Thread.Sleep(1);

                if (_ticksToRun > 0)
                    _ticksToRun--;
            }
        }

        public IWorld World
            => _world.Value;

        private bool ActorIsLocked(int index) => Features.IsActorLocked(index);

        public void ClearBoard()
        {
            var emptyId = ElementList.EmptyId;
            var boardEdgeId = State.EdgeTile.Id;
            var boardBorderId = BorderTile.Id;
            var boardBorderColor = BorderTile.Color;

            // board properties
            Board.Name = string.Empty;
            State.Message = string.Empty;
            Board.MaximumShots = Facts.DefaultMaximumShots;
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
            var element = ElementList.Player();
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

        private static int Distance(IXyPair a, IXyPair b) => (a.Y - b.Y).Square() * 2 + (a.X - b.X).Square();

        private void DrawTile(IXyPair location, AnsiChar ac) => Hud.DrawTile(location.X - 1, location.Y - 1, ac);

        private void EnterHighScore(int score)
        {
            var list = HighScoreListFactory.Load();
            if (list == null) 
                return;
            
            var name = Hud.EnterHighScore(list, score);
            if (name == null) 
                return;
            
            list.Add(name, score);
            HighScoreListFactory.Save(list);
            ShowHighScores();
        }

        private void ExecuteDeath(IOopContext context)
        {
            var location = context.Actor.Location.Clone();
            Harm(context.Index);
            PlotTile(location, context.DeathTile);
        }

        private void ExecuteMessage(IOopContext context)
        {
            var result = Features.ExecuteMessage(context);
            if (result != null && !result.Cancelled && result.Label != null)
                context.NextLine = BroadcastLabel(context.Index, result.Label, false);                            
        }

        private void FadeBoard(AnsiChar ac) => Hud.FadeBoard(ac);

        public void FadeRed()
        {
            FadeBoard(Facts.ErrorFadeTile);
            Hud.RedrawBoard();
        }

        private IXyPair GetConveyorVector(int index) => new Vector(State.Vector8[index], State.Vector8[index + 8]);

        private void InitializeElements(bool showInvisibles)
        {
            ElementList.Reset();

            // this isn't all the initializations.
            // todo: replace this with the ability to completely reinitialize engine default memory
            ElementList.Invisible().Character = showInvisibles ? 0xB0 : 0x20;
            ElementList.Invisible().Color = 0xFF;
            ElementList.Player().Character = 0x02;
        }

        private void MainLoop(bool doFade)
        {
            var alternating = false;

            if (!_step)
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
                        var playerElement = ElementList.Player();
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

                    if (!State.KeyVector.IsZero() && State.KeyArrow)
                    {
                        var target = Player.Location.Sum(State.KeyVector);
                        InteractionList.Get(ElementAt(target).Id).Interact(target, 0, State.KeyVector);
                    }

                    if (!State.KeyVector.IsZero() && State.KeyArrow)
                    {
                        var target = Player.Location.Sum(State.KeyVector);
                        if (ElementAt(target).IsFloor)
                        {
                            Features.CleanUpPauseMovement();
                            State.GamePaused = false;
                            Hud.ClearPausing();
                            State.GameCycle = Random.GetNext(Facts.MainLoopRandomCycleRange);
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

                    Tracer.TraceStep();
                    if (_step)
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

                    var element = ElementList.Player();
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

                if (!ThreadActive)
                    return;

                if (State.DefaultWorldName.Length > 0)
                {
                    State.AboutShown = true;
                    LoadWorld(State.DefaultWorldName);                    
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

            if (doFade)
                FadePurple();

            State.GameWaitTime = State.GameSpeed << 1;
            State.BreakGameLoop = false;
            State.GameCycle = Random.GetNext(Facts.MainLoopRandomCycleRange);
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

        public void PackBoard()
        {
            var board = new PackedBoard(GameSerializer.PackBoard(Tiles));
            PackBoard(World.BoardIndex, board);
        }

        private void PackBoard(int boardIndex, IPackedBoard board)
        {
            // bit of a hack to make sure we don't go out of bounds
            while (Boards.Count <= boardIndex)
                Boards.Add(null);

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

        private void Rnd(IXyPair result)
        {
            result.X = Random.GetNext(3) - 1;
            if (result.X == 0)
                result.Y = (Random.GetNext(2) << 1) - 1;
            else
                result.Y = 0;
        }

        private void ShowAbout() => Features.ShowAbout();

        private void StartInit()
        {
            State.GameSpeed = Facts.DefaultGameSpeed;
            State.GameWaitTime = 1;
            State.DefaultSaveName = Facts.DefaultSavedGameName;
            State.DefaultBoardName = Facts.DefaultBoardName;
            State.DefaultWorldName = Config.DefaultWorld ?? Facts.DefaultWorldName;
            State.ForestIndex = 2;
            State.Init = true;

            ClearWorld();

            var cfg = _configFileService.Value.Load();
            if (Config.DefaultWorld == null && cfg != null)
            {
                if (!string.IsNullOrEmpty(cfg.WorldName))
                {
                    State.DefaultWorldName = cfg.WorldName.StartsWith("*")
                        ? cfg.WorldName.Substring(1)
                        : cfg.WorldName;
                }
            }

            SetGameMode();
            Clock.Start();
        }

        private void StartMain()
        {
            StartInit();
            TitleScreenLoop();
            Exited?.Invoke(this, EventArgs.Empty);
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
                    MainLoop(gameEnded);
                    gameEnded = false;

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

        public void UnpackBoard(int boardIndex)
        {
            GameSerializer.UnpackBoard(Tiles, Boards[boardIndex].Data);
            World.BoardIndex = boardIndex;
        }

        public void Dispose()
        {
            Clock?.Stop();
        }
    }
}