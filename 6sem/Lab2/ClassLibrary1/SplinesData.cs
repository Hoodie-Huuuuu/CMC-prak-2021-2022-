using System;
using System.Runtime.InteropServices;

namespace ClassLibrary1
{
    public class SplinesData
    {
        //DLL функция
        [System.Runtime.InteropServices.DllImport("..\\..\\..\\..\\x64\\Debug\\Dll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Spline_derivatives(double[] x, double[] y, int non_uniform_points_count, int uniform_points_count, double left_der, double right_der, double[] res, ref double err);

        public MeasuredData md { get; set; } = new MeasuredData();
        public SplineParameters sp { get; set; } = new SplineParameters();

        //Узлы равномерной сетки
        private double[] X;
        public double[] Uniform_grid 
        {
            get
            {
                return X;
            }
        }

        //Свойства типа double[] для массива значений двух кубических сплайнов на равномерной сетке.
        public double[] values_spline_first { get; set; }
        public double[] values_spline_second { get; set; }

        //Инициализация md,sp и вычисление значений для сплайнов на равномерной сетке

        public SplinesData(MeasuredData md, SplineParameters sp)
        {
            this.md = md;
            this.sp = sp;
        }

        public SplinesData()
        {

        }

        //Mетод для вычисления значений для сплайнов на равномерной сетке
        public void Spline_approximation()
        {
            try
            {
                Calculation calculation = new Calculation();
                double step = (md.Right_border - md.Left_border) / (sp.Length - 1);
                double[] x = new double[sp.Length];

                x[0] = md.Left_border;
                x[sp.Length - 1] = md.Right_border;
                for (int i = 1; i < sp.Length - 1; i++)
                    x[i] = x[0] + i * step;

                X = x;
                double[] result1 = new double[sp.Length];
                double[] result2 = new double[sp.Length];

                double err1 = 0;
                double err2 = 0;

                Spline_derivatives(md.Grid, md.Data, md.Length, sp.Length, sp.First_spline_left_border_derivative, sp.First_spline_right_border_derivative, result1, ref err1);
                Spline_derivatives(md.Grid, md.Data, md.Length, sp.Length, sp.Second_spline_left_border_derivative, sp.Second_spline_right_border_derivative, result2, ref err2);

                if ((err1 != 0) || (err2 != 0))
                    throw new Exception();

                values_spline_first = result1;
                values_spline_second = result2;
            }
            catch
            {
                throw new Exception("Не удалось провести сплайн интерполяцию");
            }
        }

    }
}
