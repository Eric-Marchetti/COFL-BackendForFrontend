using System;
using System.Collections.Generic;

namespace Coflnet.Sky.Commands.Shared;

public class OwnedConfigs
{
    public List<OwnedConfig> Configs { get; set; } = new();
    public class OwnedConfig
    {
        public string Name { get; set; }
        public int Version { get; set; }
        public string ChangeNotes { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public int PricePaid { get; set; }
        public DateTime BoughtAt { get; set; } = DateTime.UtcNow;
    }
}
