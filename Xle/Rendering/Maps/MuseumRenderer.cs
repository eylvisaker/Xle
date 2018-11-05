
using AgateLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Xle.Maps.Museums;
using Xle.Maps.XleMapTypes.MuseumDisplays;

namespace Xle.Rendering.Maps
{
    public interface IMuseumRenderer : IXleMapRenderer
    {

    }

    [Transient]
    public class MuseumRenderer : Map3DRenderer, IMuseumRenderer
    {
        Exhibit mCloseup => RenderState.Closeup;
        bool mDrawStatic => RenderState.DrawStatic;

        public override bool AnimateExhibits => RenderState.AnimateExhibits;

        public override bool DrawCloseup => RenderState.DrawCloseup;

        public ITextRenderer TextRenderer { get; set; }

        public MuseumExtender MuseumExtender { get { return (MuseumExtender)Extender; } }

        public MuseumRenderState RenderState { get; set; }

        protected override void OnExtenderSet()
        {
            base.OnExtenderSet();

            RenderState = MuseumExtender.RenderState;
        }

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

        public override void Update(GameTime time)
        {
            base.Update(time);

            UpdateExhibits(time);
        }

        #region --- Museum Exhibits ---

        void UpdateExhibits(GameTime time)
        {
            mCloseup?.Update(time);
        }

        protected override void DrawCloseupImpl(SpriteBatch spriteBatch, Rectangle inRect)
        {
            Rectangle displayRect = ExhibitCloseupRect;
            Rectangle screenDisplayRect = displayRect;

            screenDisplayRect.X += inRect.X;
            screenDisplayRect.Y += inRect.Y;

            if (mDrawStatic == false)
            {
                //Surfaces.ExhibitOpen.Draw(inRect);

                spriteBatch.Draw(Surfaces.ExhibitOpen, inRect, Color.White);
                mCloseup.Draw(spriteBatch, screenDisplayRect);
            }
            else
            {
                //Surfaces.ExhibitClosed.Draw(inRect);
                spriteBatch.Draw(Surfaces.ExhibitClosed, inRect, Color.White);

                if (AnimateExhibits)
                {
                    FillRect(spriteBatch, screenDisplayRect, XleColor.DarkGray);
                    DrawExhibitStatic(spriteBatch, inRect, displayRect, mCloseup.ExhibitColor);
                }
            }

            DrawExhibitText(spriteBatch, inRect, mCloseup);
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

        public SpriteBatch spriteBatch { get; set; }

        protected override void DrawMuseumExhibit(SpriteBatch spriteBatch,
            int distance, Rectangle destRect, int val)
        {
            var exhibit = MuseumExtender.GetExhibitByTile(val);

            if (distance == 1)
                DrawExhibitText(spriteBatch, destRect, exhibit);
        }

        private void DrawExhibitText(SpriteBatch spriteBatch,
            Rectangle destRect, Exhibit exhibit)
        {
            int px = 176;
            int py = 208;

            int textLength = exhibit.Name.Length;

            px -= (textLength / 2) * 16;

            px += destRect.X;
            py += destRect.Y;

            FillRect(spriteBatch, px, py, textLength * 16, 16, Color.Black);

            Color clr = exhibit.TitleColor;
            TextRenderer.WriteText(spriteBatch, px, py, exhibit.Name, clr);
        }

    }
}
