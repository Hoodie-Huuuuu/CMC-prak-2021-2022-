using System;


namespace lab1
{
    abstract class V4Data
    {
        public string Name { get; }
        public DateTime Date { get; }
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
    }
}
