using System;
using System.Numerics;
using System.Collections.Generic;
namespace lab1
{
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
    }
}
