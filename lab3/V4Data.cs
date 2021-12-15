using System;
using System.Collections;
using System.Collections.Generic;


abstract class V4Data: IEnumerable<DataItem>
{
    public string Name { get; protected set; }
    public DateTime Date { get; protected set; }

    public V4Data(string name, DateTime date)
    {
        Name = name;
        Date = date;
    }
    public abstract int Count { get; }
    public abstract float MaxFromOrigin { get; }
    public abstract string ToLongString(string format);
    public override string ToString()
    {
        return Name.ToString() + " " + Date.ToString();
    }
    //Методы интерфейса
    public abstract IEnumerator<DataItem> GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}
