using System.Numerics;
using System;
namespace lab1
{
    class V4DataArray : V4Data
    {
        public int Xstep { get; }
        public int Ystep { get; }
        public Vector2 Step { get; }
        public Vector2[,] Grid { get; }
        public V4DataArray(string name, DateTime date) : base(name, date)
        {
            Grid = new Vector2[0, 0];
        }

        public V4DataArray(string name, DateTime date, int nX, int nY,
                                Vector2 step, Fv2Vector2 F) : base(name, date)
        {
            Step = step;
            Xstep = nX;
            Ystep = nY;
            Grid = new Vector2[nX, nY];

            for (int i = 0; i < nX; ++i)
                for (int j = 0; j < nY; ++j)
                    Grid[i, j] = F(new Vector2(i * Step.X, j * Step.Y));
        }

        public override int Count { get { return Grid.Length; } }
        public override float MaxFromOrigin
        {
            get
            {
                if (Xstep == 0 || Ystep == 0) return 0;
                Vector2 temp = new Vector2((Xstep - 1) * Step.X,
                                                            (Ystep-1) * Step.Y);
                return temp.Length();
            }
        }

        public override string ToString()
        {
            return "Type: " + this.GetType() + "\nName: " + Name + "\nDate: " +
                Date.ToString() + "\nXstep: " + Xstep + "\nYstep: " + Ystep +
                "\nStep: " + Step.ToString() + '\n';
        }

        public override string ToLongString(string format)
        {
            string res = this.ToString() + '\n';
            for (int i = 0; i < Xstep; ++i)
                for (int j = 0; j < Ystep; ++j)
                    res += "Point: " + '<' + i * Step.X + ", " + j * Step.Y +
                            ">\nValue: " + Grid[i, j].ToString(format) +
                            "\nAbs: " + Vector2.Abs(Grid[i, j]).ToString(format) + "\n\n";

            return res;
        }

        public static explicit operator V4DataList(V4DataArray arData)
        {
            V4DataList res = new V4DataList(arData.Name, arData.Date);

            for (int i = 0; i < arData.Xstep; ++i)
                for (int j = 0; j < arData.Ystep; ++j)
                {
                    Vector2 point = new Vector2(i * arData.Step.X,
                                                            j * arData.Step.Y);
                    res.Add(new DataItem(point, arData.Grid[i, j]));
                }
                    
            return res;
        }
    }
}
