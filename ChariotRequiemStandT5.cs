using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;
using JoJoStands.Networking;
using System;
using JoJoStands.NPCs;

namespace JoJoStands.Projectiles.PlayerStands.ChariotRequiemStandT5
{
    public class ChariotRequiemStandT5 : StandClass
    {
        public override float MaxDistance => 9999999999f;
        public override int PunchDamage => 57;
        public override int PunchTime => 7;
        public override int HalfStandHeight => 37;
        public override int FistWhoAmI => 10;
        public override int TierNumber => 5;
        public override int AmountOfPunchVariants => 3;
        public override string PunchTexturePath => "JoJoStands/Projectiles/PlayerStands/ChariotRequiem/ChariotRequiemStandT5_Stab_";
        public override Vector2 PunchSize => new Vector2(20, 10);
        public override PunchSpawnData PunchData => new PunchSpawnData()
        {
            standardPunchOffset = new Vector2(6f, 0f),
            minimumLifeTime = 6,
            maximumLifeTime = 12,
            minimumTravelDistance = 20,
            maximumTravelDistance = 48,
            bonusAfterimageAmount = 0
        };
        public override StandAttackType StandType => StandAttackType.Melee;
        public new AnimationState currentAnimationState;
        public new AnimationState oldAnimationState;
        private const int AfterImagesLimit = 5;
        private const float RemoteControlMaxDistance = 60000f * 16000f;

        private bool parryFrames = false;
        private bool remoteControlled = false;
        private bool shirtless = false;
        private bool remoteMode = false;
        private float punchMovementSpeed = 5f;
        private float floatTimer = 0;

        public new enum AnimationState
        {
            Idle,
            Attack,
            Secondary,
            Parry,
            Pose
        }

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            UpdateStandSync();
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
                {
                    if (Projectile.owner == Main.myPlayer)
                    {
                        if (Main.mouseLeft)
                        {
                            currentAnimationState = AnimationState.Attack;
                            Punch();
                        }
                        else
                        {
                            attacking = false;
                            currentAnimationState = AnimationState.Idle;
                        }
                    }
                        if (!attacking)
                    {
                        if (!secondaryAbility)
                            StayBehind();
                        else
                            GoInFront();
                    }
                    if (SecondSpecialKeyPressed(false) && shootCount <= 0)
                    {
                        shootCount += 30;
                        remoteControlled = true;
                        mPlayer.standControlStyle = MyPlayer.StandControlStyle.Remote;
                    }
                }
            }

            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Remote)
            {
                 float halfScreenWidth = (float)Main.screenWidth / 2f;
                float halfScreenHeight = (float)Main.screenHeight / 2f;
                mPlayer.standRemoteModeCameraPosition = Projectile.Center - new Vector2(halfScreenWidth, halfScreenHeight);
                if (mouseX > Projectile.Center.X)
                    Projectile.direction = 1;
                else
                    Projectile.direction = -1;
                Projectile.spriteDirection = Projectile.direction;
                floatTimer += 0.06f;
                currentAnimationState = AnimationState.Idle;

                bool aboveTile = Collision.SolidTiles((int)Projectile.Center.X / 16, (int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, (int)(Projectile.Center.Y / 16) + 4);
                if (aboveTile)
                {
                    Projectile.velocity.Y = (float)Math.Sin(floatTimer) / 5f;
                }
                else
                {
                    if (Projectile.velocity.Y < 6f)
                        Projectile.velocity.Y += 0.2f;
                    if (Vector2.Distance(Projectile.Center, player.Center) >= RemoteControlMaxDistance)
                    {
                        Projectile.velocity = player.Center - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= 0.8f;
                    }
                }

                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseRight)
                    {
                        Vector2 moveVelocity = Main.MouseWorld - Projectile.Center;
                        moveVelocity.Normalize();
                        Projectile.velocity.X = moveVelocity.X * 5f;
                        if (aboveTile)
                            Projectile.velocity.Y += moveVelocity.Y * 2f;

                        if (Vector2.Distance(Projectile.Center, player.Center) >= RemoteControlMaxDistance)
                        {
                            Projectile.velocity = player.Center - Projectile.Center;
                            Projectile.velocity.Normalize();
                            Projectile.velocity *= 0.8f;
                        }
                        Projectile.netUpdate = true;
                    }
                    else
                    {
                        Projectile.velocity.X *= 0.78f;
                        Projectile.netUpdate = true;
                    }

                    if (Main.mouseLeft)
                    {
                        currentAnimationState = AnimationState.Attack;
                        Punch(punchMovementSpeed);
                    }
                    else
                    {
                        attacking = false;
                        currentAnimationState = AnimationState.Idle;
                    }
                }
                if (SecondSpecialKeyPressed(false) && shootCount <= 0)
                {
                    shootCount += 30;
                    remoteControlled = false;
                    mPlayer.standControlStyle = MyPlayer.StandControlStyle.Manual;
                }
            }

            if (remoteMode)
            {
                player.aggro -= 1200;
                float halfScreenWidth = (float)Main.screenWidth / 2f;
                float halfScreenHeight = (float)Main.screenHeight / 2f;
                mPlayer.standRemoteModeCameraPosition = Projectile.Center - new Vector2(halfScreenWidth, halfScreenHeight);
            }

            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                BasicPunchAI();
                if (!attacking)
                    currentAnimationState = AnimationState.Idle;
                else
                    currentAnimationState = AnimationState.Attack;
            }

            if (parryFrames)
                currentAnimationState = AnimationState.Parry;
            if (mPlayer.posing)
                currentAnimationState = AnimationState.Pose;
        }

        public override byte SendAnimationState() => (byte)currentAnimationState;
        public override void ReceiveAnimationState(byte state) => currentAnimationState = (AnimationState)state;

        public override void SelectAnimation()
        {
            if (oldAnimationState != currentAnimationState)
            {
                Projectile.frame = 0;
                Projectile.frameCounter = 0;
                oldAnimationState = currentAnimationState;
                Projectile.netUpdate = true;
            }

            if (currentAnimationState == AnimationState.Idle)
                PlayAnimation("Idle");
            else if (currentAnimationState == AnimationState.Attack)
                PlayAnimation("Attack");
            else if (currentAnimationState == AnimationState.Secondary)
                PlayAnimation("Secondary");
            else if (currentAnimationState == AnimationState.Parry)
                PlayAnimation("Parry");
            else if (currentAnimationState == AnimationState.Pose)
                PlayAnimation("Pose");
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/ChariotRequiem/ChariotRequiemStandT5_" + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 4, 30, true);
            else if (animationName == "Attack")
                AnimateStand(animationName, 5, newPunchTime / 2, true);
            else if (animationName == "Secondary")
                AnimateStand(animationName, 1, 1, true);
            else if (animationName == "Parry")
                AnimateStand(animationName, 6, 3, false);
            else if (animationName == "Pose")
                AnimateStand(animationName, 1, 10, true);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            parryFrames = reader.ReadBoolean();
        }
    }
}