﻿using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class DiosScarf : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dio's Scarf");
            Tooltip.SetDefault("A scarf that's been through many fights, betrayals, and murders...\nGrants 15% Damage Resistance to undead enemies.");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 30;
            item.maxStack = 1;
            item.value = Item.buyPrice(gold: 2, silver: 25);
            item.rare = ItemRarityID.LightPurple;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<VampirePlayer>().wearingDiosScarf = true;
        }
    }
}