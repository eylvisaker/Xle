using ERY.Xle.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders
{
	public interface IMuseumExtender : IMapExtender
	{
		[Obsolete]
		Exhibit GetExhibitByTile(int p);

		void CheckExhibitStatus(GameState gameState);

		void InteractWithDisplay(Player player);

		bool PlayerHasCoin(Player player, Exhibit ex);

		void NeedsCoinMessage(Player player, Exhibit ex);

		void UseCoin(Player player, Exhibit ex);

		void PrintUseCoinMessage(Player player, Exhibit ex);
	}
}
