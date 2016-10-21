namespace ElLeeSinDecentralized.Components
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using ElLeeSinDecentralized.Components.SpellManagers;
    using ElLeeSinDecentralized.Components.Spells;
    using ElLeeSinDecentralized.Enumerations;
    using ElLeeSinDecentralized.Utils;

    using LeagueSharp;
    using LeagueSharp.Common;

    /// <summary>
    ///     The spell manager.
    /// </summary>
    internal class SpellManager
    {
        #region Fields

        /// <summary>
        ///     The spells.
        /// </summary>
        private readonly List<ISpell> spells = new List<ISpell>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SpellManager" /> class.
        /// </summary>
        internal SpellManager()
        {
            try
            {
                Misc.SpellQ = new SpellQ();
                Misc.SpellW = new SpellW();
                Misc.SpellE = new SpellE();
                Misc.SpellR = new SpellR();

                this.LoadSpells(new List<ISpell>() { new SpellQ(), new SpellW(), new SpellE(), new SpellR() });
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryTrype.Error, "@SpellManager.cs: Can not initialize the spells - {0}", e);
                throw;
            }

            Game.OnUpdate += this.Game_OnUpdate;
            Obj_AI_Base.OnBuffAdd += PassiveManager.Obj_AI_Base_OnBuffAdd;
            Obj_AI_Base.OnBuffRemove += PassiveManager.Obj_AI_Base_OnBuffRemove;
            Obj_AI_Base.OnProcessSpellCast += PassiveManager.Obj_AI_Base_OnProcessSpellCast1;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The is the spell active method.
        /// </summary>
        /// <param name="spellSlot">
        ///     The spell slot.
        /// </param>
        /// <param name="orbwalkingMode">
        ///     The orbwalking mode.
        /// </param>
        /// <returns>
        ///     <see cref="bool" />
        /// </returns>
        private static bool IsSpellActive(SpellSlot spellSlot, Orbwalking.OrbwalkingMode orbwalkingMode)
        {
            if (Program.Orbwalker.ActiveMode != orbwalkingMode || !spellSlot.IsReady())
            {
                return false;
            }

            try
            {
                var orbwalkerModeLower = Program.Orbwalker.ActiveMode.ToString().ToLower();
                var spellSlotNameLower = spellSlot.ToString().ToLower();

                if ((orbwalkerModeLower.Equals("lasthit")
                    && (spellSlotNameLower.Equals("e") || spellSlotNameLower.Equals("w")
                        || spellSlotNameLower.Equals("r"))) || (orbwalkerModeLower.Equals("laneclear") && (spellSlotNameLower.Equals("r"))))
                {
                    return false;
                }

                return MyMenu.RootMenu.Item(orbwalkerModeLower + spellSlotNameLower + "use").GetValue<bool>();
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryTrype.Error, "@SpellManager.cs: Can not get spell active state for slot {0} - {1}", spellSlot.ToString(), e);
                throw;
            }
        }

        /// <summary>
        ///     The game on update callback.
        /// </summary>
        /// <param name="args">
        ///     The args.
        /// </param>
        private void Game_OnUpdate(EventArgs args)
        {
            if (ObjectManager.Player.IsDead || MenuGUI.IsChatOpen || MenuGUI.IsShopOpen) return;

            var mode = Program.Orbwalker.ActiveMode;

            foreach (var spell in this.spells)
            {
                if (!IsSpellActive(spell.SpellSlot, mode))
                {
                    continue;
                }

                switch (mode)
                {
                    case Orbwalking.OrbwalkingMode.Combo:
                        spell.OnCombo();
                        break;

                    case Orbwalking.OrbwalkingMode.Mixed:
                        spell.OnMixed();
                        break;

                    case Orbwalking.OrbwalkingMode.LaneClear:
                        spell.OnLaneClear();
                        break;

                    case Orbwalking.OrbwalkingMode.LastHit:
                        spell.OnLastHit();
                        break;
                }

                spell.OnUpdate();
            }
        }

        /// <summary>
        ///     The load spells method.
        /// </summary>
        /// <param name="spellList">
        ///     The spells.
        /// </param>
        private void LoadSpells(IEnumerable<ISpell> spellList)
        {
            foreach (var spell in spellList)
            {
                MyMenu.GenerateSpellMenu(spell.SpellSlot);
                this.spells.Add(spell);
            }
        }

        #endregion
    }
}
