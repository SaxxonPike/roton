using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Data.Impl;

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
        return (self.Code ?? []).ToStringValue();
    }

    public static void ModifyCodeAsString(this IActor self, string value)
    {
        var length = value?.Length ?? 0;
        if (self.Code == null || self.Code.Length != length)
        {
            self.Code = new byte[length];
        }
        value.ToBytes(self.Code);
    }

    public static void SetCodeAsString(this IActor self, string value)
    {
        self.Code = value.ToBytes();
    }
}