
using System;
using System.Linq.Expressions;
using Coflnet.Sky.Core;

namespace Coflnet.Sky.Commands.Shared
{
    public class ProfitPercentageDetailedFlipFilter : NumberDetailedFlipFilter
    {
        protected override Expression<Func<FlipInstance, double>> GetSelector(FilterContext filters)
        {
            return (f) => (long)f.ProfitPercentage;
        }
    }
    

}