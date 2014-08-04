﻿using AgateLib.Platform.WindowsForms;
using AgateLib.Platform.WindowsForms.ApplicationModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ERY.Xle.LotA
{
	static class LotaProgram
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			RunGame(args);
		}

		private static void RunGame(string[] args)
		{
			new PassiveModel(args).Run(() =>
			{
				XleCore core = new XleCore();
				core.ProcessArguments(args);

				System.IO.Directory.SetCurrentDirectory("LotA");

				Configuration.Images.AddPath("Images");
				Configuration.Sounds.AddPath("Audio");
				AgateLib.IO.FileProvider.MusicAssets = new AgateLib.IO.SubdirectoryProvider(AgateLib.IO.FileProvider.Assets, "Audio");

				core.Run(new LotaFactory());
			});
		}

		public static IEnumerable<Commands.Command> CommonLotaCommands
		{
			get
			{
				yield return new Commands.ArmorCommand();
				yield return new Commands.Fight();
				yield return new Commands.Gamespeed();
				yield return new Commands.Hold();
				yield return new Commands.Inventory();
				yield return new Commands.Pass();
				yield return new Commands.Use { ShowItemMenu = false };
				yield return new Commands.WeaponCommand();
				yield return new Commands.Xamine();
			}
		}
	}
}
