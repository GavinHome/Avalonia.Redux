namespace samples.Counter;

public class CounterState
{
    public int Count { get; set; }

    public override string ToString()
    {
        return $"Count: {Count.ToString()}";
    }

    internal static CounterState initState(Dictionary<string, dynamic>? param) => new CounterState() { Count = 1 };
}