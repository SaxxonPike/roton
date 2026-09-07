# Unmappable Pascal Functions

The following Pascal functions from the Reconstruction of ZZT and Reconstruction of Super ZZT
sources have no direct equivalent in the Roton.Emulation namespace. They are listed here for
later review.

## GAME.PAS

| Function | Reason |
|---|---|
| `SidebarClearLine` | UI/video rendering detail, absorbed into HUD implementation |
| `SidebarClear` | UI/video rendering detail, absorbed into HUD implementation |
| `GenerateTransitionTable` | Internal transition animation detail, no interface equivalent |
| `AdvancePointer` | Low-level Pascal pointer arithmetic, not needed in managed code |
| `TransitionDrawToFill` | Internal transition animation detail |
| `TransitionDrawToBoard` | Internal transition animation detail |
| `TransitionDrawBoardChange` | Internal transition animation detail |
| `BoardDrawBorder` | Absorbed into HUD/playfield rendering |
| `SidebarPromptCharacter` | Editor UI detail |
| `SidebarPromptSlider` | Editor UI detail |
| `SidebarPromptChoice` | Editor UI detail |
| `SidebarPromptDirection` | Editor UI detail |
| `PromptString` | Absorbed into HUD/dialog implementations |
| `SidebarPromptYesNo` | Absorbed into HUD/dialog implementations |
| `SidebarPromptString` | Absorbed into HUD/dialog implementations |
| `PopupPromptString` | Absorbed into HUD/dialog implementations |
| `PauseOnError` | DOS error handling, not applicable |
| `DisplayIOError` | DOS error handling, not applicable |
| `WorldUnload` | Absorbed into `IWorldManager.OpenWorld` |
| `GameWorldSave` | Absorbed into `IWorldManager.SaveWorld` + HUD |
| `GameWorldLoad` | Absorbed into `IWorldManager.LoadWorld` + HUD |
| `CopyStatDataToTextWindow` | Absorbed into scroll/text window implementation |
| `GetStatIdAt` | Absorbed into `IActorList.ActorIndexAt` |
| `BoardPrepareTileForPlacement` | Editor-only, absorbed into editor implementation |
| `Signum` | Math utility, inlined |
| `Difference` | Math utility, inlined |
| `GameUpdateSidebar` | Absorbed into `IHud.UpdateStatus` |
| `GameDebugPrompt` | Debug/cheat prompt, absorbed into `ICheater` |
| `GameAboutScreen` | Absorbed into dialog/HUD implementations |
| `GamePlayLoop` | Absorbed into `IGame.MainLoop` |
| `GameTitleLoop` | Absorbed into `IGame.MainLoop` |
| `GamePrintRegisterMessage` | ZZT-specific registration message, not applicable |
| `BoardPassageTeleport` | Absorbed into `PassageInteraction.Interact` |
| `BoardUpdateDrawOffset` | Super ZZT only, absorbed into `ICamera.UpdateCamera` |
| `DrawPlayfieldBorder` | Super ZZT only, absorbed into HUD/playfield rendering |
| `ClearDisplayMessage` | Super ZZT only, absorbed into `IMessenger` |
| `DrawStatusMessage` | Super ZZT only, absorbed into `IMessenger`/HUD |

## OOP.PAS

| Function | Reason |
|---|---|
| `OopError` | Absorbed into `ITracer.TraceError` |
| `OopSkipLine` | Internal parser detail, no interface equivalent |
| `OopParseDirection` | Absorbed into `IDirectionEvaluator.TryEval` |
| `OopReadDirection` | Absorbed into `IDirectionEvaluator.TryEval` |
| `OopIterateStat` | Internal label search detail, absorbed into `IBroadcaster` |
| `WorldGetFlagPosition` | Absorbed into flag/world state management |
| `WorldSetFlag` | Absorbed into flag/world state management |
| `WorldClearFlag` | Absorbed into flag/world state management |
| `OopStringToWord` | Internal string normalization, inlined |
| `GetColorForTileMatch` | Absorbed into `IColorMatcher` |
| `OopCheckCondition` | Absorbed into `IConditionEvaluator.TryEval` |
| `OopExecute` | Absorbed into `ICodeExecutor.ExecuteCode` + `IInterpreter.Execute` |

## ELEMENTS.PAS

| Function | Reason |
|---|---|
| `ElementDefaultTick` | Absorbed into default `IAction` implementation |
| `ElementDefaultTouch` | Absorbed into default `IInteraction` implementation |
| `ElementDefaultDraw` | Absorbed into default draw implementation |
| `ElementMessageTimerTick` | Absorbed into specific `IAction` implementation |
| `ElementDamagingTouch` | Absorbed into `EnemyInteraction` |
| `ElementLionTick` | Absorbed into specific `IAction` implementation |
| `ElementTigerTick` | Absorbed into specific `IAction` implementation |
| `ElementRuffianTick` | Absorbed into specific `IAction` implementation |
| `ElementBearTick` | Absorbed into specific `IAction` implementation |
| `ElementCentipedeHeadTick` | Absorbed into specific `IAction` implementation |
| `ElementCentipedeSegmentTick` | Absorbed into specific `IAction` implementation |
| `ElementBulletTick` | Absorbed into specific `IAction` implementation |
| `ElementSpinningGunDraw` | Absorbed into specific draw implementation |
| `ElementLineDraw` | Absorbed into specific draw implementation |
| `ElementSpinningGunTick` | Absorbed into specific `IAction` implementation |
| `ElementConveyorCWDraw` | Absorbed into specific draw implementation |
| `ElementConveyorCWTick` | Absorbed into `IConveyor.Convey` |
| `ElementConveyorCCWDraw` | Absorbed into specific draw implementation |
| `ElementConveyorCCWTick` | Absorbed into `IConveyor.Convey` |
| `ElementBombDraw` | Absorbed into specific draw implementation |
| `ElementBombTick` | Absorbed into specific `IAction` implementation |
| `ElementBombTouch` | Absorbed into `BombInteraction` |
| `ElementTransporterTouch` | Absorbed into `TransporterInteraction` |
| `ElementTransporterTick` | Absorbed into specific `IAction` implementation |
| `ElementTransporterDraw` | Absorbed into specific draw implementation |
| `ElementStarDraw` | Absorbed into specific draw implementation |
| `ElementStarTick` | Absorbed into specific `IAction` implementation |
| `ElementEnergizerTouch` | Absorbed into `EnergizerInteraction` |
| `ElementSlimeTick` | Absorbed into specific `IAction` implementation |
| `ElementSlimeTouch` | Absorbed into `SlimeInteraction` |
| `ElementSharkTick` | Absorbed into specific `IAction` implementation (ZZT only) |
| `ElementBlinkWallDraw` | Absorbed into specific draw implementation |
| `ElementBlinkWallTick` | Absorbed into specific `IAction` implementation |
| `ElementPushablePush` | Absorbed into `IPusher.Push` |
| `ElementDuplicatorDraw` | Absorbed into specific draw implementation |
| `ElementObjectTick` | Absorbed into specific `IAction` implementation |
| `ElementObjectDraw` | Absorbed into specific draw implementation |
| `ElementObjectTouch` | Absorbed into `ObjectInteraction` |
| `ElementDuplicatorTick` | Absorbed into specific `IAction` implementation |
| `ElementScrollTick` | Absorbed into specific `IAction` implementation |
| `ElementScrollTouch` | Absorbed into `ScrollInteraction` |
| `ElementKeyTouch` | Absorbed into `KeyInteraction` |
| `ElementAmmoTouch` | Absorbed into `AmmoInteraction` |
| `ElementGemTouch` | Absorbed into `GemInteraction` |
| `ElementPassageTouch` | Absorbed into `PassageInteraction` |
| `ElementDoorTouch` | Absorbed into `DoorInteraction` |
| `ElementPushableTouch` | Absorbed into `PuzzleInteraction` |
| `ElementPusherDraw` | Absorbed into specific draw implementation |
| `ElementPusherTick` | Absorbed into specific `IAction` implementation |
| `ElementTorchTouch` | Absorbed into `TorchInteraction` |
| `ElementInvisibleTouch` | Absorbed into `InvisibleWallInteraction` |
| `ElementForestTouch` | Absorbed into `ForestInteraction` + `IForestHandler` |
| `ElementFakeTouch` | Absorbed into `FakeWallInteraction` |
| `ElementBoardEdgeTouch` | Absorbed into `BoardEdgeInteraction` |
| `ElementWaterTouch` | Absorbed into `WaterInteraction` |
| `ElementMove` | Absorbed into `IPusher` (private `MoveTile`) |
| `GamePromptEndPlay` | Absorbed into HUD/dialog implementations |
| `DrawPlayerSurroundings` | Absorbed into `IRadiusUpdater.UpdateRadius` |
| `ElementConnectedDraw` | Super ZZT only, absorbed into specific draw implementation |
| `ElementLineDraw` (Super) | Absorbed into specific draw implementation |
| `ElementWebDraw` | Super ZZT only, absorbed into specific draw implementation |
| `ElementRotonTick` | Super ZZT only, absorbed into specific `IAction` implementation |
| `ElementDragonPupTick` | Super ZZT only, absorbed into specific `IAction` implementation |
| `ElementDragonPupDraw` | Super ZZT only, absorbed into specific draw implementation |
| `ElementPairerTick` | Super ZZT only, absorbed into specific `IAction` implementation |
| `ElementSpiderTick` | Super ZZT only, absorbed into specific `IAction` implementation |
| `ElementStoneTouch` | Super ZZT only, absorbed into `StoneInteraction` |
| `ElementStoneDraw` | Super ZZT only, absorbed into specific draw implementation |
| `ElementStoneTick` | Super ZZT only, absorbed into specific `IAction` implementation |

## TXTWIND.PAS

| Function | Reason |
|---|---|
| `UpCaseString` | String utility, inlined |
| `TextWindowInitState` | Absorbed into scroll/text window implementation |
| `TextWindowDrawTitle` | Absorbed into `IScrollRenderer` |
| `TextWindowDrawOpen` | Absorbed into `IScrollRenderer.Open` |
| `TextWindowDrawClose` | Absorbed into `IScrollRenderer.Close` |
| `TextWindowDrawLine` | Absorbed into `IScrollRenderer.RenderContent` |
| `TextWindowDraw` | Absorbed into `IScrollRenderer.RenderContent` |
| `TextWindowAppend` | Absorbed into `IScrollContent` |
| `TextWindowFree` | Absorbed into `IScrollContent` (already tagged) |
| `TextWindowPrint` | No equivalent (print to printer not implemented) |
| `TextWindowSelect` | Absorbed into `IScroll.ShowMessage` |
| `TextWindowEdit` | Editor-only, no equivalent |
| `TextWindowOpenFile` | Absorbed into `IScroll.ShowHelpFile` |
| `TextWindowSaveFile` | Editor-only, no equivalent |
| `TextWindowDisplayFile` | Absorbed into `IScroll.ShowHelpFile` |
| `TextWindowInit` | Absorbed into scroll initialization |

## SOUNDS.PAS

| Function | Reason |
|---|---|
| `SoundQueue` | Absorbed into `ISoundPlayer.PlaySound` |
| `SoundClearQueue` | Absorbed into `ISoundPlayer.ClearSound` |
| `SoundInitFreqTable` | Internal initialization, no interface equivalent |
| `SoundInitDrumTable` | Internal initialization, no interface equivalent |
| `SoundPlayDrum` | Absorbed into `ISpeaker.PlayDrum` |
| `SoundCheckTimeIntr` | DOS timer interrupt, not applicable |
| `SoundHasTimeElapsed` | Absorbed into `IBoardTime` |
| `SoundTimerHandler` | DOS timer interrupt handler, not applicable |
| `SoundUninstall` | DOS timer interrupt, not applicable |
| `SoundParse` | Absorbed into `IMusicEncoder` |

## INPUT.PAS / KEYS.PAS / VIDEO.PAS

These files contain low-level DOS input, keyboard, and video routines that are entirely
replaced by the platform abstraction layer (`ITerminal`, `IKeyboard`, `IJoystick`, etc.)
and have no direct method-level mappings.

## IGame.StepOnce

`IGame.StepOnce` has no Pascal equivalent — it is a Roton-specific addition to support
single-step debugging and test execution.
