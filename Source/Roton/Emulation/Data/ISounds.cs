using System;

namespace Roton.Emulation.Data;

public interface ISounds
{
    ReadOnlySpan<byte> Ammo { get; }
    ReadOnlySpan<byte> BombActivate { get; }
    ReadOnlySpan<byte> BombExplode { get; }
    ReadOnlySpan<byte> BombTick { get; }
    ReadOnlySpan<byte> BombTock { get; }
    ReadOnlySpan<byte> BulletDie { get; }
    ReadOnlySpan<byte> Cheat { get; }
    ReadOnlySpan<byte> DoorLocked { get; }
    ReadOnlySpan<byte> DoorOpen { get; }
    ReadOnlySpan<byte> Duplicate { get; }
    ReadOnlySpan<byte> DuplicateFail { get; }
    ReadOnlySpan<byte> EnemyDie { get; }
    ReadOnlySpan<byte> EnemyShoot { get; }
    ReadOnlySpan<byte> EnemySuicide { get; }
    ReadOnlySpan<byte> Energizer { get; }
    ReadOnlySpan<byte> EnergyOut { get; }
    ReadOnlySpan<byte> Error { get; }
    ReadOnlySpan<byte> Forest { get; }
    ReadOnlySpan<byte> GameOver { get; }
    ReadOnlySpan<byte> Gem { get; }
    ReadOnlySpan<byte> Invisible { get; }
    ReadOnlySpan<byte> Key { get; }
    ReadOnlySpan<byte> KeyAlready { get; }
    ReadOnlySpan<byte> Ouch { get; }
    ReadOnlySpan<byte> Passage { get; }
    ReadOnlySpan<byte> Push { get; }
    ReadOnlySpan<byte> Ricochet { get; }
    ReadOnlySpan<byte> Shoot { get; }
    ReadOnlySpan<byte> SlimeDie { get; }
    ReadOnlySpan<byte> TimeLow { get; }
    ReadOnlySpan<byte> TimeOut { get; }
    ReadOnlySpan<byte> Torch { get; }
    ReadOnlySpan<byte> TorchOut { get; }
    ReadOnlySpan<byte> Transporter { get; }
    ReadOnlySpan<byte> Water { get; }
}