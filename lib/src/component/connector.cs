using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace component;

internal class connector
{
}


/// Definition of Enhanced creating a store
// public delegate StoreCreator<T> StoreEnhancer<T>(StoreCreator<T> creator);

// /// Definition of Connector which connects Reducer<S> with Reducer<P>.
// /// 1. How to get an instance of type P from an instance of type S.
// /// 2. How to synchronize changes of an instance of type P to an instance of type S.
// /// 3. How to clone a new S.
// public abstract class AbstractConnector<S, P>
// {
//     public abstract P Get(S state);
//
//     /// For mutable state, there are three abilities needed to be met.
//     ///     1. get: (S) => P
//     ///     2. set: (S, P) => void
//     ///     3. shallow copy: s.clone()
//     ///
//     /// For immutable state, there are two abilities needed to be met.
//     ///     1. get: (S) => P
//     ///     2. set: (S, P) => S
//     ///
//     /// See in [connector].
//     public abstract SubReducer<S> subReducer(Reducer<P> reducer);
// }