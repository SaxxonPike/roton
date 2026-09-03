# Tips and Reminders for Developing Roton

## Project Setup

Some things to know about the solution itself:

### Dependency Injection

Roton is designed for use with a Dependency Injection framework. Lyon uses Microsoft.Extensions.DependencyInjection. But
because Roton exposes its services in a DI-agnostic manner, it shouldn't be difficult to adapt it to a different
framework.

### Running

Lyon can be run from source with this command (from the repository root):

```bash
dotnet run --project Source/Lyon
```

Command line parameters are detailed in the README. By default, Lyon will attempt to load "TOWN.ZZT" in the Original
engine, but it will fail if the world is not present in the current directory.

## OOP Behaviors

Behaviors for games are determined by both the interfaces implemented and by the `ContextAttribute`
that decorates it.

### Actions (`IActionList` > `IAction`)

Actions determine what elements on the board of a given type will do when it is time for them to act. This requires an
`IActor` to be assigned to the element at the proper location. For example, `BearAction.cs` will contain what actions
the bear enemy will perform.

### Cheats (`ICheatList` > `ICheat`)

Cheats are special actions that can be performed by the player to alter the game in some way. One such cheat is
`AmmoCheat.cs` which grants the player ammo.

### Colors (`IColorList` > `IColor`)

Colors are words that can appear in a script to indicate a color. For example, `RedColor.cs` corresponds to the color
red when specified either in an OOP script or for messages (e.g., opening a door.)

### Commands (`ICommandList` > `ICommand`)

Commands are words that can appear in a script to do or process something. For example, `TryCommand.cs`
contains logic for the `#TRY` command.

### Conditions (`IConditionList` > `ICondition`)

Conditions are tests that OOP scripts can perform to determine whether to continue execution or do some other thing. For
example, the `CONTACT` condition will determine if the player is adjacent to the object and is implemented in
`ContactCondition.cs`.

### Directions (`IDirectionList` > `IDirection`)

Directions indicate to OOP which vector should be used. Simple directions such as `NORTH` and `SOUTH` will simply return
a constant vector, but more complex directions like `SEEK` will return a vector that points towards the player. These
are implemented as `NorthDirection.cs`,
`SouthDirection.cs` and `SeekDirection.cs` respectively.

### Draws (`IDrawList` > `IDraw`)

Draws are operations that determine which character and color to plot to the screen when it's the element's turn to be
drawn. For example, the logic to determine which character of a duplicator to draw is in `DuplicatorDraw.cs`.

### Items (`IItemList` > `IItem`)

Items can be referred to by scripts for commands such as
`#TAKE`. The value is not changed in these, but a reference to the value itself is returned. For instance, `GemsItem.cs`
contains logic to return a reference to `Engine.World.Gems`.

### Targets (`ITargetList` > `ITarget`)

Targets can be referenced in scripts when dealing with label operations, like `#SEND` and `#ZAP`. For example,
`OthersTarget.cs` contains the logic for the `OTHERS`
target.

## State Management

Most game state is mapped to a memory block of 64kb in the same binary arrangement as the original games. State is
tracked in a variety of services, but the main one will be `IState`. This stores such things as keyboard input, the last
word read by the script interpreter, name of the currently loaded world, and so on.

### Variable References

Where possible, references to the actual game variables are returned for the best performance. For instance,
`World.Health` will return a reference to the player's health precisely within the emulated memory block.

To maintain compatibility with a variety of different platforms while retaining the correct byte order in the memory
block, wrapper structures are used. These include
`Word`, `PChar`, and `Vector` among others. These are necessary for any data type larger than a single byte, but some
single byte wrappers are provided for convenience such as `HWord` and `Bool`. These have implicit conversions to make
interacting with them easier.

Wrappers larger than a single byte automatically account for differences in endianness. If the platform is little
endian, the data is referenced directly and quickly. If the platform is big endian, the bytes are reversed accordingly.

## Engine Specific Behaviors

Some behaviors are specific to one version of the engine or another. These are prefixed with `Original*` and `Super*`
and correspond to `Context.Original` and `Context.Super`
respectively. For example, `IHud` is implemented by both
`OriginalHud.cs` and `SuperHud.cs`. The UIs for these engines vary drastically, so it is best to keep them separate.

## Composers

These are designed to emulate an audio/video hardware interface. The `IAudioComposer` will take instructions from the
engine for playing tones and drums and render it into an audio stream. The `ISceneComposer` arranges a text bitmap and
uses `IGlyphComposer` to load custom fonts and render text cells. The `IPaletteComposer` is used to import custom color
palettes.

## Built-In Resources

Binary blobs necessary to initialize memory are stored in `resources.zip` as an embedded resource. This archive also
contains a standard VGA font and color palette, as well as help files.

## Folder Structure

At the deeper levels, the `Impl` folder contains the actual implementation of services, whereas the parent folder
contains interfaces.

- `DotSDL`
- `Lyon`: example frontend
    - `App`: main application logic
    - `Autofac`: dependency injection modules
    - `Presenters`: used to adapt SDL3 to Roton's interfaces
- `Roton`: emulator backend
    - `Composers`: audio/video hardware emulation layers
        - `Audio`
        - `Extensions`
        - `Video`
    - `Emulation`: the core emulation logic
        - `Actions`: see "actions"
        - `Cheats`: see "cheats"
        - `Colors`: see "colors"
        - `Commands`: see "commands"
        - `Conditions`: see "conditions"
        - `Core`: non-game-specific logic, general units
        - `Data`: data structures and memory mapping
        - `Directions`: see "directions"
        - `Draws`: see "draws"
        - `Infrastructure`: common emulation/translation units, text code pages
        - `Interactions`: see "interactions"
        - `Items`: see "items"
        - `Original`: original ZZT specific logic
        - `Super`: Super ZZT specific logic
        - `Targets`: see "targets"
    - `Infrastructure`: metadata scanning and DI helpers
    - `Resources`: resource files
- `Roton.Test`: test fixtures
    - `Resources`: test file imports
    - `Roton`
        - `Integration`
            - `Elements`: tests for individual element behavior
            - `Gameplay`: tests for UI and player interactions
            - `Oop`: tests for ZZT-OOP script behavior
            - `ResourceTests`: tests that load actual world and board files

## Tests

NUnit is used for testing Roton. Find the tests in `Roton.Test`. They can be run quickly with this command (from the
repository root):

```bash
dotnet test Source/Roton.Test
```

A base test fixture called `AllContextIntegrationTestFixture` can be used to quickly simulate execution of boards on
both engines. Here's an excerpt that checks to make sure that the game is paused when the player enters a passage:

```csharp
[Test]
public void Passage_ShouldPauseWhenEntering()
{
    // Place the player.
    MovePlayerTo(3, 2);

    // Set up the passage.
    var passage = Actors[SpawnTo(2, 2, ElementList.PassageId, 1)];
    passage.P3 = 0;
    
    // Walk the player into the passage.
    Type(AnsiKey.Left);
    StepAllKeys();

    // Assert.
    GamePaused.Should().BeTrue(
        "game should pause when entering passage");
}
```

It can be assumed that a default board (not necessarily empty, depending on the engine!)
is configured and ready to be populated. Your test boards are still subject to the limitations and behaviors of the
engine. `Step(1)` can be used to run one game cycle, whereas
`StepAllKeys()` will run until the keyboard buffer is empty.

### Stubs

There are some stubs that exist to connect the test framework with the game engine. For example, `TestKeyboard` is what
allows injecting automated keyboard input during testing, and `TestTracer` redirects tracer output to the NUnit
`TestContext` so that script execution can be monitored.

Tracing can be configured with `EnableTracer()` and `DisableTracer()`. If you have a test that outputs a lot of tracer
output, and you don't care about the output content, disabling it may be a good idea. Some test fixture bases like
`ElementTestFixture` turn it on by default.
