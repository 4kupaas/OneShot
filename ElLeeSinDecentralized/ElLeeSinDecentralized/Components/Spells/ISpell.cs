﻿namespace ElLeeSinDecentralized.Components.Spells
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;

    using ElLeeSinDecentralized.Enumerations;
    using ElLeeSinDecentralized.Utils;

    using LeagueSharp;
    using LeagueSharp.Common;

    /// <summary>
    ///     Base class for the spells.
    /// </summary>
    internal class ISpell
    {
        #region Properties

        /// <summary>
        ///     Gets or sets a value indicating whether the spell is an AoE spell.
        /// </summary>
        [DefaultValue(false)]
        internal virtual bool Aoe { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the spell has collision.
        /// </summary>
        [DefaultValue(false)]
        internal virtual bool Collision { get; set; }

        /// <summary>
        ///     Gets or sets the damage type.
        /// </summary>
        internal virtual TargetSelector.DamageType DamageType { get; set; }

        /// <summary>
        ///     Gets or sets the delay.
        /// </summary>
        internal virtual float Delay { get; set; }

        /// <summary>
        ///     Gets or sets the range.
        /// </summary>
        internal virtual float Range { get; set; }

        /// <summary>
        ///     Gets or sets the skillshot type.
        /// </summary>
        internal virtual SkillshotType SkillshotType { get; set; }

        /// <summary>
        ///     Gets or sets the speed.
        /// </summary>
        internal virtual float Speed { get; set; }

        /// <summary>
        ///     Gets or sets the spell object.
        /// </summary>
        internal Spell SpellObject
        {
            get
            {
                if (this.spellObject != null)
                {
                    return this.spellObject;
                }

                this.spellObject = new Spell(this.SpellSlot, this.Range, this.DamageType);

                if (this.Targeted)
                {
                    this.spellObject.SetTargetted(this.Delay, this.Speed);
                }
                else
                {
                    this.spellObject.SetSkillshot(
                        this.Delay,
                        this.Width,
                        this.Speed,
                        this.Collision,
                        this.SkillshotType);

                    Logging.AddEntry(
                        LoggingEntryType.Debug,
                        "Delay: {0} - Width: {1} - Speed: {2} - Collision: {3} - SkillshotType : {4} - Slot: {5}",
                        this.Delay,
                        this.Width,
                        this.Speed,
                        this.Collision,
                        this.SkillshotType,
                        this.SpellSlot);
                }

                return this.spellObject;
            }
        }

        /// <summary>
        ///     The internal spell object.
        /// </summary>
        private Spell spellObject;

        /// <summary>
        ///     Gets or sets the spell slot.
        /// </summary>
        internal virtual SpellSlot SpellSlot { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the spell is targeted.
        /// </summary>
        [DefaultValue(false)]
        internal virtual bool Targeted { get; set; }

        /// <summary>
        ///     Gets or sets the width.
        /// </summary>
        internal virtual float Width { get; set; }


        #endregion

        #region Methods

        /// <summary>
        ///     The on on combo callback.
        /// </summary>
        internal virtual void OnCombo()
        {
        }

        /// <summary>
        /// The on lane clear callback.
        /// </summary>
        internal virtual void OnLaneClear()
        {
        }

        /// <summary>
        /// The on jungle clear callback.
        /// </summary>
        internal virtual void OnJungleClear()
        {
        }

        /// <summary>
        /// The on last hit callback.
        /// </summary>
        internal virtual void OnLastHit()
        {
        }

        /// <summary>
        /// The on mixed callback.
        /// </summary>
        internal virtual void OnMixed()
        {
        }

        /// <summary>
        ///     The on update callback.
        /// </summary>
        internal virtual void OnUpdate()
        {
        }

        #endregion
    }
}