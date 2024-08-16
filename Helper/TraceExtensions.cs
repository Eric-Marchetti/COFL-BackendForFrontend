using System.Diagnostics;
using System.Collections.Generic;
using Coflnet.Sky.Core;

namespace Coflnet.Sky.Commands.Shared;

#nullable enable
public static class TraceExtensions
{
    public static Activity? Log(this Activity? activity, string message, int maxcontextLength = 6_000)
    {
        return activity?.AddEvent(new ActivityEvent("log", System.DateTimeOffset.Now, new ActivityTagsCollection(new[] { new KeyValuePair<string, object?>("message", message.Truncate(maxcontextLength)) })));
    }
}

#nullable restore