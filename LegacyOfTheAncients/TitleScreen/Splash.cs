using AgateLib.DisplayLib;
using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services.Implementation;

namespace ERY.Xle.LotA.TitleScreen
{
	class Splash : TitleState
	{
		Surface titleScreenSurface;			// stores the image of the title screen.
		int frame;
		double frameTime;
		const int animTime = 75;
		double timeUntilMusicRestarts;

		public Splash()
		{
			StartMusic();
		}

		public override void KeyDown(AgateLib.InputLib.KeyCode keyCode, string keyString)
		{
			NewState = new FirstMainMenu();
			titleScreenSurface.Dispose();
		}

		public override void Update()
		{
			if (titleScreenSurface == null)
			{
				titleScreenSurface = new Surface("title.png");
				titleScreenSurface.InterpolationHint = InterpolationMode.Fastest;
			}

			if (SoundMan.IsPlaying(LotaSound.Music) == false)
			{
				timeUntilMusicRestarts -= Display.DeltaTime;

				if (timeUntilMusicRestarts < 0)
				{
					StartMusic();
				}
			}

			frameTime += Display.DeltaTime;

			if (frameTime > animTime)
			{
				frameTime -= animTime;
				frame++;
			}
		}

		private void StartMusic()
		{
			timeUntilMusicRestarts = 2000;
			SoundMan.PlaySound(LotaSound.Music);
		}

		public override void Draw()
		{
			XleCore.SetOrthoProjection(XleColor.Gray);

			Rectangle srcRect = new Rectangle(0, 0, 320, 200);
			Rectangle destRect = new Rectangle(0, 0, 640, 400);

			srcRect.Y = (frame % 8) * 200;

			titleScreenSurface.Draw(srcRect, destRect);
		}
	}
}
