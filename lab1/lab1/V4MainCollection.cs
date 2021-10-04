using System;
using System.Numerics;
using System.Collections.Generic;
namespace lab1
{
    class V4MainCollection
    {
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
}
