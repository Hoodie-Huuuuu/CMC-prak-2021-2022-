using System;
using System.IO;
using System.Numerics;
using System.Collections.Generic;

class V4DataList: V4Data
{
    public List<DataItem> Data { get; }
    public V4DataList(string name, DateTime date): base(name, date)
    {
        Data = new List<DataItem>(); 
    }

    public bool Add(DataItem newItem)
    {
        for (int i = 0; i < Data.Count; ++i)
            if (Data[i].XY == newItem.XY)
                return false;

        Data.Add(newItem);
        return true;
    }

    public int AddDefaults(int nItems, Fv2Vector2 F)
    {
        int addedItems = 0;
        for (int i = 0; i < nItems; ++i)
        {
            Random rnd = new Random();

            Vector2 point = new Vector2(rnd.Next(-5, 5), rnd.Next(-5, 5)); 
            Vector2 value = F(point);
            if (Add(new DataItem(point, value)))
                ++addedItems;
        }
        return addedItems;
    }

    public override int Count { get{ return Data.Count; }}
    public override float MaxFromOrigin
    {
        get
        {
            float maxDist = 0;
            for (int i = 0; i < Data.Count; ++i)
            {
                float temp = Data[i].XY.Length();
                if (maxDist < temp)
                    maxDist = temp;
            }
            return maxDist;
        }
    }

    public override string ToString()
    {
        return "Type: " + this.GetType() + "\nName: " + Name + "\nDate: " +
            Date.ToString() + "\nNumber of Items: " + Count + '\n';
    }

    public override string ToLongString(string format)
    {
        string res = this.ToString();
        for (int i = 0; i < Count; ++i)
        {
            res += "Item number " + (i + 1) + ":\n" +
                                        Data[i].ToLongString(format) + '\n';
        }
        return res; 
    }

    //Реализация Интерфейса
    //в классе V4DataList реализация интерфейса IEnumerable<DataItem>
    //перечисляет все элементы DataItem из списка List<DataItem>;
    public override IEnumerator<DataItem> GetEnumerator()
    {
        return Data.GetEnumerator();
    }


    //SaveBinary from Lab2
    public bool SaveBinary(string filename)
    {
        try
        {
            using (BinaryWriter writer = new BinaryWriter(
                                    File.Open(filename, FileMode.OpenOrCreate)))
            {
                writer.Write(Name);
                writer.Write(Date.ToShortDateString() + " " +
                                                       Date.ToLongTimeString());

                writer.Write(Count);
                foreach (var dItem in Data)
                {
                    writer.Write(dItem.XY.X);
                    writer.Write(dItem.XY.Y);
                    writer.Write(dItem.Values.X);
                    writer.Write(dItem.Values.Y);
                }
            }
        }
        catch(Exception e)
        {
            Console.WriteLine("V4DataList -> SaveBinary: " + e.Message);
            return false;
        }
        return true;
    }


    //LoadBinary from Lab2
    public static bool LoadBinary(string filename, ref V4DataList v4)
    {
        try
        {
            using (BinaryReader reader = new BinaryReader(
                                    File.Open(filename, FileMode.Open)))
            {
                string name = reader.ReadString();
                string date = reader.ReadString();
                V4DataList res = new V4DataList(name, DateTime.Parse(date));
                int countItems = reader.ReadInt32();
                for (int i = 0; i < countItems; ++i)
                {
                    float coordX = reader.ReadSingle();
                    float coordY = reader.ReadSingle();
                    float valsX = reader.ReadSingle();
                    float valsY = reader.ReadSingle();

                    res.Add(new DataItem(new Vector2(coordX, coordY),
                                                    new Vector2(valsX, valsY)));
                }
                v4 = res;
            }
        }

        catch (Exception e)
        {
            Console.WriteLine("V4DataList -> SaveBinary: " + e.Message);
            return false;
        }
        return true;
    }
}

