using System;
using System.Collections;
using System.Linq;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClassLibrary1
{
    public class VMBenchmark : INotifyPropertyChanged
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
            
            int status = 0;
            if (func_type == VMF.vmdErf || func_type == VMF.vmdExp)
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
                status = GlobalFuncDouble(grid.Length, args, res_mkl_ha, res_mkl_ep,
                                                                res_c, res_time, func_type);

                //новый элемент VMTime
                var VML_EP_Coef = res_time[1] / res_time[2];
                var VML_HA_Coef = res_time[0] / res_time[2];

                VMTime new_item = new VMTime(grid, func_type, (float)res_time[0], (float)res_time[2],
                                             (float)res_time[1], (float)VML_HA_Coef, (float)VML_EP_Coef);

                TimeResults.Add(new_item);
            } 
            else
            {
                //сетка 
                float[] args = new float[grid.Length];
                for (int i = 0; i < grid.Length; i++)
                    args[i] = grid.Start + grid.Step * i;

                //результаты
                float[] res_mkl_ha = new float[grid.Length];
                float[] res_mkl_ep = new float[grid.Length];
                float[] res_c = new float[grid.Length];
                float[] res_time = new float[3];

                //вызов функции
                status = GlobalFuncSingle(grid.Length, args, res_mkl_ha, res_mkl_ep,
                                                                res_c, res_time, func_type);

                var VML_EP_Coef = res_time[1] / res_time[2];
                var VML_HA_Coef = res_time[0] / res_time[2];

                VMTime new_item = new VMTime(grid, func_type, res_time[0], res_time[2],
                                             res_time[1], VML_HA_Coef, VML_EP_Coef);

                TimeResults.Add(new_item);
            }
            if (status != 0) throw new Exception($"GlobalFunc failed");

            

        }
        [DllImport("..\\..\\..\\..\\x64\\Debug\\Dll1.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GlobalFuncSingle(int n_args, float[] args, float[] res_mkl_ha,
                                                        float[] res_mkl_ep, float[] res_c,
                                                                float[] res_time, VMF func);
        [DllImport("..\\..\\..\\..\\x64\\Debug\\Dll1.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GlobalFuncDouble(int n_args, double[] args, double[] res_mkl_ha,
                                                       double[] res_mkl_ep, double[] res_c,
                                                               double[] res_time, VMF func);


        //в этом методе выполняются вычисления, создается и
        //добавляется в коллекцию ObservableCollection<VMAccuracy> новый элемент
        //VMAccuracy;
        public void AddVMAccuracy(VMF func_type, VMGrid grid)
        {
            int status = 0;
            if (func_type == VMF.vmdErf || func_type == VMF.vmdExp)
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
                status = GlobalFuncDouble(grid.Length, args, res_mkl_ha, res_mkl_ep,
                                                                       res_c, res_time, func_type);
                if (status != 0) throw new Exception($"GlobalFunc failed");

                //новый элемент VMAccuracy
                float MaxDif = 0;
                int idx = 0;
                for (int i = 0; i < grid.Length; ++i)
                {
                    float tmp = (float)Math.Abs(res_mkl_ha[i] - res_mkl_ep[i]);
                    if (tmp > MaxDif)
                    {
                        idx = i;
                        MaxDif = tmp;
                    }
                }

                VMAccuracy new_item = new VMAccuracy(grid, func_type, MaxDif,
                                                    (float)args[idx], (float)res_mkl_ha[idx], (float)res_mkl_ep[idx]);
                Accuracies.Add(new_item);
            }
            else
            {
                //сетка 
                float[] args = new float[grid.Length];
                for (int i = 0; i < grid.Length; i++)
                    args[i] = grid.Start + grid.Step * i;

                //результаты
                float[] res_mkl_ha = new float[grid.Length];
                float[] res_mkl_ep = new float[grid.Length];
                float[] res_c = new float[grid.Length];
                float[] res_time = new float[3];
                //вызов функции
                status = GlobalFuncSingle(grid.Length, args, res_mkl_ha, res_mkl_ep,
                                                                       res_c, res_time, func_type);
                if (status != 0) throw new Exception($"GlobalFunc failed");

                //новый элемент VMAccuracy
                float MaxDif = 0;
                int idx = 0;
                for (int i = 0; i < grid.Length; ++i)
                {
                    float tmp = Math.Abs(res_mkl_ha[i] - res_mkl_ep[i]);
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
        }

        //min коэфициент отоншения времен из всей коллекции
        public string MinCoefsRatio
        {
            get
            {
                if (TimeResults.Count != 0)
                    return $"Min Ha Time: {TimeResults.Min(item => item.VML_HA_Coef).ToString()}\n" +
                        $"Min Ep Time: {TimeResults.Min(item => item.VML_EP_Coef).ToString()}\n";
                return "Min Time: Collection is Empty";
            }
        }

        //событие изменения свойства
        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}