using System;
using System.Collections.Generic;
using System.Linq;
using Coflnet.Sky.Core;
using MessagePack;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Coflnet.Sky.Commands.Shared;

public class InventoryParserTests
{
    string jsonSample = """
        {
    "_events": {},
    "_eventsCount": 0,
    "id": 0,
    "type": "minecraft:inventory",
    "title": "Inventory",
    "slots": [
        null,
        null,
        null,
        null,
        {
            "type": 160,
            "count": 1,
            "metadata": 7,
            "nbt": {
                "type": "compound",
                "name": "",
                "value": {
                    "display": {
                        "type": "compound",
                        "value": {
                            "Lore": {
                                "type": "list",
                                "value": {
                                    "type": "string",
                                    "value": [
                                        "§7This slot may be changed to a",
                                        "§7shortcut for your favorite game",
                                        "§7or mode!",
                                        "",
                                        "§7You may change this slot at any",
                                        "§7time using Right Click."
                                    ]
                                }
                            },
                            "Name": {
                                "type": "string",
                                "value": "§eCustom Slot"
                            }
                        }
                    }
                }
            },
            "name": "stained_glass_pane",
            "displayName": "Stained Glass Pane",
            "stackSize": 64,
            "slot": 16
        },
        {"type":315,"count":1,"metadata":0,"nbt":{"type":"compound","name":"","value":{"ench":{"type":"list","value":{"type":"end","value":[]}},"Unbreakable":{"type":"byte","value":1},"HideFlags":{"type":"int","value":254},
            "display":{"type":"compound","value":{"Lore":{"type":"list","value":{"type":"string","value":["§7Health: §a+200","§7Defense: §a+150","§7Mining Speed: §a+230 §9(+60) §d(+90)"]}},"Name":{"type":"string","value":"§f§f§dJaded Chestplate of Divan"}}},
            "ExtraAttributes":{"type":"compound","value":{"rarity_upgrades":{"type":"int","value":1},"gems":{"type":"compound","value":{"JADE_1":{"type":"string","value":"FINE"},"JADE_0":{"type":"string","value":"FINE"},
                "unlocked_slots":{"type":"list","value":{"type":"string","value":["JADE_0","JADE_1","TOPAZ_0","AMBER_0","AMBER_1"]}},"AMBER_0":{"type":"string","value":"FINE"},"AMBER_1":{"type":"string","value":"FINE"},"TOPAZ_0":{"type":"string","value":"FINE"}}},"modifier":{"type":"string","value":"jaded"},"id":{"type":"string","value":"DIVAN_CHESTPLATE"},
                "enchantments":{"type":"compound","value":{"ultimate_wisdom":{"type":"int","value":1},"growth":{"type":"int","value":5},"protection":{"type":"int","value":5}}},"uuid":{"type":"string","value":"ea533251-6328-4a3c-8477-649cfb93ff45"},"timestamp":{"type":"string","value":"7/24/23 2:42 AM"}}}}},
                "stackId":null,"name":"golden_chestplate","displayName":"Golden Chestplate","stackSize":1,"maxDurability":112,"slot":32},
        {
            "type": 306,
            "count": 1,
            "metadata": 0,
            "nbt": {
                "type": "compound",
                "name": "",
                "value": {
                    "ench": {
                        "type": "list",
                        "value": {
                            "type": "end",
                            "value": []
                        }
                    },
                    "Unbreakable": {
                        "type": "byte",
                        "value": 1
                    },
                    "HideFlags": {
                        "type": "int",
                        "value": 254
                    },
                    "display": {
                        "type": "compound",
                        "value": {
                            "Lore": {
                                "type": "list",
                                "value": {
                                    "type": "string",
                                    "value": [
                                        "┬º7Defense: ┬ºa+10",
                                        "",
                                        "┬º7Growth I",
                                        "┬º7Grants ┬ºa+15 ┬ºcÔØñ Health┬º7.",
                                        "",
                                        "┬º7┬ºcYou do not have a high enough",
                                        "┬ºcEnchanting level to use some of",
                                        "┬ºcthe enchantments on this item!",
                                        "",
                                        "┬º7┬º8This item can be reforged!",
                                        "┬ºf┬ºlCOMMON HELMET"
                                    ]
                                }
                            },
                            "Name": {
                                "type": "string",
                                "value": "┬ºfIron Helmet"
                            }
                        }
                    },
                    "ExtraAttributes": {
                        "type": "compound",
                        "value": {
                            "id": {
                                "type": "string",
                                "value": "PET"
                            },
                            "gems": {
                                "type": "compound",
                                "value": {
                                    "JADE_0": {
                                        "type": "string",
                                        "value": "FINE"
                                    },
                                    "COMBAT_0": {
                                        "type": "compound",
                                        "value": {
                                            "uuid": {
                                                "type": "string",
                                                "value": "a5c233ba-9554-4c80-a697-1a78c66c045d"
                                            },
                                            "quality": {
                                                "type": "string",
                                                "value": "PERFECT"
                                            }
                                        }
                                    }
                                }
                            },
                            "ability_scroll": {
                                "type": "list",
                                "value": {
                                    "type": "string",
                                    "value": [
                                        "WITHER_SHIELD_SCROLL",
                                        "SHADOW_WARP_SCROLL"
                                    ]
                                }
                            },
                            "enchantments": {
                                "type": "compound",
                                "value": {
                                    "growth": {
                                        "type": "int",
                                        "value": 1
                                    }
                                }
                            },
                            "uuid": {
                                "type": "string",
                                "value": "0cf52647-c130-43ec-9c46-e2dc162d4894"
                            },
                            "modifier": {
                                "type": "string",
                                "value": "heavy"
                            },
                            "mined_crops": {
                                "type": "long",
                                "value": [
                                    1,
                                    8314091
                                ]
                            },
                            "necromancer_souls": {
                                "type": "list",
                                "value": {
                                    "type": "compound",
                                    "value": [
                                        {
                                            "mob_id": {
                                                "type": "string",
                                                "value": "MASTER_CRYPT_TANK_ZOMBIE_70"
                                            }
                                        },
                                        {
                                            "mob_id": {
                                                "type": "string",
                                                "value": "MASTER_CRYPT_TANK_ZOMBIE_70"
                                            }
                                        },
                                        {
                                            "mob_id": {
                                                "type": "string",
                                                "value": "MASTER_CRYPT_TANK_ZOMBIE_70"
                                            }
                                        },
                                        {
                                            "mob_id": {
                                                "type": "string",
                                                "value": "MASTER_CRYPT_TANK_ZOMBIE_70"
                                            }
                                        },
                                        {
                                            "mob_id": {
                                                "type": "string",
                                                "value": "MASTER_CRYPT_TANK_ZOMBIE_70"
                                            }
                                        },
                                        {
                                            "mob_id": {
                                                "type": "string",
                                                "value": "MASTER_CRYPT_TANK_ZOMBIE_70"
                                            }
                                        }
                                    ]
                                }
                            },
                            "petInfo": {
                                "type": "string",
                                "value": "{\"type\":\"ELEPHANT\",\"active\":false,\"exp\":3.397827122665796E7,\"tier\":\"LEGENDARY\",\"hideInfo\":false,\"heldItem\":\"PET_ITEM_FARMING_SKILL_BOOST_EPIC\",\"candyUsed\":10,\"uuid\":\"8760755f-f72b-4624-8cf2-c51b21e35acc\",\"hideRightClick\":false}"
                            },
                            "timestamp": {
                                "type": "string",
                                "value": "2/18/23 4:27 AM"
                            }
                        }
                    }
                }
            },
            "name": "iron_helmet",
            "displayName": "Iron Helmet",
            "stackSize": 1,
            "slot": 5
        }
	  ]
}
""";
    [Test]
    public void Parse()
    {
        var parser = new InventoryParser();
        var serialized = MessagePackSerializer.Serialize(parser.Parse(jsonSample));
        var deserialized = MessagePackSerializer.Deserialize<List<SaveAuction>>(serialized);
        var item = deserialized
                        .Where(i => i != null).Last();
        Console.WriteLine(JsonConvert.SerializeObject(item, Formatting.Indented));
        Assert.That("PET_ELEPHANT", Is.EqualTo(item.Tag));
        Assert.That("┬ºfIron Helmet", Is.EqualTo(item.ItemName));
        Assert.That(1, Is.EqualTo(item.Enchantments.Count));
        Assert.That(1, Is.EqualTo(item.Enchantments.Where(e => e.Type == Core.Enchantment.EnchantmentType.growth).First().Level));
        Assert.That("0cf52647-c130-43ec-9c46-e2dc162d4894", Is.EqualTo(item.FlatenedNBT["uuid"]));
        Assert.That("PET_ITEM_FARMING_SKILL_BOOST_EPIC", Is.EqualTo(item.FlatenedNBT["heldItem"]));
        Assert.That("33978271,22665796", Is.EqualTo(item.FlatenedNBT["exp"].Replace('.', ',')));
        Assert.That("FINE", Is.EqualTo(item.FlatenedNBT["JADE_0"]));
        Assert.That("PERFECT", Is.EqualTo(item.FlatenedNBT["COMBAT_0"]));
        Assert.That("4303281387", Is.EqualTo(item.FlatenedNBT["mined_crops"]));
        Assert.That("SHADOW_WARP_SCROLL WITHER_SHIELD_SCROLL", Is.EqualTo(item.FlatenedNBT["ability_scroll"]));
        Assert.That("6", Is.EqualTo(item.FlatenedNBT["MASTER_CRYPT_TANK_ZOMBIE_70"]));
        Assert.That(ItemReferences.Reforge.Heavy, Is.EqualTo(item.Reforge));
        Assert.That(Tier.COMMON, Is.EqualTo(item.Tier));

        var divan = deserialized.Where(i => i != null).Skip(1).First();
        Assert.That(new DateTime(2023, 7, 24), Is.EqualTo(divan.ItemCreatedAt.Date));
        Assert.That("AMBER_0,AMBER_1,JADE_0,JADE_1,TOPAZ_0", Is.EqualTo(divan.FlatenedNBT["unlocked_slots"]));
    }

    string petSample = """
        {
    "_events": {},
    "_eventsCount": 0,
    "id": 0,
    "type": "minecraft:inventory",
    "title": "Inventory",
    "slots": [
{
  "type": 397,
  "count": 1,
  "metadata": 3,
  "nbt": {
    "type": "compound",
    "name": "",
    "value": {
      "HideFlags": {
        "type": "int",
        "value": 254
      },
      "SkullOwner": {
        "type": "compound",
        "value": {
          "Id": {
            "type": "string",
            "value": "3a80504c-0993-3be5-92cd-2200f94a72b6"
          },
          "Properties": {
            "type": "compound",
            "value": {
              "textures": {
                "type": "list",
                "value": {
                  "type": "compound",
                  "value": [
                    {
                      "Value": {
                        "type": "string",
                        "value": "ewogICJ0aW1lc3RhbXAiIDogMTYwNjQyODA5NTg3NywKICAicHJvZmlsZUlkIiA6ICIzZmM3ZmRmOTM5NjM0YzQxOTExOTliYTNmN2NjM2ZlZCIsCiAgInByb2ZpbGVOYW1lIiA6ICJZZWxlaGEiLAogICJzaWduYXR1cmVSZXF1aXJlZCIgOiB0cnVlLAogICJ0ZXh0dXJlcyIgOiB7CiAgICAiU0tJTiIgOiB7CiAgICAgICJ1cmwiIDogImh0dHA6Ly90ZXh0dXJlcy5taW5lY3JhZnQubmV0L3RleHR1cmUvZjVmMjlhOTc1NTI5Mjc2ZDkxNmZjNjc5OTg4MzNjMTFlZTE3OGZmMjFlNTk0MWFmZGZiMGZhNzAxMGY4Mzc0ZSIKICAgIH0KICB9Cn0"
                      }
                    }
                  ]
                }
              }
            }
          }
        }
      },
      "display": {
        "type": "compound",
        "value": {
          "Lore": {
            "type": "list",
            "value": {
              "type": "string",
              "value": [
                "§8Fishing Pet, Grown-up Skin",
                "",
                "§b§lMAX LEVEL",
                "",
                "§7§eRight-click to add this pet to",
                "§eyour pet menu!",
                "",
                "§6§lLEGENDARY",
                "§8§m-----------------",
                "§7Seller: §b[MVP§e+§b] Parakkz",
                "§7Buy it now: §6190,000,000 coins",
                "",
                "§7Ends in: §e23h",
                "",
                "§eClick to inspect!"
              ]
            }
          },
          "Name": {
            "type": "string",
            "value": "§f§f§7[Lvl 100] §6Baby Yeti ✦"
          }
        }
      },
      "ExtraAttributes": {
        "type": "compound",
        "value": {
          "petInfo": {
            "type": "string",
            "value": "{\"type\":\"BABY_YETI\",\"active\":false,\"exp\":3.892246997045735E7,\"tier\":\"LEGENDARY\",\"hideInfo\":false,\"heldItem\":\"DWARF_TURTLE_SHELMET\",\"candyUsed\":10,\"skin\":\"YETI_GROWN_UP\",\"uuid\":\"28fdb3cb-1029-48cf-9591-3e7d8d67ba9a\",\"hideRightClick\":false}"
          },
          "id": {
            "type": "string",
            "value": "PET"
          },
          "uuid": {
            "type": "string",
            "value": "28fdb3cb-1029-48cf-9591-3e7d8d67ba9a"
          },
          "timestamp": {
            "type": "long",
            "value": [
                391,
                733787264
            ]
          }
        }
      }
    }
  },
  "stackId": null,
  "name": "skull",
  "displayName": "Head",
  "stackSize": 64,
  "slot": 11
}
]}
""";

    [Test]
    public void ParsePetRarity()
    {
        var parser = new InventoryParser();
        var serialized = MessagePackSerializer.Serialize(parser.Parse(petSample));
        var item = MessagePackSerializer.Deserialize<List<SaveAuction>>(serialized).First();
        Assert.That(Tier.LEGENDARY, Is.EqualTo(item.Tier));
        Assert.That(new DateTime(2023, 3, 29), Is.EqualTo(item.ItemCreatedAt.Date));
    }



    private string jsonSampleCT = """
[{"id":"minecraft:tnt","Count":2,"tag":{"ench":[],"HideFlags":254,"display":{"Lore":["§7Breaks weak walls. Can be used","§7to blow up Crypts in §cThe","§cCatacombs §7and §3Crystal","§3Hollows§7.","","§9§lRARE"],
"Name":"§9Superboom TNT"},"ExtraAttributes":{"id":"SUPERBOOM_TNT"}},"Damage":0},
{"id":"minecraft:stained_glass","Count":1,"tag":{"HideFlags":254,"display":{"Lore":["§7§oA rare space helmet forged","§7§ofrom shards of moon glass.","","§7§8This item can be reforged!","§c§lSPECIAL HELMET","pricePaid xyy"],
"Name":"§cSpace Helmet"},"ExtraAttributes":{"id":"DCTR_SPACE_HELM","uuid":"b14aefbd-cbf8-4ca1-aa2e-5c0422807c60","timestamp":"4/8/23 10:01 AM", enchantments:{impaling:3,chance:4,piercing:1,infinite_quiver:10,ultimate_soul_eater:5,snipe:3,telekinesis:1,power:7}}},"Damage":14}]
""";
    [Test]
    public void ParseCT()
    {
        var parser = new InventoryParser();
        var serialized = MessagePackSerializer.Serialize(parser.Parse(jsonSampleCT));
        var item = MessagePackSerializer.Deserialize<List<SaveAuction>>(serialized)
                        .Where(i => i != null).Last();
        Assert.That("DCTR_SPACE_HELM", Is.EqualTo(item.Tag));
        Assert.That("§cSpace Helmet", Is.EqualTo(item.ItemName));
        Assert.That(ItemReferences.Reforge.None, Is.EqualTo(item.Reforge));
        Assert.That(8, Is.EqualTo(item.Enchantments.Count));
        Assert.That(3, Is.EqualTo(item.Enchantments.Where(e => e.Type == Core.Enchantment.EnchantmentType.impaling).First().Level));
        Assert.That(4, Is.EqualTo(item.Enchantments.Where(e => e.Type == Core.Enchantment.EnchantmentType.chance).First().Level));
        Assert.That(1, Is.EqualTo(item.Count));
        Assert.That("b14aefbd-cbf8-4ca1-aa2e-5c0422807c60", Is.EqualTo(item.FlatenedNBT["uuid"]));
        Assert.That("4/8/23 10:01 AM", Is.EqualTo(item.FlatenedNBT["timestamp"]));
        Assert.That(Tier.SPECIAL, Is.EqualTo(item.Tier));
    }

    /// <summary>
    /// Some hypixel item tags are split up in multiple virtual items
    /// The parsing needs to reflect that
    /// </summary>
    [Test]
    public void SpecialItemIdParsing()
    {
        var parser = new InventoryParser();
        var data = parser.Parse("""
        {
        "title": "Inventory",
        "slots": [
            {
            "count": 1,
            "metadata": 0,
            "nbt": {
                "type": "compound",
                "name": "",
                "value": {
                    "ExtraAttributes": {
                        "type": "compound",
                        "value": {
                            "potion_level": {
                                "type": "int",
                                "value": 5
                            },
                            "potion": {
                                "type": "string",
                                "value": "harvest_harbinger"
                            },
                            "potion_type": {
                                "type": "string",
                                "value": "POTION"
                            },
                            "id": {
                                "type": "string",
                                "value": "POTION"
                            }
                        }
                    }
                }
            }},
            {
            "count": 1,
            "metadata": 0,
            "nbt": {
                "type": "compound",
                "name": "",
                "value": {
                    "ExtraAttributes": {
                        "type": "compound",
                        "value": {
                            "runes": {
                                "type": "compound",
                                "value": {
                                    "ICE_SKATES": {
                                        "type": "int",
                                        "value": 3
                                    }
                                }
                            },
                            "id": {
                                "type": "string",
                                "value": "UNIQUE_RUNE"
                            }
                        }
                    }
                }
            }}
        ]}
        """);
        Assert.That(data.First().Tag, Is.EqualTo("POTION_harvest_harbinger"));
        Assert.That(data.Last().Tag, Is.EqualTo("UNIQUE_RUNE_ICE_SKATES"));
        Console.WriteLine(JsonConvert.SerializeObject(data.Last().FlatenedNBT, Formatting.Indented));
        Assert.That(data.Last().FlatenedNBT["RUNE_ICE_SKATES"], Is.EqualTo("3"));
    }

    [Test]
    public void Parse117Strings()
    {
        var parser = new InventoryParser();
        var data = parser.Parse("""
        {
        "_events": {},
        "_eventsCount": 0,
        "id": 0,
        "type": "minecraft:inventory",
        "title": "Inventory",
        "slots": [
            {
            "type": 746,
            "count": 1,
            "metadata": 0,
            "nbt": {
                "type": "compound",
                "name": "",
                "value": {
                    "Unbreakable": {
                        "type": "byte",
                        "value": 1
                    },
                    "HideFlags": {
                        "type": "int",
                        "value": 255
                    },
                    "display": {
                        "type": "compound",
                        "value": {
                            "Lore": {
                                "type": "list",
                                "value": {
                                    "type": "string",
                                    "value": [
                                        "{\"italic\":false,\"extra\":[{\"color\":\"gray\",\"text\":\"Defense: \"},{\"text\":\" \"},{\"color\":\"green\",\"text\":\"+10\"}],\"text\":\"\"}",
                                        "{\"italic\":false,\"text\":\"\"}",
                                        "{\"italic\":false,\"extra\":[{\"color\":\"gray\",\"text\":\"Growth I\"}],\"text\":\"\"}",
                                        "{\"italic\":false,\"extra\":[{\"color\":\"gray\",\"text\":\"Grants \"},{\"color\":\"green\",\"text\":\"+15 \"},{\"color\":\"red\",\"text\":\"❤ Health\"},{\"color\":\"gray\",\"text\":\".\"}],\"text\":\"\"}",
                                        "{\"italic\":false,\"text\":\"\"}",
                                        "{\"italic\":false,\"extra\":[{\"color\":\"gray\",\"text\":\"\"},{\"color\":\"red\",\"text\":\"You do not have a high enough\"}],\"text\":\"\"}",
                                        "{\"italic\":false,\"extra\":[{\"color\":\"red\",\"text\":\"Enchanting level to use some of\"}],\"text\":\"\"}",
                                        "{\"italic\":false,\"extra\":[{\"color\":\"red\",\"text\":\"the enchantments on this item!\"}],\"text\":\"\"}",
                                        "{\"italic\":false,\"text\":\"\"}",
                                        "{\"italic\":false,\"extra\":[{\"color\":\"gray\",\"text\":\"\"},{\"color\":\"dark_gray\",\"text\":\"This item can be reforged!\"}],\"text\":\"\"}",
                                        "{\"italic\":false,\"extra\":[{\"bold\":true,\"color\":\"white\",\"text\":\"COMMON HELMET\"}],\"text\":\"\"}"
                                    ]
                                }
                            },
                            "Name": {
                                "type": "string",
                                "value": "{\"italic\":false,\"extra\":[{\"color\":\"white\",\"text\":\"Iron Helmet\"}],\"text\":\"\"}"
                            }
                        }
                    },
                    "Enchantments": {
                        "type": "list",
                        "value": {
                            "type": "compound",
                            "value": [
                                {
                                    "lvl": {
                                        "type": "short",
                                        "value": 0
                                    },
                                    "id": {
                                        "type": "string",
                                        "value": "minecraft:protection"
                                    }
                                }
                            ]
                        }
                    },
                    "ExtraAttributes": {
                        "type": "compound",
                        "value": {
                            "id": {
                                "type": "string",
                                "value": "IRON_HELMET"
                            },
                            "enchantments": {
                                "type": "compound",
                                "value": {
                                    "growth": {
                                        "type": "int",
                                        "value": 1
                                    }
                                }
                            },
                            "uuid": {
                                "type": "string",
                                "value": "0cf52647-c130-43ec-9c46-e2dc162d4894"
                            },
                            "timestamp": {
                                "type": "string",
                                "value": "2/18/23 4:27 AM"
                            }
                        }
                    }
                }
            },
            "name": "iron_helmet",
            "displayName": "Iron Helmet",
            "stackSize": 1,
            "slot": 5
            },
        ]}
        """);

        Assert.That("§fIron Helmet", Is.EqualTo(data.First().ItemName));
        Assert.That(data.First().Context["lore"].StartsWith("§7Defense:  §a+10\n\n§7Growth I\n§7Grants §a+15 §c❤ Health"));
        Assert.That(new DateTime(2023, 2, 18), Is.EqualTo(data.First().ItemCreatedAt.Date));
    }

    [Test]
    public void DoNotCrashForVanilla()
    {
        var parser = new InventoryParser();
        var data = parser.Parse("""
        {
        "_events": {},
        "_eventsCount": 0,
        "id": 0,
        "type": "minecraft:inventory",
        "slots":[
            {"type":844,"count":1,"metadata":0,"nbt":{"type":"compound","name":"","value":{"HideFlags":{"type":"int","value":254},
            "display":{"type":"compound","value":{"Lore":{"type":"list","value":{"type":"string","value":["{\"italic\":false,\"extra\":[{\"color\":\"dark_gray\",\"text\":\"Bed Wars\"}],\"text\":\"\"}","{\"italic\":false,\"text\":\"\"}","{\"italic\":false,\"extra\":[{\"color\":\"gray\",\"text\":\"Join a game of 4v4v4v4.\"}],\"text\":\"\"}","{\"italic\":false,\"text\":\"\"}","{\"italic\":false,\"extra\":[{\"color\":\"green\",\"text\":\"► Click to Play\"}],\"text\":\"\"}","{\"italic\":false,\"extra\":[{\"color\":\"gray\",\"text\":\"1,171 currently playing!\"}],\"text\":\"\"}","{\"italic\":false,\"text\":\"\"}","{\"italic\":false,\"extra\":[{\"color\":\"dark_gray\",\"text\":\"Right Click to change this slot!\"}],\"text\":\"\"}"]}},
            "Name":{"type":"string","value":"{\"italic\":false,\"extra\":[{\"color\":\"green\",\"text\":\"4v4v4v4 \"}],\"text\":\"\"}"}}},"ExtraAttributes":{"type":"compound","value":{"animatedItemIdentifier":{"type":"string","value":"c7925960-db90-404a-aaf1-546f776bcc24"}}},"Damage":{"type":"int","value":14}}},"stackId":null,"name":"red_bed","displayName":"Red Bed","stackSize":1,"slot":14}
        ]}
        """);
        Assert.That(data.First()?.ItemName, Is.Null);
    }
}