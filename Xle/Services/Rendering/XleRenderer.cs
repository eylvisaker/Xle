using AgateLib;
using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Xle.Data;
using Xle.Maps;
using Xle.Services.Commands;
using Xle.Services.Game;
using Xle.Services.Rendering.Maps;
using Xle.Services.ScreenModel;

namespace Xle.Services.Rendering
{
    public interface IXleRenderer
    {
        Point PlayerDrawPoint { get; set; }

        void Draw(SpriteBatch spriteBatch);

        void Update(GameTime time);

        void DrawFrame(SpriteBatch spriteBatch, Color color);

        void DrawFrameHighlight(SpriteBatch spriteBatch, Color color);

        void DrawInnerFrameHighlight(SpriteBatch spriteBatch, int p1, int p2, int p3, int p4, Color color);

        void DrawFrameLine(SpriteBatch spriteBatch, int p1, int p2, int p3, int p4, Color color);

        void DrawObject(SpriteBatch spriteBatch, TextWindow wind);

        void DrawObject(SpriteBatch spriteBatch, ColorScheme mColorScheme);

        void DrawTile(SpriteBatch spriteBatch, int drawx, int drawy, int tile);

        void DrawMonster(SpriteBatch spriteBatch, int p1, int p2, int DisplayMonsterID);

        void DrawCharacterSprite(SpriteBatch spriteBatch, int rx, int ry, Direction facing, bool p1, int p2, bool p3, Color color);
    }

    [Singleton, InjectProperties]
    public class XleRenderer : IXleRenderer
    {
        private ICommandList commands;

        public int raftAnim;                // raft animation frame

        // TODO: Which of these are obsolete?
        private static double timeToNextRaftAnim = 0;

        private IPlayerAnimator playerAnimator;
        private readonly IRectangleRenderer rects;
        private IXleImages images;
        private IStatsDisplay statsDisplay;

        public XleRenderer(
            ICommandList commands,
            IXleImages images,
            IPlayerAnimator playerAnimator,
            IXleScreen screen,
            IRectangleRenderer rects,
            IStatsDisplay statsDisplay)
        {
            this.commands = commands;
            this.images = images;
            this.Screen = screen;
            this.rects = rects;
            this.playerAnimator = playerAnimator;
            this.statsDisplay = statsDisplay;
        }

        public GameState GameState { get; set; }
        public ITextArea TextArea { get; set; }
        public ITextAreaRenderer TextAreaRenderer { get; set; }
        public ITextRenderer TextRenderer { get; set; }
        public XleData Data { get; set; }
        public IXleGameFactory Factory { get; set; }

        private IXleScreen Screen { get; set; }

        public Size WindowSize { get; set; }

        private Size GameAreaSize { get { return new Size(640, 400); } }

        private Texture2D Tiles { get { return images.Tiles; } }

        private Player Player { get { return GameState.Player; } }

        public void DrawFrame(SpriteBatch spriteBatch, Color boxColor)
        {
            DrawFrameLine(spriteBatch, 0, 0, 1, GameAreaSize.Width, boxColor);
            DrawFrameLine(spriteBatch, 0, 0, 0, GameAreaSize.Height - 2, boxColor);
            DrawFrameLine(spriteBatch, 0, GameAreaSize.Height - 16, 1, GameAreaSize.Width, boxColor);
            DrawFrameLine(spriteBatch, GameAreaSize.Width - 12, 0, 0, GameAreaSize.Height - 2, boxColor);
        }

        public void DrawFrameHighlight(SpriteBatch spriteBatch, Color innerColor)
        {
            DrawInnerFrameHighlight(spriteBatch, 0, 0, 1, GameAreaSize.Width, innerColor);
            DrawInnerFrameHighlight(spriteBatch, 0, 0, 0, GameAreaSize.Height - 2, innerColor);
            DrawInnerFrameHighlight(spriteBatch, 0, GameAreaSize.Height - 16, 1, GameAreaSize.Width + 2, innerColor);
            DrawInnerFrameHighlight(spriteBatch, GameAreaSize.Width - 12, 0, 0, GameAreaSize.Height - 2, innerColor);
        }

        /// <summary>
        /// This function draws a single colored line at the point specified.	
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="direction"></param>
        /// <param name="length"></param>
        /// <param name="boxColor"></param>
        public void DrawFrameLine(SpriteBatch spriteBatch, 
                        int left, int top, int direction,
                      int length, Color boxColor)
        {
            int boxWidth = 12;

            top += 4;

            if (direction == 1)
            {
                boxWidth -= 2;

                FillRect(spriteBatch, left, top, length, boxWidth, boxColor);
            }
            else
            {
                length -= 4;

                FillRect(spriteBatch, left, top, boxWidth, length, boxColor);
            }
        }

        public void DrawInnerFrameHighlight(SpriteBatch spriteBatch, int left, int top, int direction,
                      int length, Color innerColor)
        {
            int boxWidth = 12;
            int innerOffsetH = 8;
            int innerOffsetV = 4;
            int innerWidth = 2;

            top += 2;

            if (direction == 1)
            {
                FillRect(spriteBatch,
                    left + innerOffsetH,
                    top + innerOffsetV,
                    length - boxWidth + 2,
                    innerWidth,
                    innerColor);
            }
            else
            {

                FillRect(spriteBatch,
                    left + innerOffsetH,
                    top + innerOffsetV,
                    innerWidth + 2,
                    length - boxWidth,
                    innerColor);

            }

        }

        private void WriteText(SpriteBatch spriteBatch, int px, int py, string theText)
        {
            TextRenderer.WriteText(spriteBatch, px, py, theText);
        }

        private void WriteText(SpriteBatch spriteBatch, int px, int py, string theText, Color c)
        {
            TextRenderer.WriteText(spriteBatch, px, py, theText, c);
        }

        private void WriteText(SpriteBatch spriteBatch, int px, int py, string theText, Color[] coloring)
        {
            TextRenderer.WriteText(spriteBatch, px, py, theText, coloring);
        }

        public void DrawTile(SpriteBatch spriteBatch, int px, int py, int tile)
        {
            int tx, ty;

            Rectangle tileRect;
            Rectangle destRect;

            tx = tile % 16 * 16;
            ty = tile / 16 * 16;

            tileRect = new Rectangle(tx, ty, 16, 16);
            destRect = new Rectangle(px, py, 16, 16);

            spriteBatch.Draw(Tiles, destRect, tileRect, Color.White);
        }

        /// <summary>
        /// Draws monsters on the outside maps.
        /// </summary>
        /// <param name="px">The x position of the monster, in screen pixels.</param>
        /// <param name="py">The y position of the monster, in screen pixels.</param>
        /// <param name="monst"></param>
        public void DrawMonster(SpriteBatch spriteBatch, int px, int py, int monst)
        {
            int tx, ty;

            Rectangle monstRect;
            Rectangle destRect;
            var size = Data.OverworldMonsterSize;

            tx = (monst % 8) * size.Width;
            ty = (monst / 8) * size.Height;

            monstRect = new Rectangle(tx, ty, size.Width, size.Height);
            destRect = new Rectangle(px, py, size.Width, size.Height);

            if (Factory.Monsters != null)
            {
                spriteBatch.Draw(Factory.Monsters, destRect, monstRect, Color.White);
            }

        }

        public Point PlayerDrawPoint { get; set; }

        /// <summary>
        /// Draws the player character.
        /// </summary>
        /// <param name="animating"></param>
        /// <param name="animFrame"></param>
        /// <param name="vertLine"></param>
        private void DrawCharacter(SpriteBatch spriteBatch, bool animating, int animFrame, int vertLine)
        {
            DrawCharacter(spriteBatch, animating, animFrame, vertLine, Player.RenderColor);
        }

        private void DrawCharacter(SpriteBatch spriteBatch, bool animating, int animFrame, int vertLine, Color clr)
        {
            int px = vertLine + 16;
            int py = 16 + 7 * 16;
            int width = (624 - px) / 16;

            px += 11 * 16;
            PlayerDrawPoint = new Point(px, py);

            DrawCharacterSprite(spriteBatch, px, py, GameState.Player.FaceDirection, animating, animFrame, true, clr);

            CharRect = new Rectangle(px, py, 32, 32);
        }

        public void DrawCharacterSprite(SpriteBatch spriteBatch, int destx, int desty, Direction facing, bool animating, int animFrame, bool allowPingPong, Color clr)
        {
            int tx = 0, ty;

            Rectangle charRect;
            Rectangle destRect;

            if (allowPingPong && (facing == Direction.North || facing == Direction.South))
            {
                animFrame %= 4;

                // ping-pong animation
                if (animFrame == 3)
                    animFrame = 1;
            }
            else
            {
                animFrame %= 3;
            }

            if (animating)
                tx = (1 + animFrame) * 32;

            ty = ((int)facing - 1) * 32;

            charRect = new Rectangle(tx, ty, 32, 32);
            destRect = new Rectangle(destx, desty, 32, 32);

            spriteBatch.Draw(Factory.Character, destRect, charRect, clr);
        }

        public Rectangle CharRect { get; private set; }

        /// <summary>
        /// Draws the rafts that should be on the screen.
        /// </summary>
        /// <param name="inRect"></param>
        private void DrawRafts(SpriteBatch spriteBatch, Rectangle inRect)
        {
            Player player = GameState.Player;
            int tx, ty;
            int lx = inRect.Left;
            int width = inRect.Width;
            int px = lx + (width / 16) / 2 * 16;
            int py = 128;
            int rx, ry;
            Rectangle charRect;
            Rectangle destRect;


            foreach (var raft in player.Rafts)
            {
                if (raft.RaftImage > 0)
                    raftAnim %= 4;
                else
                    raftAnim = 1 + raftAnim % 3;

                int sourceX = raftAnim * 32;
                int sourceY = 256;

                tx = sourceX;
                ty = sourceY;

                if (GameState.Map.MapID != raft.MapNumber)
                    continue;
                if (raft.RaftImage > 0)
                {
                    tx += 32 * 4;
                }

                rx = px - (player.X - raft.X) * 16;
                ry = py - (player.Y - raft.Y) * 16;

                if (raft == player.BoardedRaft)
                {
                    if (player.RaftFaceDirection == Direction.West)
                    {
                        charRect = new Rectangle(tx, ty + 64, 32, 32);
                    }
                    else
                    {
                        charRect = new Rectangle(tx, ty + 32, 32, 32);
                    }
                }
                else
                {
                    charRect = new Rectangle(tx, ty, 32, 32);
                }

                if (rx >= lx && ry >= 16 && rx <= 592 && ry < 272)
                {
                    destRect = new Rectangle(rx, ry, 32, 32);

                    spriteBatch.Draw(Factory.Character, destRect, charRect, Color.White);
                }
            }
        }

        public XleMapRenderer MapRenderer { get { return GameState?.MapExtender?.MapRenderer; } }

        public ColorScheme ColorScheme => GameState.Map.ColorScheme;

        public void Draw(SpriteBatch spriteBatch)
        {
            if (GameState == null)
                return;

            DrawTextAreaBackColor(spriteBatch, GameState.Map.ColorScheme);

            Player player = GameState.Player;
            XleMap map = GameState.Map;

            int i = 0;
            Color boxColor = map.ColorScheme.FrameColor;
            Color innerColor = map.ColorScheme.FrameHighlightColor;
            int horizLine = 18 * 16;
            int vertLine = (38 - map.ColorScheme.MapAreaWidth) * 16;

            Screen.FontColor = map.ColorScheme.TextColor;
            Color menuColor = map.ColorScheme.TextColor;

            if (commands.IsLeftMenuActive)
            {
                menuColor = XleColor.Yellow;
            }

            DrawFrame(spriteBatch, boxColor);

            DrawFrameLine(spriteBatch, vertLine, 0, 0, horizLine + 12, boxColor);
            DrawFrameLine(spriteBatch, 0, horizLine, 1, GameAreaSize.Width, boxColor);

            DrawFrameHighlight(spriteBatch, innerColor);

            DrawInnerFrameHighlight(spriteBatch, vertLine, 0, 0, horizLine + 12, innerColor);
            DrawInnerFrameHighlight(spriteBatch, 0, horizLine, 1, GameAreaSize.Width, innerColor);

            Rectangle mapRect = RectangleX.FromLTRB
                (vertLine + 16, 16, GameAreaSize.Width - 16, horizLine);

            MapRenderer.Draw(spriteBatch, player.Location, player.FaceDirection, mapRect);

            i = 0;
            int cursorPos = 0;
            foreach (var cmd in commands.Items.ToList())
            {
                WriteText(spriteBatch, 48, 16 * (i + 1), cmd.Name, menuColor);

                if (cmd == commands.CurrentCommand)
                    cursorPos = i;

                i++;
            }

            WriteText(spriteBatch, 32, 16 * (cursorPos + 1), "`", menuColor);

            Color hpColor = statsDisplay.HPColor;

            WriteText(spriteBatch, 48, 16 * 15, "H.P. " + statsDisplay.HP, hpColor);
            WriteText(spriteBatch, 48, 16 * 16, "Food " + statsDisplay.Food, hpColor);
            WriteText(spriteBatch, 48, 16 * 17, "Gold " + statsDisplay.Gold, hpColor);

            TextAreaRenderer.Draw(spriteBatch, TextArea);

            if (map.AutoDrawPlayer)
            {
                DrawRafts(spriteBatch, mapRect);

                if (player.IsOnRaft == false)
                    DrawCharacter(spriteBatch, playerAnimator.Animating, playerAnimator.AnimFrame, vertLine);
            }

            if (Screen.PromptToContinue)
            {
                FillRect(spriteBatch, 192, 384, 17 * 16, 16, XleColor.Black);
                WriteText(spriteBatch, 208, 384, "(Press to Cont)", XleColor.Yellow);
            }
        }

        private void FillRect(SpriteBatch spriteBatch, int x, int y, int width, int height, Color color)
        {
            rects.Fill(spriteBatch, new Rectangle(x, y, width, height), color);
        }

        public void DrawObject(SpriteBatch spriteBatch, TextWindow textWindow)
        {
            if (textWindow.Visible == false)
                return;

            var location = textWindow.Location;
            var csb = textWindow.ColoredString;

            WriteText(spriteBatch,
                location.X * 16,
                location.Y * 16,
                csb.Text,
                csb.Colors);
        }

        /// <summary>
        /// Animates the rafts.
        /// </summary>
        private void AnimateRafts(GameTime time)
        {
            timeToNextRaftAnim -= time.ElapsedGameTime.TotalMilliseconds;

            if (timeToNextRaftAnim < 0)
            {
                raftAnim++;

                timeToNextRaftAnim += 100;
            }
        }

        private void DrawTextAreaBackColor(SpriteBatch spriteBatch, ColorScheme cs)
        {
            int hp = cs.HorizontalLinePosition * 16 + 8;

            FillRect(spriteBatch, 0, hp, 640, 400 - hp, cs.TextAreaBackColor);
        }

        public void DrawObject(SpriteBatch spriteBatch, ColorScheme cs)
        {
            //DrawScreenBackColor(spriteBatch, cs.BackColor);
            DrawTextAreaBackColor(spriteBatch, cs);

            // Draw the borders
            DrawFrame(spriteBatch, cs.FrameColor);
            DrawFrameLine(spriteBatch, 0, cs.HorizontalLinePosition * 16, 1, 640, cs.FrameColor);

            DrawFrameHighlight(spriteBatch, cs.FrameHighlightColor);
            DrawInnerFrameHighlight(spriteBatch, 0, cs.HorizontalLinePosition * 16, 1, 640, cs.FrameHighlightColor);
        }

        private void DrawScreenBackColor(SpriteBatch spriteBatch, Color backColor)
        {
            FillRect(spriteBatch, -20, -20, 800, 800, backColor);
        }

        public void Update(GameTime time)
        {
            MapRenderer?.Update(time);
            playerAnimator.Update(time);

            AnimateRafts(time);
        }
    }
}
