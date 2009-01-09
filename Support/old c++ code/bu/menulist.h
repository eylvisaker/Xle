
#ifndef __menuList__
#define __menuList__

#include "lota.h"

class MenuItemList
{
private:
	String			list[32];
	int				totalItems;

public:
	MenuItemList();
	MenuItemList(int foo, ...);			// constructs with the arguments passed.  foo can be anything
	MenuItemList::MenuItemList(MenuItemList &theList);		// copy constructor
	~MenuItemList();

	void			AddItem(const String);				// adds the text item specified
	void			GetItem(int item, char *szBuffer);	// returns the item specified
	int				TotalItems();						// returns the total number of items
	void			Clear();						// clears the list

};

class SubMenu
{
private:

public:
	MenuItemList		theList;				// show list
	bool				onScreen;				// display on screen
	String				title;					// title of menu
	int					value;					// value of menu
	int					width;					// width in CHARACTERS!!!

};
#endif