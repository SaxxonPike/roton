using System;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Data.Impl
{
    public static class ActorExtensions
    {
        public static void CopyFrom(this IActor self, IActor actor)
        {
            self.Cycle = actor.Cycle;
            self.Follower = actor.Follower;
            self.Instruction = actor.Instruction;
            self.Leader = actor.Leader;
            self.Length = actor.Length;
            self.Location.CopyFrom(actor.Location);
            self.P1 = actor.P1;
            self.P2 = actor.P2;
            self.P3 = actor.P3;
            self.Pointer = actor.Pointer;
            self.UnderTile.CopyFrom(actor.UnderTile);
            self.Vector.CopyFrom(actor.Vector);
            self.Code = actor.Code;
        }

        public static string GetCodeAsString(this IActor self)
        {
            return (self.Code ?? new byte[0]).ToStringValue();
        }

        public static void ModifyCodeAsString(this IActor self, string value)
        {
            if (self.Code == null)
            {
                self.Code = new byte[0];
            }
            var code = self.Code;
            var newCode = value.ToBytes();
            Array.Resize(ref code, newCode.Length);
            Array.Copy(newCode, code, newCode.Length);
        }

        public static void SetCodeAsString(this IActor self, string value)
        {
            self.Code = value.ToBytes();
        }
    }
}