using ERY.Xle.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services.Implementation;
using ERY.Xle.Data;

namespace ERY.Xle.XleEventTypes.Extenders
{
	public class TreasureChestExtender : EventExtender
    {
        public XleData Data { get; set; }

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
			var itemName = Data.ItemList[item].Name;
			var space = "aeiou".Contains(itemName.ToLowerInvariant()[0]) ? "n " : " ";

			TextArea.PrintLine("You find a" + space + itemName + "!");
		}

		public virtual void PlayObtainItemSound(GameState state, int item, int count)
		{
			SoundMan.PlaySound(LotaSound.VeryGood);
		}

		public virtual string AlreadyOpenMessage { get { return "Chest already open."; } }

		private void PrintAlreadyOpenMessage()
		{
			TextArea.PrintLine(AlreadyOpenMessage);
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
			TextArea.PrintLine(" chest");
		}

		public override bool Open(GameState state)
		{
			UpdateCommand();

			TextArea.PrintLine();

			if (TheEvent.Closed == false)
			{
				PrintAlreadyOpenMessage();

				return true;
			}

			PlayOpenChestSound();

			GameControl.Wait(GameState.GameSpeed.CastleOpenChestSoundTime);

			SetOpenTilesOnMap(GameState.Map);

			if (MakesGuardsAngry)
				SetAngry(GameState);

			if (TheEvent.ContainsItem)
			{
				int count = 1;
				int item = TheEvent.Contents;

				BeforeGiveItem(GameState, ref item, ref count);

				GameState.Player.Items[item] += count;

				PrintObtainItemMessage(GameState, item, count);
				PlayObtainItemSound(GameState, item, count);
			}
			else
			{
				int gd = TheEvent.Contents;

				TextArea.PrintLine("You find " + gd.ToString() + " gold.");

				GameState.Player.Gold += gd;
				SoundMan.PlaySound(LotaSound.Sale);
			}

			TheEvent.Closed = false;

			GameControl.Wait(GameState.GameSpeed.CastleOpenChestTime);

			MarkChestAsOpen(GameState);

			return true;
		}
		public override bool Take(GameState state)
		{
			TextArea.PrintLine("\n\n" + TakeFailMessage);

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
