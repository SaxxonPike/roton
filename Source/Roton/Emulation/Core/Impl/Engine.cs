using System;
using Roton.Emulation.Cheats;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Engine : IEngine, IDisposable
{
    private readonly IClock _clock;
    private readonly IActorList _actors;
    private readonly IAlerts _alerts;
    private readonly IBoard _board;
    private readonly IElementList _elements;
    private readonly IInterpreter _interpreter;
    private readonly ITiles _tiles;
    private readonly ISounds _sounds;
    private readonly IParser _parser;
    private readonly ICheatList _cheats;
    private readonly IHud _hud;
    private readonly IState _state;
    private readonly IWorld _world;
    private readonly IFacts _facts;
    private readonly IObjectMover _objectMover;
    private readonly ITracer _tracer;
    private readonly ISoundUnit _soundUnit;
    private readonly IBoardTime _boardTime;
    private readonly IBoardUpdater _boardUpdater;
    private readonly IBroadcaster _broadcaster;
    private readonly IRadiusUpdater _radiusUpdater;
    private readonly IPusher _pusher;
    private readonly IMessageHandler _messageHandler;
    private readonly ISpawner _spawner;
    private readonly IMessenger _messenger;
    private readonly IScheduler _scheduler;
    private readonly ITileRemover _tileRemover;
    private readonly IActorRemover _actorRemover;
    private readonly IGame _game;
    private readonly IGameThread _gameThread;

    public Engine(
        IClock clock,
        IActorList actors,
        IAlerts alerts,
        IBoard board,
        IElementList elements,
        IInterpreter interpreter,
        ITiles tiles,
        ISounds sounds,
        IParser parser,
        ICheatList cheats,
        IHud hud,
        IState state,
        IWorld world,
        IFacts facts,
        IObjectMover objectMover,
        ITracer tracer,
        IEngineAccessor engineAccessor,
        ISoundUnit soundUnit,
        IBoardTime boardTime,
        IBoardUpdater boardUpdater,
        IBroadcaster broadcaster,
        IRadiusUpdater radiusUpdater,
        IPusher pusher,
        IMessageHandler messageHandler,
        ISpawner spawner,
        IMessenger messenger,
        IScheduler scheduler,
        ITileRemover tileRemover,
        IActorRemover actorRemover,
        IGame game,
        IGameThread gameThread)
    {
        engineAccessor.Instance = this;

        _clock = clock;
        _actors = actors;
        _alerts = alerts;
        _board = board;
        _elements = elements;
        _interpreter = interpreter;
        _tiles = tiles;
        _sounds = sounds;
        _parser = parser;
        _cheats = cheats;
        _hud = hud;
        _state = state;
        _world = world;
        _facts = facts;
        _objectMover = objectMover;
        _tracer = tracer;
        _soundUnit = soundUnit;
        _boardTime = boardTime;
        _boardUpdater = boardUpdater;
        _broadcaster = broadcaster;
        _radiusUpdater = radiusUpdater;
        _pusher = pusher;
        _messageHandler = messageHandler;
        _spawner = spawner;
        _messenger = messenger;
        _scheduler = scheduler;
        _tileRemover = tileRemover;
        _actorRemover = actorRemover;
        _game = game;
        _gameThread = gameThread;
    }

    public void Cheat()
    {
        var cheatText = _hud.EnterCheat().UpCased() ?? "";
        var clear = false;

        if (!_gameThread.ThreadActive)
            return;

        if (!string.IsNullOrEmpty(cheatText))
        {
            switch (cheatText[0])
            {
                case '-':
                {
                    cheatText = cheatText.Substring(1);
                    while (_world.Flags.Contains(cheatText))
                        _world.Flags.Remove(cheatText);
                    clear = true;
                    break;
                }
                case '+':
                    cheatText = cheatText.Substring(1);
                    _world.Flags.Add(cheatText);
                    break;
            }
        }

        var cheat = _cheats.Get(cheatText);
        cheat?.Execute(clear);
        _hud.UpdateStatus();

        _soundUnit.PlaySound(10, _sounds.Cheat);
    }

    public void Attack(int index, Location location)
    {
        if (index == 0 && _world.EnergyCycles > 0)
        {
            _world.Score += _tiles.ElementAt(location).Points;
            _hud.UpdateStatus();
        }
        else
        {
            Harm(index);
        }

        if (index > 0 && index <= _state.ActIndex) _state.ActIndex--;

        if (_tiles[location].Id == _elements.PlayerId && _world.EnergyCycles > 0)
        {
            _world.Score += _tiles.ElementAt(_actors[index].Location).Points;
            _hud.UpdateStatus();
        }
        else
        {
            Destroy(location);
            _soundUnit.PlaySound(2, _sounds.EnemySuicide);
        }
    }

    public void Destroy(Location location)
    {
        var index = _actors.ActorIndexAt(location);
        if (index == -1)
            _tileRemover.RemoveItem(location);
        else
            Harm(index);
    }

    public void StepOnce()
    {
        _gameThread.Step = true;
        _game.MainLoop(true);
        _gameThread.Step = false;
    }

    public void Harm(int index)
    {
        var actor = _actors[index];
        if (index == 0)
        {
            if (_world.Health > 0)
            {
                _world.Health -= _facts.HealthLostPerHit;
                _hud.UpdateStatus();
                _messenger.SetMessage(_facts.ShortMessageDuration, _alerts.OuchMessage);
                _tiles[actor.Location].Color = (_tiles.ElementAt(actor.Location).Color & 0x0F) | 0x70;

                if (_world.Health > 0)
                {
                    _world.TimePassed = 0;
                    if (_board.RestartOnZap)
                    {
                        _soundUnit.PlaySound(4, _sounds.TimeOut);
                        _tileRemover.RemoveItem(actor.Location);
                        var oldLocation = actor.Location;
                        actor.Location = _board.Entrance;
                        _radiusUpdater.UpdateRadius(oldLocation, 0);
                        _radiusUpdater.UpdateRadius(actor.Location, 0);
                        _state.GamePaused = true;
                    }

                    _soundUnit.PlaySound(4, _sounds.Ouch);
                }
                else
                {
                    _soundUnit.PlaySound(5, _sounds.GameOver);
                }
            }
        }
        else
        {
            var element = _tiles[actor.Location].Id;
            if (element == _elements.BulletId)
                _soundUnit.PlaySound(3, _sounds.BulletDie);
            else if (element != _elements.ObjectId) _soundUnit.PlaySound(3, _sounds.EnemyDie);

            _actorRemover.RemoveActor(index);
        }
    }


    public void PlotTile(Location location, Tile tile)
    {
        if (_tiles.ElementAt(location).Id == _elements.PlayerId)
            return;

        var targetElement = _elements[tile.Id];
        ref var existingTile = ref _tiles[location];
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
                existingTile = new Tile(targetElement.Id, targetColor);
            else
                _spawner.SpawnActor(location, new Tile(targetElement.Id, targetColor), targetElement.Cycle,
                    _state.DefaultActor);
        }

        _boardUpdater.UpdateBoard(location);
    }

    public void PutTile(Location location, Vector vector, Tile kind)
    {
        if (!_tiles.CanPutTile(location))
            return;

        if (location.X >= 1 && location.X <= _tiles.Width && location.Y >= 1 &&
            location.Y <= _tiles.Height)
        {
            if (!_tiles.ElementAt(location).IsFloor)
                _pusher.Push(location, vector);
            PlotTile(location, kind);
        }
    }

    public void Delay(int msec)
    {
        var waitUntil = DateTime.Now + TimeSpan.FromMilliseconds(msec);
        while (DateTime.Now < waitUntil)
            _scheduler.WaitForTick();
    }

    public int ResetBoardTimeHsec() =>
        _boardTime.Elapse();

    public void Dispose() =>
        _clock.Stop();
}