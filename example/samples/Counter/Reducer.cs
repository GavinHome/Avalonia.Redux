﻿using Redux;
using Redux.Component;

namespace samples.Counter;

internal class Reducer
{
    internal static Reducer<CounterState> buildReducer() => ReducerConverter.asReducers<CounterState>(new Dictionary<object, Reducer<CounterState>>()
    {
        {
            CounterAction.add, _add
        }
    });

    private static CounterState _add(CounterState state, Redux.Action action)
    {
        CounterState newState = state.Clone(); //clone
        newState.Count += action.Payload;
        return newState;
    }
}
