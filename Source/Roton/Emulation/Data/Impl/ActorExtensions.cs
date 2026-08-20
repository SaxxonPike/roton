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
        self.Location = actor.Location;
        self.P1 = actor.P1;
        self.P2 = actor.P2;
        self.P3 = actor.P3;
        self.Pointer = actor.Pointer;
        self.UnderTile = actor.UnderTile;
        self.Vector = actor.Vector;
        self.Code = actor.Code;
    }
}