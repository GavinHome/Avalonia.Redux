using ReactiveUI;
using samples.Counter;

namespace samples.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        //public int _counter = 0;

        //public int Counter
        //{
        //    get => _counter;
        //    set => this.RaiseAndSetIfChanged(ref _counter, value);
        //}

        //public CounterState _counterState = new CounterState();

        //public CounterState CounterState
        //{
        //    get => _counterState;
        //    set => this.RaiseAndSetIfChanged(ref _counterState, value);
        //}
    }
}