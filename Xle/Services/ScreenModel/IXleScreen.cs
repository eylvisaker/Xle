using System;
using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;

namespace ERY.Xle.Services.ScreenModel
{
	public interface IXleScreen : IXleService
	{
		void OnDraw();

		Color FontColor { get; set; }
		bool CurrentWindowClosed { get; }

		/// <summary>
		/// Set to true to show the (press to cont) prompt.
		/// </summary>
		bool PromptToContinue { get; set; }

		event EventHandler Draw;
		event EventHandler Update;

		void OnUpdate();
	}
}
