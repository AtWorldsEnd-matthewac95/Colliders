using System.Collections.ObjectModel;

namespace AWE {

    public class CyclicCollectionIterator<T> : ACyclicIndexIterator<T> {

        public static CyclicCollectionIterator<T> operator++ (CyclicCollectionIterator<T> iterator) {

            iterator.MoveNext ();
            return iterator;

        }

        public static CyclicCollectionIterator<T> operator-- (CyclicCollectionIterator<T> iterator) {

            iterator.MovePrevious ();
            return iterator;

        }

        protected ReadOnlyCollection<T> collection;

        public override int count => this.collection.Count;
        public override T current => this.collection[this._index];
        public override T next => this.collection[this.nextIndex];
        public override T previous => this.collection[this.previousIndex];

        public CyclicCollectionIterator (ReadOnlyCollection<T> collection, int startingIndex = 0, int startingCycleCount = 0) {

            this.collection = collection;
            this._index = startingIndex;
            this.cycles = startingCycleCount;

        }

        public override ACyclicIndexIterator<T> Copy (bool copyCycles = false, int startingIndex = -1) => new CyclicCollectionIterator<T> (
            this.collection,
            ((startingIndex < 0) ? this._index : startingIndex),
            (copyCycles ? this.cycles : 0)
        );

    }
}