namespace Roton.Core
{
    public interface ISounds
    {
        byte[] Ammo { get; }
        byte[] BombActivate { get; }
        byte[] BombExplode { get; }
        byte[] BombTick { get; }
        byte[] BombTock { get; }
        byte[] BulletDie { get; }
        byte[] Cheat { get; }
        byte[] DoorLocked { get; }
        byte[] DoorOpen { get; }
        byte[] Duplicate { get; }
        byte[] DuplicateFail { get; }
        byte[] EnemyDie { get; }
        byte[] EnemyShoot { get; }
        byte[] EnemySuicide { get; }
        byte[] Energizer { get; }
        byte[] EnergyOut { get; }
        byte[] Error { get; }
        byte[] Forest { get; }
        byte[] GameOver { get; }
        byte[] Gem { get; }
        byte[] Invisible { get; }
        byte[] Key { get; }
        byte[] KeyAlready { get; }
        byte[] Ouch { get; }
        byte[] Passage { get; }
        byte[] Push { get; }
        byte[] Ricochet { get; }
        byte[] Shoot { get; }
        byte[] SlimeDie { get; }
        byte[] TimeLow { get; }
        byte[] TimeOut { get; }
        byte[] Torch { get; }
        byte[] TorchOut { get; }
        byte[] Transporter { get; }
        byte[] Water { get; }
    }
}