
using System;
using System.Numerics;

class Program
{
    static void Main()
    {
        V4DataArray arData1 = new V4DataArray("First Lab Data", DateTime.Now,
                                   8, 2, new Vector2(0.05f, 0.05f), Functions.F1);
        Console.WriteLine(arData1.ToLongString("F2"));

        
        try
        {
            //Подсчет второй производной
            Console.WriteLine(arData1.SecondDerivative());
            Console.WriteLine();
            Console.WriteLine("2ые Производные:");
            Console.WriteLine("Точка          Через Сплайн       Через Приближение");

            string res = "";
            for (int i = 0; i < arData1.Ny; ++i)
            {
                for (int j = 0; j < arData1.Nx; ++j)
                {
                    Vector2 splDer = (Vector2)arData1.SecondDerivativeSplineAt(j, i);
                    Vector2 myDer = (Vector2)arData1.SecondDerivativeCDAt(j, i);
                    //$"<{splDer.X.ToString("F2")},  {splDer.Y.ToString("F2")}>"
                    //$"<{myDer.X.ToString("F2")},  {myDer.Y.ToString("F2")}>"

                    res += $"<{(j * arData1.Step.X).ToString("F2")}, {(i* arData1.Step.Y).ToString("F2")}>   " +
                        myDer.ToString("F2") +  "       " +  myDer.ToString("F2")+ "\n";
                }
            }
            Console.WriteLine(res);
            Console.WriteLine();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
    }
}
    