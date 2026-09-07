using Roton.Emulation.Cheats;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Cheater(
    IHud hud,
    IGameThread gameThread,
    IWorld world,
    ICheatList cheats,
    ISoundPlayer soundPlayer,
    ISounds sounds)
    : ICheater
{
    public void Cheat()
    {
        var cheatText = hud.EnterCheat().UpCased() ?? "";
        var clear = false;

        if (!gameThread.ThreadActive)
            return;

        if (!string.IsNullOrEmpty(cheatText))
        {
            switch (cheatText[0])
            {
                case '-':
                {
                    cheatText = cheatText.Substring(1);
                    while (world.Flags.Contains(cheatText))
                        world.Flags.Remove(cheatText);
                    clear = true;
                    break;
                }
                case '+':
                    cheatText = cheatText.Substring(1);
                    world.Flags.Add(cheatText);
                    break;
            }
        }

        var cheat = cheats.Get(cheatText);
        cheat?.Execute(clear);
        hud.UpdateStatus();

        soundPlayer.PlaySound(10, sounds.Cheat);
    }
}