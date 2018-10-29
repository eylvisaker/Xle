using AgateLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Xle.Services.Rendering;
using Xle.Services.ScreenModel;

namespace Xle.XleEventTypes.Stores.Extenders
{
    [Transient, InjectProperties]
    public class StoreFrontRenderer : IRenderer
    {
        private Player Player => GameState.Player;

        public StoreFrontScreen Screen { get; set; } = new StoreFrontScreen();

        public GameState GameState { get; set; }
        public IXleRenderer Renderer { get; set; }
        public ITextAreaRenderer TextAreaRenderer { get; set; }
        public ITextRenderer TextRenderer { get; set; }

        public ITextArea TextArea { get; set; }

        public IRectangleRenderer Rects { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            Renderer.DrawObject(spriteBatch, Screen.ColorScheme);

            // Draw the title
            DrawTitle(spriteBatch, Screen.Title);

            foreach (var window in Screen.Windows)
            {
                Renderer.DrawObject(spriteBatch, window);
            }

            DrawGoldText(spriteBatch);

            TextAreaRenderer.Draw(spriteBatch, TextArea);
        }

        private void DrawGoldText(SpriteBatch spriteBatch)
        {
            if (Screen.ShowGoldText == false)
                return;

            string goldText;
            if (Screen.Robbing == false)
            {
                // Draw Gold
                goldText = " Gold: ";
                goldText += Player.Gold;
                goldText += " ";
            }
            else
            {
                // don't need gold if we're robbing it!
                goldText = " Robbery in progress ";
            }

            Rects.Fill(spriteBatch, new Rectangle(
                320 - (goldText.Length / 2) * 16,
                Screen.ColorScheme.HorizontalLinePosition * 16,
                goldText.Length * 16,
                14),
                Screen.ColorScheme.BackColor);

            TextRenderer.WriteText(spriteBatch, 320 - (goldText.Length / 2) * 16, 18 * 16, goldText, XleColor.White);

        }

        private void DrawTitle(SpriteBatch spriteBatch, string title)
        {
            if (string.IsNullOrEmpty(title))
                return;

            Rects.Fill(spriteBatch, new Rectangle(
                320 - (title.Length + 2) / 2 * 16, 
                0,
                (title.Length + 2) * 16, 
                16), 
                Screen.ColorScheme.BackColor);

            TextRenderer.WriteText(spriteBatch, 320 - (title.Length / 2) * 16, 0, title, Screen.ColorScheme.TitleColor);
        }

        public void Update(GameTime time) { }
    }
}
