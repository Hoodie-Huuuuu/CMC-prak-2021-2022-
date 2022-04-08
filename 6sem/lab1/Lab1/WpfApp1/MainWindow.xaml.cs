using ClassLibrary1;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace WpfApp1
{
    //пользовательский преобразователь типа VMTime
    [ValueConversion(typeof(VMTime), typeof(string))]
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => value.ToString();
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => value;
    }

    //пользовательский преобразователь типа VM
    [ValueConversion(typeof(VMAccuracy), typeof(string))]
    public class AccuracyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => value.ToString();
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => value;
    }

    

    //окно
    public partial class MainWindow : Window
    {
        //сетка и ViewData 
        public ViewData Vdata { get; set; }
        public VMGrid grid { get; set; }

        //конструктор
        public MainWindow()
        {
            Vdata = new ViewData();
            grid = new VMGrid();

            //инициализация компонентов
            InitializeComponent();
            this.DataContext = Vdata;
            
            //пока не выбрали функцию нельзя добавлять элементы
            addTime.IsEnabled = false;
            addAcc.IsEnabled = false;
        }


        //обработчик закрытия окна (спросить про изменения пользователя)
        private void Window_Closed(object sender, CancelEventArgs e)
        {
            if (Vdata.Changed == true)
            {
                MessageBoxResult res = MessageBox.Show("Сохранить файл?", "Изменения не сохранены", MessageBoxButton.YesNoCancel);
                if (res == MessageBoxResult.Yes)
                {
                    try
                    {
                        var file = new Microsoft.Win32.SaveFileDialog();
                        if (file.ShowDialog() == true) Vdata.Save(file.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Exception: {ex.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else if (res == MessageBoxResult.Cancel) return;
            }
        }

        
        //обработичк загрузки окна
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            combo1.ItemsSource = Enum.GetValues(typeof(VMF));
        }


        //узлы
        private void TextBox_Nodes_Input(object sender, TextCompositionEventArgs e)
        {
            int val;
            if (!Int32.TryParse(e.Text, out val))
            {
                e.Handled = true; // отклоняем ввод
            }
        }


        //start end
        private void TextBox_Start_End_Input(object sender, TextCompositionEventArgs e)
        {
            int val;
            if (!Int32.TryParse(e.Text, out val) && e.Text != ",")
            {
                e.Handled = true; // отклоняем ввод
            }
        }

        
        private void TextBox_Nodes_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((TextBox_Nodes.Text.Length == 0) || (TextBox_Start.Text.Length == 0) || (TextBox_End.Text.Length == 0))
            {
                addTime.IsEnabled = false;
                addAcc.IsEnabled = false;
            }
            else
            {
                addTime.IsEnabled = true;
                addAcc.IsEnabled = true;
            }
            try
            {
                grid.Length = Convert.ToInt32(TextBox_Nodes.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void TextBox_Start_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((TextBox_Nodes.Text.Length == 0) || (TextBox_Start.Text.Length == 0) || (TextBox_End.Text.Length == 0))
            {
                addTime.IsEnabled = false;
                addAcc.IsEnabled = false;
            }
            else
            {
                addTime.IsEnabled = true;
                addAcc.IsEnabled = true;
            }
            try
            {
                grid.Start = Convert.ToSingle(TextBox_Start.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TextBox_End_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((TextBox_Nodes.Text.Length == 0) || (TextBox_Start.Text.Length == 0) || (TextBox_End.Text.Length == 0))
            {
                addTime.IsEnabled = false;
                addAcc.IsEnabled = false;
            }
            else
            {
                addTime.IsEnabled = true;
                addAcc.IsEnabled = true;
            }
            try
            {
                grid.End = Convert.ToSingle(TextBox_End.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //добавиТЬ Time
        private void addTime_Click(object sender, RoutedEventArgs e)
        {
            if (Vdata.SelectedFunc == null) MessageBox.Show("Выберите функцию");
            else Vdata.AddVMTime(grid);
        }

        //добавить Accuracy
        private void addAcc_Click(object sender, RoutedEventArgs e)
        {
            if (Vdata.SelectedFunc == null) MessageBox.Show("Выберите функцию");
            else Vdata.AddVMAccuracy(grid);
        }


        //New
        private void MenuItem_New_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Vdata.Changed == true)
                {
                    MessageBoxResult res = MessageBox.Show("Сохранить файл?", "Изменения не сохранены", MessageBoxButton.YesNoCancel);
                    if (res == MessageBoxResult.Yes) MenuItem_Save_Click(sender, e);
                    else if (res == MessageBoxResult.Cancel) return;
                }        
                Vdata.Benchmark.TimeResults.Clear();
                Vdata.Benchmark.Accuracies.Clear();

                //известить приложение
                Vdata.Changed = false;
                Vdata.ChangedString = "New";
                Vdata.RaisePropertyChanged(nameof(Vdata.ChangedString));
                Vdata.Benchmark.RaisePropertyChanged(nameof(Vdata.Benchmark.MinCoefsRatio));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Open
        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            if (Vdata.Changed == true)
            {
                MessageBoxResult res = MessageBox.Show("Сохранить файл?", "Изменения не сохранены", MessageBoxButton.YesNoCancel);
                if (res == MessageBoxResult.Yes) MenuItem_Save_Click(sender, e);
                else if (res == MessageBoxResult.Cancel) return;
            }

            var file = new Microsoft.Win32.OpenFileDialog();

            if (file.ShowDialog() == true)
            {
                Vdata.Load(file.FileName);
                DataContext = Vdata;
            }

        }


        //Save
        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var file = new Microsoft.Win32.SaveFileDialog();
                if (file.ShowDialog() == true) Vdata.Save(file.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
