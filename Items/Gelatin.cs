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
	public class Gelatin : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 20;
            ItemID.Sets.IsFood[Type] = true;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));
            ItemID.Sets.FoodParticleColors[Item.type] = new Color[3] {
                new Color(42, 107, 242),
                new Color(31, 78, 171),
                new Color(120, 120, 120)
            };
        }
        public override void SetDefaults()
        {
            //Made to slurp and grant 10 seconds of food buff.
            Item.DefaultToFood(22, 22, BuffID.WellFed, 600, true);
            Item.value = 20;
		}
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Gel, 20)
                .Register();
        }
    }
}