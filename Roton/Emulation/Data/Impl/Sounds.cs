﻿namespace Roton.Emulation.Data.Impl
{
    public abstract class Sounds : ISounds
    {
        public ISound Ammo { get; } = new Sound
        (
            0x30, 0x01,
            0x31, 0x01,
            0x32, 0x01
        );

        public ISound BombActivate { get; } = new Sound
        (
            0x30, 0x01,
            0x35, 0x01,
            0x40, 0x01,
            0x45, 0x01,
            0x50, 0x01
        );

        public ISound BombExplode { get; } = new Sound
        (
            0x60, 0x01,
            0x50, 0x01,
            0x40, 0x01,
            0x30, 0x01,
            0x20, 0x01
        );

        public ISound BombTick { get; } = new Sound
        (
            0xF8, 0x01
        );

        public ISound BombTock { get; } = new Sound
        (
            0xF5, 0x01
        );

        public ISound BulletDie { get; } = new Sound
        (
            0x20, 0x01
        );

        public ISound Cheat { get; } = new Sound
        (
            0x27, 0x04
        );

        public ISound DoorLocked { get; } = new Sound
        (
            0x17, 0x01,
            0x10, 0x01
        );

        public ISound DoorOpen { get; } = new Sound
        (
            0x30, 0x01,
            0x37, 0x01,
            0x3B, 0x01,
            0x30, 0x01,
            0x37, 0x01,
            0x3B, 0x01,
            0x40, 0x04
        );

        public ISound Duplicate { get; } = new Sound
        (
            0x30, 0x02,
            0x32, 0x02,
            0x34, 0x02,
            0x35, 0x02,
            0x37, 0x02
        );

        public ISound DuplicateFail { get; } = new Sound
        (
            0x18, 0x01,
            0x16, 0x01
        );

        public ISound EnemyDie { get; } = new Sound
        (
            0x40, 0x01,
            0x10, 0x01,
            0x50, 0x01,
            0x30, 0x01
        );

        public ISound EnemyShoot { get; } = new Sound
        (
            0x30, 0x01,
            0x26, 0x01
        );

        public ISound EnemySuicide { get; } = new Sound
        (
            0x10, 0x01
        );

        public ISound Energizer { get; } = new Sound
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

        public ISound EnergyOut { get; } = new Sound
        (
            0x20, 0x03,
            0x1A, 0x03,
            0x17, 0x03,
            0x16, 0x03,
            0x15, 0x03,
            0x13, 0x03,
            0x10, 0x03
        );

        public ISound Error { get; } = new Sound
        (
            0x50, 0x0A
        );

        public abstract ISound Forest { get; }

        public ISound GameOver { get; } = new Sound
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

        public ISound Gem { get; } = new Sound
        (
            0x40, 0x01,
            0x37, 0x01,
            0x34, 0x01,
            0x30, 0x01
        );

        public ISound Invisible { get; } = new Sound
        (
            0x12, 0x01,
            0x10, 0x01
        );

        public ISound Key { get; } = new Sound
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

        public ISound KeyAlready { get; } = new Sound
        (
            0x30, 0x02,
            0x20, 0x02
        );

        public ISound Ouch { get; } = new Sound
        (
            0x10, 0x01,
            0x20, 0x01,
            0x13, 0x01,
            0x23, 0x01
        );

        public ISound Passage { get; } = new Sound
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

        public ISound Push { get; } = new Sound
        (
            0x15, 0x01
        );

        public ISound Ricochet { get; } = new Sound
        (
            0xF9, 0x01
        );

        public ISound Shoot { get; } = new Sound
        (
            0x40, 0x01,
            0x30, 0x01,
            0x20, 0x01
        );

        public ISound SlimeDie { get; } = new Sound
        (
            0x20, 0x01,
            0x23, 0x01
        );

        public ISound TimeLow { get; } = new Sound
        (
            0x40, 0x06,
            0x45, 0x06,
            0x40, 0x06,
            0x35, 0x06,
            0x40, 0x06,
            0x45, 0x06,
            0x40, 0x0A
        );

        public ISound TimeOut { get; } = new Sound
        (
            0x20, 0x01,
            0x23, 0x01,
            0x27, 0x01,
            0x30, 0x01,
            0x10, 0x01
        );

        public ISound Torch { get; } = new Sound
        (
            0x30, 0x01,
            0x39, 0x01,
            0x34, 0x02
        );

        public ISound TorchOut { get; } = new Sound
        (
            0x30, 0x01,
            0x20, 0x01,
            0x10, 0x01
        );

        public ISound Transporter { get; } = new Sound
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

        public ISound Water { get; } = new Sound
        (
            0x40, 0x01,
            0x50, 0x01
        );
    }
}