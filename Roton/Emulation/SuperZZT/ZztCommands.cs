using System.Collections.Generic;
using Roton.Emulation.Commands;

namespace Roton.Emulation.SuperZZT
{
    public class ZztCommands : Commands
    {
        public ZztCommands(IEnumerable<ICommand> commands) : base(commands, new[]
        {
            "BECOME",
            "BIND",
            "CHANGE",
            "CHAR",
            "CLEAR",
            "CYCLE",
            "DIE",
            "END",
            "ENDGAME",
            "GIVE",
            "GO",
            "IDLE",
            "IF",
            "LOCK",
            "PLAY",
            "PUT",
            "RESTART",
            "RESTORE",
            "SEND",
            "SET",
            "SHOOT",
            "TAKE",
            "THEN",
            "THROWSTAR",
            "TRY",
            "UNLOCK",
            "WALK",
            "ZAP"
        })
        {
        }
    }
}