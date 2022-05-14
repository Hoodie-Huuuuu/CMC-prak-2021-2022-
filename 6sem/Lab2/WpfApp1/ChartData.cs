using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    //Данные для построения графиков
    public class ChartData
    {
        //Точки неравномерной сетки
        public double[] non_uniform_x { get; set; }
        public double[] non_uniform_y { get; set; }
        public double[] X { get; set; } // начения абсцисс равномерной сетки
        public List<double[]> YL { get; set; } // Значения ординат равномерной сетки
        public List<string> legends { get; set; } //Легенды
        public ChartData(double[] x, double[] y1, double[] y2, double[] non_uniform_partition_x, double[] non_uniform_partition_y)
        {
            legends = new List<string>();
            YL = new List<double[]>();
            X = x;
            non_uniform_x = non_uniform_partition_x;
            non_uniform_y = non_uniform_partition_y;
            YL.Add(y1);
            YL.Add(y2);
            legends.Add("Сплайн 1");
            legends.Add("Сплайн 2");
        }
    }
}
