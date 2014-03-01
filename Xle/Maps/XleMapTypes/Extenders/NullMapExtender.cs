using AgateLib.Geometry;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders
{
	public class NullMapExtender : IMapExtender
	{
		public XleMap TheMap { get; set; }

		public virtual int GetOutsideTile(Point playerPoint, int x, int y)
		{
			return -1;
		}

		public virtual void OnLoad(GameState state)
		{ }


		public virtual void PlayerStep(GameState state)
		{ }

		public virtual void SetColorScheme(ColorScheme scheme)
		{
			throw new NotImplementedException();
		}


		public virtual IEventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			return (IEventExtender)Activator.CreateInstance(defaultExtender);
		}


		public virtual int StepSize
		{
			get { return 1; }
		}


		public virtual void PlayerUse(GameState state, int item, ref bool handled)
		{
		}


		public virtual void BeforeEntry(GameState state, ref int targetEntryPoint)
		{
		}


		public virtual void OnAfterEntry(GameState GameState)
		{
		}


		public virtual void AfterExecuteCommand(GameState state, AgateLib.InputLib.KeyCode cmd)
		{
		}
	}
}
