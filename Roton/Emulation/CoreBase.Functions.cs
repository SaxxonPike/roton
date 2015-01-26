﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Roton.Emulation
{
    internal partial class CoreBase
    {
        internal int ActorIndex(Location location)
        {
            int index = 0;
            foreach (Actor actor in Actors)
            {
                if (actor.Location.X == location.X && actor.Location.Y == location.Y)
                    return index;
                index++;
            }
            return -1;
        }

        internal int Adjacent(Location location, int id)
        {
            return ((location.Y <= 1 || Tiles[location.Sum(Vector.North)].Id == id) ? 1 : 0) |
                ((location.Y >= Tiles.Height || Tiles[location.Sum(Vector.South)].Id == id) ? 2 : 0) |
                ((location.X <= 1 || Tiles[location.Sum(Vector.West)].Id == id) ? 4 : 0) |
                ((location.X >= Tiles.Width || Tiles[location.Sum(Vector.East)].Id == id) ? 8 : 0);
        }

        virtual internal string AmmoMessage
        {
            get { return @"Ammunition - 5 shots per container."; }
        }

        internal void Attack(int index, Location location)
        {
            if (index == 0 && EnergyCycles > 0)
            {
                Score += Elements[Tiles[location].Id].Points;
                UpdateStatus();
            }
            else
            {
                Harm(index);
            }

            if (index > 0 && index <= ActIndex)
            {
                ActIndex--;
            }

            if (Tiles[location].Id == Elements.PlayerId && EnergyCycles > 0)
            {
                Score += Elements[Tiles[Actors[index].Location].Id].Points;
                UpdateStatus();
            }
            else
            {
                PlaySound(2, Sounds.EnemySuicide);
            }
        }

        virtual internal string BombMessage
        {
            get { return @"Bomb activated!"; }
        }

        internal void ClearBoard()
        {
            int emptyId = Elements.EmptyId;
            int boardEdgeId = EdgeTile.Id;
            int boardBorderId = BorderTile.Id;
            int boardBorderColor = BorderTile.Color;

            // board properties
            BoardName = "";
            Message = "";
            Shots = 0xFF;
            Dark = false;
            RestartOnZap = false;
            TimeLimit = 0;
            ExitEast = 0;
            ExitNorth = 0;
            ExitSouth = 0;
            ExitWest = 0;

            // build board edges
            for (int y = 0; y <= Tiles.Height + 1; y++)
            {
                TileAt(0, y).Id = boardEdgeId;
                TileAt(Width + 1, y).Id = boardEdgeId;
            }
            for (int x = 0; x <= Width + 1; x++)
            {
                TileAt(x, 0).Id = boardEdgeId;
                TileAt(x, Height + 1).Id = boardEdgeId;
            }

            // clear out board
            for (int x = 1; x <= Width; x++)
            {
                for (int y = 1; y <= Height; y++)
                {
                    TileAt(x, y).SetTo(emptyId, 0);
                }
            }

            // build border
            for (int y = 1; y <= Height; y++)
            {
                TileAt(1, y).SetTo(boardBorderId, boardBorderColor);
                TileAt(Width, y).SetTo(boardBorderId, boardBorderColor);
            }
            for (int x = 1; x <= Width; x++)
            {
                TileAt(x, 1).SetTo(boardBorderId, boardBorderColor);
                TileAt(x, Height).SetTo(boardBorderId, boardBorderColor);
            }

            // generate player actor
            var element = Elements.PlayerElement;
            ActorCount = 0;
            Player.Location.SetTo(Width / 2, Height / 2);
            TileAt(Player.Location).SetTo(element.Index, element.Color);
            Player.Cycle = 1;
            Player.UnderTile.SetTo(0, 0);
            Player.Pointer = 0;
            Player.Length = 0;
        }

        internal void ClearSound()
        {
        }

        internal void ClearWorld()
        {
            BoardCount = 0;
            Boards.Clear();
            ResetAlerts();
            ClearBoard();
            Boards.Add(new PackedBoard(Disk.PackBoard(Tiles)));
            Board = 0;
            Ammo = 0;
            Gems = 0;
            Health = 100;
            EnergyCycles = 0;
            Torches = 0;
            TorchCycles = 0;
            Score = 0;
            TimePassed = 0;
            Stones = -1;
            Keys.Clear();
            Flags.Clear();
            SetBoard(0);
            BoardName = "Introduction screen";
            WorldFileName = "";
            WorldName = "";
        }

        virtual internal void Convey(Location center, int direction)
        {
        }

        virtual internal string DarkMessage
        {
            get { return @"Room is dark - you need to light a torch!"; }
        }

        internal void Destroy(Location location)
        {
        }

        internal int Distance(Location a, Location b)
        {
            return ((a.Y - b.Y).Square() * 2) + ((a.X - b.X).Square());
        }

        virtual internal string DoorClosedMessage(int color)
        {
            return @"The " + Colors[color] + " door is locked!";
        }
        
        virtual internal string DoorOpenMessage(int color)
        {
            return @"The " + Colors[color] + " door is now open.";
        }

        internal void DrawChar(Location location, AnsiChar ac)
        {
            Display.DrawChar(location.X, location.Y, ac);
        }

        internal void DrawString(Location location, string text, int color)
        {
            Display.DrawString(location.X, location.Y, text, color);
        }

        internal void DrawTile(Location location, AnsiChar ac)
        {
            Display.DrawTile(location.X - 1, location.Y - 1, ac);
        }

        internal Element ElementAt(Location location)
        {
            return Elements[TileAt(location).Id];
        }

        internal Element ElementAt(int x, int y)
        {
            return ElementAt(new Location(x, y));
        }

        virtual internal string EnergizerMessage
        {
            get { return @"Energizer - You are invincible"; }
        }

        virtual internal void EnterBoard()
        {
            Enter.CopyFrom(Player.Location);
            if (Dark && AlertDark)
            {
                SetMessage(0xC8, DarkMessage);
                AlertDark = false;
            }
            TimePassed = 0;
            UpdateStatus();
        }

        internal void EnterHighScore(int score)
        {
        }

        virtual internal void ExecutePassage(Location location)
        {
            int searchColor = TileAt(location).Color;
            int oldBoard = Board;
            int passageIndex = ActorIndex(location);
            int passageTarget = Actors[passageIndex].P3;
            SetBoard(passageTarget);
            Location target = new Location();

            for (int x = 1; x <= Width; x++)
            {
                for (int y = 1; y <= Height; y++)
                {
                    if (TileAt(x, y).Id == Elements.PassageId)
                    {
                        if (TileAt(x, y).Color == searchColor)
                        {
                            target.SetTo(x, y);
                        }
                    }
                }
            }

            // this is what causes the black holes when using passages
            TileAt(Player.Location).SetTo(Elements.EmptyId, 0);

            if (target.X != 0)
            {
                Player.Location.CopyFrom(target);
            }
            GamePaused = true;
            PlaySound(4, Sounds.Passage);
            FadePurple();
            EnterBoard();
        }

        internal void FadeBoard(AnsiChar ac)
        {
            Display.FadeBoard(ac);
        }

        internal void FadePurple()
        {
            FadeBoard(new AnsiChar(0xDB, 0x05));
            RedrawBoard();
        }

        internal void FadeRed()
        {
            FadeBoard(new AnsiChar(0xDB, 0x04));
            RedrawBoard();
        }

        virtual internal string FakeMessage
        {
            get { return @"A fake wall - secret passage!"; }
        }

        virtual internal void ForcePlayerColor()
        {
            if (TileAt(Player.Location).Color != Elements.PlayerElement.Color || Elements.PlayerElement.Character != 0x02)
            {
                Elements.PlayerElement.Character = 2;
                TileAt(Player.Location).Color = Elements.PlayerElement.Color;
                UpdateBoard(Player.Location);
            }
        }

        virtual internal string ForestMessage
        {
            get { return @"A path is cleared through the forest."; }
        }

        virtual internal string GameOverMessage
        {
            get { return @" Game over  -  Press ESCAPE"; }
        }

        virtual internal string GemMessage
        {
            get { return @"Gems give you Health!"; }
        }

        internal void Harm(int index)
        {
        }

        internal int Height
        {
            get { return Tiles.Height; }
        }

        internal void InitializeElementDelegates()
        {
            foreach (var element in Elements)
            {
                if (element.Index == Elements.AmmoId) 
                {
                    element.Interact = Interact_Ammo; 
                }
                else if (element.Index == Elements.BearId) 
                { 
                    element.Act = Act_Bear;
                    element.Interact = Interact_Enemy;
                }
                else if (element.Index == Elements.BlinkWallId)
                {
                    element.Act = Act_BlinkWall;
                    element.Draw = Draw_BlinkWall;
                }
                else if (element.Index == Elements.BoardEdgeId)
                {
                    element.Interact = Interact_BoardEdge;
                }
                else if (element.Index == Elements.BombId)
                {
                    element.Act = Act_Bomb;
                    element.Draw = Draw_Bomb;
                    element.Interact = Interact_Bomb;
                }
                else if (element.Index == Elements.BoulderId)
                {
                    element.Interact = Interact_Pushable;
                }
                else if (element.Index == Elements.BulletId)
                {
                    element.Act = Act_Bullet;
                    element.Interact = Interact_Enemy;
                }
                else if (element.Index == Elements.ClockwiseId)
                {
                    element.Draw = Draw_Clockwise;
                }
                else if (element.Index == Elements.CounterId)
                {
                    element.Draw = Draw_Counter;
                }
                else if (element.Index == Elements.DoorId)
                {
                    element.Interact = Interact_Door;
                }
                else if (element.Index == Elements.DragonPupId)
                {
                    element.Act = Act_DragonPup;
                    element.Draw = Draw_DragonPup;
                    element.Interact = Interact_Enemy;
                }
                else if (element.Index == Elements.DuplicatorId)
                {
                    element.Act = Act_Duplicator;
                    element.Draw = Draw_Duplicator;
                }
                else if (element.Index == Elements.EnergizerId)
                {
                    element.Interact = Interact_Energizer;
                }
                else if (element.Index == Elements.FakeId)
                {
                    element.Interact = Interact_Fake;
                }
                else if (element.Index == Elements.ForestId)
                {
                    element.Interact = Interact_Forest;
                }
                else if (element.Index == Elements.GemId)
                {
                    element.Interact = Interact_Gem;
                }
                else if (element.Index == Elements.HeadId)
                {
                    element.Act = Act_Head;
                    element.Interact = Interact_Enemy;
                }
                else if (element.Index == Elements.InvisibleId)
                {
                    element.Interact = Interact_Invisible;
                }
                else if (element.Index == Elements.KeyId)
                {
                    element.Interact = Interact_Key;
                }
                else if (element.Index == Elements.LavaId || element.Index == Elements.WaterId)
                {
                    element.Interact = Interact_Water;
                }
                else if (element.Index == Elements.LineId)
                {
                    element.Draw = Draw_Line;
                }
                else if (element.Index == Elements.LionId)
                {
                    element.Act = Act_Lion;
                    element.Interact = Interact_Enemy;
                }
                else if (element.Index == Elements.MessengerId)
                {
                    element.Act = Act_Messenger;
                }
                else if (element.Index == Elements.MonitorId)
                {
                    element.Act = Act_Monitor;
                }
                else if (element.Index == Elements.ObjectId)
                {
                    element.Act = Act_Object;
                    element.Draw = Draw_Object;
                    element.Interact = Interact_Object;
                }
                else if (element.Index == Elements.PairerId)
                {
                    element.Act = Act_Pairer;
                }
                else if (element.Index == Elements.PassageId)
                {
                    element.Interact = Interact_Passage;
                }
                else if (element.Index == Elements.PlayerId)
                {
                    element.Act = Act_Player;
                }
                else if (element.Index == Elements.PusherId)
                {
                    element.Act = Act_Pusher;
                    element.Draw = Draw_Pusher;
                }
                else if (element.Index == Elements.RotonId)
                {
                    element.Act = Act_Roton;
                    element.Interact = Interact_Enemy;
                }
                else if (element.Index == Elements.RuffianId)
                {
                    element.Act = Act_Ruffian;
                    element.Interact = Interact_Enemy;
                }
                else if (element.Index == Elements.ScrollId)
                {
                    element.Act = Act_Scroll;
                    element.Interact = Interact_Scroll;
                }
                else if (element.Index == Elements.SegmentId)
                {
                    element.Act = Act_Segment;
                    element.Interact = Interact_Enemy;
                }
                else if (element.Index == Elements.SharkId)
                {
                    element.Act = Act_Shark;
                }
                else if (element.Index == Elements.SliderEWId || element.Index == Elements.SliderNSId)
                {
                    element.Interact = Interact_Pushable;
                }
                else if (element.Index == Elements.SlimeId)
                {
                    element.Act = Act_Slime;
                    element.Interact = Interact_Slime;
                }
                else if (element.Index == Elements.SpiderId)
                {
                    element.Act = Act_Spider;
                    element.Interact = Interact_Enemy;
                }
                else if (element.Index == Elements.SpinningGunId)
                {
                    element.Act = Act_SpinningGun;
                    element.Draw = Draw_SpinningGun;
                }
                else if (element.Index == Elements.StarId)
                {
                    element.Act = Act_Star;
                    element.Draw = Draw_SpinningGun;
                    element.Interact = Interact_Enemy;
                }
                else if (element.Index == Elements.StoneId)
                {
                    element.Act = Act_Stone;
                    element.Draw = Draw_Stone;
                    element.Interact = Interact_Stone;
                }
                else if (element.Index == Elements.TigerId)
                {
                    element.Act = Act_Tiger;
                    element.Interact = Interact_Enemy;
                }
                else if (element.Index == Elements.TorchId)
                {
                    element.Interact = Interact_Torch;
                }
                else if (element.Index == Elements.TransporterId)
                {
                    element.Act = Act_Transporter;
                    element.Draw = Draw_Transporter;
                    element.Interact = Interact_Transporter;
                }
                else if (element.Index == Elements.WebId)
                {
                    element.Draw = Draw_Web;
                }
            }
        }

        virtual internal string InvisibleMessage
        {
            get { return @"You are blocked by an invisible wall."; }
        }

        virtual internal string KeyAlreadyMessage(int color)
        {
            return @"You already have a " + Colors[color] + @" key!";
        }

        virtual internal string KeyMessage(int color)
        {
            return @"You now have the " + Colors[color] + " key.";
        }

        virtual internal void MoveThing(int index, Location target)
        {
            var actor = Actors[index];
            var sourceLocation = actor.Location.Clone();
            var sourceTile = TileAt(actor.Location);
            var targetTile = TileAt(target);
            var underTile = actor.UnderTile.Clone();

            actor.UnderTile.CopyFrom(targetTile);
            if (sourceTile.Id == Elements.PlayerId)
            {
                targetTile.CopyFrom(sourceTile);
            }
            else if (targetTile.Id == Elements.EmptyId)
            {
                targetTile.SetTo(sourceTile.Id, sourceTile.Color & 0x0F);
            }
            else
            {
                targetTile.SetTo(sourceTile.Id, (targetTile.Color & 0x70) | (sourceTile.Color & 0x0F));
            }
            sourceTile.CopyFrom(underTile);
            actor.Location.CopyFrom(target);
            UpdateBoard(target);
            UpdateBoard(sourceLocation);
            if (index == 0)
            {
                // todo: do some torch shit
            }
        }

        internal void PackBoard()
        {
            PackedBoard board = new PackedBoard(Disk.PackBoard(Tiles));
            Boards[Board] = board;
        }

        internal Actor Player
        {
            get { return Actors[0]; }
        }

        internal void PlaySound(int priority, byte[] sound)
        {
        }

        internal void Rnd(Vector result)
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

        public void ReadInput()
        {
            KeyShift = false;
            KeyArrow = false;
            KeyPressed = 0;
            KeyVector.SetTo(0, 0);

            var key = Keyboard.GetKey();
            if (key >= 0)
            {
                KeyPressed = key;
                KeyShift = Keyboard.Shift;
                switch (key)
                {
                    case 0xCB:
                        KeyVector.CopyFrom(Vector.West);
                        KeyArrow = true;
                        break;
                    case 0xCD:
                        KeyVector.CopyFrom(Vector.East);
                        KeyArrow = true;
                        break;
                    case 0xC8:
                        KeyVector.CopyFrom(Vector.North);
                        KeyArrow = true;
                        break;
                    case 0xD0:
                        KeyVector.CopyFrom(Vector.South);
                        KeyArrow = true;
                        break;
                }
            }
        }

        public int ReadKey()
        {
            var key = Keyboard.GetKey();
            if (key > 0)
            {
                KeyPressed = key;
            }
            else
            {
                KeyPressed = 0;
            }
            return KeyPressed;
        }

        internal void RedrawBoard()
        {
            Display.RedrawBoard();
        }

        virtual internal void RemoveActor(int index)
        {
        }

        internal void ResetAlerts()
        {
            AlertAmmo = false;
            AlertDark = false;
            AlertEnergy = false;
            AlertFake = false;
            AlertForest = false;
            AlertGem = false;
            AlertNoAmmo = false;
            AlertNoShoot = false;
            AlertNotDark = false;
            AlertNoTorch = false;
        }

        internal void Seek(Location location, Vector result)
        {
            result.SetTo(0, 0);
            if (RandomNumberDeterministic(2) == 0 || Player.Y == location.Y)
            {
                result.X = (Player.X - location.X).Polarity();
            }
            if (result.X == 0)
            {
                result.Y = (Player.Y - location.Y).Polarity();
            }
            if (EnergyCycles > 0)
            {
                result.SetOpposite();
            }
        }

        virtual internal void SendLabel(int index, string label, bool force)
        {
        }

        internal void SetBoard(int boardIndex)
        {
            var element = Elements.PlayerElement;
            TileAt(Player.Location).SetTo(element.Index, element.Color);
            PackBoard();
            UnpackBoard(boardIndex);
        }

        internal void SetMessage(int duration, string message)
        {
            // todo
        }

        internal void ShowAbout()
        {
        }

        virtual internal void SpawnActor(Location location, Tile tile, int cycle, Actor source)
        {
            // must reserve one actor for player, and one for messenger
            if (ActorCount < (Actors.Capacity - 2))
            {
                ActorCount++;
                var actor = Actors[ActorCount];

                if (source as Actor == null)
                {
                    source = DefaultActor;
                }
                actor.CopyFrom(source);
                actor.Location.CopyFrom(location);
                actor.Cycle = cycle;
                actor.UnderTile.CopyFrom(TileAt(location));
                actor.Code = source.Code;
                if (ElementAt(actor.Location).EditorFloor)
                {
                    var newColor = TileAt(actor.Location).Color & 0x70;
                    newColor |= (tile.Color & 0x0F);
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

        virtual internal bool SpawnProjectile(int id, Location location, Vector vector, bool enemyOwned)
        {
            return false;
        }

        internal void Start()
        {
            if (!ThreadActive)
            {
                ThreadActive = true;
                Thread = new Thread(new ThreadStart(StartMain));
                TimerTick = CoreTimer.Tick;
                Thread.Start();
            }
        }

        internal void Stop()
        {
            if (ThreadActive)
            {
                ThreadActive = false;
            }
        }

        private Thread Thread
        {
            get;
            set;
        }

        private bool ThreadActive
        {
            get;
            set;
        }

        internal Tile TileAt(Location l)
        {
            return Tiles[l];
        }

        internal Tile TileAt(int x, int y)
        {
            return Tiles[new Location(x, y)];
        }

        internal int TimerTick
        {
            get;
            private set;
        }

        virtual internal string TorchMessage
        {
            get { return @"Torch - used for lighting in the underground."; }
        }

        internal void UnpackBoard(int boardIndex)
        {
            Disk.UnpackBoard(Tiles, Boards[boardIndex].Data);
            Board = boardIndex;
        }

        internal void UpdateBoard(Location location)
        {
            AnsiChar ac;
            if (!Dark || ElementAt(location).Shown || (TorchCycles > 0 && Distance(Player.Location, location) < 50) || EditorMode)
            {
                ac = Draw(location);
            }
            else
            {
                ac = new AnsiChar(0xB0, 0x07);
            }
            DrawTile(location, ac);
        }

        internal void UpdateBorder()
        {
            Display.UpdateBorder();
        }

        internal void UpdateRadius(Location location, RadiusMode mode)
        {
        }

        internal void UpdateStatus()
        {
            Display.UpdateStatus();
        }

        public void WaitForTick()
        {
            while (TimerTick == CoreTimer.Tick && ThreadActive)
            {
                Thread.Sleep(1);
                //Thread.Sleep(0);
            }
            TimerTick++;
        }

        virtual internal string WaterMessage
        {
            get { return @"Your way is blocked by water."; }
        }

        internal int Width
        {
            get { return Tiles.Width; }
        }
    }
}
