using Xle.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.Commands.Implementation
{
    public class MagicWithFancyPrompt : MagicCommand
    {
        protected override async Task<MagicSpell> RunMagicMenu(IList<MagicSpell> magics)
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine();
            await TextArea.PrintLine("Use which magic?", XleColor.Purple);
            await TextArea.PrintLine();

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

            int choice = await QuickMenu.QuickMenu(menu, 2, defaultValue,
                XleColor.Purple, XleColor.White);

            if (choice == 0)
                return Data.MagicSpells[1];
            else if (choice == 1)
                return Data.MagicSpells[2];
            else
            {
                if (anyOthers == false)
                    return null;

                await TextArea.PrintLine(" - select above", XleColor.White);
                await TextArea.PrintLine();

                return await MagicMenu(magics.Skip(otherStart).ToList());
            }
        }

    }
}
