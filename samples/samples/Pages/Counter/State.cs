using ReactiveUI;
using samples.ViewModels;

namespace samples.Counter;

[Serializable]
public class CounterState : ViewModelBase
{
    public int _count;

    public int Count
    {
        get => _count;
        set => this.RaiseAndSetIfChanged(ref _count, value);
    }

    ////public int Count { get; set; }

    public override string ToString()
    {
        return $"Count: {Count}";
    }
}