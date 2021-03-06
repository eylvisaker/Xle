﻿using Xle.Serialization;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Xle.Maps.XleMapTypes
{
    public class Town : Map2D
    {
        public List<int> Mail { get; set; }

        #region --- Construction and Serialization ---

        public Town()
        {
            HasRoofs = true;
            HasGuards = true;
        }

        protected override void WriteData(XleSerializationInfo info)
        {
            base.WriteData(info);

            info.Write("Mail", Mail.ToArray());
        }
        protected override void ReadData(XleSerializationInfo info)
        {
            base.ReadData(info);

            Mail = info.ReadInt32Array("Mail").ToList();
        }

        #endregion

        public override IEnumerable<string> AvailableTileImages
        {
            get
            {
                yield return "towntiles.png";
            }
        }

        public int RoofTile(int xx, int yy)
        {
            foreach (var r in Roofs)
            {
                Rectangle boundingRect = r.Rectangle;

                if (boundingRect.Contains(new Point(xx, yy)))
                {
                    var result = r[xx - r.X, yy - r.Y];

                    if (result == 0 || result == 127)
                        continue;

                    if (r.Open)
                        return 0;
                }
            }

            for (int i = 0; i < Roofs.Count; i++)
            {
                Roof r = Roofs[i];
                Rectangle boundingRect = r.Rectangle;

                if (boundingRect.Contains(new Point(xx, yy)))
                {
                    var result = r[xx - r.X, yy - r.Y];

                    if (result == 0 || result == 127)
                        continue;

                    return r[xx - r.X, yy - r.Y];
                }
            }

            return 0;
        }
    }
}
