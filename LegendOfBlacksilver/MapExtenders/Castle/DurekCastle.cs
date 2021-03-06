﻿using AgateLib;
using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Xle.Blacksilver.MapExtenders.Castle.EventExtenders;
using Xle.Maps;
using Xle.Maps.Castles;
using Xle.Commands;

namespace Xle.Blacksilver.MapExtenders.Castle
{
    [Transient("DurekCastle")]
    public class DurekCastle : CastleExtender
    {
        private CastleDamageCalculator cdc;

        public DurekCastle(Random random)
        {
            cdc = new CastleDamageCalculator(random) { v5 = 0.9, v6 = 0.95, v7 = 0.95 };
        }
        protected LobStory Story { get { return GameState.Story(); } }

        public override double ChanceToHitGuard(Guard guard, int distance)
        {
            return cdc.ChanceToHitGuard(Player, distance);
        }
        public override double ChanceToHitPlayer(Guard guard)
        {
            return cdc.ChanceToHitPlayer(Player);
        }
        public override int RollDamageToGuard(Guard guard)
        {
            return cdc.RollDamageToGuard(Player);
        }
        public override int RollDamageToPlayer(Guard guard)
        {
            return cdc.RollDamageToPlayer(Player);
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
            commands.Items.Add(CommandFactory.Armor());
            commands.Items.Add(CommandFactory.Gamespeed());
            commands.Items.Add(CommandFactory.Inventory());
            commands.Items.Add(CommandFactory.Pass());
            commands.Items.Add(CommandFactory.Weapon());

            var fight = (LobCastleFight)CommandFactory.Fight("LobCastleFight");
            fight.DamageCalculator = cdc;

            commands.Items.Add(fight);
            commands.Items.Add(CommandFactory.Leave());
            commands.Items.Add(CommandFactory.Magic("LobCastleMagic"));
            commands.Items.Add(CommandFactory.Open());
            commands.Items.Add(CommandFactory.Speak("DurekCastleSpeak"));
            commands.Items.Add(CommandFactory.Take());
            commands.Items.Add(CommandFactory.Use("DurekCastleUse"));
            commands.Items.Add(CommandFactory.Xamine());
        }

        public override void SetColorScheme(ColorScheme scheme)
        {
            scheme.TextColor = XleColor.White;

            scheme.FrameColor = XleColor.LightGray;
            scheme.FrameHighlightColor = XleColor.Yellow;
        }

        public override void OnLoad()
        {
            if (Player.Items[LobItem.FalconFeather] == 0)
            {
                RemoveFalconFeatherDoor();
            }
            if (Story.ClearedRockSlide)
            {
                RemoveRockSlide();
            }

            if (Story.DefeatedOrcs == false)
            {
                ColorOrcs();
            }
        }

        private void RemoveRockSlide()
        {
            var sc = Events.OfType<SingingCrystal>().FirstOrDefault();

            sc.RemoveRockSlide(sc.TheEvent.Rectangle);
        }

        private void ColorOrcs()
        {
            if (Story.DefeatedOrcs == false)
            {
                Rectangle area = RectangleX.FromLTRB(66, 0, TheMap.Width, 68);

                foreach (var guard in TheMap.Guards)
                {
                    if (area.Contains(guard.Location))
                        guard.Color = XleColor.Blue;
                }
            }
        }

        private void RemoveFalconFeatherDoor()
        {
            var door = Events.OfType<FeatherDoor>().First();

            door.RemoveDoor();
        }

        public bool StoredAngryFlag { get; set; }

        public bool InOrcArea { get; set; }
    }
}
