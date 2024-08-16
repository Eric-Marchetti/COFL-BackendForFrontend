
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Coflnet.Sky.Commands.Tests;
using Coflnet.Sky.Core;
using Coflnet.Sky.Filter;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Coflnet.Sky.Commands.Shared
{
    public class FlipFilterTests
    {
        FlipInstance sampleFlip;

        [SetUp]
        public void Setup()
        {
            DiHandler.OverrideService<FilterEngine, FilterEngine>(new FilterEngine());
            sampleFlip = new FlipInstance()
            {
                MedianPrice = 10,
                Volume = 10,
                Auction = new SaveAuction()
                {
                    Bin = false,
                    Enchantments = new List<Enchantment>(){
                    new(Enchantment.EnchantmentType.critical,4)
                },
                    FlatenedNBT = new Dictionary<string, string>() { { "candy", "3" } }
                },
                Context = new Dictionary<string, string>(),
                Finder = LowPricedAuction.FinderType.SNIPER_MEDIAN
            };
        }
        [Test]
        public void FlipFilterLoad()
        {
            var settings = JsonConvert.DeserializeObject<FlipSettings>(File.ReadAllText("mock/bigsettings.json"));
            sampleFlip.Auction.StartingBid = 10;
            sampleFlip.MedianPrice = 1000000;
            sampleFlip.Auction.ItemName = "hi";
            NoMatch(settings, sampleFlip);
            var watch = Stopwatch.StartNew();
            for (int i = 0; i < 2000; i++)
            {
                NoMatch(settings, sampleFlip);
            }
            Assert.That(watch.ElapsedMilliseconds, Is.LessThanOrEqualTo(6 * TestConstants.DelayMultiplier));
        }

        [Test]
        public void IsMatch()
        {
            var settings = new FlipSettings()
            {
                BlackList = new List<ListEntry>() { new() { filter = new Dictionary<string, string>() { { "Bin", "true" } } } }
            };
            var matches = settings.MatchesSettings(sampleFlip);
            Assert.That(matches.Item1, "flip should match");
            sampleFlip.Auction.Bin = true;
            Assert.That(!settings.MatchesSettings(sampleFlip).Item1, "flip should not match");
        }


        [Test]
        public void EnchantmentMatch()
        {
            var settings = new FlipSettings()
            {
                BlackList = new List<ListEntry>() { new() { filter = new Dictionary<string, string>() { { "Enchantment", "aiming" }, { "EnchantLvl", "1" } } } }
            };
            var matches = settings.MatchesSettings(sampleFlip);
            Assert.That(matches.Item1, "flip should match");
        }


        [Test]
        public void EnchantmentBlacklistMatch()
        {
            var settings = new FlipSettings()
            {
                BlackList = new List<ListEntry>() { new() { filter = new Dictionary<string, string>() { { "Enchantment", "critical" }, { "EnchantLvl", "4" } } } }
            };
            var matches = settings.MatchesSettings(sampleFlip);
            Assert.That(!matches.Item1, "flip should not match");
        }

        [Test]
        public void CandyBlacklistMatch()
        {
            NBT.Instance = new NBTMock();
            sampleFlip.Auction.FlatenedNBT["candyUsed"] = "1";
            var settings = new FlipSettings()
            {
                BlackList = new List<ListEntry>() { new() { filter = new Dictionary<string, string>() { { "Candy", "any" } } } }
            };
            var matches = settings.MatchesSettings(sampleFlip);
            Console.WriteLine(new FilterEngine().GetMatchExpression(settings.BlackList[0].filter).ToString());
            Assert.That(!matches.Item1, "flip should not match " + matches.Item2);
            sampleFlip.Auction.FlatenedNBT["candyUsed"] = "0";
            matches = settings.MatchesSettings(sampleFlip);
            Assert.That(matches.Item1, "flip should match " + matches.Item2);
        }

        [Test]
        public void WhitelistBookEnchantBlackistItem()
        {
            NBT.Instance = new NBTMock();
            var tag = "ENCHANTED_BOOK";
            FlipInstance bookOfa = CreatOfaAuction(tag);
            FlipInstance reaperOfa = CreatOfaAuction("REAPER");
            var oneForAllFilter = new Dictionary<string, string>() { { "Enchantment", "ultimate_one_for_all" }, { "EnchantLvl", "1" } };
            var settings = new FlipSettings()
            {
                BlackList = new List<ListEntry>() { new() { ItemTag = "REAPER", filter = oneForAllFilter } },
                WhiteList = new List<ListEntry>() { new() { ItemTag = "ENCHANTED_BOOK", filter = oneForAllFilter } }
            };
            var matches = settings.MatchesSettings(bookOfa);
            var shouldNotBatch = settings.MatchesSettings(reaperOfa);
            Assert.That(matches.Item1, "flip should match");
            Assert.That(!shouldNotBatch.Item1, "flip should not match");
        }


        [Test]
        public void MinProfitFilterMatch()
        {
            NBT.Instance = new NBTMock();
            sampleFlip.Auction.NBTLookup = new NBTLookup[] { new(1, 2) };
            var settings = new FlipSettings()
            {
                MinProfit = 10000,
                WhiteList = new List<ListEntry>() { new() { filter = new Dictionary<string, string>() { { "MinProfit", "5" } } } }
            };
            var matches = settings.MatchesSettings(sampleFlip);
            System.Console.WriteLine(sampleFlip.Profit);
            Assert.That(matches.Item1, matches.Item2);
        }



        [Test]
        public void VolumeDeciamalFilterMatch()
        {
            NBT.Instance = new NBTMock();
            sampleFlip.Auction.NBTLookup = new NBTLookup[] { new(1, 2) };
            var settings = new FlipSettings()
            {
                MinProfit = 1,
                MinVolume = 0.5,

            };
            sampleFlip.Volume = 0.8f;
            var matches = settings.MatchesSettings(sampleFlip);
            Assert.That(matches.Item1, matches.Item2);
            sampleFlip.Volume = 0.2f;
            var matches2 = settings.MatchesSettings(sampleFlip);
            Assert.That(!matches2.Item1, matches2.Item2);
        }

        [Test]
        public void VolumeDeciamalFilterWhitelistMatch()
        {
            var settings = new FlipSettings()
            {
                MinProfit = 1,
                MinVolume = 50,
            };
            settings.WhiteList = new List<ListEntry>() { new() { filter = new Dictionary<string, string>() { { "Volume", "<0.5" } } } };
            sampleFlip.Volume = 0.1f;
            var matches3 = settings.MatchesSettings(sampleFlip);
            Assert.That(matches3.Item1, matches3.Item2);
            sampleFlip.Volume = 1;
            var notMatch = settings.MatchesSettings(sampleFlip);
            Assert.That(!notMatch.Item1, notMatch.Item2);
        }

        [Test]
        [TestCase("1", 1, true)]
        [TestCase("<1", 0.5f, true)]
        [TestCase(">1", 0.5f, false)]
        [TestCase("<0.5", 0.1f, true)]
        public void VolumeDeciamalFilterSingleMatch(string val, float vol, bool result)
        {
            var volumeFilter = new VolumeDetailedFlipFilter();
            var exp = volumeFilter.GetExpression(null, val);
            Assert.That(exp.Compile().Invoke(new FlipInstance() { Volume = vol }), Is.EqualTo(result));
        }
        [Test]
        [TestCase("1", true)]
        [TestCase("2", false)]
        public void ReferenceAgeFilterMatch(string val, bool result)
        {
            var settings = new FlipSettings
            {
                MinProfit = 100,
                WhiteList = new List<ListEntry>() { new() { filter = new Dictionary<string, string>() { { "ReferenceAge", "<2" } } } }
            };
            sampleFlip.Context["refAge"] = val;
            var matches3 = settings.MatchesSettings(sampleFlip);
            Assert.That(result, Is.EqualTo(matches3.Item1), matches3.Item2);
        }



        [Test]
        public void FlipFilterFinderCustomMinProfitNoBinMatch()
        {
            var settings = new FlipSettings()
            {
                MinProfit = 10000,
                WhiteList = new List<ListEntry>() { new() { filter = new Dictionary<string, string>() {
                    { "MinProfit", "5" },{"FlipFinder", "SNIPER_MEDIAN"},{"Bin","false"} } } }
            };
            sampleFlip.Auction.StartingBid = 10;
            sampleFlip.MedianPrice = 100;
            sampleFlip.Finder = LowPricedAuction.FinderType.SNIPER_MEDIAN;
            Matches(settings, sampleFlip);
            sampleFlip.Finder = LowPricedAuction.FinderType.FLIPPER;
            NoMatch(settings, sampleFlip);
        }
        [Test]
        public void FlipFilterFinderCustomMinProfitMatch()
        {
            var settings = new FlipSettings()
            {
                MinProfit = 10000,
                WhiteList = new List<ListEntry>() { new() { filter = new Dictionary<string, string>() {
                    { "MinProfit", "5" } } } }
            };
            sampleFlip.Auction.StartingBid = 10;
            sampleFlip.MedianPrice = 100;
            sampleFlip.Finder = LowPricedAuction.FinderType.SNIPER_MEDIAN;
            Matches(settings, sampleFlip);
        }

        [Test]
        public void RenownedFivestaredMythic()
        {
            NBT.Instance = new NBTMock();
            var filters = new Dictionary<string, string>() { { "Stars", "5" }, { "Reforge", "Renowned" }, { "Rarity", "MYTHIC" } };
            var matcher = new ListEntry() { filter = filters, ItemTag = "abc" };
            var result = matcher.GetExpression().Compile()(new FlipInstance()
            {
                Auction = new SaveAuction()
                {
                    Reforge = ItemReferences.Reforge.Renowned,
                    Tier = Tier.MYTHIC,
                    FlatenedNBT = new Dictionary<string, string>() { { "upgrade_level", "5" } }
                }
            });
            Assert.That(result);
        }

        [Test]
        public void WhitelistAfterMain()
        {
            var settings = new FlipSettings()
            {
                WhiteList = new List<ListEntry>() { new() { filter = new() { { "Reforge", "Sharp" }, { "AfterMainFilter", "true" } } } },
                MinProfit = 1000
            };
            sampleFlip.Auction.Reforge = ItemReferences.Reforge.Sharp;
            sampleFlip.Auction.StartingBid = 5;
            sampleFlip.MedianPrice = 500;
            var matches = settings.MatchesSettings(sampleFlip);
            Assert.That(!matches.Item1, "flip shouldn't match below minprofit");
            sampleFlip.MedianPrice = 5000;
            matches = settings.MatchesSettings(sampleFlip);
            Assert.That(matches.Item1, "flip should match above minprofit");
        }


        [Test]
        public void ForceBlacklistOverwritesWhitelist()
        {
            var settings = new FlipSettings
            {
                MinProfit = 0,
                MinVolume = 0,
                WhiteList = new List<ListEntry>() { new() { filter = new() { { "Volume", "<0.5" } } } },
                BlackList = new List<ListEntry>() { new() { filter = new() { { "Volume", "<0.5" }, { "ForceBlacklist", "" } } } }
            };
            sampleFlip.Volume = 0.1f;
            var result = settings.MatchesSettings(sampleFlip);

            Assert.That(!result.Item1, result.Item2);
            Assert.That("forced blacklist matched general filter", Is.EqualTo(result.Item2));
        }

        [Test]
        public void JujuHighProfit()
        {
            var settings = new FlipSettings
            {
                MinProfit = 0,
                MinVolume = 0,
                WhiteList = new List<ListEntry>() { new() { filter = new() { { "FlipFinder", "SNIPER_MEDIAN" }, { "MinProfitPercentage", "40" } }, ItemTag = "JUJU_SHORTBOW" } },
                BlackList = new List<ListEntry>() { new() { filter = new() { { "FlipFinder", "SNIPER_MEDIAN" } } } }
            };
            sampleFlip.Volume = 0.1f;
            sampleFlip.Auction.Tag = "JUJU_SHORTBOW";
            sampleFlip.MedianPrice = 35800000;
            sampleFlip.Auction.StartingBid = 6000;
            sampleFlip.Finder = LowPricedAuction.FinderType.SNIPER_MEDIAN;
            var result = settings.MatchesSettings(sampleFlip);

            Assert.That(result.Item1, result.Item2);
            Assert.That("whitelist matched filter for item", Is.EqualTo(result.Item2));
        }

        [Test]
        public void FlipFilterFinderBlacklist()
        {
            var settings = new FlipSettings()
            {
                MinProfit = 100,
                BlackList = new List<ListEntry>() { new() { filter = new Dictionary<string, string>() {
                    { "FlipFinder", "FLIPPER" } } } }
            };
            sampleFlip.Auction.StartingBid = 10;
            sampleFlip.MedianPrice = 1000000;
            sampleFlip.Finder = LowPricedAuction.FinderType.FLIPPER;
            NoMatch(settings, sampleFlip);
        }
        [Test]
        public void MinProfitPercentage()
        {
            var settings = new FlipSettings()
            {
                MinProfit = 10000,
                WhiteList = new List<ListEntry>() { new() { filter = new Dictionary<string, string>() {
                    { "ProfitPercentage", ">5" } }
                } }
            };
            sampleFlip.Auction.StartingBid = 10;
            sampleFlip.MedianPrice = 10000;
            Matches(settings, sampleFlip);
        }
        [Test]
        public void RangeProfitPercentage()
        {
            var settings = new FlipSettings()
            {
                MinProfit = 10000,
                WhiteList = new List<ListEntry>() { new() { filter = new Dictionary<string, string>() {
                    { "ProfitPercentage", "1-10" } }
                } }
            };
            sampleFlip.Auction.StartingBid = 50;
            sampleFlip.MedianPrice = 55;
            Console.WriteLine(sampleFlip.ProfitPercentage);
            Matches(settings, sampleFlip);
        }

        [Test]
        public void CheckCombinedFinder()
        {
            var settings = new FlipSettings()
            {
                MinProfit = 35000000,
                WhiteList = new List<ListEntry>() { new() { ItemTag = "PET_ENDER_DRAGON",
                filter = new Dictionary<string, string>() {
                    { "FlipFinder", "FLIPPER_AND_SNIPERS" },
                    { "MinProfitPercentage", "5" }
                } } },
                BlackList = new List<ListEntry>() { new() { ItemTag = "PET_ENDER_DRAGON" } }
            };
            sampleFlip.Auction.Tag = "PET_ENDER_DRAGON";
            sampleFlip.Auction.StartingBid = 250000000;
            sampleFlip.MedianPrice = 559559559;
            sampleFlip.Finder = LowPricedAuction.FinderType.SNIPER_MEDIAN;
            Matches(settings, sampleFlip);
        }

        private static void Matches(FlipSettings targetSettings, FlipInstance flip)
        {
            var matches = targetSettings.MatchesSettings(flip);
            Assert.That(matches.Item1, matches.Item2);
        }
        private static void NoMatch(FlipSettings targetSettings, FlipInstance flip)
        {
            var matches = targetSettings.MatchesSettings(flip);
            Assert.That(!matches.Item1, matches.Item2);
        }

        private static ListEntry CreateFilter(string key, string value)
        {
            return new ListEntry() { filter = new Dictionary<string, string>() { { key, value } } };
        }

        private static FlipInstance CreatOfaAuction(string tag)
        {
            return new FlipInstance()
            {
                MedianPrice = 10,
                Volume = 10,
                Auction = new SaveAuction()
                {
                    Tag = tag,
                    Enchantments = new List<Enchantment>(){
                        new(Enchantment.EnchantmentType.ultimate_one_for_all,1)
                    }
                },
                Finder = LowPricedAuction.FinderType.SNIPER
            };
        }

        class NBTMock : INBT
        {
            public short GetKeyId(string name)
            {
                return 1;
            }

            public int GetValueId(short key, string value)
            {
                return 2;
            }
        }
    }
}