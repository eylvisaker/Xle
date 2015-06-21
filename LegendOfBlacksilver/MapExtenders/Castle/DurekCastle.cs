using AgateLib.Geometry;
using ERY.Xle.LoB.MapExtenders.Castle.EventExtenders;
using ERY.Xle.Services;
using ERY.Xle.Services.Implementation;
using ERY.Xle.Services.Implementation.Commands;
using ERY.Xle.XleEventTypes;
using ERY.Xle.Maps.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERY.Xle.Maps;

namespace ERY.Xle.LoB.MapExtenders.Castle
{
    public class DurekCastle : CastleExtender
    {
        CastleDamageCalculator cdc;

        public DurekCastle(Random random)
        {
            cdc = new CastleDamageCalculator(random) { v5 = 0.9, v6 = 0.95, v7 = 0.95 };
        }
        protected LobStory Story { get { return GameState.Story(); } }
        
        public override double ChanceToHitGuard(Player player, Guard guard, int distance)
        {
            return cdc.ChanceToHitGuard(player, distance);
        }
        public override double ChanceToHitPlayer(Player player, Guard guard)
        {
            return cdc.ChanceToHitPlayer(player);
        }
        public override int RollDamageToGuard(Player player, Guard guard)
        {
            return cdc.RollDamageToGuard(player);
        }
        public override int RollDamageToPlayer(Player player, Guard guard)
        {
            return cdc.RollDamageToPlayer(player);
        }

        public override int GetOutsideTile(Point playerPoint, int x, int y)
        {
            if (playerPoint.X < 12 && playerPoint.Y < 12)
                return 0;

            if (playerPoint.X < 45 && playerPoint.Y < 22)
                return 17;

            if (playerPoint.X > 50 && playerPoint.Y > 75)
                return 16;

            return 32;
        }

        public override void SetCommands(ICommandList commands)
        {
            commands.Items.AddRange(LobProgram.CommonLobCommands);

            commands.Items.Add(CommandFactory.Leave());
            commands.Items.Add(CommandFactory.Open());
            commands.Items.Add(CommandFactory.Magic());
            commands.Items.Add(CommandFactory.Take());
            commands.Items.Add(CommandFactory.Speak());
        }

        public override void SetColorScheme(ColorScheme scheme)
        {
            scheme.TextColor = XleColor.White;

            scheme.FrameColor = XleColor.LightGray;
            scheme.FrameHighlightColor = XleColor.Yellow;
        }

        public override void OnLoad(GameState state)
        {
            if (Player.Items[LobItem.FalconFeather] == 0)
            {
                RemoveFalconFeatherDoor(state);
            }
            if (Story.ClearedRockSlide)
            {
                RemoveRockSlide(state);
            }

            if (Story.DefeatedOrcs == false)
            {
                ColorOrcs(state);
            }
        }

        private void RemoveRockSlide(GameState state)
        {
            var sc = Events.OfType<SingingCrystal>().FirstOrDefault();

            sc.RemoveRockSlide(sc.TheEvent.Rectangle);
        }

        private void ColorOrcs(GameState state)
        {
            if (Story.DefeatedOrcs == false)
            {
                Rectangle area = Rectangle.FromLTRB(66, 0, TheMap.Width, 68);

                foreach (var guard in TheMap.Guards)
                {
                    if (area.Contains(guard.Location))
                        guard.Color = XleColor.Blue;
                }
            }
        }

        private void RemoveFalconFeatherDoor(GameState state)
        {
            var door = TheMap.Events.OfType<Door>().First(
                x => x is Door && (x as Door).RequiredItem == (int)LobItem.FalconFeather);

            door.RemoveDoor(state);
        }

        public override void SpeakToGuard(GameState state)
        {
            TextArea.PrintLine();
            TextArea.PrintLine();

            if (state.Player.Items[LobItem.FalconFeather] > 0)
            {
                TextArea.PrintLine("I see you have the feather,");
                TextArea.PrintLine("why not use it?");
                GameControl.Wait(1500);
            }
            else
            {
                TextArea.PrintLine("I should not converse, sir.");
            }
        }

        public override void PlayerUse(GameState state, int item, ref bool handled)
        {
            if (item == (int)LobItem.FalconFeather)
            {
                TextArea.PrintLine("You're not by a door.");
                handled = true;
            }
        }

        public bool StoredAngryFlag { get; set; }

        public bool InOrcArea { get; set; }
    }
}
