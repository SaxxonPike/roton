using System;
using System.Collections.Generic;

namespace Roton.Emulation.Data;

public interface IAlerts
{
    IReadOnlyList<string> AmmoMessage { get; }
    IReadOnlyList<string> BombMessage { get; }
    IReadOnlyList<string> DarkMessage { get; }
    IReadOnlyList<string> EnergizerMessage { get; }
    IReadOnlyList<string> FakeMessage { get; }
    IReadOnlyList<string> ForestMessage { get; }
    IReadOnlyList<string> GameOverMessage { get; }
    IReadOnlyList<string> GemMessage { get; }
    IReadOnlyList<string> InvisibleMessage { get; }
    IReadOnlyList<string> NoAmmoMessage { get; }
    IReadOnlyList<string> NoShootMessage { get; }
    IReadOnlyList<string> NotDarkMessage { get; }
    IReadOnlyList<string> NoTorchMessage { get; }
    IReadOnlyList<string> OuchMessage { get; }
    IReadOnlyList<string> StoneMessage { get; }
    IReadOnlyList<string> TimeMessage { get; }
    IReadOnlyList<string> TorchMessage { get; }
    IReadOnlyList<string> WaterMessage { get; }
    ref Bool AmmoPickup { get; }
    ref Bool CantShootHere { get; }
    ref Bool Dark { get; }
    ref Bool EnergizerPickup { get; }
    ref Bool FakeWall { get; }
    ref Bool Forest { get; }
    ref Bool GemPickup { get; }
    ref Bool NotDark { get; }
    ref Bool NoTorches { get; }
    ref Bool OutOfAmmo { get; }
    ref Bool TorchPickup { get; }

    IReadOnlyList<string> DoorLockedMessage(int color);
    IReadOnlyList<string> DoorOpenMessage(int color);
    IReadOnlyList<string> ErrorMessage(ReadOnlySpan<char> error);
    IReadOnlyList<string> KeyAlreadyMessage(int color);
    IReadOnlyList<string> KeyPickupMessage(int color);

    /// <remarks>
    /// RoZ: ResetMessageNotShownFlags
    /// </remarks>
    void Reset();

    void SetAll();
}