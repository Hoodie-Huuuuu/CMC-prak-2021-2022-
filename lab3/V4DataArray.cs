﻿using System.Numerics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;



class V4DataArray : V4Data
{
    public int Nx { get; private set; }
    public int Ny { get; private set; }
    public Vector2 Step { get; private set; }
    public Vector2[,] Grid { get; private set; }

    //массив значений второй производной поля по переменной x в узлах сетки
    public Vector2[,] SecondDerivativeX { get; set; }


    //возвращает значение второй производной поля по переменной x в узле сетки
    //с индексами[jx, jy], вычисленное с помощью сплайн интерполяции.
    //null если выход за границы
    //public Vector2? SecondDerivativeSplineAt(int jx, int jy)
    //{

    //}

    //вторая производная поля по переменной x
    //null если выход за границы
    public Vector2? SecondDerivativeCDAt(int jx, int jy)
    {
        //Выход за пределы
        if (jx < 0 || jx >= Nx || jy < 0 || jy >= Ny) return null;

        //Производная на границах
        int left = jx - 1, right = jx + 1;
        if (jx == 0) left = 0;
        if (jx == Nx - 1) right = Nx-1;

        //Подсчет производных
        float F1_secondDer = (Grid[left, jy].X - 2 * Grid[jx, jy].X + Grid[right, jy].X)
                                / (Step.X * Step.X);
        float F2_secondDer = (Grid[left, jy].Y - 2 * Grid[jx, jy].Y + Grid[right, jy].Y)
                                / (Step.X * Step.X);
        return new Vector2(F1_secondDer, F2_secondDer);
    }


    [DllImport("path/to/lab3/mylib.dylib")]
    static extern void say(string message);

    public bool SecondDerivative()
    {
        say("chlen");
        //Console.WriteLine(getpid());
        return true;
    }



    //SaveAsText from Lab2
    public bool SaveAsText(string filename)
    {
        
        try
        {
            using (StreamWriter sw = File.CreateText("test.txt"))
            {
                
                sw.WriteLine("\"Name\": " + Name);
                sw.WriteLine("\"Date\": " + Date.ToShortDateString() + " " +
                                                       Date.ToLongTimeString());
                sw.WriteLine("\"Xstep\": " + Nx);
                sw.WriteLine("\"Ystep\": " + Ny);
                sw.WriteLine("\"Step\": " + Step);
                sw.WriteLine("\"Grid\": [");
                foreach (var item in Grid)
                {
                    sw.WriteLine(item);
                }
                sw.WriteLine("]");
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
                            
                            v4.Nx = int.Parse(res[1]);
                            break;

                        case "\"Ystep\":":
                            v4.Ny = int.Parse(res[1]);
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

                            Vector2[,] tempGrid = new Vector2[v4.Nx, v4.Ny];
                            for (int i = 0; i < v4.Nx; ++i)
                            {
                                for(int j = 0; j < v4.Ny; ++j)
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

    public V4DataArray(string name, DateTime date, int NX, int NY,
                            Vector2 step, Fv2Vector2 F) : base(name, date)
    {
        Step = step;
        this.Nx = NX;
        this.Ny = NY;
        Grid = new Vector2[Nx, Ny];

        for (int i = 0; i < Nx; ++i)
            for (int j = 0; j < Ny; ++j)
                Grid[i, j] = F(new Vector2(i * Step.X, j * Step.Y));
    }

    public override int Count { get { return Grid.Length; } }
    public override float MaxFromOrigin
    {
        get
        {
            if (Nx == 0 || Ny == 0) return 0;
            Vector2 temp = new Vector2((Nx - 1) * Step.X,
                                                        (Ny-1) * Step.Y);
            return temp.Length();
        }
    }

    public override string ToString()
    {
        return "Type: " + this.GetType() + "\nName: " + Name + "\nDate: " +
            Date.ToString() + "\nXstep: " + Nx + "\nYstep: " + Ny +
            "\nStep: " + Step.ToString() + '\n';
    }

    public override string ToLongString(string format)
    {
        string res = this.ToString() + '\n';
        for (int i = 0; i < Nx; ++i)
            for (int j = 0; j < Ny; ++j)
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

        for (int i = 0; i < arData.Nx; ++i)
            for (int j = 0; j < arData.Ny; ++j)
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
        for (int i = 0; i < Nx; ++i)
            for (int j = 0; j < Ny; ++j)
                yield return new DataItem(new Vector2(i * Step.X, j * Step.Y),
                                                                    Grid[i, j]);  
    }
}

