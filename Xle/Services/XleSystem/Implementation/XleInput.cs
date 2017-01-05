using System;

using AgateLib.Diagnostics;
using AgateLib.InputLib;
using AgateLib.InputLib.Legacy;

using ERY.Xle.Services.Commands;
using ERY.Xle.Services.ScreenModel;

namespace ERY.Xle.Services.XleSystem.Implementation
{
	public class XleInput : IXleInput
	{
		private GameState gameState;
		private IXleScreen screen;

		public XleInput(
			IXleScreen screen,
			GameState gameState)
		{
			this.screen = screen;
			this.gameState = gameState;

			screen.Update += gameControl_Update;
			Input.Unhandled.KeyDown += Keyboard_KeyDown;
		}

		void gameControl_Update(object sender, EventArgs e)
		{
			if (AgateConsole.IsVisible == false)
			{
				CheckArrowKeys();
			}
		}

		private void Keyboard_KeyDown(object sender, AgateInputEventArgs e)
		{
			if (AcceptKey == false)
				return;
			if (e.KeyCode == AgateConsole.VisibleToggleKey)
				return;

			try
			{
				AcceptKey = false;
				OnDoCommand(e.KeyCode);
			}
			finally
			{
				AcceptKey = true;
			}
		}

		private void OnDoCommand(KeyCode command)
		{
			if (DoCommand != null)
				DoCommand(this, new CommandEventArgs(command));
		}

		public event EventHandler<CommandEventArgs> DoCommand;

		public bool AcceptKey { get; set; }

		public void CheckArrowKeys()
		{
			if (AcceptKey == false)
				return;
			if (gameState == null)
				return;

			try
			{
				AcceptKey = false;

				if (Input.Unhandled.Keys[KeyCode.Down])
					OnDoCommand(KeyCode.Down);
				else if (Input.Unhandled.Keys[KeyCode.Left])
					OnDoCommand(KeyCode.Left);
				else if (Input.Unhandled.Keys[KeyCode.Up])
					OnDoCommand(KeyCode.Up);
				else if (Input.Unhandled.Keys[KeyCode.Right])
					OnDoCommand(KeyCode.Right);
			}
			finally
			{
				AcceptKey = true;
			}
		}


		/// <summary>
		/// Waits for one of the specified keys, while redrawing the screen.
		/// </summary>
		/// <param name="keys">A list of keys which will break out of the wait. 
		/// Pass none for any key to break out.</param>
		/// <returns></returns>
		public KeyCode WaitForKey(params KeyCode[] keys)
		{
			return WaitForKey(screen.OnDraw, keys);
		}

		/// <summary>
		/// Waits for one of the specified keys, while calling the delegate
		/// to redraw the screen.
		/// </summary>
		/// <param name="redraw"></param>
		/// <param name="keys">A list of keys which will break out of the wait. 
		/// Pass none for any key to break out.</param>
		/// <returns></returns>
		public KeyCode WaitForKey(Action redraw, params KeyCode[] keys)
		{
			KeyCode key = KeyCode.None;
			bool done = false;

			using (var input = new SimpleInputHandler())
			{
				Input.Handlers.Add(input);

				EventHandler<AgateInputEventArgs> keyhandler = (sender, e) => key = e.KeyCode;

				screen.PromptToContinue = PromptToContinueOnWait;

				input.Keys.ReleaseAll();
				input.KeyDown += keyhandler;

				do
				{
					redraw();

					if (screen.CurrentWindowClosed == true)
					{
						if (keys.Length > 0)
							key = keys[0];
						else
							key = KeyCode.Escape;

						break;
					}

					if ((keys == null || keys.Length == 0) && key != KeyCode.None)
						break;

					for (int i = 0; i < keys.Length; i++)
					{
						if (keys[i] == key)
						{
							done = true;
							break;
						}
					}

				} while (!done && screen.CurrentWindowClosed == false);
			}

			screen.PromptToContinue = false;
			PromptToContinueOnWait = true;

			return key;
		}


		/// <summary>
		/// Set to false to have WaitForKey not display a prompt 
		/// with the standard drawing method.
		/// </summary>
		public bool PromptToContinueOnWait { get; set; }
	}
}
