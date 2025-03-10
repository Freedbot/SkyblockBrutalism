using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;
using SkyblockBrutalism.Items;
using Terraria.ModLoader.Config;
using System.ComponentModel;

namespace SkyblockBrutalism
{
	public class Config : ModConfig
	{
        // ConfigScope.ClientSide should be used for client side, usually visual or audio tweaks.
        // ConfigScope.ServerSide should be used for basically everything else, including disabling items or changing NPC behaviors
        public override ConfigScope Mode => ConfigScope.ServerSide;

        // The things in brackets are known as "Attributes".

        //[Header("Items")] // Headers are like titles in a config. You only need to declare a header on the item it should appear over, not every item in the category. 
                          // [Label("$Some.Key")] // A label is the text displayed next to the option. This should usually be a short description of what it does. By default all ModConfig fields and properties have an automatic label translation key, but modders can specify a specific translation key.
                          // [Tooltip("$Some.Key")] // A tooltip is a description showed when you hover your mouse over the option. It can be used as a more in-depth explanation of the option. Like with Label, a specific key can be provided.

        [ReloadRequired] // Marking it with [ReloadRequired] makes tModLoader force a mod reload if the option is changed. It should be used for things like item toggles, which only take effect during mod loading
        public bool EarlyFurniture;

        [ReloadRequired]
        public bool RestrictedMode;

        [ReloadRequired]
        [DefaultValue(true)]
        public bool LifelessVoodooDoll;

        [ReloadRequired]
        [DefaultValue(true)]
        public bool SkyblockWorldGen;

        [ReloadRequired]
        [DefaultValue(true)]
        public bool SkyGuideStart;
    }
}