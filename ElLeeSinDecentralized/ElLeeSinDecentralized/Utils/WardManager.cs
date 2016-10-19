namespace ElLeeSinDecentralized.Utils
{
    using System.Linq;

    using ElLeeSinDecentralized.Enumerations;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    internal class WardManager
    {
        /// <summary>
        ///     The ward ids.
        /// </summary>
        private static readonly ItemId[] WardIds =
            {
                ItemId.Warding_Totem_Trinket, ItemId.Greater_Stealth_Totem_Trinket,
                ItemId.Greater_Vision_Totem_Trinket, ItemId.Sightstone,
                ItemId.Ruby_Sightstone, ItemId.Vision_Ward, (ItemId)3711,
                (ItemId)1411, (ItemId)1410, (ItemId)1408, (ItemId)1409
            };

        /// <summary>
        ///     The jump position.
        /// </summary>
        internal static Vector2 JumpPosition;

        /// <summary>
        ///     The last ward placement.
        /// </summary>
        internal static int LastWardPlacement;

        /// <summary>
        ///     Player is wardjumping.
        /// </summary>
        internal static bool WardJumping;

        /// <summary>
        ///     The wardjump handler.
        /// </summary>
        /// <param name="position">
        ///     The cursor position.
        /// </param>
        /// <param name="jumpToAllies">
        ///     Wardjump to allies.
        /// </param>
        /// <param name="jumpToMinions">
        ///     Wardjump to minions.
        /// </param>
        /// <param name="maxrangeJump">
        ///     Wardjump maxrange.
        /// </param>
        internal static void WardjumpHandler(Vector3 position, bool jumpToAllies = true, bool jumpToMinions = true, bool maxrangeJump = false)
        {
            if (!Misc.IsWOne)
            {
                return;
            }

            var playerPosition = ObjectManager.Player.Position.To2D();
            var newPosition = (position.To2D() - ObjectManager.Player.Position.To2D());

            if (JumpPosition == new Vector2())
            {
                JumpPosition = playerPosition + (newPosition.Normalized() * (ObjectManager.Player.Distance(position)));
            }
        }
    }
}
