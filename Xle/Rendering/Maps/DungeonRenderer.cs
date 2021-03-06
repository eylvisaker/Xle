﻿using Xle.Data;
using Xle.Maps.Dungeons;
using Xle.Maps.XleMapTypes;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using AgateLib;

namespace Xle.Rendering.Maps
{
    [Transient]
    public class DungeonRenderer : Map3DRenderer
    {
        public XleData Data { get; set; }

        public override bool AnimateExhibits => false;
        public override bool DrawCloseup => false;

        protected override ExtraType GetExtraType(int val, int side)
        {
            if (side != 0)
                return ExtraType.None;

            ExtraType extraType = ExtraType.None;

            switch (val)
            {
                case 0x11:
                    extraType = ExtraType.GoUp;
                    break;
                case 0x12:
                    extraType = ExtraType.GoDown;
                    break;
                case 0x13:
                    extraType = ExtraType.Needle;
                    break;
                case 0x14:
                    extraType = ExtraType.Slime;
                    break;
                case 0x15:
                    extraType = ExtraType.TripWire;
                    break;
                case 0x16:
                    extraType = ExtraType.GasVent;
                    break;
                case 0x1d:
                    extraType = ExtraType.Urn;
                    break;
                case 0x1e:
                    extraType = ExtraType.Box;
                    break;
                case 0x30:
                case 0x31:
                case 0x32:
                case 0x33:
                case 0x34:
                case 0x35:
                case 0x36:
                case 0x37:
                case 0x38:
                case 0x39:
                case 0x3a:
                case 0x3b:
                case 0x3c:
                case 0x3d:
                case 0x3e:
                case 0x3f:
                    extraType = ExtraType.Chest;
                    break;

            }
            return extraType;
        }

        public new Dungeon TheMap { get { return (Dungeon)base.TheMap; } }

        public new DungeonExtender Extender { get { return (DungeonExtender)base.Extender; } }

        protected override void DrawMonsters(SpriteBatch spriteBatch,
            int x, int y, Direction faceDirection, Rectangle inRect, 
            int maxDistance)
        {
            Point stepDir = faceDirection.StepDirection();

            for (int distance = 1; distance <= maxDistance; distance++)
            {
                Point loc = new Point(x + stepDir.X * distance, y + stepDir.Y * distance);

                var monster = Extender.Combat.MonsterAt(GameState.Player.DungeonLevel, loc);

                if (monster == null)
                    continue;

                var data = Data.DungeonMonsters[monster.MonsterID];
                int image = distance - 1;
                var imageInfo = data.Images[image];

                Vector2 drawPoint = imageInfo.DrawPoint.ToVector2();
                drawPoint.X += inRect.X;
                drawPoint.Y += inRect.Y;

                var srcRect = imageInfo.SourceRects[0];

                spriteBatch.Draw(data.Image, drawPoint, srcRect, Color.White);

                break;
            }
        }
    }
}
