using System;
using System.Collections.Generic;
using System.Linq;

using AgateLib.Serialization.Xle;

using ERY.Xle.Services.Implementation;

namespace ERY.Xle.Maps
{
    public class TileSet : IXleSerializable, IEnumerable<KeyValuePair<int, TileInfo>>
    {
        Dictionary<int, TileInfo> mTiles = new Dictionary<int, TileInfo>();

        public TileSet()
        {
            TileGroups = new List<TileGroup>();
        }
        public TileInfo this[int index]
        {
            get
            {
                if (mTiles.ContainsKey(index) == false)
                {
                    SoundMan.PlaySound(LotaSound.Bad);
                    System.Diagnostics.Debug.Print("Tileset does not contain tile " + index.ToString());

                    return TileInfo.Normal;
                }

                return mTiles[index];
            }
            set
            {
                if (index > 5000) throw new ArgumentOutOfRangeException();
                if (index < 0) throw new ArgumentOutOfRangeException();

                mTiles[index] = value;
            }
        }


        public List<TileGroup> TileGroups { get; set; }

        public bool ContainsKey(int value)
        {
            return mTiles.ContainsKey(value);
        }

        void IXleSerializable.WriteData(XleSerializationInfo info)
        {
            List<TileInfo> tiles = new List<TileInfo>();

            if (mTiles.Count == 0)
                info.Write("Tiles", new int[] { });
            else
            {
                tiles.Capacity = mTiles.Keys.Max() + 1;
                for (int i = 0; i < mTiles.Keys.Max() + 1; i++)
                    tiles.Add(TileInfo.Normal);

                foreach (int key in mTiles.Keys)
                {
                    tiles[key] = mTiles[key];
                }

                info.Write("Tiles", tiles.Select(x => (int)x).ToArray(), NumericEncoding.Csv);
            }
            info.Write("TileGroups", TileGroups);
        }
        void IXleSerializable.ReadData(XleSerializationInfo info)
        {
            int[] tiles = info.ReadArray<int>("Tiles");

            for (int i = 0; i < tiles.Length; i++)
            {
                mTiles[i] = (TileInfo)tiles[i];
            }

            if (info.ContainsKey("TileGroups"))
                TileGroups = info.ReadList<TileGroup>("TileGroups");

            if (TileGroups == null)
                TileGroups = new List<TileGroup>();
        }

        IEnumerator<KeyValuePair<int, TileInfo>> IEnumerable<KeyValuePair<int, TileInfo>>.GetEnumerator()
        {
            return mTiles.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mTiles.GetEnumerator();
        }
    }
}
