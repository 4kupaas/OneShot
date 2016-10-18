namespace ElLeeSinDecentralized.Utils
{
    using System;

    using ElLeeSinDecentralized.Enumerations;

    using LeagueSharp;

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

        /// <summary>
        ///     Called on buff update [ seems broken ]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        internal static void Obj_AI_Base_OnBuffUpdateCount(Obj_AI_Base sender, Obj_AI_BaseBuffUpdateCountEventArgs args)
        {
            try
            {
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryTrype.Error, "@PassiveManager.cs: Can not {0} -", e);
                throw;
            }
        }
    }
}
