
using System;
using System.Numerics;
using System.Linq;
class Program
{
    static void Main()
    {
        Console.WriteLine("==== ОТЛАДКА LINQ ====");
        TestLinq();

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();

        Console.WriteLine("==== ОТЛАДКА РАБОТЫ С ФАЙЛАМИ ====");
        TestFile();

    }

    //==================  ОТЛАДКА LINQ  ===================
    static void TestLinq()
    {
        // Создать объект типа V4MainCollection
        V4MainCollection col = new V4MainCollection();


        //добавить элементы в коллекцию

        V4DataArray arData1 = new V4DataArray("ArrayData1", DateTime.Now,
                               3, 3, new Vector2(0.5f, 0.5f), Functions.F1);

        V4DataArray arData2 = new V4DataArray("ArrayData2", DateTime.Today,
                                   4, 4, new Vector2(1f, 1f), Functions.F2);

        V4DataList listData1 = new V4DataList("ListData1",
                                       new DateTime(2002, 2, 27, 12, 0, 0));
        listData1.Add(new DataItem(new Vector2(1, 1), new Vector2(2, 2)));


        V4DataList emptyList = new V4DataList("Empty List",
                                        new DateTime(1945, 5, 9, 0, 43, 0));

        V4DataList emptyList2 = new V4DataList("Empty List2",
                                        new DateTime(2002, 2, 27, 12, 0, 0));
        V4DataArray emptyArray = new V4DataArray("Empty Array",
                                                            DateTime.Today);


        Console.WriteLine("=======ДЛЯ ПУСТОЙ КОЛЛЕКЦИИ:");
        CheckProperties(col);


        Console.WriteLine();
        Console.WriteLine("======ДЛЯ НЕПУСТОЙ КОЛЛЕКЦИИ БЕЗ ПУСТЫХ ЭЛЕМЕНТОВ:");
        col.Add(arData1);
        col.Add(arData2);
        col.Add(listData1);
        CheckProperties(col);

       
        Console.WriteLine();
        Console.WriteLine("======ДЛЯ НЕПУСТОЙ КОЛЛЕКЦИИ C ПУСТЫМИ и НЕПУСТЫМИ ЭЛЕМЕНТАМИ:");
        col.Add(emptyList);
        col.Add(emptyArray);
        col.Add(emptyList2);
        CheckProperties(col);


        Console.WriteLine();
        Console.WriteLine("======ДЛЯ НЕПУСТОЙ КОЛЛЕКЦИИ C ПУСТЫМИ ЭЛЕМЕНТАМИ:");
        // Создать объект типа V4MainCollection
        V4MainCollection col2 = new V4MainCollection();
        col2.Add(emptyList);
        col2.Add(emptyArray);
        col2.Add(emptyList2);
        CheckProperties(col2);
    }


    //======================== ОТЛАДКА РАБОТЫ С ФАЙЛАМИ ========================
    static void TestFile()
    {
        Console.WriteLine("=== ДЛЯ ПУСТЫХ ДАННЫХ ===");
        Console.WriteLine();


        //V4DataArray Пустой в Файле
        V4DataArray arData = new V4DataArray("ArrayData", DateTime.Today);
                                      
        Console.WriteLine("=== ИСХОДНЫЙ ОБЪЕКТ V4DataArray:");
        Console.WriteLine(arData.ToLongString("F2"));

        arData.SaveAsText("test.txt");
        V4DataArray arDataReturned = new V4DataArray("EmptyArr",
                                          new DateTime(2002, 2, 27, 12, 0, 0));
        V4DataArray.LoadAsText("test.txt", ref arDataReturned);

        Console.WriteLine("=== НОВЫЙ ОБЪЕКТ V4DataArray ИЗ ФАЙЛА:");
        Console.WriteLine(arDataReturned.ToLongString("F2"));


        //V4DataList Пустой в Файле
        V4DataList listData = new V4DataList("ListData", DateTime.Today);

        Console.WriteLine("=== ИСХОДНЫЙ ОБЪЕКТ V4DataList:");
        Console.WriteLine(listData.ToLongString("F2"));

        listData.SaveBinary("testBinary");
        V4DataList listDataBinary = new V4DataList("EmptyList",
                                           new DateTime(2002, 2, 27, 12, 0, 0));
        V4DataList.LoadBinary("testBinary", ref listDataBinary);

        Console.WriteLine("=== НОВЫЙ ОБЪЕКТ V4DataList ИЗ БИНАРНОГО ФАЙЛА:");
        Console.WriteLine(listDataBinary.ToLongString("F2"));


        Console.WriteLine("=== ДЛЯ НЕПУСТЫХ ДАННЫХ ===");
        Console.WriteLine();


        //V4DataArray НЕпустой в файле
        arData = new V4DataArray("ArrayData", DateTime.Today,
                                       4, 4, new Vector2(1f, 1f), Functions.F2);

        Console.WriteLine("=== ИСХОДНЫЙ ОБЪЕКТ V4DataArray:");
        Console.WriteLine(arData.ToLongString("F2"));

        arData.SaveAsText("test.txt");
        arDataReturned = new V4DataArray("EmptyArr",
                                          new DateTime(2002, 2, 27, 12, 0, 0));
        V4DataArray.LoadAsText("test.txt", ref arDataReturned);

        Console.WriteLine("=== НОВЫЙ ОБЪЕКТ V4DataArray ИЗ ФАЙЛА:");
        Console.WriteLine(arDataReturned.ToLongString("F2"));


        //V4DataList НЕпустой в Файле
        for (int i = 0; i < 8; ++i)
        {
            listData.Add(new DataItem(new Vector2(i, i),
                                                new Vector2(100 * i, 100 * i)));
        }

        Console.WriteLine("=== ИСХОДНЫЙ ОБЪЕКТ V4DataList:");
        Console.WriteLine(listData.ToLongString("F2"));

        listData.SaveBinary("testBinary");
        listDataBinary = new V4DataList("EmptyList",
                                           new DateTime(2002, 2, 27, 12, 0, 0));
        V4DataList.LoadBinary("testBinary", ref listDataBinary);

        Console.WriteLine("=== НОВЫЙ ОБЪЕКТ V4DataList ИЗ БИНАРНОГО ФАЙЛА:");
        Console.WriteLine(listDataBinary.ToLongString("F2"));
    }


    //Метод для красоты кода, проверяет каждое свойство, использующее LINQ 
    static void CheckProperties(V4MainCollection col)
    {
        //Проверка свойства ResAnyZero
        Console.WriteLine();

        var res = col.ResAnyZero;
        if (res == null)
            Console.WriteLine("ResAnyZero = null");
        else
            Console.WriteLine($"ResAnyZero = {res}");



        //Проверка свойства SortDateList
        Console.WriteLine();
        var res2 = col.SortDateList;
        Console.WriteLine($"SortDateList.Count() = " +
                                            $"{res2.Count()}");
        try
        {
            foreach (var group in res2)
            {
                Console.WriteLine($"Key = {group.Key}");
                foreach (var item in group)
                {
                    Console.WriteLine(item.Name);
                }
                Console.WriteLine();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("SortDateList: " + e.Message);
        }
        Console.WriteLine();


        //Проверка свойства PointInEachData
        Console.WriteLine("PointInEachData");
        var res3 = col.PointInEachData;
        if (res == null)
            Console.WriteLine("Таких точек нет, проверьте," +
                                       "что в коллекции нет пустых элементов");
        else
            foreach (var item in res3)
                Console.WriteLine(item);


        //вывести данные объекта V4MainCollection с помощью метода
        //ToLongString (string format).
        Console.WriteLine();
        Console.WriteLine("===ИСХОДНЫЕ ДАННЫЕ");
        if (col.Count == 0) Console.WriteLine("Пустая коллекция");
        for (int i = 0; i < col.Count; ++i)
        {
            Console.WriteLine();
            Console.WriteLine(col[i].ToLongString("F2"));
        }
    }
}