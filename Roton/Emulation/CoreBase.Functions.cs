﻿using System;
using System.Threading;

namespace Roton.Emulation
{
    internal partial class CoreBase
    {
        internal virtual Actor ActorAt(Location location)
        {
            return Actors[ActorIndexAt(location)];
        }

        internal virtual int ActorIndexAt(Location location)
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

        internal virtual void Attack(int index, Location location)
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
                Destroy(location);
                PlaySound(2, Sounds.EnemySuicide);
            }
        }

        internal virtual void ClearBoard()
        {
            var emptyId = Elements.EmptyId;
            var boardEdgeId = EdgeTile.Id;
            var boardBorderId = BorderTile.Id;
            var boardBorderColor = BorderTile.Color;

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
            for (var y = 0; y <= Tiles.Height + 1; y++)
            {
                TileAt(0, y).Id = boardEdgeId;
                TileAt(Width + 1, y).Id = boardEdgeId;
            }
            for (var x = 0; x <= Width + 1; x++)
            {
                TileAt(x, 0).Id = boardEdgeId;
                TileAt(x, Height + 1).Id = boardEdgeId;
            }

            // clear out board
            for (var x = 1; x <= Width; x++)
            {
                for (var y = 1; y <= Height; y++)
                {
                    TileAt(x, y).SetTo(emptyId, 0);
                }
            }

            // build border
            for (var y = 1; y <= Height; y++)
            {
                TileAt(1, y).SetTo(boardBorderId, boardBorderColor);
                TileAt(Width, y).SetTo(boardBorderId, boardBorderColor);
            }
            for (var x = 1; x <= Width; x++)
            {
                TileAt(x, 1).SetTo(boardBorderId, boardBorderColor);
                TileAt(x, Height).SetTo(boardBorderId, boardBorderColor);
            }

            // generate player actor
            var element = Elements.PlayerElement;
            ActorCount = 0;
            Player.Location.SetTo(Width/2, Height/2);
            TileAt(Player.Location).SetTo(element.Index, element.Color);
            Player.Cycle = 1;
            Player.UnderTile.SetTo(0, 0);
            Player.Pointer = 0;
            Player.Length = 0;
        }

        internal virtual void ClearSound()
        {
            SoundBufferLength = 0;
            SoundPlaying = false;
            StopSound();
        }

        internal virtual void ClearWorld()
        {
            BoardCount = 0;
            Boards.Clear();
            ResetAlerts();
            ClearBoard();
            Boards.Add(new PackedBoard(Serializer.PackBoard(Tiles)));
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

        internal virtual void Convey(Location center, int direction)
        {
        }

        internal virtual void Destroy(Location location)
        {
            var index = ActorIndexAt(location);
            if (index == -1)
            {
                TileAt(location).Id = Elements.EmptyId;
            }
            else
            {
                Harm(index);
            }
        }

        internal virtual void DrawChar(Location location, AnsiChar ac)
        {
            Display.DrawChar(location.X, location.Y, ac);
        }

        internal virtual void DrawString(Location location, string text, int color)
        {
            Display.DrawString(location.X, location.Y, text, color);
        }

        internal virtual void DrawTile(Location location, AnsiChar ac)
        {
            Display.DrawTile(location.X - 1, location.Y - 1, ac);
        }

        internal virtual Element ElementAt(Location location)
        {
            return Elements[TileAt(location).Id];
        }

        internal virtual Element ElementAt(int x, int y)
        {
            return ElementAt(new Location(x, y));
        }

        internal virtual void EnterBoard()
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

        internal virtual void ExecutePassage(Location location)
        {
            var searchColor = TileAt(location).Color;
            var oldBoard = Board;
            var passageIndex = ActorIndexAt(location);
            var passageTarget = Actors[passageIndex].P3;
            SetBoard(passageTarget);
            var target = new Location();

            for (var x = 1; x <= Width; x++)
            {
                for (var y = 1; y <= Height; y++)
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

        internal virtual void FadeBoard(AnsiChar ac)
        {
            Display.FadeBoard(ac);
        }

        internal virtual void FadePurple()
        {
            FadeBoard(new AnsiChar(0xDB, 0x05));
            RedrawBoard();
        }

        internal virtual void FadeRed()
        {
            FadeBoard(new AnsiChar(0xDB, 0x04));
            RedrawBoard();
        }

        internal virtual void ForcePlayerColor()
        {
            if (TileAt(Player.Location).Color != Elements.PlayerElement.Color ||
                Elements.PlayerElement.Character != 0x02)
            {
                Elements.PlayerElement.Character = 2;
                TileAt(Player.Location).Color = Elements.PlayerElement.Color;
                UpdateBoard(Player.Location);
            }
        }

        internal bool GetMainTimeElapsed(int interval)
        {
            var result = false;
            while (GetTimeDifference(TimerTick, MainTime) > 0)
            {
                result = true;
                MainTime = (MainTime + interval) & 0x7FFF;
            }
            return result;
        }

        internal bool GetPlayerTimeElapsed(int interval)
        {
            var result = false;
            while (GetTimeDifference(TimerTick, PlayerTime) > 0)
            {
                result = true;
                PlayerTime = (PlayerTime + interval) & 0x7FFF;
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

        internal virtual Vector GetVector4(int index)
        {
            return new Vector(Vector4[index], Vector4[index + 4]);
        }

        internal virtual Vector GetVector8(int index)
        {
            return new Vector(Vector8[index], Vector8[index + 8]);
        }

        internal virtual void Harm(int index)
        {
            var actor = Actors[index];
            if (index == 0)
            {
                if (Health > 0)
                {
                    Health -= 10;
                    UpdateStatus();
                    SetMessage(0x64, @"Ouch!");
                    var color = TileAt(actor.Location).Color;
                    color &= 0x0F;
                    color |= 0x70;
                    TileAt(actor.Location).Color = color;
                    if (Health > 0)
                    {
                        TimePassed = 0;
                        if (RestartOnZap)
                        {
                            PlaySound(4, Sounds.TimeOut);
                            TileAt(actor.Location).Id = Elements.EmptyId;
                            UpdateBoard(actor.Location);
                            var oldLocation = actor.Location.Clone();
                            actor.Location.CopyFrom(Enter);
                            UpdateRadius(oldLocation, 0);
                            UpdateRadius(actor.Location, 0);
                            GamePaused = true;
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
                var element = TileAt(actor.Location).Id;
                if (element == Elements.BulletId)
                {
                    PlaySound(3, Sounds.BulletDie);
                }
                else if (element != Elements.ObjectId)
                {
                    PlaySound(3, Sounds.EnemyDie);
                }
                RemoveActor(index);
            }
        }

        public virtual int Height => Tiles.Height;

        internal virtual void InitializeElements(bool showInvisibles)
        {
            // this isn't all the initializations.
            // todo: replace this with the ability to completely reinitialize engine default memory
            Elements.InvisibleElement.Character = showInvisibles ? 0xB0 : 0x20;
            Elements.InvisibleElement.Color = 0xFF;
            Elements.PlayerElement.Character = 0x02;
        }

        internal virtual void InitializeElementDelegates()
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
                    element.Act = Act_Clockwise;
                    element.Draw = Draw_Clockwise;
                }
                else if (element.Index == Elements.CounterId)
                {
                    element.Act = Act_Counter;
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
                else if (element.Index == Elements.SliderEwId || element.Index == Elements.SliderNsId)
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

        internal virtual byte[] LoadFile(string filename)
        {
            try
            {
                return Disk.ReadFile(filename);
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal virtual void MoveActor(int index, Location target)
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
            if (index == 0 && Dark)
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
                            if ((Distance(sourceLocation, glowLocation) < 50) ^ (Distance(target, glowLocation) < 50))
                            {
                                UpdateBoard(glowLocation);
                            }
                        }
                    }
                }
            }
            if (index == 0)
            {
                UpdateCamera();
            }
        }

        internal virtual void MoveTile(Location source, Location target)
        {
            var sourceIndex = ActorIndexAt(source);
            if (sourceIndex >= 0)
            {
                MoveActor(sourceIndex, target);
            }
            else
            {
                TileAt(target).CopyFrom(TileAt(source));
                TileAt(source).Id = Elements.EmptyId;
                UpdateBoard(source);
                UpdateBoard(target);
            }
        }

        internal virtual void PackBoard()
        {
            var board = new PackedBoard(Serializer.PackBoard(Tiles));
            Boards[Board] = board;
        }

        public virtual Actor Player => Actors[0];

        internal virtual void Push(Location location, Vector vector)
        {
            // this is here to prevent endless push loops
            // but doesn't exist in the original code
            if (vector.IsZero)
            {
                throw Exceptions.PushStackOverflow;
            }

            var tile = TileAt(location);
            if ((tile.Id == Elements.SliderEwId && vector.Y == 0) || (tile.Id == Elements.SliderNsId && vector.X == 0) ||
                Elements[tile.Id].Pushable)
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
                if (!furtherElement.Floor && furtherElement.Destructible && furtherTile.Id != Elements.PlayerId)
                {
                    Destroy(location.Sum(vector));
                }

                furtherElement = Elements[furtherTile.Id];
                if (furtherElement.Floor)
                {
                    MoveTile(location, location.Sum(vector));
                }
            }
        }

        internal virtual void PushThroughTransporter(Location location, Vector vector)
        {
            // todo
        }

        internal virtual Vector Rnd()
        {
            var result = new Vector();
            Rnd(result);
            return result;
        }

        internal virtual void Rnd(Vector result)
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

        public virtual void ReadInput()
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

        public virtual int ReadKey()
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

        internal virtual void RedrawBoard()
        {
            Display.RedrawBoard();
        }

        internal virtual void RemoveActor(int index)
        {
            var actor = Actors[index];
            if (actor.Pointer != 0)
            {
                var refCount = 0;
                for (var i = 1; i < ActorCount; i++)
                {
                    if (Actors[i].Pointer == actor.Pointer)
                    {
                        refCount++;
                    }
                }
                if (refCount < 1)
                {
                    actor.Code = "";
                }
                actor.Pointer = 0;
            }

            if (index < ActIndex)
            {
                ActIndex--;
            }

            TileAt(actor.Location).CopyFrom(actor.UnderTile);
            if (actor.Location.Y > 0)
            {
                UpdateBoard(actor.Location);
            }

            for (var i = 1; i < ActorCount; i++)
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

            if (index < ActorCount)
            {
                for (var i = index; i < ActorCount; i++)
                {
                    Actors[i].CopyFrom(Actors[i + 1]);
                }
            }

            ActorCount--;
        }

        internal virtual void ResetAlerts()
        {
            AlertAmmo = true;
            AlertDark = true;
            AlertEnergy = true;
            AlertFake = true;
            AlertForest = true;
            AlertGem = true;
            AlertNoAmmo = true;
            AlertNoShoot = true;
            AlertNotDark = true;
            AlertNoTorch = true;
            AlertTorch = true;
        }

        internal virtual void Seek(Location location, Vector result)
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

        internal virtual void SetBoard(int boardIndex)
        {
            var element = Elements.PlayerElement;
            TileAt(Player.Location).SetTo(element.Index, element.Color);
            PackBoard();
            UnpackBoard(boardIndex);
        }

        internal virtual void SetEditorMode()
        {
            InitializeElements(true);
            EditorMode = true;
        }

        internal virtual void SetGameMode()
        {
            InitializeElements(false);
            EditorMode = false;
        }

        internal virtual void SetMessage(int duration, string message, string message2 = "")
        {
            var index = ActorIndexAt(new Location(0, 0));
            if (index >= 0)
            {
                RemoveActor(index);
                UpdateBorder();
            }
            if ((message != null && message.Length > 0) || (message2 != null && message2.Length > 0))
            {
                SpawnActor(new Location(0, 0), new Tile(Elements.MessengerId, 0), 1, DefaultActor);
                Actors[ActorCount].P2 = duration/(GameWaitTime + 1);
            }
            Message = message;
            Message2 = message;
        }

        internal virtual void ShowAbout()
        {
        }

        internal virtual void SpawnActor(Location location, Tile tile, int cycle, Actor source)
        {
            // must reserve one actor for player, and one for messenger
            if (ActorCount < Actors.Capacity - 2)
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
                if (ElementAt(actor.Location).EditorFloor)
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

        internal virtual bool SpawnProjectile(int id, Location location, Vector vector, bool enemyOwned)
        {
            var target = location.Sum(vector);
            var element = ElementAt(target);

            if (element.Floor || element.Index == Elements.WaterId)
            {
                SpawnActor(target, new Tile(id, Elements[id].Color), 1, DefaultActor);
                var actor = Actors[ActorCount];
                actor.P1 = enemyOwned ? 1 : 0;
                actor.Vector.CopyFrom(vector);
                actor.P2 = 0x64;
                return true;
            }
            else
            {
                if (element.Index != Elements.BreakableId)
                {
                    if (element.Destructible)
                    {
                        if (enemyOwned != (element.Index == Elements.PlayerId) || EnergyCycles > 0)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                    Destroy(target);
                    PlaySound(2, Sounds.BulletDie);
                    return true;
                }
            }
            return false;
        }

        internal Tile TileAt(Location l)
        {
            return Tiles[l];
        }

        internal Tile TileAt(int x, int y)
        {
            return Tiles[new Location(x, y)];
        }

        internal int TimerBase => CoreTimer.Tick & 0x7FFF;

        private int _timerTick;

        internal int TimerTick
        {
            get { return _timerTick; }
            private set { _timerTick = value & 0x7FFF; }
        }

        internal virtual void UnpackBoard(int boardIndex)
        {
            Serializer.UnpackBoard(Tiles, Boards[boardIndex].Data);
            Board = boardIndex;
        }

        internal virtual void UpdateBoard(Location location)
        {
            DrawTile(location, Draw(location));
        }

        internal virtual void UpdateBorder()
        {
            Display.UpdateBorder();
        }

        internal virtual void UpdateCamera()
        {
            Display.UpdateCamera();
        }

        internal virtual void UpdateRadius(Location location, RadiusMode mode)
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
                    if (x >= 1 && x <= Width && y >= 1 && y <= Height)
                    {
                        var target = new Location(x, y);
                        if (mode != RadiusMode.Update)
                        {
                            if (Distance(source, target) < 50)
                            {
                                var element = ElementAt(target);
                                if (mode == RadiusMode.Explode)
                                {
                                    if (element.Code.Length > 0)
                                    {
                                        var actorIndex = ActorIndexAt(target);
                                        if (actorIndex > 0)
                                        {
                                            BroadcastLabel(-actorIndex, @"BOMBED", false);
                                        }
                                    }
                                    if (element.Destructible || element.Index == Elements.StarId)
                                    {
                                        Destroy(target);
                                    }
                                    if (element.Index == Elements.EmptyId || element.Index == Elements.BreakableId)
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

        internal virtual void UpdateStatus()
        {
            Display.UpdateStatus();
        }

        public virtual void WaitForTick()
        {
            while (TimerTick == TimerBase && ThreadActive)
            {
                Thread.Sleep(1);
                //Thread.Sleep(0);
            }
            TimerTick++;
        }

        public virtual int Width => Tiles.Width;
    }
}