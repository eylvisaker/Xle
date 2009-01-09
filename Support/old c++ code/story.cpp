
#include "lota.h"

extern Global g;

void Casandra()
{
	unsigned int colors[40];
	int lastClr = lotaCyan;

	g.commandMode = cmdBad;
	g.ClearBottom();

	LotaPlaySound(snd_VeryGood, 0, 0);

	while (LotaGetSoundStatus(snd_VeryGood) == DSBSTATUS_PLAYING)
	{
		if (lastClr == lotaCyan)
		{
			lastClr = lotaYellow;
		}
		else
		{
			lastClr = lotaCyan;
		}

		for (int i = 0; i < 40; i++)
			colors[i] = lastClr;
		
		
		g.UpdateBottom("      Casandra the Temptress", 3, colors);
		wait(50);

	}

	for (int i = 0; i < 40; i++)
		colors[i] = lotaYellow;
		
	g.UpdateBottom("      Casandra the Temptress", 3, colors);
	wait(1);

	if (g.player.casandra == 0)
	{
		g.WriteSlow("You may visit my magical room", 1, lotaGreen);
		g.WriteSlow("Only this once.  My Power can", 0, lotaCyan);
		g.AddBottom("");
		g.WriteSlow("Bring you different rewards.", 0, lotaYellow);

		MenuItemList menu(2, "Gold", "Charm");

		int choice = QuickMenu(menu, 2, 0);

		if (choice == 0)		// gold
		{
			g.AddBottom("Gold   +5,000");

			g.player.GainGold(5000);

		}
		else					// charm
		{
			g.AddBottom("Charm  +15");

			g.player.Attribute(charm, 15);

		}

		g.AddBottom("");


		LotaPlaySound(snd_VeryGood);

		while (LotaGetSoundStatus(snd_VeryGood) == DSBSTATUS_PLAYING)
			wait(1);


		g.commandMode = cmdEnterCommand;
		g.player.casandra = 1;
	}
	else
	{
		g.WriteSlow("I helped you already - Be gone.", 1, lotaWhite);
	}
	
}