using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;
using ClassLibrary1;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace WpfApp1
{
    public class ViewData : INotifyPropertyChanged
    {
        public ViewData()
        {
            curr_func = new VMfString(VMF.unmatched, "unmachted");
            _benchmark = new VMBenchmark();
            Functions = new ObservableCollection<VMfString>() {
                new VMfString(VMF.vmsErf, "vmsErf"),
                new VMfString(VMF.vmdErf, "vmdErf"),
                new VMfString(VMF.vmsExp, "vmsExp"),
                new VMfString(VMF.vmdExp, "vmdExp")
            };
        }

        //свойства
        //выбранная функция
        public ObservableCollection<VMfString> Functions;

        private VMfString curr_func;
        public VMfString SelectedFunc
        {
            get => curr_func;
            set
            {
                curr_func = value;
                RaisePropertyChanged(nameof(SelectedFunc));
            }
        }



        private VMBenchmark _benchmark;
        public VMBenchmark Benchmark
        {
            get => _benchmark;

            set
            {
                Benchmark = value;
                RaisePropertyChanged(nameof(Benchmark));
            }
        }

        public bool Changed = false;
        public string WarningChanged
        {
            get
            {
                if (Changed) return "Collection has been Changed";
                else return "";
            }
        }


        //методы
        public void AddVMTime(VMGrid grid)
        {
            try
            {
                Benchmark.AddVMTime(SelectedFunc.FuncType, grid);
                RaisePropertyChanged(nameof(AddVMTime));
                Changed = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void AddVMAccuracy(VMGrid grid)
        {
            try
            {
                Benchmark.AddVMAccuracy(SelectedFunc.FuncType, grid);
                RaisePropertyChanged(nameof(AddVMTime));
                Changed = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //сохранить в файл
        public bool Save(string filename)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filename, false))
                {
                    writer.WriteLine(Benchmark.TimeResults.Count);
                    foreach (var item in Benchmark.TimeResults)
                    {
                        writer.WriteLine(item.Params.Length);
                        writer.WriteLine($"{item.Params.Start:0.00000000}");
                        writer.WriteLine($"{item.Params.End:0.00000000}");
                        writer.WriteLine($"{item.Params.Step:0.00000000}");
                        writer.WriteLine((int)item.FunctionType);
                        writer.WriteLine($"{item.VML_HA_Time:0.00000000}");
                        writer.WriteLine($"{item.VML_EP_Time:0.00000000}");
                        writer.WriteLine($"{item.Time_c:0.00000000}");
                        writer.WriteLine($"{item.VML_HA_Coef:0.00000000}");
                        writer.WriteLine($"{item.VML_EP_Coef:0.00000000}");
                    }
                    writer.WriteLine(Benchmark.Accuracies.Count);
                    foreach (var item in Benchmark.Accuracies)
                    {
                        writer.WriteLine(item.Params.Length);
                        writer.WriteLine($"{item.Params.Start:0.00000000}");
                        writer.WriteLine($"{item.Params.End:0.00000000}");
                        writer.WriteLine($"{item.Params.Step:0.00000000}");
                        writer.WriteLine((int)item.FunctionType);
                        writer.WriteLine($"{item.MaxDif:0.00000000}");
                        writer.WriteLine($"{item.MaxDifArg:0.00000000}");
                        writer.WriteLine($"{item.MaxDifValue_VML_HA:0.00000000}");
                        writer.WriteLine($"{item.MaxDifValue_VML_EP:0.00000000}");
                    }
                    Changed = false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Exception:\n{e.Message}");
                return false;
            }
            return true;
        }

        //загрузить из файла
        public bool Load(string filename)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    Benchmark.TimeResults.Clear();
                    Benchmark.Accuracies.Clear();
                    int count1 = Int32.Parse(reader.ReadLine());
                    for (int i = 0; i < count1; i++)
                    {
                        VMTime item = new VMTime();
                        int GridLength = Int32.Parse(reader.ReadLine());
                        double GridStart = double.Parse(reader.ReadLine());
                        double GridEnd = double.Parse(reader.ReadLine());
                        double GridStep = double.Parse(reader.ReadLine());
                        VMF Function_type = (VMF)int.Parse(reader.ReadLine());
                        VMGrid Grid = new VMGrid(GridLength, (float)GridStart, (float)GridEnd, (float)GridStep);
                        item.Params = Grid;
                        item.VML_HA_Time = double.Parse(reader.ReadLine());
                        item.VML_EP_Time = double.Parse(reader.ReadLine());
                        item.Time_c = double.Parse(reader.ReadLine());
                        item.VML_HA_Coef = double.Parse(reader.ReadLine());
                        item.VML_EP_Coef = double.Parse(reader.ReadLine());
                        Benchmark.TimeResults.Add(item);
                    }
                    int count2 = Int32.Parse(reader.ReadLine());
                    for (int i = 0; i < count2; i++)
                    {
                        VMAccuracy item = new VMAccuracy();
                        int GridLength = Int32.Parse(reader.ReadLine());
                        double GridStart = double.Parse(reader.ReadLine());
                        double GridEnd = double.Parse(reader.ReadLine());
                        double GridStep = double.Parse(reader.ReadLine());
                        VMF Function_type = (VMF)int.Parse(reader.ReadLine());
                        VMGrid Grid = new VMGrid(GridLength, (float)GridStart, (float)GridEnd, (float)GridStep);
                        item.Params = Grid;
                        item.MaxDif = double.Parse(reader.ReadLine());
                        item.MaxDifArg = double.Parse(reader.ReadLine());
                        item.MaxDifValue_VML_HA = double.Parse(reader.ReadLine());
                        item.MaxDifValue_VML_EP = double.Parse(reader.ReadLine());
                        Benchmark.Accuracies.Add(item);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Exception:\n{e.Message}");
                return false;
            }
            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}