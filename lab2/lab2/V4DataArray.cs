using System.Numerics;
using System;
using System.Collections.Generic;
using System.IO;
//using System.Text.Json;


class V4DataArray : V4Data
{
    public int Xstep { get; private set; }
    public int Ystep { get; private set; }
    public Vector2 Step { get; private set; }
    public Vector2[,] Grid { get; private set; }


    //SaveAsText from Lab2
    public bool SaveAsText(string filename)
    {

        //string serialized = JsonSerializer.Serialize<V4DataArray>(this);
        
        try
        {
            using (StreamWriter sw = File.CreateText("test.txt"))
            {
                
                sw.WriteLine("\"Name\": " + Name);
                sw.WriteLine("\"Date\": " + Date.ToShortDateString() + " " +
                                                       Date.ToLongTimeString());
                sw.WriteLine("\"Xstep\": " + Xstep);
                sw.WriteLine("\"Ystep\": " + Ystep);
                sw.WriteLine("\"Step\": " + Step);
                sw.WriteLine("\"Grid\": [");
                foreach (var item in Grid)
                {
                    sw.WriteLine(item);
                }
                sw.WriteLine("]");

                //sw.WriteLine(serialized);
            }
        }
        catch(Exception e)
        {
            Console.WriteLine("V4DataArray -> SaveAsText -> " + e.Message);
            return false;
        }
        return true;
       
    }

    //LoadAsText from Lab2
    public static bool LoadAsText(string filename, ref V4DataArray v4)
    {
        
        try
        {
            using (StreamReader sr = new StreamReader(filename))
            {
                //string serialized = sr.ReadToEnd();
                //v4 = JsonSerializer.Deserialize<V4DataArray>(serialized);
                
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] res = line.Split(' ');
                    switch (res[0])
                    {

                        case "\"Name\":":
                            v4.Name = res[1];
                            break;

                        case "\"Date\":":
                            v4.Date = DateTime.Parse(res[1] + " " + res[2] );
                            break;

                        case "\"Xstep\":":
                            
                            v4.Xstep = int.Parse(res[1]);
                            break;

                        case "\"Ystep\":":
                            v4.Ystep = int.Parse(res[1]);
                            break;

                        case "\"Step\":":
                            res[1] = res[1].Trim('<');
                            res[2] = res[2].Trim('>');
                            v4.Step = new Vector2(float.Parse(res[1]),
                                                            float.Parse(res[2]));
                            
                            break;

                        case "\"Grid\":":
                            if (res[1] != "[") throw
                                            new FormatException("\"[\" missed");

                            Vector2[,] tempGrid = new Vector2[v4.Xstep, v4.Ystep];
                            for (int i = 0; i < v4.Xstep; ++i)
                            {
                                for(int j = 0; j < v4.Ystep; ++j)
                                {
                                    if ((line = sr.ReadLine()) == null) throw
                                        new FormatException("not enough elements");

                                    line = line.Trim(new Char[] { '<', '>' });
                                    string[] vals = line.Split(' ');
                                    tempGrid[i, j] = new Vector2(float.Parse(vals[0]),
                                                                 float.Parse(vals[1]));
                                }
                            }
                            if ((line = sr.ReadLine()) != "]") throw
                                            new FormatException("\"]\" missed");
                            v4.Grid = tempGrid;
                            
                            break;

                        default:
                            throw new FormatException(res[0] + ": unexpected string");
                            
                    }
                }

                
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("V4DataArray -> LoadAsText -> " + e.Message);
            return false;
        }
        return true;
    }


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
            {
                res += "Point: " + '<' + i * Step.X + ", " + j * Step.Y +
                    ">\nValue: " + Grid[i, j].ToString(format) + "\nAbs: "
                    + Vector2.Abs(Grid[i, j]).ToString(format) + "\n\n";
            }
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
    //Реализация интерфейса
    public override IEnumerator<DataItem> GetEnumerator()
    {
        //в классе V4DataArray реализация интерфейса IEnumerable<DataItem>
        //перечисляет все данные на сетке как экземпляры DataItem − для каждого
        //узла сетки создается экземпляр DataItem с координатами узла сетки и
        //значением типа Vector2 в узле сетки.
        for (int i = 0; i < Xstep; ++i)
            for (int j = 0; j < Ystep; ++j)
                yield return new DataItem(new Vector2(i * Step.X, j * Step.Y),
                                                                    Grid[i, j]);  
    }
}

