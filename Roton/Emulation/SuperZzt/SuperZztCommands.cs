using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Commands;

namespace Roton.Emulation.SuperZzt
{
    public class SuperZztCommands : Commands.Commands
    {
        public SuperZztCommands(ICollection<ICommand> commands) : base(new Dictionary<string, ICommand>
        {
            {"BECOME", commands.OfType<BecomeCommand>().Single()},
            {"BIND", commands.OfType<BindCommand>().Single()},
            {"CHANGE", commands.OfType<ChangeCommand>().Single()},
            {"CHAR", commands.OfType<CharCommand>().Single()},
            {"CLEAR", commands.OfType<ClearCommand>().Single()},
            {"CYCLE", commands.OfType<CycleCommand>().Single()},
            {"DIE", commands.OfType<DieCommand>().Single()},
            {"END", commands.OfType<EndCommand>().Single()},
            {"ENDGAME", commands.OfType<EndgameCommand>().Single()},
            {"GIVE", commands.OfType<GiveCommand>().Single()},
            {"GO", commands.OfType<GoCommand>().Single()},
            {"IDLE", commands.OfType<IdleCommand>().Single()},
            {"IF", commands.OfType<IfCommand>().Single()},
            {"LOCK", commands.OfType<LockCommand>().Single()},
            {"PLAY", commands.OfType<PlayCommand>().Single()},
            {"PUT", commands.OfType<PutCommand>().Single()},
            {"RESTART", commands.OfType<RestartCommand>().Single()},
            {"RESTORE", commands.OfType<RestoreCommand>().Single()},
            {"SEND", commands.OfType<SendCommand>().Single()},
            {"SET", commands.OfType<SetCommand>().Single()},
            {"SHOOT", commands.OfType<ShootCommand>().Single()},
            {"TAKE", commands.OfType<TakeCommand>().Single()},
            {"THEN", commands.OfType<ThenCommand>().Single()},
            {"THROWSTAR", commands.OfType<ThrowstarCommand>().Single()},
            {"TRY", commands.OfType<TryCommand>().Single()},
            {"UNLOCK", commands.OfType<UnlockCommand>().Single()},
            {"WALK", commands.OfType<WalkCommand>().Single()},
            {"ZAP", commands.OfType<ZapCommand>().Single()}
        })
        {
        }
    }
}