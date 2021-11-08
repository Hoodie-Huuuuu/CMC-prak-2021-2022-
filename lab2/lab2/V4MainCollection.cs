using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;

class V4MainCollection
{
    //Cвойство типа int?, возвращающее число результатов измерений во всей
    //коллекции V4MainCollection, у которых хотя бы одна компонента вектора поля
    //равна нулю.Если в коллекции нет элементов, свойство возвращает значение
    //null. Если у всех результатов измерений обе компоненты вектора поля
    //отличны от нуля, свойство возвращает значение -1
    public int? ResAnyZero
    {
        get
        {
            if (Count == 0) return null;
            var res = from test in List
                      from point in test
                      where point.Values.X == 0 || point.Values.Y == 0
                      select point;
            if (res.Count() == 0) return -1;
            return res.Count();
        }
    }


    //Свойство типа IEnumerable<IGrouping<DateTime, V4DataList>>, которое
    //группирует все элементы из V4MainCollection, которые имеют тип V4DataList,
    //по дате измерения поля(значение свойства типа DateTime базового класса
    //V4Data).
    public IEnumerable<IGrouping<DateTime, V4DataList>> SortDateList
    {
        get
        {
            var res = List.OfType<V4DataList>().GroupBy(x => x.Date);        
            return res;
        }
    }


    //Свойство IEnumerable<Vector2>, которое перечисляет без повторов как
    //экземпляры Vector2 точки, в которых измерено поле и которые встречаются в
    //каждом элементе V4Data в коллекции V4MainCollection.
    public IEnumerable<Vector2> PointInEachData
    {
        get
        {
            if (Count == 0) return null;
            var first_points = List[0].Select(x => x.XY);
            var res = List.Aggregate(first_points, (intersected, next) =>
                                intersected.Intersect(next.Select(x => x.XY)));
            return res;       
        }
    }


    private List<V4Data> List;
    public V4MainCollection()
    {
        List = new List<V4Data>();
    }
    public int Count { get { return List.Count; } }
    public V4Data this[int idx]
    {
        get
        {
            return List[idx];
        }
    }
    public bool Add(V4Data v4Data)
    {
        for (int i = 0; i < Count; ++i)
            if (List[i].Name == v4Data.Name)
                return false;

        List.Add(v4Data);
        return true;
    }
    public string ToLongString(string format)
    {
        string res = "";
        for (int i = 0; i < Count; ++i)
            res += List[i].ToLongString(format);

        return res;
    }
    public override string ToString()
    {
        string res = "";
        for (int i = 0; i < Count; ++i)
            res += List[i].ToString();

        return res;
    }
}

