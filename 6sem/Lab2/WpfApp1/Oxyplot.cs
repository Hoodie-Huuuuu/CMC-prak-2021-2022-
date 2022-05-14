
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Legends;

namespace WpfApp1
{
    class OxyPlotModel
    {
        public ChartData data;
        public PlotModel plotModel { get; private set; }
        public OxyPlotModel(ChartData data)
        {
            this.data = data;
            this.plotModel = new PlotModel { Title = "Интерполяция" };
            this.plotModel.Series.Clear();
        }
        public OxyPlotModel()
        {
            this.plotModel.Series.Clear();

        }
        //отрисовка сплайнов
        public void AddSeries()
        {
            //при повторном нажатии на кнопку
            this.plotModel.Series.Clear();
            this.Scatter();

            for (int js = 0; js < data.YL.Count; js++)
            {
                OxyColor color = (js == 0) ? OxyColors.Red : OxyColors.Blue;
                LineSeries lineSeries = new LineSeries();
                for (int j = 0; j < data.X.Length; j++) lineSeries.Points.Add(new DataPoint(data.X[j], data.YL[js][j]));
                lineSeries.Color = color;

                lineSeries.MarkerType = MarkerType.Circle;
                lineSeries.MarkerSize = 4;
                lineSeries.MarkerStroke = color;
                lineSeries.MarkerFill = color;
                lineSeries.Title = data.legends[js];

                Legend legend = new Legend();
                plotModel.Legends.Add(legend);
                this.plotModel.Series.Add(lineSeries);
            }
        }


        //отрисовка неравномерной сетки
        public void Scatter()
        {

            var s1 = new ScatterSeries()
            {
                MarkerType = MarkerType.Triangle,
                MarkerFill = OxyColors.Black

            };
            for (int i = 0; i < this.data.non_uniform_x.Length; i++)
                s1.Points.Add(new ScatterPoint(this.data.non_uniform_x[i], this.data.non_uniform_y[i], 6, 400));
            

            this.plotModel.Series.Add(s1);
        }
    }
}
