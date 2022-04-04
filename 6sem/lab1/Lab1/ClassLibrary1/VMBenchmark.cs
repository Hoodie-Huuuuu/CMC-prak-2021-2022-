using System;
using System.Collections;
using System.Linq;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClassLibrary1
{
    public class VMBenchmark
    {
        
        public VMBenchmark()
        {
            TimeResults = new ObservableCollection<VMTime>();
            Accuracies = new ObservableCollection<VMAccuracy>();
        }


        //для хранения результатов сравнения времени
        public ObservableCollection<VMTime> TimeResults { get; set; }

        //для хранения результатов сравнения точности вычисления функций
        public ObservableCollection<VMAccuracy> Accuracies { get; set; }

        //в этом методе выполняются
        //вычисления, создается и добавляется в коллекцию
        //ObservableCollection<VMTime> новый элемент VMTime;
        public void AddVMTime(VMF func_type, VMGrid grid)
        {
            //сетка 
            double[] args = new double[grid.Length];
            for (int i = 0; i < grid.Length; i++)
                args[i] = grid.Start + grid.Step * i;

            //результаты
            double[] res_mkl_ha = new double[grid.Length];
            double[] res_mkl_ep = new double[grid.Length];
            double[] res_c = new double[grid.Length];
            double[] res_time = new double[3];
            //вызов функции
            int status = GlobalFunc(grid.Length, args, res_mkl_ha, res_mkl_ep,
                                                                res_c, res_time, func_type);
            if (status != 0) throw new Exception($"GlobalFunc failed");

            //новый элемент VMTime
            var VML_EP_Coef = res_time[1] / res_time[2];
            var VML_HA_Coef = res_time[0] / res_time[2];

            VMTime new_item = new VMTime(grid, func_type, res_time[0], res_time[2],
                                         res_time[1], VML_HA_Coef, VML_EP_Coef);

            TimeResults.Add(new_item);
        }
        [DllImport("..\\..\\..\\..\\DllLab1\\x64\\Debug\\DllLab1.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GlobalFunc(int n_args, double[] args, double[] res_mkl_ha,
                                                        double[] res_mkl_ep, double[] res_c,
                                                                double[] res_time, VMF func);

        //в этом методе выполняются вычисления, создается и
        //добавляется в коллекцию ObservableCollection<VMAccuracy> новый элемент
        //VMAccuracy;
        public void AddVMAccuracy(VMF func_type, VMGrid grid)
        {
            //сетка 
            double[] args = new double[grid.Length];
            for (int i = 0; i < grid.Length; i++)
                args[i] = grid.Start + grid.Step * i;

            //результаты
            double[] res_mkl_ha = new double[grid.Length];
            double[] res_mkl_ep = new double[grid.Length];
            double[] res_c = new double[grid.Length];
            double[] res_time = new double[3];
            //вызов функции
            int status = GlobalFunc(grid.Length, args, res_mkl_ha, res_mkl_ep, 
                                                                   res_c, res_time, func_type);
            if (status != 0) throw new Exception($"GlobalFunc failed");

            //новый элемент VMAccuracy
            double MaxDif = 0;
            int idx = 0;
            for (int i = 0; i < grid.Length; ++i)
            {
                double tmp = Math.Abs(res_mkl_ha[i] - res_mkl_ep[i]);
                if (tmp > MaxDif)
                {
                    idx = i;
                    MaxDif = tmp;
                }
            }

            VMAccuracy new_item = new VMAccuracy(grid, func_type, MaxDif,
                                                args[idx], res_mkl_ha[idx], res_mkl_ep[idx]);
            Accuracies.Add(new_item);
        }

        //min коэфициент отоншения времен из всей коллекции
        public double MinCoefRatio_VML_HA
        {
            get => TimeResults.Min(item => item.VML_HA_Coef);
        }

        public double MinCoefRatio_VML_EP
        {
            get => TimeResults.Min(item => item.VML_EP_Coef);
        }
    }
}