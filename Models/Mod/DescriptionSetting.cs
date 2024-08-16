using System.Collections.Generic;
using Coflnet.Sky.Commands.Shared;

namespace Coflnet.Sky.Api.Models.Mod;

/// <summary>
/// Custom settings of what modifications to include in the response
/// </summary>
public class DescriptionSetting
{
    /// <summary>
    /// Lines and which elements to put into these lines
    /// </summary>
    public List<List<DescriptionField>> Fields { get; set; }
    /// <summary>
    /// If black and whitelist matches should be highlighted
    /// </summary>
    [SettingsDoc("Highlight items in ah and trade windows when matching black or whitelist filter")]
    public bool HighlightFilterMatch;
    [SettingsDoc("What is the minimum profit for highlighting best flip on page")]
    public long MinProfitForHighlight;

    public static DescriptionSetting Default => new DescriptionSetting()
    {
        Fields = new List<List<DescriptionField>>() {
                    new() { DescriptionField.LBIN, DescriptionField.BazaarBuy, DescriptionField.BazaarSell },
                    new() { DescriptionField.MEDIAN, DescriptionField.VOLUME },
                    new() { DescriptionField.FullCraftCost } }
    };

    [SettingsDoc("If the extra lore should be displayed or not")]
    public bool Disabled { get; set; }
}
