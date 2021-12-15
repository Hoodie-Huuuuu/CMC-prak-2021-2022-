#include "mylib.h"

void say(string message)
{
	cout << message << endl;
}

void say (vector<string> vec)
{
	for (int i=0; i < vec.size(); i++){
		cout << vec[i] << endl;
	}
}