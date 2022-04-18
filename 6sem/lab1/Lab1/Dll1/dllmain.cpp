// dllmain.cpp : Определяет точку входа для приложения DLL.
#include "pch.h"


using namespace std;
using namespace std::chrono;

//======================= Для замера времени ==========================
class Timer
{
private:
	using clock_t = high_resolution_clock;
	using duration_t = duration<double, ratio<1> >;

	time_point<clock_t> curr_tick;

public:
	Timer() : curr_tick(clock_t::now()) {}

	void reset() { curr_tick = clock_t::now(); }

	double fix() const { return duration_cast<duration_t>(clock_t::now() - curr_tick).count(); }
};


//=============================== Глобальная функция float ==============================
extern "C"  _declspec(dllexport)
int GlobalFuncSingle(MKL_INT n_args, const float* args, float* res_mkl_ha, float* res_mkl_ep, float* res_c, float* res_time, int func)
{
	/*vmsExp = 0,
	vmdExp = 1,
	vmsErf = 2,
	vmdErf = 3*/
	Timer t;
	switch (func) {
	case 0:
		t.reset();
		vmsExp(n_args, args, res_mkl_ha, VML_HA);
		res_time[0] = t.fix();

		t.reset();
		vmsExp(n_args, args, res_mkl_ep, VML_EP);
		res_time[1] = t.fix();

		t.reset();
		for (int i = 0; i < n_args; ++i)
			res_c[i] = exp(args[i]);
		res_time[2] = t.fix();
		return 0;
		break;

	case 2:
		t.reset();
		vmsErf(n_args, args, res_mkl_ha, VML_HA);
		res_time[0] = t.fix();

		t.reset();
		vmsErf(n_args, args, res_mkl_ep, VML_EP);
		res_time[1] = t.fix();

		t.reset();
		for (int i = 0; i < n_args; ++i)
			res_c[i] = erf(args[i]);
		res_time[2] = t.fix();
		return 0;
		break;
	}

	return -1;
}



//=============================== Глобальная функция double ==============================
extern "C"  _declspec(dllexport)
int GlobalFuncDouble(MKL_INT n_args, const double* args, double* res_mkl_ha, double* res_mkl_ep, double* res_c, double* res_time, int func)
{
	/*vmsExp = 0,
	vmdExp = 1,
	vmsErf = 2,
	vmdErf = 3*/
	Timer t;
	switch (func) {

	case 1:
		t.reset();
		vmdExp(n_args, args, res_mkl_ha, VML_HA);
		res_time[0] = t.fix();

		t.reset();
		vmdExp(n_args, args, res_mkl_ep, VML_EP);
		res_time[1] = t.fix();

		t.reset();
		for (int i = 0; i < n_args; ++i)
			res_c[i] = exp(args[i]);
		res_time[2] = t.fix();
		return 0;
		break;

	case 3:
		t.reset();
		vmdErf(n_args, args, res_mkl_ha, VML_HA);
		res_time[0] = t.fix();

		t.reset();
		vmdErf(n_args, args, res_mkl_ep, VML_EP);
		res_time[1] = t.fix();

		t.reset();
		for (int i = 0; i < n_args; ++i)
			res_c[i] = erf(args[i]);
		res_time[2] = t.fix();
		return 0;
		break;
	}

	return -1;
}