using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
	public class UltraSunscreen : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ultra Sunscreen");
			Tooltip.SetDefault("Has an SPF of 247!");
		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.HoldingUp;
            item.value = Item.buyPrice(0, 0, 10, 0);
			item.rare = ItemRarityID.Green;
			item.buffTime = 3 * 60 * 60;
			item.buffType = mod.BuffType("UltraSunscreenBuff");
        }

        public override void AddRecipes()
        {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("Sunscreen"), 3);
			recipe.AddIngredient(ItemID.Sunflower);
			recipe.AddIngredient(ItemID.PlatinumBar);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 3);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("Sunscreen"), 3);
			recipe.AddIngredient(ItemID.Sunflower);
			recipe.AddIngredient(ItemID.GoldBar);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 3);
			recipe.AddRecipe();
		}
    }
}