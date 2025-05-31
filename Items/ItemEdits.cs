using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using static Terraria.ModLoader.PlayerDrawLayer;
using Terraria.Localization;
using System.Linq;
using Terraria.Chat;

namespace SkyblockBrutalism.Items
{
	public class ItemEdits : GlobalItem
	{
        public static LocalizedText GraveDigDirt { get; private set; }
        public override void SetStaticDefaults()
        {
            GraveDigDirt = Language.GetOrRegister(Mod.GetLocalizationKey($"ItemEdits.{nameof(GraveDigDirt)}"));
            //The Slimes ate all the traps!
            ItemID.Sets.OreDropsFromSlime[ItemID.DartTrap] = (2, 6);
            //Part of assigning stone to extractonator use
            ItemID.Sets.ExtractinatorMode[ItemID.StoneBlock] = ItemID.StoneBlock;

            //Custom Shimmer Recipes
            ItemID.Sets.ShimmerTransformToItem[ItemID.GoldenKey] = ItemID.GoldChest;
            ItemID.Sets.ShimmerTransformToItem[ItemID.ShadowKey] = ItemID.ShadowChest;
        }
        //Gravedigger Shovel Tierdown.  See RecipeEdits and NPCEdits.
        public override void SetDefaults(Item entity)
        {
            if (entity.type == ItemID.GravediggerShovel)
            {
                entity.damage = 8;
                entity.value = 215;
            }
        }
        //Most tooltips and names changed in language file, but adding to the item tooltip requires some code.
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.GravediggerShovel)
            {
                tooltips.Add(new(Mod, "GraveDigDirt", GraveDigDirt.Value));
            }
        }
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.LavaCrate)
            {
                itemLoot.Add(ItemDropRule.Common(ItemID.Hellstone, 3, 15, 30));
            }
            if (item.type == ItemID.LavaCrateHard)
            {
                itemLoot.Add(ItemDropRule.Common(ItemID.Hellstone, 2, 20, 35));
            }
            if (item.type == ItemID.ObsidianLockbox)
            {
                itemLoot.Add(ItemDropRule.Common(ItemID.Hellforge, 5));
            }
            if (item.type == ItemID.OasisCrate || item.type == ItemID.OasisCrateHard)
            {
                itemLoot.Add(ItemDropRule.Common(ItemID.FlyingCarpet, 35));
            }
            if (item.type == ItemID.LockBox)
            {
                itemLoot.Add(ItemDropRule.Common(ItemID.WaterBolt, 10));
            }
            if (item.type == ItemID.DungeonFishingCrateHard)
            {
                itemLoot.Add(ItemDropRule.OneFromOptions(3,
                ItemID.ZombieArmStatue,
                ItemID.BatStatue,
                ItemID.BloodZombieStatue,
                ItemID.BoneSkeletonStatue,
                ItemID.ChestStatue,
                ItemID.CorruptStatue,
                ItemID.CrabStatue,
                ItemID.DripplerStatue,
                ItemID.EyeballStatue,
                ItemID.GoblinStatue,
                ItemID.GraniteGolemStatue,
                ItemID.HarpyStatue,
                ItemID.HopliteStatue,
                ItemID.HornetStatue,
                ItemID.ImpStatue,
                ItemID.JellyfishStatue,
                ItemID.MedusaStatue,
                ItemID.PigronStatue,
                ItemID.PiranhaStatue,
                ItemID.SharkStatue,
                ItemID.SkeletonStatue,
                ItemID.SlimeStatue,
                ItemID.UndeadVikingStatue,
                ItemID.UnicornStatue,
                ItemID.WallCreeperStatue,
                ItemID.WraithStatue,
                ItemID.BombStatue,
                ItemID.HeartStatue,
                ItemID.StarStatue,
                ItemID.MushroomStatue,
                ItemID.AngelStatue));
            }

        }
        public override void ExtractinatorUse(int extractType, int extractinatorBlockType, ref int resultType, ref int resultStack)
        {
            // If the extractinator type is stone, set stack to 1 and return sand 10% of the time.
            if (extractType == ItemID.StoneBlock)
            {
                resultStack = 1;
                resultType = Main.rand.NextBool(10) ? ItemID.SandBlock : ItemID.None;
            }
        }

        public override bool? UseItem(Item item, Player player)
        {
            //remove 200 health check from Snow Globe for Frost Legion.  See NPCs.NPCEdits.cs
            //I check for invasion size rather than the presence of an invasion because the server doesn't inform the client about invasion type very often, which caused multiplayer clients to not consume snowglobes, but the invasion still starts on the server.
            if (item.type == ItemID.SnowGlobe && player.ConsumedLifeCrystals < 5 && Main.invasionSize <= 0)
            {
                //The Frost Legion has arrived!
                SoundEngine.PlaySound(SoundID.Roar, player.position);
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    Main.NewText(Lang.misc[7].ToString(), 175, 75);
                }
                else if (Main.netMode == NetmodeID.Server)
                {
                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[7].Key), new Color(175, 75, 255));
                }
                //Remade/butchered the StartInvasion code from Main.cs here to start manually because the health check is everywhere.
                //Main.StartInvasion(InvasionID.SnowLegion);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int playerCount = 0;
                    for (int index = 0; index < (int)byte.MaxValue; ++index)
                    {
                        if (Main.player[index].active)
                            ++playerCount;
                    }
                    Main.invasionType = InvasionID.SnowLegion;
                    Main.invasionDelay = 0;
                    Main.invasionSize = 80 + 40 * playerCount;
                    Main.invasionSizeStart = Main.invasionSize;
                    Main.invasionProgress = 0;
                    Main.invasionProgressWave = 0;
                    Main.invasionProgressMax = Main.invasionSizeStart;
                    Main.invasionProgressIcon = InvasionID.SnowLegion + 3;
                    Main.invasionWarn = 0;
                    Main.invasionX = Main.spawnTileX - 1;
                }
                return true;
            }
            else return null;
        }
    }
}