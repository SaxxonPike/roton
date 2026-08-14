# Roton Emulation Analysis vs. Original ZZT/Super ZZT Engines

This document outlines the discrepancies and missing features identified in the Roton project after comparing it with the byte-accurate Pascal reconstructions of ZZT and Super ZZT.

### 1. OOP Execution Engine

#### Command Limit Discrepancy
- **Original Behavior**: ZZT and Super ZZT limit each actor to executing at most 33 OOP commands per cycle to prevent infinite loops from hanging the game.
- **Pascal Reference**: `OOPS.PAS`, `ExecuteOOP` procedure. The loop `while (Stat[StatInd].Instruction >= 0) and (CommandCount <= 33) do` (ZZT) or similar in Super ZZT.
- **C# Location**: `Roton.Emulation.Core.Impl.Interpreter.Execute`. The `while` loop lacks the `CommandsExecuted <= 33` constraint.
- **Roton Status**: Roton currently lacks this hard limit. An object with an infinite loop (e.g., `:loop\r#zap loop\r#restore loop\r#goto loop`) can cause the emulation to hang or consume excessive CPU time within a single tick.

#### Instruction Pointer Handling
- **Original Behavior**: When parsing tokens, the original engine increments the instruction pointer as it reads. For certain commands like `#if`, it checks the next token by looking ahead.
- **Pascal Reference**: `OOPS.PAS`, `ReadWord`, `ReadNumber` functions. They typically increment the pointer and don't rely on backtracking unless absolutely necessary.
- **C# Location**: `Roton.Emulation.Core.Impl.Parser`. Methods like `ReadNumber` and `ReadWord` use `instructionSource.Instruction--` to peek or adjust the pointer, which can cause issues at code boundaries.
- **Roton Status**: Roton's `Parser` often peeks at tokens by decrementing the instruction pointer after reading, or relies on specific pointer math that may not perfectly replicate edge cases involving labels at code boundaries or EOF.

### 2. Random Number Generation (RNG)

#### Constant Inaccuracy
- **Original Behavior**: ZZT uses the standard `Random` function from Turbo Pascal 5.5. The LCG constants used are `Seed = (Seed * $8088405) + 1`.
- **Pascal Reference**: Turbo Pascal `SYSTEM.PAS` (intrinsic `Random` function) and `OOPS.PAS` where it's wrapped.
- **C# Location**: `Roton.Emulation.Core.Impl.Randomizer.GetRandom`. The current implementation uses the constant `33797` and logic that doesn't match the 32-bit LCG of Turbo Pascal.
- **Roton Status**: Roton uses a custom `Randomizer` implementation. While it attempts to emulate the Pascal LCG, the constants or the bit-truncation logic used in `Randomizer.cs` (specifically around the value `33797`) do not perfectly align with the expected behavior of Turbo Pascal 5.5's 32-bit seed implementation.
- **Impact**: This leads to different movement patterns for "random" elements like Centipedes or Slime compared to the original engine.

### 3. Movement and Interaction Logic

#### Push Safety Checks
- **Original Behavior**: The `Push` function in Pascal simply attempts to move an element in a given direction. If the direction is `(0, 0)`, it typically results in no action or uses the existing element's logic.
- **Pascal Reference**: `ELEMENTS.PAS`, `Push` procedure.
- **C# Location**: `Roton.Emulation.Core.Impl.Engine.Push`. Lines 985-986: `if (vector.IsZero()) throw Exceptions.PushStackOverflow;`. This guard is not present in the original Pascal.
- **Roton Status**: Roton includes an explicit check for zero-vector directions in its pushing logic. While "safer" for modern code, this diverts from the original's potential side effects when a zero-vector is passed to the internal `Move` routines.

#### Forest Tiles (Super ZZT)
- **Original Behavior**: The specific interaction between the Player and Forest tiles in Super ZZT involves a specific sound/delay pattern and state changes.
- **Pascal Reference**: Super ZZT `ELEMENTS.PAS`, `Forest` interaction logic.
- **C# Location**: `Roton.Emulation.Interactions.Impl.ForestInteraction.Interact` and `Roton.Emulation.Super.SuperFeatures.ClearForest`.
- **Roton Status**: The specific interaction between the Player and Forest tiles in Super ZZT (which requires a specific sound/delay pattern) is simplified in Roton. The original Pascal code handles the "clearing" of forest with a very specific state machine that Roton approximates but does not replicate exactly.

### 4. Data Structures and Dimensions

#### Super ZZT Board Limits
- **Original Behavior**: Super ZZT boards are fixed at 96x80.
- **Pascal Reference**: Super ZZT `GAME.PAS`, `BoardType` definition.
- **C# Location**: `Roton.Emulation.Super.SuperBoard` and `Roton.Emulation.Data.Impl.PackedBoard`.
- **Roton Status**: Roton correctly identifies these dimensions in `SuperBoard`, but the underlying buffer management in `PackedBoard` and `Tiles` uses a more dynamic approach that doesn't always strictly enforce the original memory layout constraints, which could affect how "corrupted" worlds (out-of-bounds memory) behave.

### 5. OOP Instruction Set Missing/Inaccurate Features
- **#CHAR**: Roton's implementation of `#char` changes the actor's character but does not always trigger a redraw of the tile immediately in the same way the original `Graph` unit calls did.
  - **Pascal Reference**: `OOPS.PAS`, case statement for `CHAR` command, calls `DrawTile`.
  - **C# Location**: `Roton.Emulation.Commands.Impl.CharCommand.Execute`.
- **#BIND**: The binding logic in Roton is functional but lacks the specific "search from top of actor list" order that determines which object is bound when multiple objects share a name.
  - **Pascal Reference**: `OOPS.PAS`, `BIND` command, search loop starts from `1 to StatCount`.
  - **C# Location**: `Roton.Emulation.Targets.Impl.DefaultTarget.Execute`. The `while` loop starting from `context.SearchIndex` (which is often `0`) might not match the original search order if `SearchIndex` is not reset correctly or if the starting index differs.

---
*Analysis performed on 2026-08-13.*
