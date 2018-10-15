using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ERY.Xle.Data
{
    public class DungeonMonsterData
    {
        public DungeonMonsterData()
        {
            Images = new List<DungeonMonsterImage>();
        }

        public string Name { get; set; }
        public int ID { get; set; }
        public string ImageFile { get; set; }

        public Texture2D Image { get; set; }

        public List<DungeonMonsterImage> Images { get; private set; }

        public bool IsValid
        {
            get
            {
                if (Images.Count == 0) return false;
                if (string.IsNullOrWhiteSpace(ImageFile)) return false;

                foreach (var image in Images)
                {
                    if (image.SourceRects.Count == 0)
                        return false;
                }

                return true;
            }
        }
    }

    public class DungeonMonsterImage
    {
        public DungeonMonsterImage()
        {
            SourceRects = new List<Rectangle>();
        }

        public Point DrawPoint { get; set; }
        public List<Rectangle> SourceRects { get; private set; }
    }
}
