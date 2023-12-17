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
	public class StormCloud : ModItem
	{
        public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.ManaCrystal);
            Item.value = 0;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = null;
		}
        public override bool? UseItem(Player player)
        {

            if (!Main.IsItStorming && !Main.IsItRaining)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    float wind = Main.rand.Next(650, 800) * (Main.rand.NextBool(2) ? -1 : 1);

                    Main.cloudAlpha = (float)Main.rand.Next(65,80) * 0.01f;
                    Main.windSpeedTarget = wind * 0.001f;
                    Main.ResetWindCounter();
                    Main.StartRain();
                    Main.SyncRain();
                }
                if (player.whoAmI == Main.myPlayer)
                {
                    SoundEngine.PlaySound(SoundID.Thunder, player.position);
                }
                return true;
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.RainCloud, 10)
                .Register();
        }
    }
}