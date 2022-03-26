using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;




namespace ClassLibrary1
{
    public class VMGrid
    {
        public VMGrid(int len, float start, float end, float step)
        {
            Length = len;
            Start = start;
            End = end;
            Step = step;
        }

        public VMGrid()
        {
            Length = 100;
            Start = 0;
            End = 1;
            Step = 0.01f;
        }
        //длина вектора аргументов функции
        public int Length { get; set; }
        public float Start { get; set; }
        public float End { get; set; }
        public float Step { get; }

        public override string ToString()
        {
            return $"Grid Params:\nGrid size: {Length}\nStart: {Start}\nEnd: {End}\n" +
                $"Step: {Step}\n";
        }
    }
}
