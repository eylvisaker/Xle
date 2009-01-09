
#include "lota.h"

extern Global g;

SpecialEvent::SpecialEvent()
{
	data = new unsigned char[g.map.SpecialDataLength() + 1];

	data[0] = 0;

	robbed = 0;
	id = -1;

}

SpecialEvent::SpecialEvent(SpecialEvent& se)
{
	data = new unsigned char[g.map.SpecialDataLength() + 1];

	for (int i = 0; i < g.map.SpecialDataLength(); i++)
		data[i] = se.data[i];

	data[i] = 0;

	sx = se.sx;
	sy = se.sy;
	swidth = se.swidth;
	sheight = se.sheight; 
	type = se.type;
	robbed = se.robbed;
	marked = se.marked;
	id = se.id;


}

SpecialEvent::~SpecialEvent()
{
	delete [] data;

}

void SpecialEvent::operator =(SpecialEvent& se)
{
	delete [] data;

	data = new unsigned char[g.map.SpecialDataLength() + 1];

	for (int i = 0; i < g.map.SpecialDataLength(); i++)
		data[i] = se.data[i];

	data[i] = 0;

	sx = se.sx;
	sy = se.sy;
	type = se.type;
	id = se.id;
	swidth = se.swidth;
	sheight = se.sheight;
	marked = se.marked;

}

