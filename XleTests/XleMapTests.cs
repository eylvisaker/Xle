using ERY.Xle;
using ERY.Xle.Maps;
using ERY.Xle.XleEventTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.XleTests
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

    [TestClass]
    public class XleMapTests
    {
    }
}
