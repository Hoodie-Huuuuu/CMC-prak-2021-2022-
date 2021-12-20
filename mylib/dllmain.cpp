// dllmain.cpp : Определяет точку входа для приложения DLL.
#include "pch.h"
#include "mylib.h"
#include <mkl.h>
#include <iostream>

extern "C"  _declspec(dllexport)
bool SecondDer(int nx, const double* x, int ny, const double *y, double *res)
{
	
	//дескриптор задачи
	DFTaskPtr task_descriptor = new DFTaskPtr();
	//создаем задачу
	MKL_INT xhint = DF_UNIFORM_PARTITION;
	MKL_INT yhint = DF_MATRIX_STORAGE_ROWS;

	int status = dfdNewTask1D(&task_descriptor, nx, x, xhint, ny, y, yhint);
	if (status != DF_STATUS_OK) return false;

	//выставляем параметры
	double* scoeff = new double[ny * (nx-1) * DF_PP_CUBIC];
	status = dfdEditPPSpline1D(task_descriptor, DF_PP_CUBIC, DF_PP_NATURAL, DF_BC_FREE_END, NULL, DF_NO_IC, NULL, scoeff, DF_NO_HINT);
	if (status != DF_STATUS_OK) return false;


	
	//Построить сплайн
	status = dfdConstruct1D(task_descriptor, DF_PP_SPLINE, DF_METHOD_STD);
	if (status != DF_STATUS_OK) return false;


	//Вычисление значений и производных
	MKL_INT ndorder = 3;
	MKL_INT dorder[] = {0, 0, 1};

	
	status = dfdInterpolate1D(task_descriptor,
		DF_INTERP, DF_METHOD_PP, nx, x, DF_UNIFORM_PARTITION, ndorder, dorder,
		NULL, res, DF_MATRIX_STORAGE_ROWS, NULL);
	if (status != DF_STATUS_OK) return false;


	//удалить задачу
	status = dfDeleteTask(&task_descriptor);
	if (status != DF_STATUS_OK) return false;

	return true;
}