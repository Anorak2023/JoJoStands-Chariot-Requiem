using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands
{
    public class StickyFingersStandFinal : ModProjectile
    {
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/PlayerStands/StickyFingersStandT1"; }
        }

        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 10;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 38;
            projectile.height = 1;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.netImportant = true;
            projectile.minionSlots = 1;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            MyPlayer.stopimmune.Add(mod.ProjectileType(Name));
        }

        public Vector2 velocityAddition = Vector2.Zero;
        public float mouseDistance = 0f;
        protected float shootSpeed = 16f;
        public bool normalFrames = false;
        public bool attackFrames = false;
        public float maxDistance = 0f;
        public int punchDamage = 76;
        public int altDamage = 67;
        public int shootCount = 0;
        public int punchTime = 9;
        public int halfStandHeight = 39;
        public bool zipperPunch = false;
        public float fistWhoAmI = 4f;
        public float tierNumber = 4f;
        public int updateTimer = 0;

        public override void AI()
        {
            SelectFrame();
            if (shootCount > 0)
            {
                shootCount--;
            }
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            if (modPlayer.StandOut)
            {
                projectile.timeLeft = 2;
            }
            if (projectile.spriteDirection == 1)
            {
                drawOffsetX = -10;
            }
            if (projectile.spriteDirection == -1)
            {
                drawOffsetX = -60;
            }
            drawOriginOffsetY = -halfStandHeight;
            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }

            if (!modPlayer.StandAutoMode)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer && player.ownedProjectileCounts[mod.ProjectileType("StickyFingersFistExtended")] == 0)
                {
                    attackFrames = true;
                    normalFrames = false;
                    Main.mouseRight = false;
                    projectile.netUpdate = true;
                    float rotaY = Main.MouseWorld.Y - projectile.Center.Y;
                    projectile.rotation = MathHelper.ToRadians((rotaY * projectile.spriteDirection) / 6f);
                    if (Main.MouseWorld.X > projectile.position.X)
                    {
                        projectile.spriteDirection = 1;
                        projectile.direction = 1;
                    }
                    if (Main.MouseWorld.X < projectile.position.X)
                    {
                        projectile.spriteDirection = -1;
                        projectile.direction = -1;
                    }
                    if (projectile.position.X < Main.MouseWorld.X - 5f)
                    {
                        velocityAddition.X = 5f;
                    }
                    if (projectile.position.X > Main.MouseWorld.X + 5f)
                    {
                        velocityAddition.X = -5f;
                    }
                    if (projectile.position.X > Main.MouseWorld.X - 5f && projectile.position.X < Main.MouseWorld.X + 5f)
                    {
                        velocityAddition.X = 0f;
                    }
                    if (projectile.position.Y > Main.MouseWorld.Y + 5f)
                    {
                        velocityAddition.Y = -5f;
                    }
                    if (projectile.position.Y < Main.MouseWorld.Y - 5f)
                    {
                        velocityAddition.Y = 5f;
                    }
                    if (projectile.position.Y < Main.MouseWorld.Y + 5f && projectile.position.Y > Main.MouseWorld.Y - 5f)
                    {
                        velocityAddition.Y = 0f;
                    }
                    mouseDistance = Vector2.Distance(Main.MouseWorld, projectile.Center);
                    if (mouseDistance > 40f)
                    {
                        projectile.velocity = player.velocity + velocityAddition;
                    }
                    if (mouseDistance <= 40f)
                    {
                        projectile.velocity = Vector2.Zero;
                    }
                    if (shootCount <= 0)
                    {
                        shootCount += punchTime - modPlayer.standSpeedBoosts;
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("Fists"), (int)(punchDamage * modPlayer.standDamageBoosts), 2f, Main.myPlayer, fistWhoAmI, tierNumber);
                        Main.projectile[proj].netUpdate = true;
                    }
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        attackFrames = false;
                        normalFrames = true;
                    }
                }
                if (!attackFrames)
                {
                    Vector2 vector131 = player.Center;
                    if (player.ownedProjectileCounts[mod.ProjectileType("StickyFingersFistExtended")] == 0)
                    {
                        vector131.X -= (float)((12 + player.width / 2) * player.direction);
                    }
                    if (player.ownedProjectileCounts[mod.ProjectileType("StickyFingersFistExtended")] != 0)
                    {
                        vector131.X += (float)((12 + player.width / 2) * player.direction);
                    }
                    vector131.Y -= -35f + halfStandHeight;
                    projectile.Center = Vector2.Lerp(projectile.Center, vector131, 0.2f);
                    projectile.velocity *= 0.8f;
                    projectile.direction = (projectile.spriteDirection = player.direction);
                    projectile.rotation = 0;
                }
                if (Main.mouseRight && shootCount <= 0 && player.ownedProjectileCounts[mod.ProjectileType("StickyFingersFistExtended")] == 0 && projectile.owner == Main.myPlayer)
                {
                    shootCount += 120;
                    Main.mouseLeft = false;
                    Vector2 shootVel = Main.MouseWorld - projectile.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 1f);
                    }
                    shootVel.Normalize();
                    shootVel *= shootSpeed;
                    int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("StickyFingersFistExtended"), (int)(altDamage * modPlayer.standDamageBoosts), 2f, Main.myPlayer, projectile.whoAmI);
                    Main.projectile[proj].netUpdate = true;
                    projectile.netUpdate = true;
                }
                if (player.ownedProjectileCounts[mod.ProjectileType("StickyFingersFistExtended")] != 0)
                {
                    projectile.frame = 8;
                    Main.mouseLeft = false;
                    normalFrames = false;
                    attackFrames = false;
                    projectile.netUpdate = true;
                }
                if (JoJoStands.SpecialHotKey.JustPressed && shootCount <= 0 && player.ownedProjectileCounts[mod.ProjectileType("StickyFingersZipperPoint2")] == 0 && !player.HasBuff(mod.BuffType("AbilityCooldown")) && projectile.owner == Main.myPlayer)
                {
                    shootCount += 20;
                    Vector2 shootVel = Main.MouseWorld - projectile.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 1f);
                    }
                    shootVel.Normalize();
                    shootVel *= shootSpeed * 3;
                    int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("StickyFingersZipperPoint2"), 0, 0f, Main.myPlayer, 1f);
                    Main.projectile[proj].netUpdate = true;
                    projectile.netUpdate = true;
                }
            }
            if (modPlayer.StandAutoMode)
            {
                NPC target = null;
                Vector2 targetPos = projectile.position;
                Vector2 vector131 = player.Center;
                float targetDist = maxDistance * 3f;
                if (target == null)
                {
                    for (int k = 0; k < 200; k++)       //the targeting system
                    {
                        NPC npc = Main.npc[k];
                        if (npc.CanBeChasedBy(this, false))
                        {
                            float distance = Vector2.Distance(npc.Center, player.Center);
                            if (distance < targetDist && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                            {
                                if (npc.boss)       //is gonna try to detect bosses over anything
                                {
                                    targetDist = distance;
                                    targetPos = npc.Center;
                                    target = npc;
                                }
                                else        //if it fails to detect a boss, it'll detect the next best thing
                                {
                                    targetDist = distance;
                                    targetPos = npc.Center;
                                    target = npc;
                                }
                            }
                        }
                    }
                }
                if (targetDist > (maxDistance * 1.5f) || zipperPunch || target == null)
                {
                    normalFrames = true;
                    attackFrames = false;
                    if (zipperPunch)
                    {
                        vector131.X += (float)((12 + player.width / 2) * player.direction);
                    }
                    else
                    {
                        vector131.X -= (float)((12 + player.width / 2) * player.direction);
                        projectile.spriteDirection = projectile.direction = player.direction;
                    }
                    vector131.Y -= -35f + halfStandHeight;
                    projectile.Center = Vector2.Lerp(projectile.Center, vector131, 0.2f);
                    projectile.velocity *= 0.8f;
                    projectile.rotation = 0;
                }
                if (target != null)
                {
                    if (targetDist < (maxDistance * 1.5f))
                    {
                        attackFrames = true;
                        normalFrames = false;
                        if ((targetPos - projectile.Center).X > 0f)
                        {
                            projectile.spriteDirection = projectile.direction = 1;
                        }
                        else if ((targetPos - projectile.Center).X < 0f)
                        {
                            projectile.spriteDirection = projectile.direction = -1;
                        }
                        if (targetPos.X > projectile.position.X)
                        {
                            projectile.velocity.X = 4f;
                        }
                        if (targetPos.X < projectile.position.X)
                        {
                            projectile.velocity.X = -4f;
                        }
                        if (targetPos.Y > projectile.position.Y)
                        {
                            projectile.velocity.Y = 4f;
                        }
                        if (targetPos.Y < projectile.position.Y)
                        {
                            projectile.velocity.Y = -4f;
                        }
                        if (shootCount <= 0)
                        {
                            if (Main.myPlayer == projectile.owner)
                            {
                                shootCount += punchTime - modPlayer.standSpeedBoosts;
                                Vector2 shootVel = targetPos - projectile.Center;
                                if (shootVel == Vector2.Zero)
                                {
                                    shootVel = new Vector2(0f, 1f);
                                }
                                shootVel.Normalize();
                                if (projectile.direction == 1)
                                {
                                    shootVel *= shootSpeed;
                                }
                                int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("Fists"), (int)((punchDamage * modPlayer.standDamageBoosts) * 0.9f), 3f, Main.myPlayer, fistWhoAmI);
                                Main.projectile[proj].netUpdate = true;
                                projectile.netUpdate = true;
                            }
                        }
                    }
                    else if (Main.rand.Next(0, 101) <= 1 && targetDist > (maxDistance * 1.5f))
                    {
                        zipperPunch = true;
                    }
                }
                if (zipperPunch)
                {
                    projectile.frame = 8;
                    zipperPunch = true;
                    attackFrames = false;
                    normalFrames = false;
                    if ((targetPos - projectile.Center).X > 0f)
                    {
                        projectile.spriteDirection = projectile.direction = 1;
                    }
                    else if ((targetPos - projectile.Center).X < 0f)
                    {
                        projectile.spriteDirection = projectile.direction = -1;
                    }
                    if (shootCount <= 0)
                    {
                        if (Main.myPlayer == projectile.owner)
                        {
                            if (shootCount <= 0)
                            {
                                shootCount += 28;
                                Vector2 shootVel = targetPos - projectile.Center - new Vector2(0f, 2f);
                                if (shootVel == Vector2.Zero)
                                {
                                    shootVel = new Vector2(0f, 1f);
                                }
                                shootVel.Normalize();
                                shootVel *= shootSpeed;
                                int proj = Projectile.NewProjectile(projectile.position.X + 5f, projectile.position.Y - 3f, shootVel.X, shootVel.Y, mod.ProjectileType("StickyFingersFistExtended"), (int)((altDamage * modPlayer.standDamageBoosts) * 0.9f), 2f, Main.myPlayer, projectile.whoAmI);
                                Main.projectile[proj].netUpdate = true;
                                projectile.netUpdate = true;
                            }
                        }
                    }
                }
                if (zipperPunch && player.ownedProjectileCounts[mod.ProjectileType("StickyFingersFistExtended")] == 0)
                {
                    zipperPunch = false;
                }
            }

            Vector2 direction = player.Center - projectile.Center;      //needs to be down here so velocity changes get applied when the stand enters the rim
            float distanceTo = direction.Length();
            maxDistance = 98f + modPlayer.standRangeBoosts;
            if (distanceTo > maxDistance)
            {
                if (projectile.position.X <= player.position.X - 15f)
                {
                    ////projectile.position = new Vector2(projectile.position.X + 0.2f, projectile.position.Y);
                    projectile.velocity = player.velocity + new Vector2(0.8f, 0f);
                }
                if (projectile.position.X >= player.position.X + 15f)
                {
                    //projectile.position = new Vector2(projectile.position.X - 0.2f, projectile.position.Y);
                    projectile.velocity = player.velocity + new Vector2(-0.8f, 0f);
                }
                if (projectile.position.Y >= player.position.Y + 15f)
                {
                    // projectile.position = new Vector2(projectile.position.X, projectile.position.Y - 0.2f);
                    projectile.velocity = player.velocity + new Vector2(0f, -0.8f);
                }
                if (projectile.position.Y <= player.position.Y - 15f)
                {
                    ///projectile.position = new Vector2(projectile.position.X, projectile.position.Y + 0.2f);
                    projectile.velocity = player.velocity + new Vector2(0f, 0.8f);
                }
            }
            if (distanceTo >= maxDistance + 22f)
            {
                if (!modPlayer.StandAutoMode)
                {
                    Main.mouseLeft = false;
                    Main.mouseRight = false;
                }
                projectile.Center = player.Center;
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(normalFrames);
            writer.Write(attackFrames);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            normalFrames = reader.ReadBoolean();
            attackFrames = reader.ReadBoolean();
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[projectile.owner];
            if (MyPlayer.RangeIndicators)
            {
                Texture2D texture = mod.GetTexture("Extras/RangeIndicator");        //the initial tile amount the indicator covers is 20 tiles, 320 pixels, border is included in the measurements
                spriteBatch.Draw(texture, player.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.White * (((float)MyPlayer.RangeIndicatorAlpha * 3.9215f) / 1000f), 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), maxDistance / 122.5f, SpriteEffects.None, 0);
            }
        }

        public virtual void SelectFrame()
        {
            Player player = Main.player[projectile.owner];
            projectile.frameCounter++;
            if (attackFrames)
            {
                normalFrames = false;
                if (projectile.frameCounter >= punchTime - player.GetModPlayer<MyPlayer>().standSpeedBoosts)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame <= 3)
                {
                    projectile.frame = 4;
                }
                if (projectile.frame >= 8)
                {
                    projectile.frame = 4;
                }
            }
            if (normalFrames)
            {
                if (projectile.frameCounter >= 30)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 4)
                {
                    projectile.frame = 0;
                }
            }
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                projectile.frame = 9;
            }
        }
    }
}