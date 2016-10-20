namespace ElLeeSinDecentralized.Components.Spells
{
    using System;
    using System.Linq;

    using ElLeeSinDecentralized.Enumerations;
    using ElLeeSinDecentralized.Utils;

    using LeagueSharp;
    using LeagueSharp.Common;

    using System.Drawing;

    using ElLeeSinDecentralized.Components.SpellManagers;

    /// <summary>
    ///     The spell Q.
    /// </summary>
    internal class SpellQ : ISpell
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the damage type.
        /// </summary>
        internal override TargetSelector.DamageType DamageType => TargetSelector.DamageType.Physical;

        /// <summary>
        ///     Gets the delay.
        /// </summary>
        internal override float Delay => 250f;

        /// <summary>
        ///     Gets the range.
        /// </summary>
        internal override float Range => 1100f;

        /// <summary>
        ///     Gets the collision.
        /// </summary>
        internal override bool Collision => true; // todo : test without

        /// <summary>
        ///     Gets or sets the skillshot type.
        /// </summary>
        internal override SkillshotType SkillshotType => SkillshotType.SkillshotLine;

        /// <summary>
        ///     Gets the speed.
        /// </summary>
        internal override float Speed => 1800f;

        /// <summary>
        ///     Gets the spell slot.
        /// </summary>
        internal override SpellSlot SpellSlot => SpellSlot.Q;

        /// <summary>
        ///     Gets the width.
        /// </summary>
        internal override float Width => 65f;

        /// <summary>
        ///     Gets the BlindMonkTwoRange
        /// </summary>
        internal const float BlindMonkTwoRange = 1300f;

        #endregion

        #region Methods

        /// <summary>
        ///     The on combo callback.
        /// </summary>
        internal override void OnCombo()
        {
            try
            {
                var target = Misc.GetTarget(this.Range + this.Width, this.DamageType);
                if (target != null)
                {
                    if (Misc.IsQOne)
                    {
                        /*var prediction = Prediction.GetPrediction(
                            target,
                            this.Delay,
                            this.Width,
                            this.Speed,
                            new CollisionableObjects[] { CollisionableObjects.YasuoWall, CollisionableObjects.Minions });*/

                        var prediction = this.SpellObject.GetPrediction(target);
                        // todo: test this.
                        Logging.AddEntry(LoggingEntryTrype.Debug, "Prediction: {0}", prediction.Hitchance);
                        if (prediction.Hitchance >= HitChance.High)
                        {
                            this.SpellObject.Cast(target);
                        }
                    }
                    else
                    {
                        if (MyMenu.RootMenu.Item("comboq2use").IsActive())
                        {
                            if (ObjectManager.Player.IsDashing() || !Misc.HasBlindMonkQOne(target) || (Misc.HasBlindMonkQOne(target) && target.Distance(ObjectManager.Player) > BlindMonkTwoRange))
                            {
                                return;
                            }

                            if (Misc.CanCastQ2 || this.SpellObject.GetDamage(target, 1) > target.Health + target.PhysicalShield 
                                || ObjectManager.Player.Distance(target) > Orbwalking.GetRealAutoAttackRange(ObjectManager.Player) || PassiveManager.FlurryStacks == 0)
                            {
                                this.SpellObject.Cast();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryTrype.Error, "@SpellQ.cs: Can not run OnCombo - {0}", e);
                throw;
            }
        }

        /// <summary>
        ///     The on mixed callback.
        /// </summary>
        internal override void OnMixed()
        {
            this.OnCombo();
        }

        /// <summary>
        ///     The on last hit callback.
        /// </summary>
        internal override void OnLastHit()
        {
            var minion =
                MinionManager.GetMinions(this.Range)
                    .Where(obj => this.SpellObject.IsKillable(obj))
                    .MinOrDefault(obj => obj.Health);

            if (minion != null)
            {
                this.SpellObject.Cast(minion);
            }
        }

        /// <summary>
        ///     The on lane clear callback.
        /// </summary>
        internal override void OnLaneClear()
        {
            var minions =
                MinionManager.GetMinions(
                    ObjectManager.Player.ServerPosition,
                    this.SpellObject.Range + this.SpellObject.Width).OrderBy(x => x.Health).FirstOrDefault();

            if (minions == null)
            {
                return;
            }

            if (Misc.IsQOne)
            {
                if (PassiveManager.FlurryStacks == 2)
                {
                    return;
                }

                this.SpellObject.Cast(minions);
            }
            else
            {
                if (this.SpellObject.GetDamage(minions, 1) > minions.Health && Misc.HasBlindMonkQOne(minions))
                {
                    this.SpellObject.Cast();
                }
            }
        }

        /// <summary>
        ///     The on jungle clear callback.
        /// </summary>
        internal override void OnJungleClear()
        {
        }

        #endregion

    }
}