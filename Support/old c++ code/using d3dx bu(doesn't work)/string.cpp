
#include "lota.h"

String::String()
{
	ptr = new char;
	ptr[0] = 0;

}

String::String(const char* a)
{
	if (a)
	{
		ptr = new char[strlen(a) + 1];
		strcpy(ptr, a);
	}
	else 
	{
		ptr = new char;
		ptr[0] = 0;
	}

}

String::String(String& a)
{

	ptr = new char[strlen(a.ptr) + 1];
	strcpy(ptr, a.ptr);

}

String::~String()
{
	if (ptr)
		delete [] ptr;
}

void String::operator=(const char*a) 
{
	if (ptr)
		delete [] ptr;

	if (a)
	{
		ptr = new char[strlen(a) + 1];
		strcpy (ptr, a);
	}
	else
	{
		ptr = new char;
		ptr = 0;
	}

}

void String::operator=(const String &a) 
{
	delete [] ptr;
	ptr = new char[strlen(a.ptr) + 1];
	strcpy (ptr, a.ptr);

}

String::operator const char *()
{
	return ptr;
}

void String::operator=(int a)
{
	char tmpspace[15];

	itoa(a, tmpspace, 10);

	String::operator=(tmpspace);

}


void String::operator+=(const char*a)
{
	int b = strlen(ptr) + strlen(a) + 1;
	char* tmpptr = ptr;

	ptr = new char[b];

	strcpy(ptr, tmpptr);
	strcat(ptr, a);

	if (tmpptr)
		delete [] tmpptr;

}

void String::operator+=(int a)
{
	char tmpspace[15];

	itoa(a, tmpspace, 10);

	String::operator+=(tmpspace);

}

void String::operator+=(char a)
{
	char tmpspace[2];

	tmpspace[0] = a;
	tmpspace[1] = 0;

	String::operator+=(tmpspace);

}

bool String::operator==(const char*a)
{
	if (!strcmp(a, ptr))
	{
		return true;
	}

	return false;
}

bool String::operator !=(const char *a)
{
	return !String::operator ==(a);
}

void String::operator++()
{
	String::operator+=(" ");

}

void String::operator--()
{
	char * tmpptr = ptr;
	int length = len();

	ptr = new char[len()];

	lstrcpyn(ptr, tmpptr, length);
	
	delete [] tmpptr;

}

char String::c(int index)
{
	return ptr[index];
}

char String::c(int index, char nc)
{
	if (index < len())
	{
		ptr[index] = nc;

		return ptr[index];
	}

	return 0;
}

void String::trim()
{
	ltrim();
	rtrim();
}

void String::rtrim()
{
	for (int i = len() - 1; ptr[i] == ' ' && i >= 0; i--)
	{
		String::operator--();
	}
}

void String::ltrim()
{
	//for (int i = 0; ptr[i] == ' ' && i <= len(); i++)
	//{
//		ptr[i] = 0;
//	}
}

int String::len()
{
	return lstrlen(ptr);
}
