using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.TitleScreen
{
	class EraseGame : TitleState
	{
		public override void KeyDown(AgateLib.InputLib.KeyCode keyCode, string keyString)
		{
			NewState = new SecondMainMenu();
		}

		protected override void DrawFrame()
		{
			throw new NotImplementedException();
		}
	}
}
