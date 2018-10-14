
using ERY.Xle.Maps.Museums;
using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using Microsoft.Xna.Framework;

namespace ERY.Xle.Services.Rendering.Maps
{
    public class MuseumRenderer : Map3DRenderer
    {
        public ITextRenderer TextRenderer { get; set; }
        public MuseumExtender MuseumExtender { get { return (MuseumExtender)Extender; } }

        protected override ExtraType GetExtraType(int val, int side)
        {
            if (val >= 0x50 && val <= 0x5f)
            {
                if (side == -1) return ExtraType.DisplayCaseLeft;
                if (side == 1) return ExtraType.DisplayCaseRight;
            }
            if (val == 1)
            {
                if (side == -1) return ExtraType.TorchLeft;
                if (side == 1) return ExtraType.TorchRight;
            }

            return ExtraType.None;
        }
        protected override bool ExtraScale
        {
            get
            {
                return true;
            }
        }

        #region --- Museum Exhibits ---

        public Exhibit mCloseup;
        public bool mDrawStatic;

        protected override void DrawCloseupImpl(Rectangle inRect)
        {
            Rectangle displayRect = ExhibitCloseupRect;
            Rectangle screenDisplayRect = displayRect;

            screenDisplayRect.X += inRect.X;
            screenDisplayRect.Y += inRect.Y;

            if (mDrawStatic == false)
            {
                Surfaces.ExhibitOpen.Draw(inRect);

                mCloseup.Draw(screenDisplayRect);
            }
            else
            {
                Surfaces.ExhibitClosed.Draw(inRect);

                if (AnimateExhibits)
                {
                    Display.FillRect(screenDisplayRect, XleColor.DarkGray);
                    DrawExhibitStatic(inRect, displayRect, mCloseup.ExhibitColor);
                }
            }


            DrawExhibitText(inRect, mCloseup);

        }

        protected virtual Rectangle ExhibitCloseupRect
        {
            get { return new Rectangle(64, 64, 240, 128); }
        }

        #endregion

        protected override Color ExhibitColor(int val)
        {
            var exhibit = MuseumExtender.GetExhibitByTile(val);

            if (exhibit == null)
                return base.ExhibitColor(val);

            return exhibit.ExhibitColor;
        }

        protected override void DrawMuseumExhibit(int distance, Rectangle destRect, int val)
        {
            var exhibit = MuseumExtender.GetExhibitByTile(val);

            if (distance == 1)
                DrawExhibitText(destRect, exhibit);
        }

        private void DrawExhibitText(Rectangle destRect, Exhibit exhibit)
        {
            int px = 176;
            int py = 208;

            int textLength = exhibit.Name.Length;

            px -= (textLength / 2) * 16;

            px += destRect.X;
            py += destRect.Y;

            AgateLib.DisplayLib.Display.FillRect(px, py, textLength * 16, 16, Color.Black);

            Color clr = exhibit.TitleColor;
            TextRenderer.WriteText(px, py, exhibit.Name, clr);
        }

    }
}
