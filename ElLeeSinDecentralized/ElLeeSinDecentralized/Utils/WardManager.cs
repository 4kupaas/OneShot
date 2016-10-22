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
        ///     Player is wardjumping.
        /// </summary>
        internal static bool WardJumping;

        /// <summary>
        ///     The last ward position.
        /// </summary>
        private static Vector3 LastWardPosition;

        /// <summary>
        ///     The last warding time.
        /// </summary>
        public static int LastWardPlacement;

        /// <summary>
        ///     Recheck the wards.
        /// </summary>
        private static bool reCheckWard = true;

        /// <summary>
        ///     Get the ready ward.
        /// </summary>
        /// <returns></returns>
        public static SpellSlot GetReadyWard()
        {
            var ward =
                ObjectManager.Player.InventoryItems.Where(s => WardIds.Contains(s.Id) && s.SpellSlot.IsReady())
                    .Select(s => s.SpellSlot)
                    .ToList();

            if (ward.Count == 0)
            {
                return SpellSlot.Unknown;
            }

            return ward.Contains(SpellSlot.Trinket) ? SpellSlot.Trinket : ward.FirstOrDefault();
        }

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

            if (JumpPosition != new Vector2() && reCheckWard)
            {
                // recheck
                reCheckWard = false;
                Utility.DelayAction.Add(
                    20,
                    () =>
                        {
                            if (JumpPosition != new Vector2())
                            {
                                JumpPosition = new Vector2();
                                reCheckWard = true;
                            }
                        });
            }

            if (!Misc.SpellW.SpellSlot.IsReady() || !Misc.IsWOne || ObjectManager.Player.Distance(JumpPosition) > Misc.SpellW.Range) // require in range
            {
                return;
            }

            if (jumpToAllies || jumpToMinions)
            {
                if (!Misc.IsWOne)
                {
                    return;
                }

                if (jumpToAllies)
                {
                    var closestAlly = HeroManager.Allies.Where(x => x.Distance(ObjectManager.Player) < Misc.SpellW.Range && x.Distance(position) < 200 && !x.IsMe).OrderByDescending(i => i.Distance(ObjectManager.Player))
                            .ToList()
                            .FirstOrDefault();

                    if (closestAlly != null)
                    {
                        if (!Misc.IsWOne)
                        {
                            return;
                        }

                        // Cast W on champion.
                        Misc.SpellW.SpellObject.CastOnUnit(closestAlly);
                        return;
                    }
                }

                if (jumpToMinions)
                {
                    var closestMinion =
                        ObjectManager.Get<Obj_AI_Minion>()
                            .Where(
                                m =>
                                    m.IsAlly && m.Distance(ObjectManager.Player) < Misc.SpellW.Range && m.Distance(position) < 200
                                    && !m.Name.ToLower().Contains("ward"))
                            .OrderByDescending(i => i.Distance(ObjectManager.Player))
                            .ToList()
                            .FirstOrDefault();

                    if (closestMinion != null)
                    {
                        if (!Misc.IsWOne)
                        {
                            return;
                        }

                        // Cast W on minion.
                        Misc.SpellW.SpellObject.CastOnUnit(closestMinion);
                        return;
                    }
                }
            }

            var isWard = false;

            var ward =
               ObjectManager.Get<Obj_AI_Base>()
                   .Where(o => o.IsAlly && o.Name.ToLower().Contains("ward") && o.Distance(JumpPosition) < 200)
                   .ToList()
                   .FirstOrDefault();

            if (ward != null)
            {
                isWard = true;
                if (!Misc.IsWOne)
                {
                    return;
                }

                // Cast W on ward.
                Misc.SpellW.SpellObject.CastOnUnit(ward);
            }

            if (!isWard)
            {
                var readyWard = GetReadyWard();
                if (readyWard.Equals(SpellSlot.Unknown))
                {
                    return;
                }

                if (Misc.SpellW.SpellSlot.IsReady() && Misc.IsWOne && LastWardPlacement + 400 < Utils.TickCount)
                {
                    ObjectManager.Player.Spellbook.CastSpell(readyWard, JumpPosition.To3D());
                    LastWardPosition = JumpPosition.To3D();
                    LastWardPlacement = Utils.TickCount;
                }
            }
        }
    }
}
