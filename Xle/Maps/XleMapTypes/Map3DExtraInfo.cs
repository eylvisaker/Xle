using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Xle.Maps.XleMapTypes
{
    public class Map3DExtraInfo
    {
        private Dictionary<int, Map3DExtraImage> mImages = new Dictionary<int, Map3DExtraImage>();

        public Dictionary<int, Map3DExtraImage> Images
        {
            get { return mImages; }
        }
    }

    public class Map3DExtraImage
    {
        public Map3DExtraImage()
        {
            Animations = new List<Map3DExtraAnimation>();
        }

        public Rectangle SrcRect { get; set; }
        public Rectangle DestRect { get; set; }

        public List<Map3DExtraAnimation> Animations { get; private set; }
        public int CurrentAnimation { get; set; }
    }

    public class Map3DExtraAnimation
    {
        public Map3DExtraAnimation()
        {
            Images = new List<Map3DExtraImage>();
        }

        public List<Map3DExtraImage> Images { get; private set; }

        public double FrameTime { get; set; }
        public double TimeToNextFrame { get; set; }

        public int CurrentFrame { get; set; }
    }
}
