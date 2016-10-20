namespace ElLeeSinDecentralized.Components
{
    using System;

    using ElLeeSinDecentralized.Enumerations;
    using ElLeeSinDecentralized.Utils;

    using LeagueSharp;
    using LeagueSharp.Common;

    /// <summary>
    ///     The my menu class.
    /// </summary>
    internal class MyMenu
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MyMenu" /> class.
        /// </summary>
        internal MyMenu()
        {
            RootMenu = new Menu("ElLeeSinDecentralized", "ElLeeSinDecentralized", true);

            RootMenu.AddSubMenu(GetTargetSelectorNode());
            RootMenu.AddSubMenu(GetOrbwalkerNode());

            RootMenu.AddToMainMenu();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the root menu.
        /// </summary>
        internal static Menu RootMenu { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     The generate spell menu method.
        /// </summary>
        /// <param name="spellSlot">
        ///     The spell slot.
        /// </param>
        internal static void GenerateSpellMenu(SpellSlot spellSlot)
        {
            try
            {
                var spellSlotName = spellSlot.ToString();
                var spellSlotNameLower = spellSlotName.ToLower();

                var node = new Menu(spellSlot + " Settings", "spellmenu" + spellSlotNameLower);

                var nodeCombo = new Menu("Combo", spellSlotNameLower + "combomenu");
                {
                    nodeCombo.AddItem(new MenuItem("combo" + spellSlotNameLower + "use", "Use " + spellSlotName).SetValue(true));

                    if (spellSlotNameLower.Equals("w", StringComparison.InvariantCultureIgnoreCase))
                    {
                        nodeCombo.AddItem(new MenuItem("wardjump.key", "Wardjump key").SetValue(new KeyBind("Z".ToCharArray()[0], KeyBindType.Press)));
                    }

                    if (spellSlotNameLower.Equals("q", StringComparison.InvariantCultureIgnoreCase))
                    {
                        nodeCombo.AddItem(new MenuItem("combo" + spellSlotNameLower + "2use", "Use " + spellSlotName).SetValue(true));
                    }
                }

                node.AddSubMenu(nodeCombo);

                var nodeMixed = new Menu("Mixed", spellSlotNameLower + "mixedmenu");
                {
                    nodeMixed.AddItem(new MenuItem("mixed" + spellSlotNameLower + "use", "Use " + spellSlotName).SetValue(true));
                }

                node.AddSubMenu(nodeMixed);

                if (spellSlotNameLower.Equals("q", StringComparison.InvariantCultureIgnoreCase))
                {
                    var nodeLastHit = new Menu("LastHit", spellSlotNameLower + "lasthitmenu");
                    {
                        nodeLastHit.AddItem(
                            new MenuItem("lasthit" + spellSlotNameLower + "use", "Use " + spellSlotName).SetValue(true));
                    }
                    node.AddSubMenu(nodeLastHit);
                }


                if (!spellSlotNameLower.Equals("r", StringComparison.InvariantCultureIgnoreCase))
                {
                    var nodeLaneClear = new Menu("Clear", spellSlotNameLower + "laneclearmenu");
                    {
                        nodeLaneClear.AddItem(new MenuItem("laneclear" + spellSlotNameLower + "use", "Use " + spellSlotName).SetValue(true));
                    }

                    node.AddSubMenu(nodeLaneClear);
                }

                RootMenu.AddSubMenu(node);
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryTrype.Error, "@MyMenu.cs: Can not generate menu for spell - {0}", e);
                throw;
            }
        }

        /// <summary>
        ///     The get orbwalker node.
        /// </summary>
        /// <returns>
        ///     <see cref="Menu" /> class for the orbwalker.
        /// </returns>
        private static Menu GetOrbwalkerNode()
        {
            var node = new Menu("Orbwalker", "ElLeeSinDecentralizedorbwalker");
            Program.Orbwalker = new Orbwalking.Orbwalker(node);

            return node;
        }

        /// <summary>
        ///     The get target selector node.
        /// </summary>
        /// <returns>
        ///     <see cref="Menu" /> class for the target selector.
        /// </returns>
        private static Menu GetTargetSelectorNode()
        {
            var node = new Menu("Target Selector", "ElLeeSinDecentralizedtargetselector");
            TargetSelector.AddToMenu(node);

            return node;
        }

        #endregion
    }
}