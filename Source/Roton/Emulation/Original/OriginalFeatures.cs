using System;
using System.Linq;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original
{
    [Context(Context.Original)]
    public sealed class OriginalFeatures : IFeatures
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public OriginalFeatures(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        

        public void LockActor(int index)
        {
            Engine.Actors[index].P2 = 1;
        }

        public void UnlockActor(int index)
        {
            Engine.Actors[index].P2 = 0;
        }

        public bool IsActorLocked(int index)
        {
            return Engine.Actors[index].P2 != 0;
        }

        public string GetHighScoreName(string baseName) => $"{baseName}.HI";

        public void EnterBoard()
        {
            Engine.Board.Entrance.CopyFrom(Engine.Player.Location);
            if (Engine.Board.IsDark && Engine.Alerts.Dark)
            {
                Engine.SetMessage(Engine.Facts.LongMessageDuration, Engine.Alerts.DarkMessage);
                Engine.Alerts.Dark = false;
            }

            Engine.World.TimePassed = 0;
            Engine.UpdateStatus();
        }

        public IScrollState ExecuteMessage(IOopContext context)
        {
            if (context.Message.Count == 1)
            {
                Engine.SetMessage(Engine.Facts.LongMessageDuration, new Message(context.Message));
                return null;
            }
            else
            {
                Engine.State.KeyVector.SetTo(0, 0);
                return Engine.Hud.ShowScroll(false, context.Name, context.Message.ToArray());
            }
        }

        public void HandlePlayerInput(IActor actor)
        {
            switch (Engine.State.KeyPressed.ToUpperCase())
            {
                case EngineKeyCode.T:
                    if (Engine.World.TorchCycles <= 0)
                    {
                        if (Engine.World.Torches <= 0)
                        {
                            if (Engine.Alerts.NoTorches)
                            {
                                Engine.SetMessage(Engine.Facts.LongMessageDuration, Engine.Alerts.NoTorchMessage);
                                Engine.Alerts.NoTorches = false;
                            }
                        }
                        else if (!Engine.Board.IsDark)
                        {
                            if (Engine.Alerts.NotDark)
                            {
                                Engine.SetMessage(Engine.Facts.LongMessageDuration, Engine.Alerts.NotDarkMessage);
                                Engine.Alerts.NotDark = false;
                            }
                        }
                        else
                        {
                            Engine.World.Torches--;
                            Engine.World.TorchCycles = 0xC8;
                            Engine.UpdateRadius(actor.Location, RadiusMode.Update);
                            Engine.Hud.UpdateStatus();
                        }
                    }

                    break;
                case EngineKeyCode.F:
                    break;
            }
        }

        public bool CanPutTile(IXyPair location)
        {
            // do not allow #put on the bottom row
            return location.Y < Engine.Tiles.Height;
        }

        public void ClearForest(IXyPair location)
        {
            Engine.RemoveItem(location);
        }

        public void CleanUpPassageMovement()
        {
            Engine.Tiles[Engine.Player.Location].SetTo(Engine.ElementList.EmptyId, 0);
        }

        public void ForcePlayerColor(int index)
        {
            var actor = Engine.Actors[index];
            var playerElement = Engine.ElementList[Engine.ElementList.PlayerId];
            if (Engine.Tiles[actor.Location].Color == playerElement.Color &&
                playerElement.Character == Engine.Facts.PlayerCharacter) 
                return;
            
            playerElement.Character = Engine.Facts.PlayerCharacter;
            Engine.Tiles[actor.Location].Color = playerElement.Color;
            Engine.UpdateBoard(actor.Location);
        }

        public string[] GetMessageLines()
        {
            return new[] {Engine.State.Message};
        }

        public void ShowAbout()
        {
            Engine.ShowHelp("About Roton...", "ABOUT");
        }

        public int BaseMemoryUsage => 205791;
        
        public void CleanUpPauseMovement()
        {
            var target = Engine.Player.Location.Sum(Engine.State.KeyVector);
            
            if (Engine.ElementAt(Engine.Player.Location).Id == Engine.ElementList.PlayerId)
            {
                Engine.MoveActor(0, target);
            }
            else
            {
                Engine.UpdateBoard(Engine.Player.Location);
                Engine.Player.Location.Add(Engine.State.KeyVector);
                Engine.Tiles[Engine.Player.Location].SetTo(Engine.ElementList.PlayerId,
                    Engine.ElementList[Engine.ElementList.PlayerId].Color);
                Engine.UpdateBoard(Engine.Player.Location);
                Engine.UpdateRadius(Engine.Player.Location, RadiusMode.Update);
                Engine.UpdateRadius(Engine.Player.Location.Difference(Engine.State.KeyVector), RadiusMode.Update);
            }
        }

        public string OpenWorld()
        {
            return Engine.ShowLoad("ZZT Worlds", "zzt");
        }

        public bool HandleTitleInput()
        {
            switch (Engine.State.KeyPressed.ToUpperCase())
            {
                case EngineKeyCode.P:
                    return true;
                case EngineKeyCode.W:
                    Engine.OpenWorld();
                    break;
                case EngineKeyCode.A:
                    ShowAbout();
                    break;
                case EngineKeyCode.E:
                    break;
                case EngineKeyCode.S:
                    Engine.Hud.CreateStatusText();
                    Engine.State.GameSpeed = Engine.Hud.SelectParameter(
                        true, 0x42, 0x15, @"Game speed:;FS", Engine.State.GameSpeed, null);
                    break;
                case EngineKeyCode.R:
                    break;
                case EngineKeyCode.H:
                    Engine.ShowHighScores();
                    break;
                case EngineKeyCode.QuestionMark:
                    Engine.Hud.EnterCheat();
                    break;
                case EngineKeyCode.Escape:
                case EngineKeyCode.Q:
                    Engine.State.QuitEngine = Engine.Hud.QuitEngineConfirmation();
                    break;
            }

            return false;
        }

        public void RemoveItem(IXyPair location)
        {
            Engine.Tiles[location].Id = Engine.ElementList.EmptyId;
            Engine.UpdateBoard(location);
        }

        public void ShowInGameHelp()
        {
            Engine.ShowHelp("Playing Roton", "GAME");
        }

        public string GetWorldName(string baseName)
        {
            return $"{baseName}.ZZT";
        }
    }
}