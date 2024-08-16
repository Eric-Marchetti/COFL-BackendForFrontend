
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Coflnet.Sky.Filter;

namespace Coflnet.Sky.Commands.Shared;

public class PerfectArmorTierDetailedFlipFilter : NumberDetailedFlipFilter
{
    public object[] Options => new object[] { 1, 12 };

    public FilterType FilterType => FilterType.NUMERICAL | FilterType.LOWER | FilterType.RANGE;

    public Expression<Func<FlipInstance, bool>> GetExpression(FilterContext filters, string val)
    {
        return StartsWithPerfect().And(base.GetExpression(filters, val));
    }

    protected override Expression<Func<FlipInstance, double>> GetSelector(FilterContext filters)
    {
        return f => double.Parse(f.Tag.Split("_", 5, StringSplitOptions.None).Last());
    }

    private Expression<Func<FlipInstance, bool>> StartsWithPerfect()
    {
        return flip => flip.Tag.StartsWith("PERFECT_");
    }
}
