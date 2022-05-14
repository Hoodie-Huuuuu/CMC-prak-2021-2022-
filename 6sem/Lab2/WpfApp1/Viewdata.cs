using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1;
namespace WpfApp1
{
    public class Viewdata
    {

        //Данные для графика
        public ChartData ChartData { get; set; }
        public SplineParameters sp { get; set; }
        public SplinesData sd { get; set; }


        //Тут хранится информация MeasuredData для вывода в Listbox
        public ObservableCollection<string> MeasuredData { get; set; }

        //Коллекции с четырьмя значениями для вывода обоих сплайнов
        public ObservableCollection<string> SplineValues1 { get; set; }
        public ObservableCollection<string> SplineValues2 { get; set; }

        //Коллекции со значениями вторых производных на границе для обоих сплайнов
        public ObservableCollection<string> Derivatives1 { get; set; }
        public ObservableCollection<string> Derivatives2 { get; set; }

        public Viewdata()
        {
            sp = new SplineParameters();
            sd = new SplinesData();
            MeasuredData = new ObservableCollection<string>();
            SplineValues1 = new ObservableCollection<string>();
            SplineValues2 = new ObservableCollection<string>();
            Derivatives1 = new ObservableCollection<string>();
            Derivatives2 = new ObservableCollection<string>();
        }

    }
}

