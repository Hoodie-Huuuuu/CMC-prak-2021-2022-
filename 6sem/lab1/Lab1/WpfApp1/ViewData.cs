using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;
using ClassLibrary1;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace WpfApp1
{
    public class ViewData : INotifyPropertyChanged
    {
        //конструктор
        public ViewData()
        {
            SelectedFunc = null;
            Benchmark = new VMBenchmark();

            //подписка на изменение коллекций
            Benchmark.TimeResults.CollectionChanged += new NotifyCollectionChangedEventHandler(Handler_CollectionChanged_Time);
            Benchmark.Accuracies.CollectionChanged += new NotifyCollectionChangedEventHandler(Handler_CollectionChanged_Ac);
        }

        //событие изменения свойства
        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        //обработчики изменения - поднимают событие изменения свойства
        /////////////////////////
        void Handler_CollectionChanged_Time(object? sender, NotifyCollectionChangedEventArgs e) => RaisePropertyChanged(nameof(VMTime));
        void Handler_CollectionChanged_Ac(object? sender, NotifyCollectionChangedEventArgs e) => RaisePropertyChanged(nameof(VMAccuracy));


        //свойства
        public VMF? SelectedFunc;
        public VMTime? SelectedTime { get; set; }
        public VMAccuracy? SelectedAccuracy { get; set; }


        public VMBenchmark Benchmark;

        public bool Changed = false;
        public string ChangedString = "";
        

        //добавить элемент в коллекцию времени
        public void AddVMTime(VMGrid grid)
        {
            try
            {
                //здесь атвоматически поднимется PropertyCHanged в обработчике
                Benchmark.AddVMTime((VMF)SelectedFunc, grid);
                
                Changed = true;
                ChangedString = "Collection has been Changed";
                RaisePropertyChanged(nameof(ChangedString));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        //добавить элемент в коллекцию точности
        public void AddVMAccuracy(VMGrid grid)
        {
            try
            {
                //здесь атвоматически поднимется PropertyCHanged в обработчике
                Benchmark.AddVMAccuracy((VMF)SelectedFunc, grid);

                Changed = true;
                ChangedString = "Collection has been Changed";
                RaisePropertyChanged(nameof(ChangedString));
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
                        //параметры сетки
                        int GridLength = Int32.Parse(reader.ReadLine());
                        double GridStart = double.Parse(reader.ReadLine());
                        double GridEnd = double.Parse(reader.ReadLine());
                        double GridStep = double.Parse(reader.ReadLine());
                        
                        VMGrid Grid = new VMGrid(GridLength, (float)GridStart, (float)GridEnd, (float)GridStep);

                        //параметры VMTime
                        VMF Function_type = (VMF)int.Parse(reader.ReadLine());
                        var VML_HA_Time = double.Parse(reader.ReadLine());
                        var VML_EP_Time = double.Parse(reader.ReadLine());
                        var Time_c = double.Parse(reader.ReadLine());
                        var VML_HA_Coef = double.Parse(reader.ReadLine());
                        var VML_EP_Coef = double.Parse(reader.ReadLine());

                        VMTime item = new VMTime(Grid, Function_type, VML_HA_Time, Time_c, VML_EP_Time, VML_HA_Coef, VML_EP_Coef);
                        Benchmark.TimeResults.Add(item);
                    }
                    int count2 = Int32.Parse(reader.ReadLine());
                    for (int i = 0; i < count2; i++)
                    {
                        //параметры сетки
                        int GridLength = Int32.Parse(reader.ReadLine());
                        double GridStart = double.Parse(reader.ReadLine());
                        double GridEnd = double.Parse(reader.ReadLine());
                        double GridStep = double.Parse(reader.ReadLine());
                        
                        VMGrid Grid = new VMGrid(GridLength, (float)GridStart, (float)GridEnd, (float)GridStep);

                        //параметры VMAccuracy
                        VMF Function_type = (VMF)int.Parse(reader.ReadLine());
                        var MaxDif = double.Parse(reader.ReadLine());
                        var MaxDifArg = double.Parse(reader.ReadLine());
                        var MaxDifValue_VML_HA = double.Parse(reader.ReadLine());
                        var MaxDifValue_VML_EP = double.Parse(reader.ReadLine());

                        VMAccuracy item = new VMAccuracy(Grid, Function_type, MaxDif, MaxDifArg, MaxDifValue_VML_HA, MaxDifValue_VML_EP);
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
    }
}