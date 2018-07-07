namespace Roton.Core
{
    public interface ISounds
    {
        ISound Ammo { get; }
        ISound BombActivate { get; }
        ISound BombExplode { get; }
        ISound BombTick { get; }
        ISound BombTock { get; }
        ISound BulletDie { get; }
        ISound Cheat { get; }
        ISound DoorLocked { get; }
        ISound DoorOpen { get; }
        ISound Duplicate { get; }
        ISound DuplicateFail { get; }
        ISound EnemyDie { get; }
        ISound EnemyShoot { get; }
        ISound EnemySuicide { get; }
        ISound Energizer { get; }
        ISound EnergyOut { get; }
        ISound Error { get; }
        ISound Forest { get; }
        ISound GameOver { get; }
        ISound Gem { get; }
        ISound Invisible { get; }
        ISound Key { get; }
        ISound KeyAlready { get; }
        ISound Ouch { get; }
        ISound Passage { get; }
        ISound Push { get; }
        ISound Ricochet { get; }
        ISound Shoot { get; }
        ISound SlimeDie { get; }
        ISound TimeLow { get; }
        ISound TimeOut { get; }
        ISound Torch { get; }
        ISound TorchOut { get; }
        ISound Transporter { get; }
        ISound Water { get; }
    }
}