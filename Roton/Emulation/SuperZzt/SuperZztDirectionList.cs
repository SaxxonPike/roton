using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Directions;

namespace Roton.Emulation.SuperZzt
{
    public class SuperZztDirectionList : DirectionList
    {
        public SuperZztDirectionList(ICollection<IDirection> commands) : base(new Dictionary<string, IDirection>
        {
            {"CCW", commands.OfType<CcwDirection>().Single()},
            {"CW", commands.OfType<CwDirection>().Single()},
            {"E", commands.OfType<EastDirection>().Single()},
            {"EAST", commands.OfType<EastDirection>().Single()},
            {"FLOW", commands.OfType<FlowDirection>().Single()},
            {"I", commands.OfType<IdleDirection>().Single()},
            {"IDLE", commands.OfType<IdleDirection>().Single()},
            {"N", commands.OfType<NorthDirection>().Single()},
            {"NORTH", commands.OfType<NorthDirection>().Single()},
            {"OPP", commands.OfType<OppDirection>().Single()},
            {"RND", commands.OfType<RndDirection>().Single()},
            {"RNDNE", commands.OfType<RndNeDirection>().Single()},
            {"RNDNS", commands.OfType<RndNsDirection>().Single()},
            {"RNDP", commands.OfType<RndPDirection>().Single()},
            {"SEEK", commands.OfType<SeekDirection>().Single()},
            {"S", commands.OfType<SouthDirection>().Single()},
            {"SOUTH", commands.OfType<SouthDirection>().Single()},
            {"W", commands.OfType<WestDirection>().Single()},
            {"WEST", commands.OfType<WestDirection>().Single()}
        })
        {
        }
    }
}