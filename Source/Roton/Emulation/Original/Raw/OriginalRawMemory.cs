using System.Runtime.InteropServices;
using Roton.Emulation.Data;

namespace Roton.Emulation.Original.Raw;

/// <summary>
/// Represents the memory map for ZZT 3.2.
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10000)]
internal struct OriginalRawMemory
{
    // Const section

    [FieldOffset(0x0002)] public OriginalString255 TransporterNSChars;
    [FieldOffset(0x0102)] public OriginalString255 TransporterEWChars;
    [FieldOffset(0x0202)] public OriginalString255 StarAnimChars;
    [FieldOffset(0x0302)] public OriginalString20 NeighborBoardStrs; // array[4]
    [FieldOffset(0x0356)] public HWord ProgressAnimColors; // array[8]
    [FieldOffset(0x035E)] public OriginalString5 ProgressAnimStrings; // array[8]
    [FieldOffset(0x038E)] public OriginalString8 ColorNames; // array[7]
    [FieldOffset(0x03CE)] public Word DiagonalDeltaX;
    [FieldOffset(0x03DE)] public Word DiagonalDeltaY;
    [FieldOffset(0x03EE)] public Word NeighborDeltaX;
    [FieldOffset(0x03F6)] public Word NeighborDeltaY;
    [FieldOffset(0x03FE)] public OriginalTile TileBorder;
    [FieldOffset(0x0400)] public OriginalTile TileBoardEdge;
    [FieldOffset(0x0402)] public OriginalStat StatTemplateDefault;
    [FieldOffset(0x0424)] public OriginalString16 LineChars;
    
    // Pascal builtin

    [FieldOffset(0x046C)] public DWord RandomSeed;

    // GAMEVARS.PAS

    [FieldOffset(0x0472)] public Word PlayerDirX;
    [FieldOffset(0x0474)] public Word PlayerDirY;
    [FieldOffset(0x0476)] public Word unkVar_0476;
    [FieldOffset(0x0478)] public Word unkVar_0478;
    [FieldOffset(0x047A)] public OriginalCoord TransitionTable;
    [FieldOffset(0x23BA)] public OriginalString50 LoadedGameFileName;
    [FieldOffset(0x23EE)] public OriginalString50 SavedGameFileName;
    [FieldOffset(0x2422)] public OriginalString50 SavedBoardFileName;
    [FieldOffset(0x2456)] public OriginalString50 StartupWorldFileName;
    [FieldOffset(0x248A)] public OriginalBoard Board;
    [FieldOffset(0x45C2)] public OriginalWorld World;
    [FieldOffset(0x4AAF)] public Bool MessageAmmoNotShown;
    [FieldOffset(0x4AB0)] public Bool MessageOutOfAmmoNotShown;
    [FieldOffset(0x4AB1)] public Bool MessageNoShootingNotShown;
    [FieldOffset(0x4AB2)] public Bool MessageTorchNotShown;
    [FieldOffset(0x4AB3)] public Bool MessageOutOfTorchesNotShown;
    [FieldOffset(0x4AB4)] public Bool MessageRoomNotDarkNotShown;
    [FieldOffset(0x4AB5)] public Bool MessageHintTorchNotShown;
    [FieldOffset(0x4AB6)] public Bool MessageForestNotShown;
    [FieldOffset(0x4AB7)] public Bool MessageFakeNotShown;
    [FieldOffset(0x4AB8)] public Bool MessageGemNotShown;
    [FieldOffset(0x4AB9)] public Bool MessageEnergizerNotShown;
    [FieldOffset(0x4ABA)] public Bool unkVar_4ABA; // array[15]
    [FieldOffset(0x4AC9)] public Bool GameTitleExitRequested;
    [FieldOffset(0x4ACA)] public Bool GamePlayExitRequested;
    [FieldOffset(0x4ACC)] public Word GameStateElement;
    [FieldOffset(0x4ACE)] public Word ReturnBoardId;
    [FieldOffset(0x4AD0)] public Word TransitionTableSize;
    [FieldOffset(0x4AD2)] public HWord TickSpeed;
    [FieldOffset(0x4AD4)] public DWord IoTmpBuf; // ptr to array[20000]
    [FieldOffset(0x4AD8)] public OriginalElementDef ElementDefs; // array[54]
    [FieldOffset(0x73FA)] public Word EditorPatternCount;
    [FieldOffset(0x73FC)] public HWord EditorPatterns; // array[10]
    [FieldOffset(0x7406)] public Word TickTimeDuration;
    [FieldOffset(0x7408)] public Word CurrentTick;
    [FieldOffset(0x740A)] public Word CurrentStatTicked;
    [FieldOffset(0x740C)] public Bool GamePaused;
    [FieldOffset(0x740E)] public Word TickTimeCounter;
    [FieldOffset(0x7410)] public Bool ForceDarknessOff;
    [FieldOffset(0x7411)] public HWord InitialTextAttr;
    [FieldOffset(0x7412)] public PChar OopChar;
    [FieldOffset(0x7414)] public OriginalString20 OopWord;
    [FieldOffset(0x742A)] public Word OopValue;
    [FieldOffset(0x742C)] public Bool DebugEnabled;
    [FieldOffset(0x742E)] public OriginalHighScoreEntry HighScoreList; // array[30]
    [FieldOffset(0x7A64)] public OriginalString255 ConfigRegistration;
    [FieldOffset(0x7B64)] public OriginalString50 ConfigWorldFile;
    [FieldOffset(0x7B97)] public Bool EditorEnabled;
    [FieldOffset(0x7B98)] public OriginalString50 GameVersion;
    [FieldOffset(0x7BCB)] public Bool ParsingConfigFile;
    [FieldOffset(0x7BCC)] public Bool ResetConfig;
    [FieldOffset(0x7BCD)] public Bool JustStarted;
    [FieldOffset(0x7BCE)] public Word WorldFileDescCount;
    [FieldOffset(0x7BD0)] public OriginalString50 WorldFileDescKeys; // array[10]
    [FieldOffset(0x7DCE)] public OriginalString50 WorldFileDescValues; // array[10]

    // TXTWIND.PAS

    [FieldOffset(0x7FCC)] public Word TextWindowX;
    [FieldOffset(0x7FCE)] public Word TextWindowY;
    [FieldOffset(0x7FD0)] public Word TextWindowWidth;
    [FieldOffset(0x7FD2)] public Word TextWindowHeight;
    [FieldOffset(0x7FD4)] public OriginalString80 TextWindowStrInnerEmpty;
    [FieldOffset(0x8026)] public OriginalString80 TextWindowStrText;
    [FieldOffset(0x8078)] public OriginalString80 TextWindowStrInnerLine;
    [FieldOffset(0x80CA)] public OriginalString80 TextWindowStrTop;
    [FieldOffset(0x811C)] public OriginalString80 TextWindowStrBottom;
    [FieldOffset(0x816E)] public OriginalString80 TextWindowStrSep;
    [FieldOffset(0x81C0)] public OriginalString80 TextWindowStrInnerSep;
    [FieldOffset(0x8212)] public OriginalString80 TextWindowStrInnerArrows;
    [FieldOffset(0x8263)] public Bool TextWindowRejected;
    [FieldOffset(0x8264)] public OriginalString50 ResourceDataFileName;
    [FieldOffset(0x8298)] public OriginalResourceDataHeader ResourceDataHeader;
    [FieldOffset(0x87C2)] public DWord OrderPrintId; // ptr to string

    // INPUT.PAS

    [FieldOffset(0x88C6)] public Word InputDeltaX;
    [FieldOffset(0x88C8)] public Word InputDeltaY;
    [FieldOffset(0x88CA)] public Bool InputShiftPressed;
    [FieldOffset(0x88CB)] public Bool InputShiftAccepted;
    [FieldOffset(0x88CC)] public Bool InputJoystickEnabled;
    [FieldOffset(0x88CD)] public Bool InputMouseEnabled;
    [FieldOffset(0x88CE)] public PChar InputKeyPressed;
    [FieldOffset(0x88D0)] public Word InputMouseX;
    [FieldOffset(0x88D2)] public Word InputMouseY;
    [FieldOffset(0x88D4)] public Word InputMouseActivationX;
    [FieldOffset(0x88D6)] public Word InputMouseActivationY;
    [FieldOffset(0x88D8)] public Word InputMouseButtonX;
    [FieldOffset(0x88DA)] public Word InputMouseButtonY;
    [FieldOffset(0x88DC)] public Bool InputJoystickMoved;
    [FieldOffset(0x88DE)] public Word JoystickXInitial;
    [FieldOffset(0x88E0)] public Word JoystickYInitial;
    [FieldOffset(0x88E2)] public Word InputLastDeltaX;
    [FieldOffset(0x88E4)] public Word InputLastDeltaY;
    
    // Pascal lib

    [FieldOffset(0x88F2)] public OriginalString255 KeyBuffer;

    // SOUNDS.PAS

    [FieldOffset(0x89F2)] public Bool SoundEnabled;
    [FieldOffset(0x89F3)] public Bool SoundBlockQueueing;
    [FieldOffset(0x89F4)] public Word SoundCurrentPriority; // 7C8E
    [FieldOffset(0x89F6)] public Word SoundFreqTable; // array[255]
    [FieldOffset(0x8BF4)] public HWord SoundDurationMultiplier;
    [FieldOffset(0x8BF5)] public HWord SoundDurationCounter;
    [FieldOffset(0x8BF6)] public OriginalString255 SoundBuffer;
    [FieldOffset(0x8CF6)] public DWord SoundNewVector;
    [FieldOffset(0x8CFA)] public DWord SoundOldVector;
    [FieldOffset(0x8CFE)] public Word SoundBufferPos;
    [FieldOffset(0x8D00)] public Bool SoundIsPlaying;
    [FieldOffset(0x8D02)] public Word SoundTimeCheckCounter;
    [FieldOffset(0x8D04)] public Bool UseSystemTimeForElapsed;
    [FieldOffset(0x8D06)] public Word TimerTicks;
    [FieldOffset(0x8D08)] public Word SoundTimeCheckHSec;
    [FieldOffset(0x8D0A)] public OriginalDrumData SoundDrumTable; // array[10]

    // VIDEO.PAS

    [FieldOffset(0xA110)] public DWord VideoWriteText; // (x, y, color: byte; text: TVideoLine)
    [FieldOffset(0xA114)] public Bool VideoMonochrome;
    [FieldOffset(0xA116)] public Word VideoColumns;
    [FieldOffset(0xA118)] public Word VideoBorderColor;
    [FieldOffset(0xA11A)] public Word VideoTextSegment;
    [FieldOffset(0xA11C)] public DWord VideoTextPointer;
    [FieldOffset(0xA120)] public Bool VideoCursorVisible;
}