using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xle.Data;
using Xle.Services;
using Xle.Services.Commands.Implementation;

namespace Xle.LoB.MapExtenders.Towns
{
	[ServiceName("TownMagic")]
	public class TownMagic : MagicCommand
	{
		protected override IEnumerable<MagicSpell> ValidMagic
		{
			get
			{
				yield return Data.MagicSpells[1];
				yield return Data.MagicSpells[2];
				yield return Data.MagicSpells[3];
				yield return Data.MagicSpells[4];
			}
		}
	}
}
