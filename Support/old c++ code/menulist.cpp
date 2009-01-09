
#include "lota.h"

MenuItemList::MenuItemList()
{
	totalItems = 0;

}

MenuItemList::MenuItemList(int foo, ...)
{
	totalItems = 0;

	va_list v1;
	va_start ( v1, foo );

	for (int i = 0; i < foo; i++)
	{
		char *tmp;

		tmp = va_arg (v1, char*);

		AddItem(tmp);
	}

}

MenuItemList::MenuItemList(const MenuItemList &theList)
{
	CopyFrom(theList);

}

MenuItemList::~MenuItemList()
{

}

MenuItemList MenuItemList::operator =(const MenuItemList &theList)
{
	CopyFrom(theList);

	return *this;
}

void MenuItemList::CopyFrom(const MenuItemList &theList)
{
	totalItems = 0;

	for (int i = 0; i < theList.totalItems; i++)
	{
		AddItem(theList.list[i]);
	}
}


void MenuItemList::AddItem(const String &item)
{

	if (totalItems < 32)
	{
		list[totalItems++] = item;

	}

}

void MenuItemList::GetItem(int item, char* szBuffer) const
{
	if (item < totalItems && item >= 0)
		lstrcpy (szBuffer, list[item]);

}

String& MenuItemList::GetItem(int item)
{
	static String defaultVal("");

	if (item < totalItems && item >= 0)
		return list[item];

	return defaultVal;
}

int MenuItemList::TotalItems() const
{ 
	return totalItems ;
}

void MenuItemList::Clear()
{
	for (int i = 0; i < totalItems; i++)
	{
		list[i] = "";
	}

	totalItems = 0;

}