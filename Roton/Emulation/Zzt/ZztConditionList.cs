using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Conditions;

namespace Roton.Emulation.Zzt
{
    public class ZztConditionList : Conditions.ConditionList
    {
        public ZztConditionList(ICollection<ICondition> conditions) : base(new Dictionary<string, ICondition>
        {
            { "ALLIGNED", conditions.OfType<AlignedCondition>().Single()},
            { "ANY", conditions.OfType<AnyCondition>().Single()},
            { "BLOCKED", conditions.OfType<BlockedCondition>().Single()},
            { "CONTACT", conditions.OfType<ContactCondition>().Single()},
            { "ENERGIZED", conditions.OfType<EnergizedCondition>().Single()},
            { "NOT", conditions.OfType<NotCondition>().Single()}
        })
        {
        }
    }
}