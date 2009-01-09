
#ifndef __menuList__
#define __menuList__

#include "lota.h"

class MenuItemList
{
private:
	String			list[32];
	int				totalItems;
	
	void CopyFrom(const MenuItemList &theList);

public:
	MenuItemList();
	MenuItemList(int foo, ...);			// constructs with the arguments passed.  foo is the number of arguements
	MenuItemList::MenuItemList(const MenuItemList &theList);		// copy constructor
	~MenuItemList();

	MenuItemList	operator=(const MenuItemList &theList);		// assignment operator
	void			AddItem(const String&);				// adds the text item specified
	void			GetItem(int item, char *szBuffer) const;	// returns the item specified
	String&			GetItem(int item);					// returns the item specified
	int				TotalItems() const;						// returns the total number of items
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