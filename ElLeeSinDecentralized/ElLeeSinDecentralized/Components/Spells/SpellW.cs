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
    ///     The spell W.
    /// </summary>
    internal class SpellW : ISpell
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
        ///     Gets the targeted.
        /// </summary>
        internal override bool Targeted => true;

        /// <summary>
        ///     Gets the range.
        /// </summary>
        internal override float Range => 700f;

        /// <summary>
        ///     Gets the speed.
        /// </summary>
        internal override float Speed => 1000f;

        /// <summary>
        ///     Gets the spell slot.
        /// </summary>
        internal override SpellSlot SpellSlot => SpellSlot.W;

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
                }

                //WardManager.WardjumpHandler(Game.CursorPos, maxrangeJump: true);
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryType.Error, "@SpellW.cs: Can not run OnCombo - {0}", e);
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
        ///     The on update callback.
        /// </summary>
        internal override void OnUpdate()
        {
            if (MyMenu.RootMenu.Item("wardjump.key").GetValue<KeyBind>().Active)
            {
                WardManager.WardjumpHandler(Game.CursorPos);
            }

            if (WardManager.WardJumping)
            {
                if (Utils.TickCount - WardManager.LastWardPlacement < Game.Ping + 100)
                {
                    return;
                }

                if (!this.SpellSlot.IsReady() || !Misc.IsWOne)
                {
                    WardManager.WardJumping = false;
                    return;
                }
            }
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