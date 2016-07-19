namespace Roton.Emulation
{
    internal partial class CoreBase
    {
        internal virtual void MainLoop(bool gameIsActive)
        {
            var alternating = false;

            Display.CreateStatusText();
            Display.UpdateStatus();

            if (Init)
            {
                if (!AboutShown)
                {
                    ShowAbout();
                }
                if (DefaultWorldName.Length <= 0)
                {
                    // normally we would load the world here,
                    // however it will have already been loaded in the context
                }
                StartBoard = Board;
                SetBoard(0);
                Init = false;
            }

            var element = Elements[PlayerElement];
            TileAt(Player.Location).SetTo(element.Index, element.Color);
            if (PlayerElement == Elements.MonitorId)
            {
                SetMessage(0, @"");
                Display.DrawTitleStatus();
            }

            if (gameIsActive)
            {
                FadePurple();
            }

            GameWaitTime = GameSpeed << 1;
            BreakGameLoop = false;
            GameCycle = RandomNumberDeterministic(0x64);
            ActIndex = ActorCount + 1;

            while (ThreadActive)
            {
                if (!GamePaused)
                {
                    if (ActIndex <= ActorCount)
                    {
                        var actorData = Actors[ActIndex];
                        if (actorData.Cycle != 0)
                        {
                            if (ActIndex%actorData.Cycle == GameCycle%actorData.Cycle)
                            {
                                Elements[TileAt(actorData.Location).Id].Act(ActIndex);
                            }
                        }
                        ActIndex++;
                    }
                }
                else
                {
                    ActIndex = ActorCount + 1;
                    element = Elements[PlayerElement];
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
                    Display.DrawPausing();
                    ReadInput();
                    if (KeyPressed == 0x1B)
                    {
                        if (Health > 0)
                        {
                            BreakGameLoop = Display.EndGameConfirmation();
                        }
                        else
                        {
                            BreakGameLoop = true;
                            UpdateBorder();
                        }
                        KeyPressed = 0;
                    }
                    if (!KeyVector.IsZero)
                    {
                        var target = Player.Location.Sum(KeyVector);
                        ElementAt(target).Interact(target, 0, KeyVector);
                    }
                    if (!KeyVector.IsZero)
                    {
                        var target = Player.Location.Sum(KeyVector);
                        if (ElementAt(target).Floor)
                        {
                            if (ElementAt(Player.Location).Index == Elements.PlayerId)
                            {
                                MoveActor(0, target);
                            }
                            else
                            {
                                UpdateBoard(Player.Location);
                                Player.Location.Add(KeyVector);
                                TileAt(Player.Location).SetTo(Elements.PlayerId, Elements.PlayerElement.Color);
                                UpdateBoard(Player.Location);
                                UpdateRadius(Player.Location, RadiusMode.Update);
                                UpdateRadius(Player.Location.Difference(KeyVector), RadiusMode.Update);
                            }
                            GamePaused = false;
                            Display.ClearPausing();
                            GameCycle = RandomNumberDeterministic(100);
                            Locked = true;
                        }
                    }
                }

                if (ActIndex > ActorCount)
                {
                    if (!BreakGameLoop)
                    {
                        if (GameWaitTime <= 0 || GetMainTimeElapsed(GameWaitTime))
                        {
                            GameCycle++;
                            if (GameCycle > 420)
                            {
                                GameCycle = 1;
                            }
                            ActIndex = 0;
                            ReadInput();
                        }
                    }
                    WaitForTick();
                }

                if (BreakGameLoop)
                {
                    ClearSound();
                    if (PlayerElement == Elements.PlayerId)
                    {
                        if (Health <= 0)
                        {
                            EnterHighScore(Score);
                        }
                    }
                    else if (PlayerElement == Elements.MonitorId)
                    {
                        Display.ClearTitleStatus();
                    }
                    element = Elements.PlayerElement;
                    TileAt(Player.Location).SetTo(element.Index, element.Color);
                    GameOver = false;
                    break;
                }
            }
        }

        internal virtual void StartMain()
        {
            GameSpeed = 4;
            DefaultSaveName = "SAVED";
            DefaultBoardName = "TEMP";
            DefaultWorldName = "TOWN";
            Display.GenerateFadeMatrix();
            if (!WorldLoaded)
            {
                ClearWorld();
            }
            SetGameMode();
            TitleScreenLoop();
        }

        internal virtual void TitleScreenLoop()
        {
            QuitZzt = false;
            Init = true;
            StartBoard = 0;
            var gameEnded = true;
            while (ThreadActive)
            {
                if (!Init)
                {
                    SetBoard(0);
                }
                while (ThreadActive)
                {
                    PlayerElement = Elements.MonitorId;
                    var gameIsActive = false;
                    GamePaused = false;
                    MainLoop(gameEnded);
                    if (!ThreadActive)
                    {
                        // escape if the thread is supposed to shut down
                        break;
                    }

                    switch (KeyPressed.ToUpperCase())
                    {
                        case 0x57: // W
                            break;
                        case 0x50: // P
                            if (Locked)
                            {
                                // reload world here
                                gameIsActive = WorldLoaded;
                                StartBoard = Board;
                            }
                            else
                            {
                                gameIsActive = true;
                            }
                            if (gameIsActive)
                            {
                                SetBoard(StartBoard);
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
                        PlayerElement = Elements.PlayerId;
                        GamePaused = true;
                        MainLoop(true);
                        gameEnded = true;
                    }

                    if (gameEnded || QuitZzt)
                    {
                        break;
                    }
                }
                if (QuitZzt)
                {
                    break;
                }
            }
        }
    }
}