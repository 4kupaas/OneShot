namespace ElKatarinaDecentralized.Components.Spells
{
    using System;
    using System.Linq;

    using ElKatarinaDecentralized.Enumerations;
    using ElKatarinaDecentralized.Utils;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;
    using Color = System.Drawing.Color;

    /// <summary>
    ///     The spell E.
    /// </summary>
    internal class SpellE : ISpell
    {
        #region Properties

        /// <summary>
        ///     Gets the targeted mode.
        /// </summary>
        internal override bool Targeted => true;

        /// <summary>
        ///     Gets or sets the damage type.
        /// </summary>
        internal override TargetSelector.DamageType DamageType => TargetSelector.DamageType.Magical;

        /// <summary>
        ///     Gets the range.
        /// </summary>
        internal override float Range => 725f;

        /// <summary>
        ///     Gets the spell slot.
        /// </summary>
        internal override SpellSlot SpellSlot => SpellSlot.E;

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

                    if (ObjectManager.Player.IsChannelingImportantSpell())
                    {
                        return;
                    }

                    if (DaggerManager.ExistingDaggers.Any())
                    {
                        var dagger =
                            DaggerManager.ExistingDaggers
                                .FirstOrDefault(
                                    d =>
                                        d.DaggerPos.Distance(target.ServerPosition)
                                        <= d.Object.BoundingRadius + 175 && d.Object.IsValid);

                        if (dagger != null)
                        {
                            this.SpellObject.Cast(dagger.DaggerPos);
                            return;
                        }
                    }

                    if (!Misc.SpellQ.SpellSlot.IsReady() && Misc.SpellQ.SpellObject.LastCastedDelay(1000))
                    {
                        this.SpellObject.Cast(target);
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