﻿using ERY.Xle.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
	class Pegasus : LotaExhibit
	{
		public Pegasus() : base("Pegasus", Coin.Diamond) { }
		public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.Pegasus; } }
		public override string LongName
		{
			get { return "A flight of fancy"; }
		}

		public override bool StaticBeforeCoin
		{
			get
			{
				return false;
			}
		}
	}
}
