using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class SplineParameters: IDataErrorInfo
    {
        //Число узлов равномерной сетки
        public int Length { get; set; }

        //Вторые производные на концах отрезка для двух сплайнов
        public double First_spline_left_border_derivative { get; set; }

        public double Second_spline_left_border_derivative { get; set; }

        public double First_spline_right_border_derivative { get; set; }

        public double Second_spline_right_border_derivative { get; set; }

        public string Error { get; }

        public bool InputError = true;

        public string this[string propertyName]
        {
            get
            {
                string msg = null;
                switch (propertyName)
                {
                    case "Length":
                        if (this.Length < 2)
                        {
                            msg = "Число узлов равномерной сетки меньше 2";
                        }
                        break;
                }
                if ((this.Length >= 2))
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
