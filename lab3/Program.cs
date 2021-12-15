
using System;
using System.Numerics;

class Program
{
    static void Main()
    {
        V4DataArray arData1 = new V4DataArray("First Lab Data", DateTime.Now,
                                   3, 3, new Vector2(0.5f, 0.5f), Functions.F1);
        //Console.WriteLine(arData1.ToLongString("F2"));

        //Console.WriteLine();
        //Console.WriteLine("Производные:");
        //for (int i = 0; i < arData1.Nx + 1; ++i)
        //{
        //    for (int j = 0; j < arData1.Ny + 1; ++j)
        //    {
        //        Console.WriteLine("точка " + i + " " + j);
        //        Console.WriteLine(arData1.SecondDerivativeCDAt(i, j));
        //        Console.WriteLine();
        //    }
        //}
        try
        {
            arData1.SecondDerivative();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
    }
}
    