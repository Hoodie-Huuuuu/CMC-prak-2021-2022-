using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassLibrary1;

namespace WpfApp1
{

   

    public partial class MainWindow : Window
    {
        //Проверка на готовность MeasuredData
        public bool IsMeasured { get; set; }

        //График для рисования
        OxyPlotModel model;

        private Viewdata viewdata;
        public MainWindow()
        {
            viewdata = new Viewdata();
            InitializeComponent();
            this.DataContext = viewdata;
        }

        //обработичк загрузки окна
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Func_list.ItemsSource = Enum.GetValues(typeof(Spf));
        }

        //Проверка и исполнение комманды MeasuredData
        private void MeasuredData_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !viewdata.sd.md.InputError;
        }
        private void MeasuredData_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                viewdata.sd.md.Grid_init();
                int n = viewdata.sd.md.Length;
                viewdata.MeasuredData.Clear();
                viewdata.MeasuredData.Add("Coordinates");
                for (int i = 0; i < n; i++)
                {
                    string s = $"[{viewdata.sd.md.Grid[i]} ; {viewdata.sd.md.Data[i]}]";
                    viewdata.MeasuredData.Add(s);
                }
                IsMeasured = true;


                ChartData chart = new ChartData(viewdata.sd.Uniform_grid, viewdata.sd.values_spline_first,
                viewdata.sd.values_spline_second, viewdata.sd.md.Grid, viewdata.sd.md.Data);
                viewdata.ChartData = chart;

                model = new OxyPlotModel(viewdata.ChartData);
                model.Scatter();
                Oxyplot.DataContext = model;
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //Проверка и исполнение комманды Splines
        private void Splines_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (!viewdata.sp.InputError) && IsMeasured && !viewdata.sd.md.InputError;
        }

        private void Splines_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                viewdata.sd.sp = viewdata.sp;
                viewdata.sd.Spline_approximation();

                viewdata.Derivatives1.Clear();
                viewdata.Derivatives2.Clear();
                viewdata.SplineValues1.Clear();
                viewdata.SplineValues2.Clear();

                

                viewdata.Derivatives1.Add($"Левая граница: {viewdata.sp.First_spline_left_border_derivative}");
                viewdata.Derivatives1.Add($"Правая граница: {viewdata.sp.First_spline_right_border_derivative}");

                viewdata.Derivatives2.Add($"Левая граница: {viewdata.sp.Second_spline_left_border_derivative}");
                viewdata.Derivatives2.Add($"Правая граница: {viewdata.sp.Second_spline_right_border_derivative}");

                viewdata.SplineValues1.Add($"a: {viewdata.sd.values_spline_first[0]}");
                viewdata.SplineValues1.Add($"a + h: {viewdata.sd.values_spline_first[1]}");
                viewdata.SplineValues1.Add($"b - h: {viewdata.sd.values_spline_first[viewdata.sp.Length - 2]}");
                viewdata.SplineValues1.Add($"b: {viewdata.sd.values_spline_first[viewdata.sp.Length - 1]}");

                viewdata.SplineValues2.Add($"a: {viewdata.sd.values_spline_second[0]}");
                viewdata.SplineValues2.Add($"a + h: {viewdata.sd.values_spline_second[1]}");
                viewdata.SplineValues2.Add($"b - h: {viewdata.sd.values_spline_second[viewdata.sp.Length - 2]}");
                viewdata.SplineValues2.Add($"b: {viewdata.sd.values_spline_second[viewdata.sp.Length - 1]}");

                model.plotModel.Legends.Clear();

                ChartData chart = new ChartData(viewdata.sd.Uniform_grid, viewdata.sd.values_spline_first,
                                        viewdata.sd.values_spline_second, viewdata.sd.md.Grid, viewdata.sd.md.Data);
                viewdata.ChartData = chart;
                model.data = chart;
                model.AddSeries();
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }


    //Комманды
    public static class CustomCommands
    {
        public static readonly RoutedUICommand MeasuredData = new
            (
                "MeasuredData",
                "MeasuredData",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.D1, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand Splines = new
            (
                "Splines",
                "Splines",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.D2, ModifierKeys.Control)
                }
            );
    }

    //Перевод из строки в double и наоборот
    public class StrToSingle : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string s;
                if (value.GetType() == typeof(double))
                {
                    s = ((double)value).ToString();
                }
                else
                {
                    s = ((float)value).ToString();
                }
                return s;
            }
            catch
            {
                MessageBox.Show("Неправильный формат данных (ожидалось число)");
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                double x = Calculation.ToSingle((string)value);
                return x;
            }
            catch
            {
                MessageBox.Show("Неправильный формат данных (ожидалось число)");
                return null;
            }
        }
    }

    //Перевод из строки в int и наоборот
    public class StrToInt : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string s = ((int)value).ToString();
                return s;
            }
            catch
            {
                MessageBox.Show("Неправильный формат данных (ожидалось целое число)");
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int x = Int32.Parse((string)value);
                return x;
            }
            catch
            {
                MessageBox.Show("Неправильный формат данных (ожидалось целое число)");
                return null;
            }
        }
    }

}
