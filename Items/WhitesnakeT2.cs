﻿using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace JoJoStands.Items
{
    public class WhitesnakeT2 : ModItem
    {
        public override string Texture
        {
            get { return mod.Name + "/Items/WhitesnakeT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Whitesnake (Tier 2)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right click to throw some acid!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 38;
            item.width = 32;
            item.height = 32;
            item.useTime = 12;
            item.useAnimation = 12;
            item.useStyle = 5;
            item.maxStack = 1;
            item.knockBack = 2f;
            item.value = 0;
            item.noUseGraphic = true;
            item.rare = 6;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            MyPlayer mPlayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
            TooltipLine tooltipAddition = new TooltipLine(mod, "Speed", "Punch Speed: " + (13 - mPlayer.standSpeedBoosts));
            tooltips.Add(tooltipAddition);
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            mult *= player.GetModPlayer<MyPlayer>().standDamageBoosts;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DemoniteBar, 9);
            recipe.AddIngredient(ItemID.Diamond);
            recipe.AddIngredient(ItemID.RottenChunk, 5);
            recipe.AddIngredient(mod.ItemType("WillToControl"));
            recipe.AddIngredient(mod.ItemType("WillToChange"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CrimtaneBar, 9);
            recipe.AddIngredient(ItemID.Diamond);
            recipe.AddIngredient(ItemID.Vertebrae, 5);
            recipe.AddIngredient(mod.ItemType("WillToControl"));
            recipe.AddIngredient(mod.ItemType("WillToChange"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}