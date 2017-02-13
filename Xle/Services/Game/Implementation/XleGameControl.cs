using System;
using AgateLib;
using AgateLib.InputLib;
using AgateLib.Platform;
using ERY.Xle.Services.ScreenModel;
using ERY.Xle.Services.XleSystem;

namespace ERY.Xle.Services.Game.Implementation
{
	public class XleGameControl : IXleGameControl
	{
		private IXleScreen screen;
		private GameState gameState;
		private XleSystemState systemState;

		public XleGameControl(
			IXleScreen screen,
			GameState gameState,
			XleSystemState systemState)
		{
			this.screen = screen;
			this.gameState = gameState;
			this.systemState = systemState;
		}

		public void Wait(int howLong, bool keyBreak = false, Action redraw = null)
		{
			if (redraw == null)
				redraw = screen.OnDraw;

			IStopwatch watch = Timing.CreateStopWatch();

			using (var input = new SimpleInputHandler())
			{
				Input.Handlers.Add(input);

				do
				{
					screen.OnUpdate();

					redraw();
					KeepAlive();

					if (keyBreak && input.Keys.Any)
						break;

				} while (watch.TotalMilliseconds < howLong && AgateApp.IsAlive);
			}
		}

		public void KeepAlive()
		{
			if (gameState.MapExtender != null)
			{
				gameState.MapExtender.CheckSounds();
			}

			if (screen.CurrentWindowClosed)
				throw new MainWindowClosedException();

			AgateApp.KeepAlive();
		}

		public void RunRedrawLoop()
		{
			while (AgateApp.IsAlive &&
				systemState.ReturnToTitle == false)
			{
				Redraw();
			}
		}

		public void Redraw()
		{
			OnUpdate();
			screen.OnDraw();

			KeepAlive();
		}

		private void OnUpdate()
		{
			if (gameState != null && gameState.MapExtender != null)
			{
				gameState.MapExtender.OnUpdate(AgateApp.GameClock.Elapsed.TotalSeconds);
			}

			screen.OnUpdate();
		}

	}
}
