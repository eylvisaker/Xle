using ERY.Xle.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Extenders
{
	public class TreasureChestExtender : EventExtender
	{
		public new TreasureChestEvent TheEvent { get { return (TreasureChestEvent)base.TheEvent; } }

		public override void OnLoad(GameState state)
		{
			OpenIfMarked(state);
		}
		public virtual void SetAngry(GameState state)
		{
			state.Map.Guards.IsAngry = true;
		}

		public virtual bool MakesGuardsAngry { get { return true; } }

		public virtual void BeforeGiveItem(GameState state, ref int item, ref int count)
		{
		}

		public virtual void PrintObtainItemMessage(GameState state, int item, int count)
		{
			var itemName = XleCore.Data.ItemList[item].Name;
			var space = "aeiou".Contains(itemName.ToLowerInvariant()[0]) ? "n " : " ";

			XleCore.TextArea.PrintLine("You find a" + space + itemName + "!");
		}

		public virtual void PlayObtainItemSound(GameState state, int item, int count)
		{
			SoundMan.PlaySound(LotaSound.VeryGood);
		}

		public virtual string AlreadyOpenMessage { get { return "Chest already open."; } }

		private void PrintAlreadyOpenMessage()
		{
			XleCore.TextArea.PrintLine(AlreadyOpenMessage);
		}

		public virtual void PlayOpenChestSound()
		{
			SoundMan.PlaySound(LotaSound.OpenChest);
		}

		public virtual string TakeFailMessage { get { return "You can't \"take\" the whole chest."; } }

		public virtual void MarkChestAsOpen(GameState state)
		{
		}

		public virtual void OpenIfMarked(GameState state)
		{
		}


		protected virtual void UpdateCommand()
		{
			XleCore.TextArea.PrintLine(" chest");
		}

		public override bool Open(GameState state)
		{
			UpdateCommand();

			XleCore.TextArea.PrintLine();

			if (TheEvent.Closed == false)
			{
				PrintAlreadyOpenMessage();

				return true;
			}
			bool handled = false;

			Open(state, ref handled);
			if (handled)
				return true;

			PlayOpenChestSound();

			XleCore.Wait(state.GameSpeed.CastleOpenChestSoundTime);

			SetOpenTilesOnMap(state.Map);

			if (MakesGuardsAngry)
				SetAngry(state);

			if (TheEvent.ContainsItem)
			{
				int count = 1;
				int item = TheEvent.Contents;

				BeforeGiveItem(state, ref item, ref count);

				state.Player.Items[item] += count;

				PrintObtainItemMessage(state, item, count);
				PlayObtainItemSound(state, item, count);
			}
			else
			{
				int gd = TheEvent.Contents;

				XleCore.TextArea.PrintLine("You find " + gd.ToString() + " gold.");

				state.Player.Gold += gd;
				SoundMan.PlaySound(LotaSound.Sale);
			}

			TheEvent.Closed = false;

			XleCore.Wait(state.GameSpeed.CastleOpenChestTime);

			MarkChestAsOpen(state);

			return true;
		}
		public override bool Take(GameState state)
		{
			XleCore.TextArea.PrintLine("\n\n" + TakeFailMessage);

			return true;
		}

		public void SetOpenTilesOnMap(XleMap map)
		{
			var firstTile = map[TheEvent.X, TheEvent.Y];
			var chestGroup = map.TileSet.TileGroups.FirstOrDefault(
				x => x.GroupType == Maps.GroupType.Chest && x.Tiles.Contains(firstTile));
			var openChestGroup = (from grp in map.TileSet.TileGroups
								  where grp.GroupType == Maps.GroupType.OpenChest &&
									 grp.Tiles.All(x => x > firstTile)
								  orderby grp.Tiles.Min()
								  select grp).FirstOrDefault();

			TheEvent.Closed = false;

			if (chestGroup == null || openChestGroup == null)
				return;

			for (int j = TheEvent.Rectangle.Top; j < TheEvent.Rectangle.Bottom; j++)
			{
				for (int i = TheEvent.Rectangle.Left; i < TheEvent.Rectangle.Right; i++)
				{
					int index = chestGroup.Tiles.IndexOf(map[i, j]);

					if (index == -1 || index >= openChestGroup.Tiles.Count)
						continue;

					map[i, j] = openChestGroup.Tiles[index];
				}
			}
		}

	}
}
