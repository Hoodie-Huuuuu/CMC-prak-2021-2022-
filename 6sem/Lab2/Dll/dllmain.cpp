// dllmain.cpp : Определяет точку входа для приложения DLL.
#include "pch.h"

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}


extern "C" _declspec(dllexport)
void Spline_derivatives(double* x, double* y, int non_uniform_points_count, int uniform_points_count, double left_derivative, double right_derivative, double* res, double& err)
{
    try
    {
        
        //создаем дискриптор задачи
        DFTaskPtr task = new DFTaskPtr();


        //создаем задачу
        int status = dfdNewTask1D(&task, non_uniform_points_count, x, DF_NON_UNIFORM_PARTITION, 1, y, DF_NO_HINT);
        if (status != DF_STATUS_OK)
        {
            err = 1;
            return;
        }


        //выставляем параметры
        double* scoeff = new double[(non_uniform_points_count - 1) * DF_PP_CUBIC];
        double derivatives[] = { left_derivative , right_derivative };
        status = dfdEditPPSpline1D(task, DF_PP_CUBIC, DF_PP_NATURAL, DF_BC_2ND_LEFT_DER + DF_BC_2ND_RIGHT_DER, derivatives, DF_NO_IC, NULL, scoeff, DF_NO_HINT);
        if (status != DF_STATUS_OK)
        {
            err = 2;
            return;
        }

        //Построить сплайн
        status = dfdConstruct1D(task, DF_PP_SPLINE, DF_METHOD_STD);
        if (status != DF_STATUS_OK)
        {
            err = 3;
            return;
        }

        //Вычислить значения сплайна
        MKL_INT dorder[1] = { 1 };
        double range[] = { x[0], x[non_uniform_points_count - 1] };
        status = dfdInterpolate1D(task, DF_INTERP, DF_METHOD_PP, uniform_points_count, range, DF_UNIFORM_PARTITION, 1, dorder, NULL, res, DF_NO_HINT, NULL);
        if (status != DF_STATUS_OK)
        {
            err = 4;
            return;
        }

        //удалить дескриптор
        status = dfDeleteTask(&task);
        if (status != DF_STATUS_OK)
        {
            err = 5;
            return;
        }

    }
    catch (int e)
    {
        err = e;
    }

}
