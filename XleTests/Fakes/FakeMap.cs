﻿using System;

using Xle.Maps;

namespace Xle
{
    class FakeMap : XleMap
    {
        public override void InitializeMap(int width, int height)
        {
            throw new NotImplementedException();
        }
        public override int Width
        {
            get { throw new NotImplementedException(); }
        }
        public override int Height
        {
            get { throw new NotImplementedException(); }
        }
        public override int this[int xx, int yy]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}