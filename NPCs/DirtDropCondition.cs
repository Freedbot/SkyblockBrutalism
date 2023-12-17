using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;

namespace SkyblockBrutalism.NPCs
{
    internal class DirtDropCondition : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        private static LocalizedText Description;

        public DirtDropCondition()
        {
            Description ??= Language.GetOrRegister("Mods.SkyblockBrutalism.DropConditions.Dirt");
        }
        public bool CanDrop(DropAttemptInfo info)
        {
            return info.player.HeldItem.type == ItemID.GravediggerShovel;
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return Description.Value;
        }
    }
}
