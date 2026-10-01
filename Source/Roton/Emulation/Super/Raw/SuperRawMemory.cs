using System.Runtime.InteropServices;
using Roton.Emulation.Data;

namespace Roton.Emulation.Super.Raw;

/// <summary>
/// Represents the memory map for Super ZZT 2.0.
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10000)]
internal struct SuperRawMemory
{
    // Const section

    [FieldOffset(0x1E64)] public SuperString255 TransporterNSChars;
    [FieldOffset(0x1F64)] public SuperString255 TransporterEWChars;
    [FieldOffset(0x2064)] public SuperString255 StarAnimChars;
    [FieldOffset(0x2164)] public SuperString20 NeighborBoardStrs; // array[4]
    [FieldOffset(0x21B8)] public HWord ProgressAnimColors; // array[8]
    [FieldOffset(0x21C0)] public SuperString5 ProgressAnimStrings; // array[8]
    [FieldOffset(0x21F0)] public SuperString8 ColorNames; // array[7]
    [FieldOffset(0x2230)] public Word DiagonalDeltaX;
    [FieldOffset(0x2240)] public Word DiagonalDeltaY;
    [FieldOffset(0x2250)] public Word NeighborDeltaX;
    [FieldOffset(0x2258)] public Word NeighborDeltaY;
    [FieldOffset(0x2260)] public SuperTile TileBorder;
    [FieldOffset(0x2262)] public SuperStat StatTemplateDefault;
    [FieldOffset(0x227C)] public SuperString16 LineChars;

    // Pascal builtin

    [FieldOffset(0x232E)] public DWord RandomSeed;

    // GAMEVARS.PAS

    [FieldOffset(0x2334)] public Word ForestSoundTableIdx;
    [FieldOffset(0x2336)] public SuperCoord TransitionTable; // array[480]
    [FieldOffset(0x2AB6)] public SuperSmallString LoadedGameFileName;
    [FieldOffset(0x2AF4)] public SuperSmallString SavedGameFileName;
    [FieldOffset(0x2B32)] public SuperSmallString SavedBoardFileName;
    [FieldOffset(0x2B70)] public SuperSmallString StartupWorldFileName;
    [FieldOffset(0x2BAE)] public SuperBoard Board;
    [FieldOffset(0x7784)] public SuperWorld World;
    [FieldOffset(0x7C0B)] public Bool MessageAmmoNotShown;
    [FieldOffset(0x7C0C)] public Bool MessageOutOfAmmoNotShown;
    [FieldOffset(0x7C0D)] public Bool MessageNoShootingNotShown;
    [FieldOffset(0x7C0E)] public Bool MessageForestNotShown;
    [FieldOffset(0x7C0F)] public Bool MessageFakeNotShown;
    [FieldOffset(0x7C10)] public Bool MessageGemNotShown;
    [FieldOffset(0x7C11)] public Bool MessageEnergizerNotShown;
    [FieldOffset(0x7C12)] public Bool unkVar_7C12; // array[15]
    [FieldOffset(0x7C22)] public SuperSmallString MessageLine1;
    [FieldOffset(0x7C60)] public SuperSmallString MessageLine2;
    [FieldOffset(0x7C9E)] public Bool GameTitleExitRequested;
    [FieldOffset(0x7C9F)] public Bool GamePlayExitRequested;
    [FieldOffset(0x7CA0)] public Word GameStateElement;
    [FieldOffset(0x7CA2)] public Word ReturnBoardId;
    [FieldOffset(0x7CA4)] public HWord TickSpeed;
    [FieldOffset(0x7CA6)] public DWord IoTmpBuf; // ptr to array[20000]
    [FieldOffset(0x7CAA)] public SuperElementDef ElementDefs; // array[80]
    [FieldOffset(0xB94A)] public Word EditorPatternCount;
    [FieldOffset(0xB94C)] public HWord EditorPatterns; // array[10]
    [FieldOffset(0xB956)] public Word TickTimeDuration;
    [FieldOffset(0xB958)] public Word CurrentTick;
    [FieldOffset(0xB95A)] public Word CurrentStatTicked;
    [FieldOffset(0xB95C)] public Bool GamePaused;
    [FieldOffset(0xB95E)] public Word TickTimeCounter;
    [FieldOffset(0xB960)] public Bool ForceDarknessOff;
    [FieldOffset(0xB961)] public HWord InitialTextAttr;
    [FieldOffset(0xB962)] public PChar OopChar;
    [FieldOffset(0xB964)] public SuperString20 OopWord;
    [FieldOffset(0xB97A)] public Word OopValue;
    [FieldOffset(0xB97C)] public Bool DebugEnabled;
    [FieldOffset(0xB97E)] public SuperHighScoreEntry HighScoreList; // array[30]
    [FieldOffset(0xC0E0)] public Bool EditorEnabled;
    [FieldOffset(0xC0E1)] public Bool IsRegistered;
    [FieldOffset(0xC0E2)] public Bool ResetConfig;
    [FieldOffset(0xC0E3)] public Bool JustStarted;
    [FieldOffset(0xC0E4)] public Word WorldFileDescCount;
    [FieldOffset(0xC0E6)] public SuperSmallString WorldFileDescKeys; // array[3]
    [FieldOffset(0xC19E)] public SuperSmallString WorldFileDescValues; // array[3]
    [FieldOffset(0xC256)] public DWord WorldFileMenuScrPtrs; // array[5]
    [FieldOffset(0xC26A)] public Word WorldFileMenuScrLens; // array[5]
    [FieldOffset(0xC274)] public DWord MenuScrPointer;
    [FieldOffset(0xC278)] public Word MenuScrLen;

    // TXTWIND.PAS

    [FieldOffset(0xC27A)] public Word TextWindowX;
    [FieldOffset(0xC27C)] public Word TextWindowY;
    [FieldOffset(0xC27E)] public Word TextWindowWidth;
    [FieldOffset(0xC280)] public Word TextWindowHeight;
    [FieldOffset(0xC282)] public SuperString80 TextWindowStrInnerEmpty;
    [FieldOffset(0xC2D4)] public SuperString80 TextWindowStrText;
    [FieldOffset(0xC326)] public SuperString80 TextWindowStrInnerLine;
    [FieldOffset(0xC378)] public SuperString80 TextWindowStrTop;
    [FieldOffset(0xC3CA)] public SuperString80 TextWindowStrBottom;
    [FieldOffset(0xC41C)] public SuperString80 TextWindowStrSep;
    [FieldOffset(0xC46E)] public SuperString80 TextWindowStrInnerSep;
    [FieldOffset(0xC4C0)] public SuperString80 TextWindowStrInnerArrows;
    [FieldOffset(0xC511)] public Bool TextWindowRejected;
    [FieldOffset(0xC512)] public SuperSmallString ResourceDataFileName;
    [FieldOffset(0xC550)] public SuperResourceDataHeader ResourceDataHeader;
    [FieldOffset(0xCB6A)] public DWord OrderPrintId; // ptr to string

    // INPUT.PAS

    [FieldOffset(0xCC6E)] public Word InputDeltaX;
    [FieldOffset(0xCC70)] public Word InputDeltaY;
    [FieldOffset(0xCC72)] public Bool InputShiftPressed;
    [FieldOffset(0xCC73)] public Bool InputShiftAccepted;
    [FieldOffset(0xCC74)] public Bool InputJoystickEnabled;
    [FieldOffset(0xCC75)] public Bool InputMouseEnabled;
    [FieldOffset(0xCC76)] public PChar InputKeyPressed;
    [FieldOffset(0xCC78)] public Word InputMouseX;
    [FieldOffset(0xCC7A)] public Word InputMouseY;
    [FieldOffset(0xCC7C)] public Word InputMouseActivationX;
    [FieldOffset(0xCC7E)] public Word InputMouseActivationY;
    [FieldOffset(0xCC80)] public Word InputMouseButtonX;
    [FieldOffset(0xCC82)] public Word InputMouseButtonY;
    [FieldOffset(0xCC84)] public Bool InputJoystickMoved;
    [FieldOffset(0xCC86)] public Word JoystickXInitial;
    [FieldOffset(0xCC88)] public Word JoystickYInitial;
    [FieldOffset(0xCC8A)] public Word InputLastDeltaX;
    [FieldOffset(0xCC8C)] public Word InputLastDeltaY;

    // Pascal lib

    [FieldOffset(0xCC9A)] public SuperString255 KeyBuffer;

    // SOUNDS.PAS

    [FieldOffset(0xCD9A)] public Bool SoundEnabled;
    [FieldOffset(0xCD9B)] public Bool SoundBlockQueueing;
    [FieldOffset(0xCD9C)] public Word SoundCurrentPriority;
    [FieldOffset(0xCD9E)] public Word SoundFreqTable; // array[255]
    [FieldOffset(0xCF9C)] public HWord SoundDurationMultiplier;
    [FieldOffset(0xCF9D)] public HWord SoundDurationCounter;
    [FieldOffset(0xCF9E)] public SuperString255 SoundBuffer;
    [FieldOffset(0xD09E)] public DWord SoundNewVector;
    [FieldOffset(0xD0A2)] public DWord SoundOldVector;
    [FieldOffset(0xD0A6)] public Word SoundBufferPos;
    [FieldOffset(0xD0A8)] public Bool SoundIsPlaying;
    [FieldOffset(0xD0AA)] public Word SoundTimeCheckCounter;
    [FieldOffset(0xD0AC)] public Bool UseSystemTimeForElapsed;
    [FieldOffset(0xD0AE)] public Word TimerTicks;
    [FieldOffset(0xD0B0)] public Word SoundTimeCheckHSec;
    [FieldOffset(0xD0B2)] public SuperDrumData SoundDrumTable; // array[10]

    // VIDEO.PAS

    [FieldOffset(0xE4B8)] public DWord VideoWriteText; // (x, y, color: byte; text: TVideoLine)
    [FieldOffset(0xE4BC)] public Bool VideoMonochrome;
    [FieldOffset(0xE4BE)] public Word VideoColumns;
    [FieldOffset(0xE4C0)] public Word VideoBorderColor;
    [FieldOffset(0xE4C2)] public Word VideoTextSegment;
    [FieldOffset(0xE4C4)] public DWord VideoTextPointer;
    [FieldOffset(0xE4C8)] public Bool VideoCursorVisible;
}