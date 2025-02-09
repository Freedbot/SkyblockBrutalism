using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SkyblockBrutalism.Items
{
    public class KeyOfMagic : ModItem
    {
        //Used to summon clones of pre-hardmode mimics (like in Remix seed) as a source of gold chest loot, but no money.
        //See also SkyPlayer and MimicClone
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LightKey);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FallenStar, 10)
                .Register();
        }
    }
}