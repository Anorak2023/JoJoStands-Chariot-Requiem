using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class MetallicNunchucksProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 6;
            projectile.aiStyle = 0;
            projectile.timeLeft = 600;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
        }

        private float rotation = 0f;
        private float swingCone = 90f;      //This is the swing area
        private int playerStartDirection = 1;
        private bool setRotation = false;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (Main.player[projectile.owner].dead)
            {
                projectile.Kill();
                return;
            }

            Vector2 rota = player.Center - projectile.Center;
            projectile.rotation = (-rota).ToRotation();

            if (!setRotation)
            {
                playerStartDirection = player.direction;
                if (playerStartDirection == 1)
                {
                    rotation = 360f - swingCone;
                }
                else
                {
                    rotation = 180f + swingCone;
                }
                setRotation = true;
            }

            player.direction = playerStartDirection;
            rotation += 24f * player.direction;
            if (playerStartDirection == 1 && rotation >= 360f + swingCone)
            {
                projectile.Kill();
            }
            if (playerStartDirection == -1 && rotation <= 180f - swingCone)
            {
                projectile.Kill();
            }

            projectile.position = player.Center + (MathHelper.ToRadians(rotation).ToRotationVector2() * 32f);
            projectile.velocity = Vector2.Zero;

            int dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, 169);
            Main.dust[dustIndex].noGravity = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(0, 2) == 0)
            {
                target.AddBuff(mod.BuffType("Sunburn"), 12 * 60);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        private Texture2D chainTexture;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[projectile.owner];

            if (Main.netMode != NetmodeID.Server && chainTexture == null)
                chainTexture = mod.GetTexture("Projectiles/ChainedClaw_Chain");

            Vector2 linkCenter = player.Center;
            Vector2 center = projectile.Center;
            float rotation = (linkCenter - center).ToRotation();

            for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(center, linkCenter) / chainTexture.Width))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
            {
                Vector2 pos = Vector2.Lerp(center, linkCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                spriteBatch.Draw(chainTexture, pos, new Rectangle(0, 0, chainTexture.Width, chainTexture.Height), lightColor, rotation, new Vector2(chainTexture.Width * 0.5f, chainTexture.Height * 0.5f), projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}