using ERY.Xle.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services.Commands.Implementation
{
    public class MagicWithFancyPrompt : Magic
    {
        protected override MagicSpell RunMagicMenu(IList<MagicSpell> magics)
        {
            TextArea.PrintLine();
            TextArea.PrintLine();
            TextArea.PrintLine("Use which magic?", XleColor.Purple);
            TextArea.PrintLine();

            bool hasFlames = magics.Contains(Data.MagicSpells[1]);
            bool hasBolts = magics.Contains(Data.MagicSpells[2]);

            int defaultValue = 0;
            int otherStart = 2 - (hasBolts ? 0 : 1) - (hasFlames ? 0 : 1);
            bool anyOthers = otherStart < magics.Count;

            if (hasFlames == false)
            {
                defaultValue = 1;

                if (hasBolts == false)
                    defaultValue = 2;
            }

            var menu = new MenuItemList("Flame", "Bolt", anyOthers ? "Other" : "Nothing");

            int choice = QuickMenu.QuickMenu(menu, 2, defaultValue,
                XleColor.Purple, XleColor.White);

            if (choice == 0)
                return Data.MagicSpells[1];
            else if (choice == 1)
                return Data.MagicSpells[2];
            else
            {
                if (anyOthers == false)
                    return null;

                TextArea.PrintLine(" - select above", XleColor.White);
                TextArea.PrintLine();

                return MagicMenu(magics.Skip(otherStart).ToList());
            }
        }

    }
}
