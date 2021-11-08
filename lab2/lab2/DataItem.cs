using System.Numerics;

struct DataItem
{

    public Vector2 XY { get; set; }
    public Vector2 Values { get; set; }
    public DataItem(Vector2 coord, Vector2 vals)
    {
        XY = coord;
        Values = vals;
    }
    public string ToLongString(string format)
    {
        return "coordinates: " + XY.ToString(format) + "\nvalues of field: " +
                    Values.ToString(format) + "\nabs: " +
                    Vector2.Abs(Values).ToString(format) + '\n';
    }
    public override string ToString()
    {
        return "coordinates: " + XY.ToString() + "\nvalues of field: " +
                    Values.ToString() + '\n';
    }
}


