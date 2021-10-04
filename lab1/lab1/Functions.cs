using System.Numerics;
using System;
namespace lab1
{
    //DELEGATE
    public delegate Vector2 Fv2Vector2(Vector2 v);

    static class Functions
    {
        public static Vector2 F1(Vector2 vector)
        {
            return new Vector2(vector.X * 2, vector.Y * 2);
        }

        public static Vector2 F2(Vector2 vector)
        {
            return new Vector2(vector.X - vector.Y, vector.X + vector.Y);
        }

        public static Vector2 F3(Vector2 vector)
        {
            Random rnd = new Random();
            return new Vector2(rnd.Next((int)vector.X,(int)vector.Y),
                                rnd.Next((int)vector.X, (int)vector.Y));
        }
    }
}
