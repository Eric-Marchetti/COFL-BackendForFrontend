using System.Collections.Generic;

namespace Coflnet.Sky.Commands.Shared;

public class ConfigContainer
{
    public FlipSettings Settings { get; set; }
    public string Name { get; set; }
    public int Version { get; set; }
    public string ChangeNotes { get; set; }
    public string OwnerId { get; set; }
    public int Price { get; set; }
}

public class CreatedConfigs
{
    public HashSet<string> Configs { get; set; } = new();
}