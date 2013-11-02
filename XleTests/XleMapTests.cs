using ERY.Xle;
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
		protected override void DrawImpl(int x, int y, Direction faceDirection, AgateLib.Geometry.Rectangle inRect)
		{
			throw new NotImplementedException();
		}
		public override void GetBoxColors(out AgateLib.Geometry.Color boxColor, out AgateLib.Geometry.Color innerColor, out AgateLib.Geometry.Color fontColor, out int vertLine)
		{
			throw new NotImplementedException();
		}
		public override string[] MapMenu()
		{
			throw new NotImplementedException();
		}
		protected override bool CheckMovementImpl(Player player, int dx, int dy)
		{
			throw new NotImplementedException();
		}
		public override bool CanPlayerStepInto(Player player, int xx, int yy)
		{
			throw new NotImplementedException();
		}
		public override void PlayerCursorMovement(Player player, Direction dir)
		{
			throw new NotImplementedException();
		}
		public override bool PlayerFight(Player player)
		{
			throw new NotImplementedException();
		}
	}

	[TestClass]
	public class XleMapTests
	{
		[TestMethod]
		public void HasEventTypeTest()
		{
			XleMap map = new FakeMap();

			bool threwException = false;
			try
			{
				map.HasEventType(typeof(int));
			}
			catch (ArgumentException e)
			{
				threwException = true;
			}
			Assert.IsTrue(threwException);

			map.Events.Add(new StoreBank());

			Assert.IsTrue(map.HasEventType<StoreBank>());
			Assert.IsTrue(map.HasEventType(typeof(StoreBank)));

			Assert.IsFalse(map.HasEventType<StoreArmor>());
			Assert.IsFalse(map.HasEventType(typeof(StoreArmor)));

		}
	}
}
