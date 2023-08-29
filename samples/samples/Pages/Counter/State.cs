using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace samples.Counter;

public class CounterState : ReactiveObject
{
    ////public int _count;

    ////public int Count
    ////{
    ////    get => _count;
    ////    set => this.RaiseAndSetIfChanged(ref _count, value);
    ////}

    [Reactive]
    public int Count { get; set; }

    public override string ToString()
    {
        return $"Count: {Count}";
    }
} 