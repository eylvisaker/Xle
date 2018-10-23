using AgateLib;
using Microsoft.Xna.Framework;
using System;
using Xle.Data;
using Xle.Services.Rendering;
using Xle.Services.XleSystem;

namespace Xle.Services.Commands.Implementation
{
    [Transient]
    public class Inventory : Command
    {
        private IXleRenderer renderer;
        private XleData data;
        private XleSystemState systemState;

        public Inventory(
            IXleRenderer renderer,
            XleData data,
            XleSystemState systemState)
        {
            this.renderer = renderer;
            this.data = data;
            this.systemState = systemState;
        }

        public ITextRenderer TextRenderer { get; set; }

        public override void Execute()
        {
            TextArea.PrintLine();

            var player = GameState.Player;

            int inventoryScreen = 0;

            string tempstring;
            Color bgcolor;
            Color fontcolor;

            throw new NotImplementedException();

            //using (var input = new SimpleInputHandler())
            //{
            //    Input.Handlers.Add(input);

            //    while (inventoryScreen < 2)
            //    {
            //        if (input.Keys.Any)
            //        {
            //            input.Keys.ReleaseAll();
            //            inventoryScreen++;
            //        }

            //        // select the right colors for the screen.
            //        if (inventoryScreen == 0)
            //        {
            //            bgcolor = XleColor.Brown;
            //            fontcolor = XleColor.Yellow;
            //        }
            //        else
            //        {
            //            bgcolor = XleColor.Blue;
            //            fontcolor = XleColor.Cyan;
            //        }

            //        throw new NotImplementedException();
            //Display.BeginFrame();
            //Display.Clear(XleColor.DarkGray);
            //Display.FillRect(new Rectangle(0, 0, 640, 400), bgcolor);

            //// Draw the borders
            //renderer.DrawFrame(Color.Gray);
            //renderer.DrawFrameLine(0, 128, 1, XleOptions.myWindowWidth, XleColor.Gray);

            //renderer.DrawFrameHighlight(Color.Yellow);
            //renderer.DrawInnerFrameHighlight(0, 128, 1, XleOptions.myWindowWidth, XleColor.Yellow);

            //// Draw the title
            //Display.FillRect(new Rectangle(176, 0, 288, 16), bgcolor);
            //TextRenderer.WriteText(176, 0, " Player Inventory", fontcolor);

            //// Draw the prompt
            //Display.FillRect(144, 384, 336, 16, bgcolor);
            //TextRenderer.WriteText(144, 384, " Hit key to continue", fontcolor);

            //// Draw the top box
            //TextRenderer.WriteText(48, 32, player.Name, fontcolor);

            //tempstring = "Level        ";
            //tempstring += player.Level.ToString().PadLeft(2);
            //TextRenderer.WriteText(48, 64, tempstring, fontcolor);

            //string timeString = ((int)player.TimeDays).ToString().PadLeft(5);
            //tempstring = "Time-days ";
            //tempstring += timeString;
            //TextRenderer.WriteText(48, 96, tempstring, fontcolor);

            //tempstring = "Dexterity     ";
            //tempstring += player.Attribute[Attributes.dexterity];
            //TextRenderer.WriteText(336, 32, tempstring, fontcolor);

            //tempstring = "Strength      ";
            //tempstring += player.Attribute[Attributes.strength];
            //TextRenderer.WriteText(336, 48, tempstring, fontcolor);

            //tempstring = "Charm         ";
            //tempstring += player.Attribute[Attributes.charm];
            //TextRenderer.WriteText(336, 64, tempstring, fontcolor);

            //tempstring = "Endurance     ";
            //tempstring += player.Attribute[Attributes.endurance];
            //TextRenderer.WriteText(336, 80, tempstring, fontcolor);

            //tempstring = "Intelligence  ";
            //tempstring += player.Attribute[Attributes.intelligence];
            //TextRenderer.WriteText(336, 96, tempstring, fontcolor);

            //if (inventoryScreen == 0)
            //{
            //    TextRenderer.WriteText(80, 160, "Armor & Weapons  -  Quality", fontcolor);

            //    int yy = 11;
            //    Color tempcolor;

            //    foreach (var weapon in player.Weapons)
            //    {
            //        var clr = fontcolor;

            //        if (player.CurrentWeapon == weapon)
            //            clr = XleColor.White;

            //        TextRenderer.WriteText(128, ++yy * 16, weapon.BaseName(data), clr);
            //        TextRenderer.WriteText(416, yy * 16, weapon.QualityName(data), clr);
            //    }

            //    yy++;

            //    foreach (var armor in player.Armor)
            //    {
            //        var clr = fontcolor;

            //        if (player.CurrentArmor == armor)
            //            clr = XleColor.White;

            //        TextRenderer.WriteText(128, ++yy * 16, armor.BaseName(data), clr);
            //        TextRenderer.WriteText(416, yy * 16, armor.QualityName(data), clr);
            //    }
            //}
            //else if (inventoryScreen == 1)
            //{

            //    // Draw the middle prompt
            //    Display.FillRect(160, 128, 288, 16, bgcolor);
            //    TextRenderer.WriteText(160, 128, " Other Possesions", fontcolor);

            //    string line;
            //    int yy = 9;
            //    int xx = 48;
            //    Color tempcolor;

            //    foreach (int i in data.ItemList.Keys)
            //    {
            //        if (player.Items[i] > 0)
            //        {
            //            if (player.Hold == i)
            //            {
            //                tempcolor = XleColor.White;
            //            }
            //            else
            //            {
            //                tempcolor = fontcolor;
            //            }

            //            if (i == 17)
            //            {
            //                yy = 9;
            //                xx = 352;
            //            }
            //            if (i == 24)
            //            {
            //                yy++;
            //            }

            //            line = player.Items[i].ToString() + " ";

            //            if (i == systemState.Factory.MailItemID)
            //            {
            //                line += data.MapList[player.mailTown].Name + " ";
            //            }

            //            line += data.ItemList[i].Name;

            //            TextRenderer.WriteText(xx, ++yy * 16, line, tempcolor);
            //        }
            //    }

            //}

            //Display.EndFrame();
            //AgateApp.KeepAlive();
            //    }
            //}
        }
    }
}
