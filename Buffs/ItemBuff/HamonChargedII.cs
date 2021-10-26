using JoJoStands.Items.Hamon;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class HamonChargedII : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Hamon Charged II");
            Description.SetDefault("The large amount of Hamon in you is increasing your physical abilities.");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            HamonPlayer hPlayer = player.GetModPlayer<HamonPlayer>();
            hPlayer.hamonDamageBoosts += 0.07f;
            player.moveSpeed += 0.07f;
            player.jumpSpeedBoost += 0.07f;
            player.allDamage += 0.07f;
            player.meleeSpeed += 0.07f;
            player.pickSpeed += 0.07f;

            if (Main.rand.Next(0, 4 + 1) == 0)
            {
                int dustIndex = Dust.NewDust(player.position, player.width, player.height, 169, Scale: Main.rand.NextFloat(1f, 2f + 1f));
                Main.dust[dustIndex].noGravity = true;
            }
        }
    }
}