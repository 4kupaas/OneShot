namespace ElLeeSinDecentralized.Components.Spells
{
    using System;
    using System.Linq;

    using ElLeeSinDecentralized.Enumerations;
    using ElLeeSinDecentralized.Utils;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    /// <summary>
    ///     The spell E.
    /// </summary>
    internal class SpellE : ISpell
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
        internal override float Range => 450f;

        /// <summary>
        ///     Gets the targeted.
        /// </summary>
        internal override bool Targeted => true;

        /// <summary>
        ///     Gets the speed.
        /// </summary>
        internal override float Speed => 1400f;

        /// <summary>
        ///     Gets the spell slot.
        /// </summary>
        internal override SpellSlot SpellSlot => SpellSlot.E;

        /// <summary>
        ///     Gets the width.
        /// </summary>
        internal override float Width => 125f;

        #endregion

        #region Methods

        /// <summary>
        ///     The on combo callback.
        /// </summary>
        internal override void OnCombo()
        {
            try
            {
                if (this.SpellObject == null)
                {
                    return;
                }

                var target = Misc.GetTarget(this.Range, this.DamageType);
                if (target != null)
                {
                    if (ObjectManager.Player.IsDashing())
                    {
                        return;
                    }

                    if (Misc.IsEOne)
                    {
                        var enemiesCount =
                            HeroManager.Enemies.Where(
                                h =>
                                    h.IsValid && !h.IsDead && h.IsVisible
                                    && h.Distance(ObjectManager.Player) < this.Range -50).ToList();

                        if (enemiesCount.Count == 0)
                        {
                            return;
                        }

                        if (PassiveManager.FlurryStacks == 0 && ObjectManager.Player.Mana >= 75 || enemiesCount.Count >= 2 
                            || enemiesCount.Any(t => t.Distance(ObjectManager.Player) > Orbwalking.GetRealAutoAttackRange(ObjectManager.Player)))
                        {
                            this.SpellObject.Cast();
                            PassiveManager.lastSpellCastTime = Environment.TickCount;
                        }
                    }
                    else
                    {
                        var enemiesCount =
                            HeroManager.Enemies.Where(
                                h =>
                                    h.IsValid && !h.IsDead && h.IsVisible && Misc.HasBlindMonkTempest(h)
                                    && h.Distance(ObjectManager.Player) < 500f).ToList();

                        if (Environment.TickCount - PassiveManager.lastSpellCastTime <= 500)
                        {
                            return;
                        }

                        if (enemiesCount.Count == 0)
                        {
                            Logging.AddEntry(LoggingEntryTrype.Debug, "@SpellE.cs: Returning");
                            return;
                        }

                        if ((PassiveManager.FlurryStacks == 0 || enemiesCount.Count >= 2) 
                            || enemiesCount.Any(t => t.Distance(ObjectManager.Player) > Orbwalking.GetRealAutoAttackRange(ObjectManager.Player) + 60))
                        {
                            this.SpellObject.Cast();
                            Logging.AddEntry(LoggingEntryTrype.Debug, "@SpellE.cs: Casted second E");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryTrype.Error, "@SpellE.cs: Can not run OnCombo - {0}", e);
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
        }

        /// <summary>
        ///     The on lane clear callback.
        /// </summary>
        internal override void OnLaneClear()
        {
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