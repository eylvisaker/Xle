using Xle.Data;
using Xle.Services.Menus;
using System.Collections.Generic;
using System.Linq;
using System;
using Xle.Services.XleSystem;
using Xle.Services.Game;

namespace Xle.Services.Commands.Implementation
{
    public abstract class MagicCommand : Command
    {
        public IXleSubMenu SubMenu { get; set; }
        public IQuickMenu QuickMenu { get; set; }
        public XleData Data { get; set; }
        public ISoundMan SoundMan { get; set; }
        public IXleGameControl GameControl { get; set; }
        public Random Random { get; set; }

        public override string Name
        {
            get { return "Magic"; }
        }
        protected virtual IEnumerable<MagicSpell> ValidMagic
        {
            get { yield break; }
        }

        public override void Execute()
        {
            var magics = ValidMagic.Where(x => Player.Items[x.ItemID] > 0).ToList();

            MagicSpell magic = RunMagicMenu(magics);

            if (magic == null)
                return;

            if (Player.Items[magic.ItemID] <= 0)
            {
                TextArea.PrintLine();
                TextArea.PrintLine("You have no " + magic.PluralName + ".", XleColor.White);
                return;
            }

            Player.Items[magic.ItemID]--;

            CastSpell(magic);
        }

        protected virtual void CastSpell(MagicSpell magic)
        {
            throw new NotImplementedException();
        }

        protected virtual MagicSpell RunMagicMenu(IList<MagicSpell> magics)
        {
            return MagicMenu(magics.ToArray());
        }

        protected MagicSpell MagicMenu(IList<MagicSpell> magics)
        {
            MenuItemList menu = new MenuItemList("Nothing");

            menu.AddRange(magics.Select(x => x.Name));

            int choice = SubMenu.SubMenu("Pick magic", 0, menu);

            if (choice == 0)
            {
                TextArea.PrintLine("Select no magic.", XleColor.White);
                return null;
            }

            return magics[choice - 1];
        }

    }
}
