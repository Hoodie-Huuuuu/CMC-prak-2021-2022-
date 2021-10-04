using lab1;
using System;
using System.Numerics;
class Program
{
    static void Main()
    {
        // ========================= 1 ==============================
        //Создать объект типа V4DataArray
        V4DataArray arData1 = new V4DataArray("First Lab Data", DateTime.Now,
                                   3, 3, new Vector2(0.5f, 0.5f), Functions.F1);


        //вывести его данные с помощью метода ToLongString (string format)
        Console.WriteLine(arData1.ToLongString("F2"));


        //С помощью оператора преобразования, определенного в классе V4DataArray
        //преобразовать его в объект типа V4DataList,
        V4DataList listData1 = (V4DataList)arData1;


        //вывести данные V4DataList с помощью метода ToLongString (string format).
        Console.WriteLine(listData1.ToLongString("F2"));


        //Для исходного объекта V4DataArray и для объекта V4DataList
        //полученного  результате преобразования, вывести значения свойств
        //Count и MaxFromOrigin.
        Console.WriteLine("Array count: {0} \nArray MaxLength: {1} \nList " +
                    "count: {2} \nList MaxLength: {3} \n", arData1.Count,
                   arData1.MaxFromOrigin, listData1.Count, listData1.MaxFromOrigin);

        // ========================== 2 ==============================
        //2 Создать объект типа V4MainCollection
        V4MainCollection col = new V4MainCollection();


        //с помощью метода bool Add(V4Data v4Data) добавить в коллекцию
        //два элемента типа V4DataArray и два элемента типа V4DataList
        V4DataArray arData2 = new V4DataArray("ArrayData2", DateTime.Now,
                                   4, 4, new Vector2(1f, 1f), Functions.F2);

        V4DataList listData2 = new V4DataList("ListData2", DateTime.Now);
        listData2.Add(new DataItem(new Vector2(1, 1), new Vector2(2, 2)));
        
        if (!col.Add(arData1)) Console.WriteLine("already exists {0}",
                                                                arData1.Name);
        if (!col.Add(listData1)) Console.WriteLine("already exists {0}",
                                                                listData1.Name);
        if (!col.Add(arData2)) Console.WriteLine("already exists {0}",
                                                                 arData2.Name);
        if (!col.Add(listData2)) Console.WriteLine("already exists {0}",
                                                                listData2.Name);


        //и вывести данные объекта V4MainCollection с помощью метода
        //ToLongString (string format).
        for (int i = 0; i < col.Count; ++i)
        {
            Console.WriteLine(col[i].ToLongString("F2"));
        }

        //=============================== 3 =================================
        //Для всех элементов из V4MainCollection
        //вызвать свойства Count и MaxFromOrigin
        //и вывести их значения.
        for (int i = 0; i < col.Count; ++i)
        {
            Console.WriteLine("Count {0}: {1}\nMaxLength {0}: {2}", i + 1, col[i].Count,
                                                        col[i].MaxFromOrigin);
        }






    }
}