using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace SkyblockBrutalism.Items
{
	public class SlimyCloud : ModItem
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
            if (!Main.slimeRain)
            {
                Main.slimeRainTime = (double)Main.rand.Next(32400, 54000);
                Main.slimeRain = true;
                Main.slimeRainKillCount = 0;
                if (Main.netMode != NetmodeID.Server)
                {
                    SkyManager.Instance.Activate("Slime", default(Vector2), new object[0]);
                    SoundEngine.PlaySound(SoundID.Splash, player.position);
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Main.slimeWarningTime = Main.slimeWarningDelay;
                }
                else
                {
                    NetMessage.SendData(MessageID.WorldData);
                }
                return true;
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StormCloud>())
                .AddIngredient(ItemID.PinkGel, 10)
                .Register();
        }
    }
}