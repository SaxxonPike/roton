using System;

namespace Roton.Emulation.Data.Impl;

public abstract class Sounds : ISounds
{
    protected static byte[] CreateSound(params ReadOnlySpan<byte> data) =>
        [.. data];
    
    private readonly byte[] _ammo = CreateSound
    (
        0x30, 0x01,
        0x31, 0x01,
        0x32, 0x01
    );

    private readonly byte[] _bombActivate = CreateSound
    (
        0x30, 0x01,
        0x35, 0x01,
        0x40, 0x01,
        0x45, 0x01,
        0x50, 0x01
    );

    private readonly byte[] _bombExplode = CreateSound
    (
        0x60, 0x01,
        0x50, 0x01,
        0x40, 0x01,
        0x30, 0x01,
        0x20, 0x01
    );

    private readonly byte[] _bombTick = CreateSound
    (
        0xF8, 0x01
    );

    private readonly byte[] _bombTock = CreateSound
    (
        0xF5, 0x01
    );

    private readonly byte[] _bulletDie = CreateSound
    (
        0x20, 0x01
    );

    private readonly byte[] _cheat = CreateSound
    (
        0x27, 0x04
    );

    private readonly byte[] _doorLocked = CreateSound
    (
        0x17, 0x01,
        0x10, 0x01
    );

    private readonly byte[] _doorOpen = CreateSound
    (
        0x30, 0x01,
        0x37, 0x01,
        0x3B, 0x01,
        0x30, 0x01,
        0x37, 0x01,
        0x3B, 0x01,
        0x40, 0x04
    );

    private readonly byte[] _duplicate = CreateSound
    (
        0x30, 0x02,
        0x32, 0x02,
        0x34, 0x02,
        0x35, 0x02,
        0x37, 0x02
    );

    private readonly byte[] _duplicateFail = CreateSound
    (
        0x18, 0x01,
        0x16, 0x01
    );

    private readonly byte[] _enemyDie = CreateSound
    (
        0x40, 0x01,
        0x10, 0x01,
        0x50, 0x01,
        0x30, 0x01
    );

    private readonly byte[] _enemyShoot = CreateSound
    (
        0x30, 0x01,
        0x26, 0x01
    );

    private readonly byte[] _enemySuicide = CreateSound
    (
        0x10, 0x01
    );

    private readonly byte[] _energizer = CreateSound
    (
        0x20, 0x03,
        0x23, 0x03,
        0x24, 0x03,
        0x25, 0x03,
        0x35, 0x03,
        0x25, 0x03,
        0x23, 0x03,
        0x20, 0x03,
        0x30, 0x03,
        0x23, 0x03,
        0x24, 0x03,
        0x25, 0x03,
        0x35, 0x03,
        0x25, 0x03,
        0x23, 0x03,
        0x20, 0x03,
        0x30, 0x03,
        0x23, 0x03,
        0x24, 0x03,
        0x25, 0x03,
        0x35, 0x03,
        0x25, 0x03,
        0x23, 0x03,
        0x20, 0x03,
        0x30, 0x03,
        0x23, 0x03,
        0x24, 0x03,
        0x25, 0x03,
        0x35, 0x03,
        0x25, 0x03,
        0x23, 0x03,
        0x20, 0x03,
        0x30, 0x03,
        0x23, 0x03,
        0x24, 0x03,
        0x25, 0x03,
        0x35, 0x03,
        0x25, 0x03,
        0x23, 0x03,
        0x20, 0x03,
        0x30, 0x03,
        0x23, 0x03,
        0x24, 0x03,
        0x25, 0x03,
        0x35, 0x03,
        0x25, 0x03,
        0x23, 0x03,
        0x20, 0x03,
        0x30, 0x03,
        0x23, 0x03,
        0x24, 0x03,
        0x25, 0x03,
        0x35, 0x03,
        0x25, 0x03,
        0x23, 0x03,
        0x20, 0x03
    );

    private readonly byte[] _energyOut = CreateSound
    (
        0x20, 0x03,
        0x1A, 0x03,
        0x17, 0x03,
        0x16, 0x03,
        0x15, 0x03,
        0x13, 0x03,
        0x10, 0x03
    );

    private readonly byte[] _error = CreateSound
    (
        0x50, 0x0A
    );

    public abstract ReadOnlySpan<byte> Forest { get; }

    private readonly byte[] _gameOver = CreateSound
    (
        0x20, 0x03,
        0x23, 0x03,
        0x27, 0x03,
        0x30, 0x03,
        0x27, 0x03,
        0x2A, 0x03,
        0x32, 0x03,
        0x37, 0x03,
        0x35, 0x03,
        0x38, 0x03,
        0x40, 0x03,
        0x45, 0x03,
        0x10, 0x0A
    );

    private readonly byte[] _gem = CreateSound
    (
        0x40, 0x01,
        0x37, 0x01,
        0x34, 0x01,
        0x30, 0x01
    );

    private readonly byte[] _invisible = CreateSound
    (
        0x12, 0x01,
        0x10, 0x01
    );

    private readonly byte[] _key = CreateSound
    (
        0x40, 0x01,
        0x44, 0x01,
        0x47, 0x01,
        0x40, 0x01,
        0x44, 0x01,
        0x47, 0x01,
        0x40, 0x01,
        0x44, 0x01,
        0x47, 0x01,
        0x50, 0x02
    );

    private readonly byte[] _keyAlready = CreateSound
    (
        0x30, 0x02,
        0x20, 0x02
    );

    private readonly byte[] _ouch = CreateSound
    (
        0x10, 0x01,
        0x20, 0x01,
        0x13, 0x01,
        0x23, 0x01
    );

    private readonly byte[] _passage = CreateSound
    (
        0x30, 0x01,
        0x34, 0x01,
        0x37, 0x01,
        0x31, 0x01,
        0x35, 0x01,
        0x38, 0x01,
        0x32, 0x01,
        0x36, 0x01,
        0x39, 0x01,
        0x33, 0x01,
        0x37, 0x01,
        0x3A, 0x01,
        0x34, 0x01,
        0x38, 0x01,
        0x40, 0x01
    );

    private readonly byte[] _push = CreateSound
    (
        0x15, 0x01
    );

    private readonly byte[] _ricochet = CreateSound
    (
        0xF9, 0x01
    );

    private readonly byte[] _shoot = CreateSound
    (
        0x40, 0x01,
        0x30, 0x01,
        0x20, 0x01
    );

    private readonly byte[] _slimeDie = CreateSound
    (
        0x20, 0x01,
        0x23, 0x01
    );

    private readonly byte[] _timeLow = CreateSound
    (
        0x40, 0x06,
        0x45, 0x06,
        0x40, 0x06,
        0x35, 0x06,
        0x40, 0x06,
        0x45, 0x06,
        0x40, 0x0A
    );

    private readonly byte[] _timeOut = CreateSound
    (
        0x20, 0x01,
        0x23, 0x01,
        0x27, 0x01,
        0x30, 0x01,
        0x10, 0x01
    );

    private readonly byte[] _torch = CreateSound
    (
        0x30, 0x01,
        0x39, 0x01,
        0x34, 0x02
    );

    private readonly byte[] _torchOut = CreateSound
    (
        0x30, 0x01,
        0x20, 0x01,
        0x10, 0x01
    );

    private readonly byte[] _transporter = CreateSound
    (
        0x30, 0x01,
        0x42, 0x01,
        0x34, 0x01,
        0x46, 0x01,
        0x38, 0x01,
        0x4A, 0x01,
        0x40, 0x01,
        0x52, 0x01
    );

    private readonly byte[] _water = CreateSound
    (
        0x40, 0x01,
        0x50, 0x01
    );

    public ReadOnlySpan<byte> Ammo => _ammo;
    public ReadOnlySpan<byte> BombActivate => _bombActivate;
    public ReadOnlySpan<byte> BombExplode => _bombExplode;
    public ReadOnlySpan<byte> BombTick => _bombTick;
    public ReadOnlySpan<byte> BombTock => _bombTock;
    public ReadOnlySpan<byte> BulletDie => _bulletDie;
    public ReadOnlySpan<byte> Cheat => _cheat;
    public ReadOnlySpan<byte> DoorLocked => _doorLocked;
    public ReadOnlySpan<byte> DoorOpen => _doorOpen;
    public ReadOnlySpan<byte> Duplicate => _duplicate;
    public ReadOnlySpan<byte> DuplicateFail => _duplicateFail;
    public ReadOnlySpan<byte> EnemyDie => _enemyDie;
    public ReadOnlySpan<byte> EnemyShoot => _enemyShoot;
    public ReadOnlySpan<byte> EnemySuicide => _enemySuicide;
    public ReadOnlySpan<byte> Energizer => _energizer;
    public ReadOnlySpan<byte> EnergyOut => _energyOut;
    public ReadOnlySpan<byte> Error => _error;
    public ReadOnlySpan<byte> GameOver => _gameOver;
    public ReadOnlySpan<byte> Gem => _gem;
    public ReadOnlySpan<byte> Invisible => _invisible;
    public ReadOnlySpan<byte> Key => _key;
    public ReadOnlySpan<byte> KeyAlready => _keyAlready;
    public ReadOnlySpan<byte> Ouch => _ouch;
    public ReadOnlySpan<byte> Passage => _passage;
    public ReadOnlySpan<byte> Push => _push;
    public ReadOnlySpan<byte> Ricochet => _ricochet;
    public ReadOnlySpan<byte> Shoot => _shoot;
    public ReadOnlySpan<byte> SlimeDie => _slimeDie;
    public ReadOnlySpan<byte> TimeLow => _timeLow;
    public ReadOnlySpan<byte> TimeOut => _timeOut;
    public ReadOnlySpan<byte> Torch => _torch;
    public ReadOnlySpan<byte> TorchOut => _torchOut;
    public ReadOnlySpan<byte> Transporter => _transporter;
    public ReadOnlySpan<byte> Water => _water;
}