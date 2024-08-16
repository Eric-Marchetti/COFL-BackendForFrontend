
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Coflnet.Sky.Core;
using Coflnet.Sky.Filter;

namespace Coflnet.Sky.Commands.Shared;

[FilterDescription("Above 1 reduces by absolute number, from 0-1 uses percentage 0.2 removes 20%")]
public class ReduceTargetByDetailedFlipFilter : NumberDetailedFlipFilter
{
    public override object[] Options => [0, 10_000_000_000];

    public FilterType FilterType => FilterType.NUMERICAL;

    public override Expression<Func<FlipInstance, bool>> GetExpression(FilterContext filters, string val)
    {
        var target = NumberParser.Double(val);
        if (target < 1)
        {
            return f => f.Context.TryAdd("target", ((long)(f.Target * (1 - target))).ToString());
        }
        return f => f.Context.TryAdd("target", (f.Target - target).ToString());
    }
}
