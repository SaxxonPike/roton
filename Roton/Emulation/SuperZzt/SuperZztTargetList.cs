using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Targets;

namespace Roton.Emulation.SuperZzt
{
    public class SuperZztTargetList : Targets.TargetList
    {
        public SuperZztTargetList(ICollection<ITarget> targets) : base(new Dictionary<string, ITarget>
        {
            {"ALL", targets.OfType<AllTarget>().Single()},
            {"OTHERS", targets.OfType<OthersTarget>().Single()},
            {"SELF", targets.OfType<SelfTarget>().Single()},
        })
        {
        }
    }
}