using ClassLibrary1;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using System.Collections.ObjectModel;

namespace WpfApp1
{
    //окно
    public partial class MainWindow : Window
    {
        //сетка и ViewData 
        public ViewData Vdata { get; set; }
        VMGrid grid;

        //конструктор
        public MainWindow()
        {
            //инициализация компонентов
            InitializeComponent();
            this.DataContext = new ViewData();
            
            grid = new VMGrid();
            Vdata = (ViewData)DataContext;

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


        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    Vdata = Vdata.A.selectedFunc.func;
        //    MessageBox.Show(Vdata.B.Times.Count.ToString());
        //}


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
                //if (Vdata.Benchmark.SelectedTime != null) Vdata.Benchmark.SelectedTime.MoreInfo = "";
                Vdata.Benchmark.TimeResults.Clear();
                //if (Vdata.Benchmark.SelectedAccuracy != null) Vdata.Benchmark.SelectedAccuracy.MoreInfo = "";
                Vdata.Benchmark.Accuracies.Clear();
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
                //DataContext = null;
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

        private void Window_Closed_1(object sender, EventArgs e)
        {

        }
    }
}
