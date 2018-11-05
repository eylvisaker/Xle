using Xle.Data;
using Xle.Menus;
using System.Collections.Generic;
using System.Linq;
using System;
using Xle.XleSystem;
using Xle.Game;
using System.Threading.Tasks;

namespace Xle.Commands.Implementation
{
    public interface IMagicCommand : ICommand { }

    public abstract class MagicCommand : Command, IMagicCommand
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

        public override async Task Execute()
        {
            var magics = ValidMagic.Where(x => Player.Items[x.ItemID] > 0).ToList();

            MagicSpell magic = await RunMagicMenu(magics);

            if (magic == null)
                return;

            if (Player.Items[magic.ItemID] <= 0)
            {
                await TextArea.PrintLine();
                await TextArea.PrintLine("You have no " + magic.PluralName + ".", XleColor.White);
                return;
            }

            Player.Items[magic.ItemID]--;

           await CastSpell(magic);
        }

        protected virtual async Task CastSpell(MagicSpell magic)
        {
            await GameState.MapExtender.CastSpell(magic);
        }

        protected virtual Task<MagicSpell> RunMagicMenu(IList<MagicSpell> magics)
        {
            return MagicMenu(magics.ToArray());
        }

        protected async Task<MagicSpell> MagicMenu(IList<MagicSpell> magics)
        {
            MenuItemList menu = new MenuItemList("Nothing");

            menu.AddRange(magics.Select(x => x.Name));

            int choice = await SubMenu.SubMenu("Pick magic", 0, menu);

            if (choice == 0)
            {
                await TextArea.PrintLine("Select no magic.", XleColor.White);
                return null;
            }

            return magics[choice - 1];
        }

    }
}
