using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.XleMapTypes.Extenders
{
	public interface IMuseumExtender : IMapExtender
	{
		Exhibit GetExhibitByTile(int p);

		void CheckExhibitStatus(GameState gameState);

		void InteractWithDisplay(Player player);

		bool PlayerHasCoin(Player player, Exhibit ex);

		void NeedsCoinMessage(Player player, Exhibit ex);

		void UseCoin(Player player, Exhibit ex);

		void PrintUseCoinMessage(Player player, Exhibit ex);
	}
}
