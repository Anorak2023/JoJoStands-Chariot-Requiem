using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles.PlayerStands.ChariotRequiemStandT5;
using JoJoStands.Projectiles.PlayerStands.GoldExperienceRequiem;
using JoJoStands.Tiles;
using JoJoStands.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class ChariotRequiem : StandItemClass
    {
        public override int StandSpeed => 7;
        public override int StandType => 1;
        public override string StandProjectileName => "ChariotRequiemStandT5";
        public override int StandTier => 5;
        public override Color StandTierDisplayColor => Color.Purple;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Chariot Requiem");
            // Tooltip.SetDefault("Left-click to punch enemies and right-click to parry enemies and projectiles away!\nSpecial: Control Chariot Requiem at a distance!\nUsed in Stand Slot;
        }

        public override void SetDefaults()
        {
            Item.damage = 57;
            Item.width = 32;
            Item.height = 32;
            Item.noUseGraphic = true;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            Projectile.NewProjectile(player.GetSource_FromThis(), player.position, player.velocity, ModContent.ProjectileType<ChariotRequiemStandT5>(), 0, 0f, Main.myPlayer);
 
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SilverChariotFinal>())
                .AddIngredient(ModContent.ItemType<RequiemArrow>())
                .AddIngredient(ModContent.ItemType<RighteousLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
