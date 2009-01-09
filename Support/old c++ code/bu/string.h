
#ifndef ___STRINGH___
#define ___STRINGH___

class String
{
public:

	// constructors
	String();
	String(const char*a);
	String(String& a);
	~String();

	// operators
	operator const char *();			// const char * cast
	void operator=(const char* a);		// asignment
	void operator=(const String& a);	// asignment
	void operator=(int a);				// asignment
	void operator+=(const char* a);		// concatenate
	void operator+=(int a);				// concatenate (convert number)
	void operator+=(char a);			// concatenate
	bool operator==(const char* a);		// equals conditional test
	bool operator!=(const char* a);		// equals conditional test
	void operator++();					// adds a space
	void operator--();					// truncates the last character

	// accessors
	char c(int index);							// returns the character at c
	char c(int index, char nc);					// returns the character at c
	int len();									// returns the length
	void trim();								// trims all spaces off front and back
	void ltrim();
	void rtrim();


private:
	char*		ptr;

};


#endif
