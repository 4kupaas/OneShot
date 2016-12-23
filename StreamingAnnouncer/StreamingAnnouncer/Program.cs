namespace StreamingAnnouncer
{
    using System;
    using System.Drawing;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Color = System.Drawing.Color;

    class Program
    {
        /// <summary>
        ///     The menu.
        /// </summary>
        public static Menu Menu;

        /// <summary>
        ///     Announcer box.
        /// </summary>
        public static bool HideAnnouncer = false;

        /// <summary>
        ///     The last announcer notification.
        /// </summary>
        private static float lastAnnouncer;

        /// <summary>
        ///     The Check Interval
        /// </summary>
        private const float CheckInterval = 5000f;

        /// <summary>
        ///     Gets the top offset of the HUD elements
        /// </summary>
        private static int HudOffsetTop => Menu.Item("Stream.OffsetTop").GetValue<Slider>().Value;

        /// <summary>
        ///     Gets the right offset of the announcer hider.
        /// </summary>
        private static int HudOffsetRight => Menu.Item("Stream.OffsetRight").GetValue<Slider>().Value;

        /// <summary>
        ///     Gets the right offset of the announcer hider.
        /// </summary>
        private static int HudBarWidth => Menu.Item("Stream.Width").GetValue<Slider>().Value;

        /// <summary>
        ///     Gets the right offset of the announcer hider.
        /// </summary>
        private static int HudBarHeight => Menu.Item("Stream.Height").GetValue<Slider>().Value;

        /// <summary>
        ///     Entry point.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            MenuInitializer();
            Game.OnNotify += OnNotify;
            Drawing.OnDraw += Drawing_OnEndScene;
        }

        /// <summary>
        ///     The Menu.
        /// </summary>
        private static void MenuInitializer()
        {
            Menu = new Menu("ElStreaming", "Stream", true).SetFontStyle(FontStyle.Bold, SharpDX.Color.GreenYellow);

            Menu.AddItem(new MenuItem("Activate-Stream", "Hide Announcer").SetValue(true));
            Menu.AddItem(new MenuItem("Stream.OffsetTop", "Offset Right").SetValue(new Slider(600, 0, 1500)));
            Menu.AddItem(new MenuItem("Stream.OffsetRight", "Offset Top").SetValue(new Slider(140, 0, 1500)));
            Menu.AddItem(new MenuItem("Stream.Width", "Bar Width").SetValue(new Slider(700, 0, 1500)));
            Menu.AddItem(new MenuItem("Stream.Height", "Bar Height").SetValue(new Slider(45)));
            Menu.AddItem(new MenuItem("Stream.Color", "Bar Color").SetValue(new Circle(true, System.Drawing.Color.DeepPink)));
            Menu.AddItem(new MenuItem("empty-line-3000", string.Empty));
            Menu.AddItem(new MenuItem("AddTestCard", "Draw Test Bar").SetValue(false).DontSave());

            Menu.Item("AddTestCard").ValueChanged += (sender, args) =>
            {
                args.Process = false;

                lastAnnouncer = Environment.TickCount;
                HideAnnouncer = true;
            };

            Menu.AddToMainMenu();
        }

        /// <summary>
        ///     Called on notification.
        /// </summary>
        /// <param name="args"></param>
        private static void OnNotify(GameNotifyEventArgs args)
        {
            if (args.EventId == GameEventId.OnChampionDoubleKill
                || args.EventId == GameEventId.OnChampionTripleKill 
                || args.EventId == GameEventId.OnChampionQuadraKill 
                || args.EventId == GameEventId.OnChampionPentaKill 
                || args.EventId == GameEventId.OnChampionUnrealKill
                || args.EventId == GameEventId.OnAce 
                || args.EventId == GameEventId.OnFirstBlood 
                || args.EventId == GameEventId.OnTurretDie 
                || args.EventId == GameEventId.OnTurretKill
                || args.EventId == GameEventId.OnChampionKill
                || args.EventId == GameEventId.OnKillingSpree
                || args.EventId == GameEventId.OnKillingSpreeSet1
                || args.EventId == GameEventId.OnKillingSpreeSet2
                || args.EventId == GameEventId.OnKillingSpreeSet3
                || args.EventId == GameEventId.OnKillingSpreeSet4
                || args.EventId == GameEventId.OnKillingSpreeSet5
                || args.EventId == GameEventId.OnKillingSpreeSet6
                || args.EventId == GameEventId.OnGameModeAnnouncement1
                || args.EventId == GameEventId.OnKillDragon
                || args.EventId == GameEventId.OnKillRiftHerald
                || args.EventId == GameEventId.OnChampionSingleKill
                || args.EventId == GameEventId.OnKillDragonSteal
                || args.EventId == GameEventId.OnReconnect
                || args.EventId == GameEventId.OnQuit
                || args.EventId == GameEventId.OnShutdown)
            {
                HideAnnouncer = true;
                lastAnnouncer = Environment.TickCount;
                Console.WriteLine("[Streaming {0:HH:mm:ss}] Hiding {1}", DateTime.Now, args.EventId);
            }
        }

        /// <summary>
        ///     The drawings.
        /// </summary>
        /// <param name="args"></param>
        private static void Drawing_OnEndScene(EventArgs args)
        {
            if (HideAnnouncer && Menu.Item("Activate-Stream").GetValue<bool>())
            {
                DrawRect(HudOffsetTop, HudOffsetRight, HudBarWidth, HudBarHeight, 1, Menu.Item("Stream.Color").GetValue<Circle>().Color);

                if (lastAnnouncer + CheckInterval > Environment.TickCount)
                {
                    return;
                }

                HideAnnouncer = false;
            }
        }

        /// <summary>
        ///     Draws a rectangle
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="thickness"></param>
        /// <param name="color"></param>
        private static void DrawRect(float x, float y, int width, float height, float thickness, Color color)
        {
            for (var i = 0; i < height; i++)
            {
                Drawing.DrawLine(x, y + i, x + width, y + i, thickness, color);
            }
        }
    }
}
