using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SkyblockBrutalism.Items
{
	public class FireResistantBugNet : ModItem
	{
        public override void SetStaticDefaults()
        {
            ItemID.Sets.CatchingTool[Type] = true;
            ItemID.Sets.LavaproofCatchingTool[Type] = true;
            Item.ResearchUnlockCount = 10;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.FireproofBugNet);
            Item.value = 14000;
		}
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BugNet)
                .AddIngredient(ItemID.Fireblossom, 5)
                .AddIngredient(ItemID.Ruby)
                .AddTile(TileID.Furnaces)
                .Register();
        }
        public override bool? CanCatchNPC(NPC target, Player player)
        {
            if (player.HeldItem.type == ModContent.ItemType<FireResistantBugNet>() && (target.type == NPCID.HellButterfly || target.type == NPCID.Lavafly || target.type == NPCID.MagmaSnail) && Main.rand.NextBool(7))
            {
                WorldGen.PlaceLiquid(player.position.ToTileCoordinates().X, player.position.ToTileCoordinates().Y, (byte)LiquidID.Lava, 255);
                player.HeldItem.type = ItemID.BugNet;
            }
            return null;
        }
    }
}