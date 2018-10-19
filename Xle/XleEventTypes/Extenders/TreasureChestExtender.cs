using Xle.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xle.Data;

namespace Xle.XleEventTypes.Extenders
{
    public class TreasureChestExtender : EventExtender
    {
        public XleData Data { get; set; }

        public new TreasureChestEvent TheEvent { get { return (TreasureChestEvent)base.TheEvent; } }

        public override void OnLoad()
        {
            OpenIfMarked();
        }
        public virtual void SetAngry()
        {
            Map.Guards.IsAngry = true;
        }

        public virtual bool MakesGuardsAngry { get { return true; } }

        public virtual void BeforeGiveItem(ref int item, ref int count)
        {
        }

        public virtual void PrintObtainItemMessage(int item, int count)
        {
            var itemName = Data.ItemList[item].Name;
            var space = "aeiou".Contains(itemName.ToLowerInvariant()[0]) ? "n " : " ";

            TextArea.PrintLine("You find a" + space + itemName + "!");
        }

        public virtual void PlayObtainItemSound(int item, int count)
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

        public virtual void MarkChestAsOpen()
        {
        }

        public virtual void OpenIfMarked()
        {
        }

        protected virtual void UpdateCommand()
        {
            TextArea.PrintLine(" chest");
        }

        public override bool Open()
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

            SetOpenTilesOnMap();

            if (MakesGuardsAngry)
                SetAngry();

            if (TheEvent.ContainsItem)
            {
                int count = 1;
                int item = TheEvent.Contents;

                BeforeGiveItem(ref item, ref count);

                GameState.Player.Items[item] += count;

                PrintObtainItemMessage(item, count);
                PlayObtainItemSound(item, count);
            }
            else
            {
                int gd = TheEvent.Contents;

                TextArea.PrintLine(string.Format("You find {0} gold.", gd));

                GameState.Player.Gold += gd;
                SoundMan.PlaySound(LotaSound.Sale);
            }

            TheEvent.Closed = false;

            GameControl.Wait(GameState.GameSpeed.CastleOpenChestTime);

            MarkChestAsOpen();

            return true;
        }
        public override bool Take()
        {
            TextArea.PrintLine("\n\n" + TakeFailMessage);

            return true;
        }

        public void SetOpenTilesOnMap()
        {
            var firstTile = Map[TheEvent.X, TheEvent.Y];
            var chestGroup = Map.TileSet.TileGroups.FirstOrDefault(
                x => x.GroupType == Maps.GroupType.Chest && x.Tiles.Contains(firstTile));
            var openChestGroup = (from grp in Map.TileSet.TileGroups
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
                    int index = chestGroup.Tiles.IndexOf(Map[i, j]);

                    if (index == -1 || index >= openChestGroup.Tiles.Count)
                        continue;

                    Map[i, j] = openChestGroup.Tiles[index];
                }
            }
        }

    }
}
