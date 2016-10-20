namespace ElLeeSinDecentralized.Components.SpellManagers
{
    using System;
    using System.Collections.Generic;

    using ElLeeSinDecentralized.Enumerations;
    using ElLeeSinDecentralized.Utils;

    using LeagueSharp;
    using LeagueSharp.Common;

    internal class PassiveManager
    {
        /// <summary>
        ///     The Flurry stacks.
        /// </summary>
        internal static int FlurryStacks;

        /// <summary>
        ///     The last spell casting.
        /// </summary>
        internal static int lastSpellCastTime;

        /// <summary>
        ///     The buffnames.
        /// </summary>
        static List<String> buffNames = new List<String>(new String[] { "blindmonkqone", "blindmonkwone", "blindmonkeone", "blindmonkqtwo", "blindmonkwtwo", "blindmonketwo", "blindmonkrkick" });

        /// <summary>
        ///     Called on buff add.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        internal static void Obj_AI_Base_OnBuffAdd(Obj_AI_Base sender, Obj_AI_BaseBuffAddEventArgs args)
        {
            try
            {
                if (!sender.IsMe)
                {
                    return;
                }

                if (args.Buff.DisplayName.Equals(Misc.BlindMonkFlurry, StringComparison.InvariantCultureIgnoreCase))
                {
                    FlurryStacks = 2;
                }
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryTrype.Error, "@PassiveManager.cs: Can not {0} -", e);
                throw;
            }
        }

        /// <summary>
        ///     Testing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        internal static void Obj_AI_Base_OnProcessSpellCast1(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (args.SData.Name.Equals(Misc.BlindMonkQOne, StringComparison.InvariantCultureIgnoreCase))
            {
                Utility.DelayAction.Add(2900, () => { Misc.CanCastQ2 = true; });
            }

            foreach (string buff in buffNames)
            {
                if (buff.Equals(args.SData.Name.ToLower(), StringComparison.InvariantCultureIgnoreCase))
                {
                    FlurryStacks = 2;
                    lastSpellCastTime = Environment.TickCount;
                }
            }
        }

        /// <summary>
        ///     Called on buff remove.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        internal static void Obj_AI_Base_OnBuffRemove(Obj_AI_Base sender, Obj_AI_BaseBuffRemoveEventArgs args)
        {
            try
            {
                if (!sender.IsMe)
                {
                    return;
                }

                if (args.Buff.DisplayName.Equals(Misc.BlindMonkFlurry, StringComparison.InvariantCultureIgnoreCase))
                {
                    FlurryStacks = 0;
                }
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryTrype.Error, "@PassiveManager.cs: Can not {0} -", e);
                throw;
            }
        }
    }
}
