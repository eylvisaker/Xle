using AgateLib.Geometry;
using ERY.Xle.XleMapTypes;
using ERY.Xle.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Museum
{
	class MuseumExtender : NullMuseumExtender
	{
		public override void BeforeEntry(GameState state, ref int targetEntryPoint)
		{
			if (targetEntryPoint < 3)
				targetEntryPoint = state.Story().MuseumEntryPoint;
		}

		public override void PlayerUse(GameState state, int item, ref bool handled)
		{
			// twist gold armband
			if (item == (int)LotaItem.GoldArmband)
			{
				Point faceDir = Map3D.StepDirection(state.Player.FaceDirection);
				Point test = new Point(state.Player.X + faceDir.X, state.Player.Y + faceDir.Y);

				// door value
				if (TheMap[test.X, test.Y] == 0x02)
				{
					XleCore.Wait(1000);

					foreach(var entry in state.Map.EntryPoints)
					{
						if (entry.Location == state.Player.Location)
						{
							state.Story().MuseumEntryPoint = state.Map.EntryPoints.IndexOf(entry);
						}
					}

					TheMap.LeaveMap(state.Player);

				}
				else
				{
					XleCore.TextArea.PrintLine("The gold armband hums softly.");
				}

				handled = true;
				return;
			}
		}
	}
}
