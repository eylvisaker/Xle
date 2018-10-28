using Xle.Maps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AgateLib;

namespace Xle.Services.Rendering.Maps
{
    [Transient]
    public class TownRenderer : Map2DRenderer
    {
        protected override void Animate(GameTime time, Rectangle tileRect)
        {
            base.Animate(time, tileRect);

            TheMap.Guards.AnimateGuards(time);
        }

        public override void Draw(GameTime time, SpriteBatch spriteBatch, 
                                  Point playerPos, Direction faceDirection, 
                                  Rectangle inRect)
        {
            base.Draw(time, spriteBatch, playerPos, faceDirection, inRect);

            DrawGuards(playerPos, inRect);
        }

        protected override int TileToDraw(int x, int y)
        {
            int tile = base.TileToDraw(x, y);

            var roof = TheMap.ClosedRoofAt(x, y);

            if (roof == null)
                return tile;

            var roofTile = roof.TileAtMapCoords(x, y);

            if (roofTile == 0)
                return tile;

            return roofTile;
        }

        protected void DrawGuards(Point playerPos, Rectangle inRect)
        {
            Point topLeftMapPt = new Point(playerPos.X - 11, playerPos.Y - 7);

            int px = inRect.Left;
            int py = inRect.Top;

            for (int i = 0; i < TheMap.Guards.Count; i++)
            {
                Guard guard = TheMap.Guards[i];

                if (TheMap.ClosedRoofAt(guard.X, guard.Y) == null)
                {
                    var facing = guard.Facing;

                    int rx = px + (guard.X - topLeftMapPt.X) * 16;
                    int ry = py + (guard.Y - topLeftMapPt.Y) * 16;

                    if (rx >= inRect.Left && ry >= inRect.Top && rx <= inRect.Right - 32 && ry <= inRect.Bottom - 32)
                    {
                        Renderer.DrawCharacterSprite(rx, ry, facing, true, TheMap.Guards.AnimFrame, false, guard.Color);
                    }
                }
            }
        }
    }
}
