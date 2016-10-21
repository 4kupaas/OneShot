namespace ElLeeSinDecentralized.Utils
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    using ElLeeSinDecentralized.Components.Spells;
    using ElLeeSinDecentralized.Enumerations;

    using LeagueSharp;
    using LeagueSharp.Common;

    /// <summary>
    ///     The misc.
    /// </summary>
    internal static class Misc
    {
        #region Methods

        /// <summary>
        ///     The Q.
        /// </summary>
        internal static SpellQ SpellQ;

        /// <summary>
        ///     The W.
        /// </summary>
        internal static SpellW SpellW;

        /// <summary>
        ///     The E.
        /// </summary>
        internal static SpellE SpellE;

        /// <summary>
        ///     The R.
        /// </summary>
        internal static SpellR SpellR;

        /// <summary>
        ///     The passive buffname.
        /// </summary>
        internal static String BlindMonkFlurry = "BlindMonkFlurry";

        /// <summary>
        ///     The Q buffname.
        /// </summary>
        internal static String BlindMonkQOne = "BlindMonkQOne";

        /// <summary>
        ///     Gets the Q2 safety check value.
        /// </summary>
        [DefaultValue(false)]
        internal static bool CanCastQ2 { get; set; }

        /// <summary>
        ///     Gets the Q Instance name
        /// </summary>
        /// <value>
        ///     Q instance name
        /// </value>
        internal static bool IsQOne
            => SpellQ.SpellObject.Instance.Name.Equals(BlindMonkQOne, StringComparison.InvariantCultureIgnoreCase);


        /// <summary>
        ///     Gets the E Instance name
        /// </summary>
        /// <value>
        ///     E instance name
        /// </value>
        internal static bool IsEOne
            => SpellE.SpellObject.Instance.Name.Equals("BlindMonkEOne", StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        ///     Gets the W Instance name
        /// </summary>
        /// <value>
        ///     W instance name
        /// </value>
        internal static bool IsWOne
            => SpellW.SpellObject.Instance.Name.Equals("BlindMonkWOne", StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        ///     Checks if a target has the BlindMonkQOne buff.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        internal static bool HasBlindMonkQOne(Obj_AI_Base target) => target.HasBuff("BlindMonkQOne");

        /// <summary>
        ///     Checks if a target has the BlindMonkTempest buff.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        internal static bool HasBlindMonkTempest(Obj_AI_Base target) => target.HasBuff("BlindMonkTempest");

        /// <summary>
        ///     Gets a target from the common target selector.
        /// </summary>
        /// <param name="range">
        ///     The range.
        /// </param>
        /// <param name="damageType">
        ///     The damage type.
        /// </param>
        /// <returns>
        ///     <see cref="Obj_AI_Hero" />
        /// </returns>
        internal static Obj_AI_Hero GetTarget(float range, TargetSelector.DamageType damageType)
        {
            try
            {
                return TargetSelector.GetTarget(range, damageType);
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryType.Error, "@Misc.cs: Can not return target - {0}", e);
                throw;
            }
        }

        #endregion
    }
}