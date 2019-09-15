using System;

namespace AWE {

    public class CyclicDelegateIterator<T> : ACyclicIndexIterator<T> {

        public static CyclicDelegateIterator<T> operator++ (CyclicDelegateIterator<T> iterator) {

            iterator.MoveNext ();
            return iterator;

        }

        public static CyclicDelegateIterator<T> operator-- (CyclicDelegateIterator<T> iterator) {

            iterator.MovePrevious ();
            return iterator;

        }

        protected Func<int, T> ValueDelegate;
        protected Func<int> CountDelegate;

        public override T current => this.ValueDelegate (this._index);
        public override T next => this.ValueDelegate (this.nextIndex);
        public override T previous => this.ValueDelegate (this.previousIndex);
        public override int count => this.CountDelegate ();

        public CyclicDelegateIterator (Func<int, T> ValueDelegate, Func<int> CountDelegate, int startingIndex = 0, int startingCycleCount = 0) {

            this.ValueDelegate = ValueDelegate;
            this.CountDelegate = CountDelegate;
            this._index = startingIndex;
            this.cycles = startingCycleCount;

        }

        public override ACyclicIndexIterator<T> Copy (bool copyCycles = false, int startingIndex = -1) => new CyclicDelegateIterator<T> (
            this.ValueDelegate,
            this.CountDelegate,
            ((startingIndex < 0) ? this._index : startingIndex),
            (copyCycles ? this.cycles : 0)
        );

    }
}