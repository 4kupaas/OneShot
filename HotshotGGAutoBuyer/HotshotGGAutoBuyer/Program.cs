using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotshotGGAutoBuyer
{
    using LeagueSharp;
    using LeagueSharp.Common;
    using ItemData = LeagueSharp.Common.Data.ItemData;

    class Program
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
        }

        private static void OnLoad(EventArgs args)
        {
            Game.OnUpdate += OnUpdate;
        }

        private static void OnUpdate(EventArgs args)
        {
            if (ObjectManager.Player.InFountain())
            {
                ObjectManager.Player.BuyItem((ItemId)3363);
            }
        }
    }
}
