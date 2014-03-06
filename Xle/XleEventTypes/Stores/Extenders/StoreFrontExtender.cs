using AgateLib.Geometry;
using AgateLib.InputLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores.Extenders
{
	public class StoreFrontExtender : StoreExtender
	{
		public new StoreFront TheEvent { get { return (StoreFront)base.TheEvent; } }

		public virtual void SetColorScheme(ColorScheme cs)
		{
		}

		protected void StoreSound(LotaSound sound)
		{
			SoundMan.PlaySoundSync(TheEvent.RedrawStore, sound);
		}

		protected void Wait(int howLong)
		{
			XleCore.Wait(howLong, TheEvent.RedrawStore);
		}
		protected void WaitForKey(params KeyCode[] keys)
		{
			XleCore.WaitForKey(TheEvent.RedrawStore, keys);
		}

		protected int QuickMenu(MenuItemList menu, int spaces)
		{
			return XleCore.QuickMenu(TheEvent.RedrawStore, menu, spaces);
		}
		protected int QuickMenu(MenuItemList menu, int spaces, int value)
		{
			return XleCore.QuickMenu(TheEvent.RedrawStore, menu, spaces, value);
		}
		protected int QuickMenu(MenuItemList menu, int spaces, int value, Color clrInit)
		{
			return XleCore.QuickMenu(TheEvent.RedrawStore, menu, spaces, value, clrInit);
		}
		protected int QuickMenu(MenuItemList menu, int spaces, int value, Color clrInit, Color clrChanged)
		{
			return XleCore.QuickMenu(TheEvent.RedrawStore, menu, spaces, value, clrInit, clrChanged);
		}

		protected int ChooseNumber(int max)
		{
			return XleCore.ChooseNumber(TheEvent.RedrawStore, max);
		}
	}
}
