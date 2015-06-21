using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Platform;
using ERY.Xle.Maps;
using ERY.Xle.Maps.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services;
using ERY.Xle.Services.Implementation;

namespace ERY.Xle.Rendering
{
    public class XleRenderer : IXleRenderer
    {
        private ICommandList commands;

        public int raftAnim;				// raft animation frame

        // TODO: Which of these are obsolete?
        static double lastRaftAnim = 0;

        private IPlayerAnimator playerAnimator;

        public XleRenderer(
            ICommandList commands, 
            IXleInput input,
            IPlayerAnimator playerAnimator,
            IXleScreen screen)
        {
            this.commands = commands;
            this.Input = input;
            this.Screen = screen;
            this.playerAnimator = playerAnimator;

            Screen.Draw += Screen_Draw;
            Screen.Update += Screen_Update;
        }

        void Screen_Update(object sender, EventArgs e)
        {
            UpdateAnim();
        }

        void Screen_Draw(object sender, EventArgs e)
        {
            Draw();
        }

        public GameState GameState { get; set; }
        public ISoundMan SoundMan { get; set; }
        public ITextArea TextArea { get; set; }
        public ITextAreaRenderer TextAreaRenderer { get; set; }
        public ITextRenderer TextRenderer { get; set; }
        IXleScreen Screen { get; set; }
        IXleInput Input { get; set; }

        public Size WindowSize { get; set; }

        Size GameAreaSize { get { return new Size(640, 400); } }

        public Surface Tiles { get; set; }

        bool mOverrideHPColor;
        Color mHPColor;


        Player Player { get { return GameState.Player; } }

        public void DrawFrame(Color boxColor)
        {
            DrawFrameLine(0, 0, 1, GameAreaSize.Width, boxColor);
            DrawFrameLine(0, 0, 0, GameAreaSize.Height - 2, boxColor);
            DrawFrameLine(0, GameAreaSize.Height - 16, 1, GameAreaSize.Width, boxColor);
            DrawFrameLine(GameAreaSize.Width - 12, 0, 0, GameAreaSize.Height - 2, boxColor);
        }
        public void DrawFrameHighlight(Color innerColor)
        {
            DrawInnerFrameHighlight(0, 0, 1, GameAreaSize.Width, innerColor);
            DrawInnerFrameHighlight(0, 0, 0, GameAreaSize.Height - 2, innerColor);
            DrawInnerFrameHighlight(0, GameAreaSize.Height - 16, 1, GameAreaSize.Width + 2, innerColor);
            DrawInnerFrameHighlight(GameAreaSize.Width - 12, 0, 0, GameAreaSize.Height - 2, innerColor);

        }

        /// <summary>
        /// This function draws a single colored line at the point specified.	
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="direction"></param>
        /// <param name="length"></param>
        /// <param name="boxColor"></param>
        public void DrawFrameLine(int left, int top, int direction,
                      int length, Color boxColor)
        {
            int boxWidth = 12;

            top += 4;

            if (direction == 1)
            {
                boxWidth -= 2;

                Display.FillRect(left, top, length, boxWidth, boxColor);
            }
            else
            {
                length -= 4;

                Display.FillRect(left, top, boxWidth, length, boxColor);
            }
        }

        public void DrawInnerFrameHighlight(int left, int top, int direction,
                      int length, Color innerColor)
        {
            int boxWidth = 12;
            int innerOffsetH = 8;
            int innerOffsetV = 4;
            int innerWidth = 2;

            top += 2;

            if (direction == 1)
            {
                Display.FillRect(left + innerOffsetH,
                    top + innerOffsetV,
                    length - boxWidth + 2,
                    innerWidth,
                    innerColor);
            }
            else
            {

                Display.FillRect(left + innerOffsetH,
                    top + innerOffsetV,
                    innerWidth + 2,
                    length - boxWidth,
                    innerColor);

            }

        }

        void WriteText(int px, int py, string theText)
        {
            TextRenderer.WriteText(px, py, theText);
        }
        void WriteText(int px, int py, string theText, Color c)
        {
            TextRenderer.WriteText(px, py, theText, c);
        }
        void WriteText(int px, int py, string theText, Color[] coloring)
        {
            TextRenderer.WriteText(px, py, theText, coloring);
        }

        public void DrawTile(int px, int py, int tile)
        {
            int tx, ty;

            Rectangle tileRect;
            Rectangle destRect;

            tx = tile % 16 * 16;
            ty = (int)(tile / 16) * 16;

            tileRect = new Rectangle(tx, ty, 16, 16);
            destRect = new Rectangle(px, py, 16, 16);

            Tiles.Draw(tileRect, destRect);
        }

        /// <summary>
        /// Draws monsters on the outside maps.
        /// </summary>
        /// <param name="px">The x position of the monster, in screen pixels.</param>
        /// <param name="py">The y position of the monster, in screen pixels.</param>
        /// <param name="monst"></param>
        public void DrawMonster(int px, int py, int monst)
        {
            int tx, ty;

            Rectangle monstRect;
            Rectangle destRect;
            var size = XleCore.Data.OverworldMonsterSize;

            tx = (monst % 8) * size.Width;
            ty = (monst / 8) * size.Height;

            monstRect = new Rectangle(tx, ty, size.Width, size.Height);
            destRect = new Rectangle(px, py, size.Width, size.Height);

            if (XleCore.Factory.Monsters != null)
                XleCore.Factory.Monsters.Draw(monstRect, destRect);

        }

        public Point PlayerDrawPoint { get; set; }

        /// <summary>
        /// Draws the player character.
        /// </summary>
        /// <param name="animFrame"></param>
        /// <param name="vertLine"></param>
        void DrawCharacter(bool animating, int animFrame, int vertLine)
        {
            DrawCharacter(animating, animFrame, vertLine, Player.RenderColor);
        }
        void DrawCharacter(bool animating, int animFrame, int vertLine, Color clr)
        {
            int px = vertLine + 16;
            int py = 16 + 7 * 16;
            int width = (624 - px) / 16;

            px += 11 * 16;
            PlayerDrawPoint = new Point(px, py);

            DrawCharacterSprite(px, py, XleCore.GameState.Player.FaceDirection, animating, animFrame, true, clr);

            CharRect = new Rectangle(px, py, 32, 32);
        }

        public void DrawCharacterSprite(int destx, int desty, Direction facing, bool animating, int animFrame, bool allowPingPong, Color clr)
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

            XleCore.Factory.Character.Color = clr;
            XleCore.Factory.Character.Draw(charRect, destRect);
        }

        public Rectangle CharRect { get; private set; }

        /// <summary>
        /// Draws the rafts that should be on the screen.
        /// </summary>
        /// <param name="inRect"></param>
        void DrawRafts(Rectangle inRect)
        {
            Player player = XleCore.GameState.Player;
            int tx, ty;
            int lx = inRect.Left;
            int width = inRect.Width;
            int px = lx + (int)((width / 16) / 2) * 16;
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

                if (XleCore.GameState.Map.MapID != raft.MapNumber)
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

                    XleCore.Factory.Character.Draw(charRect, destRect);
                }
            }
        }

        public Action ReplacementDrawMethod { get; set; }

        public XleMapRenderer MapRenderer { get { return XleCore.GameState.MapExtender.MapRenderer; } }

        public void Draw()
        {
            if (XleCore.GameState == null)
                return;

            if (ReplacementDrawMethod != null)
            {
                ReplacementDrawMethod();
                return;
            }

            SetProjectionAndBackColors(XleCore.GameState.Map.ColorScheme);

            Player player = XleCore.GameState.Player;
            XleMap map = XleCore.GameState.Map;

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

            DrawFrame(boxColor);

            DrawFrameLine(vertLine, 0, 0, horizLine + 12, boxColor);
            DrawFrameLine(0, horizLine, 1, GameAreaSize.Width, boxColor);

            DrawFrameHighlight(innerColor);

            DrawInnerFrameHighlight(vertLine, 0, 0, horizLine + 12, innerColor);
            DrawInnerFrameHighlight(0, horizLine, 1, GameAreaSize.Width, innerColor);

            Rectangle mapRect = Rectangle.FromLTRB
                (vertLine + 16, 16, GameAreaSize.Width - 16, horizLine);

            MapRenderer.Draw(player.Location, player.FaceDirection, mapRect);

            i = 0;
            int cursorPos = 0;
            foreach (var cmd in commands.Items)
            {
                WriteText(48, 16 * (i + 1), cmd.Name, menuColor);

                if (cmd == commands.CurrentCommand)
                    cursorPos = i;

                i++;
            }

            WriteText(32, 16 * (cursorPos + 1), "`", menuColor);

            Color hpColor = map.ColorScheme.TextColor;
            if (mOverrideHPColor)
                hpColor = mHPColor;

            WriteText(48, 16 * 15, "H.P. " + player.HP.ToString(), hpColor);
            WriteText(48, 16 * 16, "Food " + ((int)player.Food).ToString(), hpColor);
            WriteText(48, 16 * 17, "Gold " + player.Gold.ToString(), hpColor);

            TextAreaRenderer.Draw(TextArea);

            if (map.AutoDrawPlayer)
            {
                DrawRafts(mapRect);

                if (player.IsOnRaft == false)
                    DrawCharacter(playerAnimator.Animating, playerAnimator.AnimFrame, vertLine);
            }

            if (Input.PromptToContinue)
            {
                Display.FillRect(192, 384, 17 * 16, 16, XleColor.Black);
                WriteText(208, 384, "(Press to Cont)", XleColor.Yellow);
            }
        }

        public void DrawObject(TextWindow textWindow)
        {
            if (textWindow.Visible == false)
                return;

            var location = textWindow.Location;
            var csb = textWindow.ColoredString;

            WriteText(location.X * 16, location.Y * 16,
                csb.Text, csb.Colors);
        }

        public void FlashHPWhile(Color clr, Color clr2, Func<bool> pred)
        {
            Color oldClr = mHPColor;
            Color lastColor = clr2;

            mOverrideHPColor = true;
            int count = 0;

            while (pred())
            {
                if (lastColor == clr)
                    lastColor = clr2;
                else
                    lastColor = clr;

                mHPColor = lastColor;

                XleCore.Wait(80);

                count++;

                if (count > 10000 / 80)
                    break;
            }

            mOverrideHPColor = false;
            mHPColor = oldClr;

        }

        /// <summary>
        /// Animates the rafts.
        /// </summary>
        void RaftAnim()
        {
            if (lastRaftAnim + 100 < Timing.TotalMilliseconds)
            {
                raftAnim++;

                lastRaftAnim = Timing.TotalMilliseconds;
            }
        }

        public void LoadTiles(string tileset)
        {
            if (tileset.EndsWith(".png") == false)
                tileset += ".png";

            Tiles = new Surface(tileset) { InterpolationHint = InterpolationMode.Fastest };
        }

        public void UpdateAnim()
        {
            RaftAnim();
        }

        public void SetProjectionAndBackColors(ColorScheme cs)
        {
            Display.Clear(cs.BorderColor);
            int hp = cs.HorizontalLinePosition * 16 + 8;

            Display.FillRect(new Rectangle(0, 0, 640, 400), cs.BackColor);
            Display.FillRect(0, hp, 640, 400 - hp, cs.TextAreaBackColor);
        }

        public void DrawObject(ColorScheme cs)
        {
            SetProjectionAndBackColors(cs);

            // Draw the borders
            DrawFrame(cs.FrameColor);
            DrawFrameLine(0, cs.HorizontalLinePosition * 16, 1, 640, cs.FrameColor);

            DrawFrameHighlight(cs.FrameHighlightColor);
            DrawInnerFrameHighlight(0, cs.HorizontalLinePosition * 16, 1, 640, cs.FrameHighlightColor);

        }


        public void FlashHPWhileSound(Color clr, Color? clr2 = null)
        {
            FlashHPWhile(clr, clr2 ?? Screen.FontColor, () => SoundMan.IsAnyPlaying());
        }
    }
}
