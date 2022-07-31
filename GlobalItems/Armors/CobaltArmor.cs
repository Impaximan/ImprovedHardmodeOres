using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace ImprovedHardmodeOres.GlobalItems.Armors
{
    class CobaltArmor : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            List<int> validHelmets = new List<int>()
            {
                { ItemID.CobaltHelmet },
                { ItemID.CobaltHat },
                { ItemID.CobaltMask }
            };
            if (validHelmets.Contains(head.type) && body.type == ItemID.CobaltBreastplate && legs.type == ItemID.CobaltLeggings)
            {
                return "Cobalt";
            }
            return base.IsArmorSet(head, body, legs);
        }

        public override void UpdateArmorSet(Player player, string set)
        {
            if (set == "Cobalt")
            {
                player.setBonus = player.setBonus + "\nHitting enemies chains lightning back to yourself, damaging all hostile entities in its path";
                player.GetModPlayer<CobaltPlayer>().CobaltSetBonus = true;
            }
            base.UpdateArmorSet(player, set);
        }
    }

    class CobaltPlayer : ModPlayer
    {
        public bool CobaltSetBonus = false;
        public override void ResetEffects()
        {
            CobaltSetBonus = false;
        }
    }

    class CobaltProjectile : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            if (!target.friendly && projectile.friendly && !projectile.hostile && projectile.owner != 255 && Main.player[projectile.owner].GetModPlayer<CobaltPlayer>().CobaltSetBonus && projectile.type != ModContent.ProjectileType<CobaltLightning>())
            {
                if (Main.rand.NextFloat() <= 0.25f)
                {
                    Projectile.NewProjectile(projectile.GetSource_OnHit(target), target.Center, Vector2.Zero, ModContent.ProjectileType<CobaltLightning>(), projectile.damage / 2, 0.25f, projectile.owner);
                }
            }
        }
    }

    class CobaltLightning : ModProjectile
    {
        public override string Texture => "ImprovedHardmodeOres/nothing";

        public override void SetDefaults()
        {
            Projectile.width = 15;
            Projectile.height = 15;
            Projectile.friendly = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.DamageType = DamageClass.Default;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 50;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
        }

        Vector2 currentTargetPosition;

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
        }
        public override void AI()
        {
            Dust dust = Dust.NewDustPerfect(Projectile.Center, 20);
            dust.velocity = Vector2.Zero;
            dust.scale = 1f;
            dust.noGravity = true;
        }
    }
}
