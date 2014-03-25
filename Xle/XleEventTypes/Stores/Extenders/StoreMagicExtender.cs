using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores.Extenders
{
	public class StoreMagicExtender : StoreFrontExtender
	{
		public new StoreMagic TheEvent { get { return (StoreMagic)base.TheEvent; } }

		public virtual int GetItemValue(int choice)
		{
			throw new NotImplementedException();
		}

		public virtual int GetMaxCarry(int choice)
		{
			throw new NotImplementedException();
		}

		public virtual int MagicPrice(int id)
		{
			return TheEvent.MagicPrice(XleCore.Data.MagicSpells[id]);
		}

		public virtual IEnumerable<TextWindow> CreateStoreWindows()
		{
			yield return CreateWindow();
		}

		private TextWindow CreateWindow()
		{
			TextWindow window = new TextWindow();

			window.Location = new AgateLib.Geometry.Point(8, 2);

			window.WriteLine("General Purpose      Prices", XleColor.Blue);
			window.WriteLine("");
			window.WriteLine("1. Magic flame        " + MagicPrice(1));
			window.WriteLine("2. Firebolt           " + MagicPrice(2));
			window.WriteLine("");
			window.WriteLine("Dungeon use only     Prices", XleColor.Blue);
			window.WriteLine("");
			window.WriteLine("3. Befuddle spell     " + MagicPrice(3));
			window.WriteLine("4. Psycho strength    " + MagicPrice(4));
			window.WriteLine("5. Kill Flash         " + MagicPrice(5));
			window.WriteLine("");
			window.WriteLine("Outside use only     Prices", XleColor.Blue);
			window.WriteLine("");
			window.WriteLine("6. Seek spell         " + MagicPrice(6));
			return window;
		}

		public virtual IEnumerable<MagicSpell> AvailableSpells
		{
			get { return XleCore.Data.MagicSpells.Values; }
		}
	}
}
