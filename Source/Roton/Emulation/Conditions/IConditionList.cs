using System;

namespace Roton.Emulation.Conditions;

public interface IConditionList
{
    ICondition Get(ReadOnlySpan<char> name);
}