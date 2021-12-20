#include <string>
#include <vector>
#include <iostream>

using namespace std;

extern "C"  _declspec(dllexport)
bool SecondDer(int nx, const double* x, int ny, const double* y, double* res);
