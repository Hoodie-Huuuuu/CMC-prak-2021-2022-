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
        private VMTime curr_time;
        private VMAccuracy curr_acc;
        public VMTime SelectedTime
        {
            get => curr_time;
            set
            {
                curr_time = value;
                RaisePropertyChanged(nameof(SelectedTime));
            }
        }

        public VMAccuracy SelectedAccuracy
        {
            get => curr_acc;
            set
            {
                curr_acc = value;
                RaisePropertyChanged(nameof(SelectedAccuracy));
            }
        }


        public VMBenchmark()
        {
            curr_time = new VMTime();
            curr_acc = new VMAccuracy();
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
            int status = GlobalFunc(grid.Length, args, res_mkl_ha, res_mkl_ep, res_c, res_time, func_type);
            if (status != 0) throw new Exception($"GlobalFunc failed");

            //новый элемент VMTime
            VMTime new_item = new VMTime();
            new_item.Params = grid;
            new_item.FunctionType = func_type;
            new_item.VML_EP_Time = res_time[1];
            new_item.VML_HA_Time = res_time[0];
            new_item.Time_c = res_time[2];
            new_item.VML_EP_Coef = new_item.VML_EP_Time / new_item.Time_c;
            new_item.VML_HA_Coef = new_item.VML_HA_Coef / new_item.Time_c;

            TimeResults.Add(new_item);
        }
        [DllImport("..\\..\\..\\..\\DllLab1\\x64\\Debug\\DllLab1.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GlobalFunc(int n_args, double[] args, double[] res_mkl_ha, double[] res_mkl_ep, double[] res_c, double[] res_time, VMF func);

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
            int status = GlobalFunc(grid.Length, args, res_mkl_ha, res_mkl_ep, res_c, res_time, func_type);
            if (status != 0) throw new Exception($"GlobalFunc failed");

            //новый элемент VMAccuracy
            VMAccuracy new_item = new VMAccuracy();
            new_item.Params = grid;
            new_item.FunctionType = func_type;

            new_item.MaxDif = 0;
            int idx = 0;
            for (int i = 0; i < grid.Length; ++i)
            {
                double tmp = Math.Abs(res_mkl_ha[i] - res_mkl_ep[i]);
                if (tmp > new_item.MaxDif)
                {
                    idx = i;
                    new_item.MaxDif = tmp;
                }
            }

            new_item.MaxDifArg = args[idx];
            new_item.MaxDifValue_VML_HA = res_mkl_ha[idx];
            new_item.MaxDifValue_VML_EP = res_mkl_ep[idx];

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


        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}