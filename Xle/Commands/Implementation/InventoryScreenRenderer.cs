using AgateLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xle.Data;
using Xle.Rendering;
using Xle.XleSystem;

namespace Xle.Commands.Implementation
{
    [Singleton, InjectProperties]
    public class InventoryScreenRenderer : IRenderer
    {
        private IXleRenderer renderer;
        private XleData data;
        private XleSystemState systemState;
        private Color fontcolor;
        private Color bgcolor;
        private int inventoryScreen;

        public InventoryScreenRenderer(
            IXleRenderer renderer,
            XleData data,
            XleSystemState systemState)
        {
            this.renderer = renderer;
            this.data = data;
            this.systemState = systemState;

            UpdateColorScheme();
        }

        public ColorScheme ColorScheme { get; } = new ColorScheme();

        public ITextRenderer TextRenderer { get; set; }

        public IRectangleRenderer Rects { get; set; }

        public GameState GameState { get; set; }

        public int InventoryScreen
        {
            get => inventoryScreen;
            set
            {
                inventoryScreen = value;
                UpdateColorScheme();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var player = GameState.Player;

            //Rects.Fill(spriteBatch, new Rectangle(-100, -100, 1000, 1000), Color.White);
            //Rects.Fill(spriteBatch, new Rectangle(0, 0, 640, 400), bgcolor);

            // Draw the borders
            renderer.DrawFrame(spriteBatch, Color.Gray);
            renderer.DrawFrameLine(spriteBatch, 0, 128, 1, XleOptions.myWindowWidth, XleColor.Gray);

            renderer.DrawFrameHighlight(spriteBatch, Color.Yellow);
            renderer.DrawInnerFrameHighlight(spriteBatch, 0, 128, 1, XleOptions.myWindowWidth, XleColor.Yellow);

            // Draw the title
            Rects.Fill(spriteBatch, new Rectangle(176, 0, 288, 16), bgcolor);
            TextRenderer.WriteText(spriteBatch, 176, 0, " Player Inventory", fontcolor);

            // Draw the prompt
            Rects.Fill(spriteBatch, new Rectangle(144, 384, 336, 16), bgcolor);
            TextRenderer.WriteText(spriteBatch, 144, 384, " Hit key to continue", fontcolor);

            // Draw the top box
            TextRenderer.WriteText(spriteBatch, 48, 32, player.Name, fontcolor);

            string tempstring = "Level        ";
            tempstring += player.Level.ToString().PadLeft(2);
            TextRenderer.WriteText(spriteBatch, 48, 64, tempstring, fontcolor);

            string timeString = ((int)player.TimeDays).ToString().PadLeft(5);
            tempstring = "Time-days ";
            tempstring += timeString;
            TextRenderer.WriteText(spriteBatch, 48, 96, tempstring, fontcolor);

            tempstring = "Dexterity     ";
            tempstring += player.Attribute[Attributes.dexterity];
            TextRenderer.WriteText(spriteBatch, 336, 32, tempstring, fontcolor);

            tempstring = "Strength      ";
            tempstring += player.Attribute[Attributes.strength];
            TextRenderer.WriteText(spriteBatch, 336, 48, tempstring, fontcolor);

            tempstring = "Charm         ";
            tempstring += player.Attribute[Attributes.charm];
            TextRenderer.WriteText(spriteBatch, 336, 64, tempstring, fontcolor);

            tempstring = "Endurance     ";
            tempstring += player.Attribute[Attributes.endurance];
            TextRenderer.WriteText(spriteBatch, 336, 80, tempstring, fontcolor);

            tempstring = "Intelligence  ";
            tempstring += player.Attribute[Attributes.intelligence];
            TextRenderer.WriteText(spriteBatch, 336, 96, tempstring, fontcolor);

            if (InventoryScreen == 0)
            {
                TextRenderer.WriteText(spriteBatch, 80, 160, "Armor & Weapons  -  Quality", fontcolor);

                int yy = 11;

                foreach (var weapon in player.Weapons)
                {
                    var clr = fontcolor;

                    if (player.CurrentWeapon == weapon)
                        clr = XleColor.White;

                    TextRenderer.WriteText(spriteBatch, 128, ++yy * 16, weapon.BaseName(data), clr);
                    TextRenderer.WriteText(spriteBatch, 416, yy * 16, weapon.QualityName(data), clr);
                }

                yy++;

                foreach (var armor in player.Armor)
                {
                    var clr = fontcolor;

                    if (player.CurrentArmor == armor)
                        clr = XleColor.White;

                    TextRenderer.WriteText(spriteBatch, 128, ++yy * 16, armor.BaseName(data), clr);
                    TextRenderer.WriteText(spriteBatch, 416, yy * 16, armor.QualityName(data), clr);
                }
            }
            else if (InventoryScreen == 1)
            {

                // Draw the middle prompt
                Rects.Fill(spriteBatch, new Rectangle(160, 128, 288, 16), bgcolor);
                TextRenderer.WriteText(spriteBatch, 160, 128, " Other Possesions", fontcolor);

                string line;
                int yy = 9;
                int xx = 48;
                Color tempcolor;

                foreach (int i in data.ItemList.Keys)
                {
                    if (player.Items[i] > 0)
                    {
                        if (player.Hold == i)
                        {
                            tempcolor = XleColor.White;
                        }
                        else
                        {
                            tempcolor = fontcolor;
                        }

                        if (i == 17)
                        {
                            yy = 9;
                            xx = 352;
                        }
                        if (i == 24)
                        {
                            yy++;
                        }

                        line = player.Items[i].ToString() + " ";

                        if (i == systemState.Factory.MailItemID)
                        {
                            line += data.MapList[player.mailTown].Name + " ";
                        }

                        line += data.ItemList[i].Name;

                        TextRenderer.WriteText(spriteBatch, xx, ++yy * 16, line, tempcolor);
                    }
                }

            }

        }

        public void Update(GameTime time)
        {
            UpdateColorScheme();
        }

        private void UpdateColorScheme()
        {
            ColorScheme.BorderColor = XleColor.Black;

            // select the right colors for the screen.
            if (InventoryScreen == 0)
            {
                bgcolor = XleColor.Brown;
                fontcolor = XleColor.Yellow;
            }
            else
            {
                bgcolor = XleColor.Blue;
                fontcolor = XleColor.Cyan;
            }

            ColorScheme.BackColor = bgcolor;
        }
    }
}
