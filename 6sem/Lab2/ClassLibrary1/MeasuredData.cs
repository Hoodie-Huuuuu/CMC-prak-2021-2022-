using System;
using System.ComponentModel;

namespace ClassLibrary1
{
    //Перечисление SPf с элементами, отвечающим функциям, которые используются для вычисления данных.
    public enum Spf
    {
        Cubic_polynomial = 0,
        Sin = 1,
        Random_func = 2
    }

    //В этом классе хранятся узлы в которых вычислена определенная функция, а также результаты вычисления
    public class MeasuredData:IDataErrorInfo
    {
        //Число узлов неравномерной сетки
        private double l;
        public int Length { get; set; }

        //Левая и правая границы
        public double Left_border { get; set; }

        public double Right_border { get; set; }

        //Функция
        public Spf Function { get; set; }

        //Неравномерная сетка значения
        private double[] grid;
        public double[] Grid 
        { 
            get { return grid; }
 
        }

        //Вычисленные значения в узлах сетки Grid
        public double[] Data { get; set; }

        //Инициализация неравномерной сетки и значений в узлах сетки
        public void Grid_init()
        {
            try
            {
                Random generator = new Random();
                grid = new double[Length];
                Data = new double[Length];
                Grid[0] = Left_border;
                Grid[Length - 1] = Right_border;
                for (int i = 1; i < Length - 1; i++)
                {
                    double temp = generator.NextDouble() * (Right_border - Left_border) + Left_border;
                    while (Array.IndexOf(Grid, temp) != -1)
                    {
                        temp = generator.NextDouble() * (Right_border - Left_border) + Left_border;
                    }
                    Grid[i] = temp;
                }
                Array.Sort(Grid);
                Calculation calculation = new Calculation();
                Data = calculation.Calc(Length, Grid, Function);
            }
            catch
            {
                throw new Exception("MeasuredData не инициализировалось");
            }
        }
        //Конструктор
        public MeasuredData()
        {

        }

        public bool InputError = true;

        //IDataErrorInfo
        public string Error
        {
            get { throw new NotImplementedException(); }
        }
        public string this[string propertyName]
        {
            get
            {
                string msg = null;
                switch (propertyName)
                {
                    case "Right_border":
                        if (this.Left_border >= this.Right_border )
                        {
                            msg = "Правая граница должна быть больше левой";
                        }
                        break;
                    case "Left_border":
                        if (this.Left_border >= this.Right_border)
                        {
                            msg = "Левая граница должна быть меньше правой";
                        }
                        break;
                    case "Length":
                        if (this.Length < 2)
                        {
                            msg = "Число узлов неравномерной сетки меньше 2";
                        }
                        break;
                }
                if((this.Length >= 2) && (this.Left_border < this.Right_border))
                {
                    InputError = false;
                }
                else
                    InputError = true;
                return msg;
            }
        }
    }
}
