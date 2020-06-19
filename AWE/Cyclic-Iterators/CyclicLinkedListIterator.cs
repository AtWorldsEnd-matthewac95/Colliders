namespace AWE {

    public class CyclicLinkedListIterator<T> : ICyclicIterator<LinkedListPosition<T>> {

        public static CyclicLinkedListIterator<T> operator++ (CyclicLinkedListIterator<T> iterator) {

            iterator.MoveNext ();
            return iterator;

        }

        public static CyclicLinkedListIterator<T> operator-- (CyclicLinkedListIterator<T> iterator) {

            iterator.MovePrevious ();
            return iterator;

        }

        protected LinkedList<T> list;

        // If the iterated object is empty, set the cycle count to 1.
        // This helps prevent index out of bounds exceptions if the iterator
        //     used in a for loop that checks cycle count as an exit criteria.
        protected int defaultCycles => (this.list.isEmpty ? 1 : 0);

        public LinkedListPosition<T> current { get; protected set; }
        public int cycles { get; protected set; }

        public LinkedListPosition<T> next => this.LookNext (out _);
        public LinkedListPosition<T> previous => this.LookPrevious (out _);

        public CyclicLinkedListIterator (LinkedList<T> list, int startingCycleCount = 0) : this (list, list.CopyFrontPosition (), startingCycleCount) {}
        public CyclicLinkedListIterator (LinkedList<T> list, LinkedListPosition<T> startingPosition, int startingCycleCount = 0) {

            this.list = list;
            this.current = (list.Contains (startingPosition) ? startingPosition : list.CopyFrontPosition ());
            this.cycles = ((startingCycleCount < 1) ? this.defaultCycles : startingCycleCount);

        }

        private LinkedListPosition<T> LookNext (out bool isNextIntermediate) {

            var value = current.Copy ();
            value.MoveNext ();
            isNextIntermediate = value.hasValue;

            if (!isNextIntermediate) {

                value = list.CopyFrontPosition ();

            }

            return value;

        }

        private LinkedListPosition<T> LookPrevious (out bool isNextIntermediate) {

            var value = current.Copy ();
            value.MovePrevious ();
            isNextIntermediate = value.hasValue;

            if (!isNextIntermediate) {

                value = list.CopyEndPosition ();

            }

            return value;

        }

        public bool MoveNext () {

            this.current = this.LookNext (out bool isNextIntermediate);

            if (!isNextIntermediate) {

                cycles++;

            }

            return isNextIntermediate;

        }

        public bool MovePrevious () {

            this.current = this.LookPrevious (out bool isNextIntermediate);

            if (!isNextIntermediate) {

                cycles--;

            }

            return isNextIntermediate;

        }

        ICyclicIterator<LinkedListPosition<T>> ICyclicIterator<LinkedListPosition<T>>.Copy (bool copyCycles) => this.Copy (copyCycles);
        public CyclicLinkedListIterator<T> Copy (bool copyCycles = false) => this.Copy (this.list.CopyFrontPosition (), copyCycles);
        public CyclicLinkedListIterator<T> Copy (LinkedListPosition<T> startingPosition, bool copyCycles = false) => new CyclicLinkedListIterator<T> (
            this.list,
            startingPosition,
            (copyCycles ? this.cycles : this.defaultCycles)
        );

        public int ResetCycles () {

            var cycles = this.cycles;
            this.cycles = this.defaultCycles;
            return cycles;

        }
    }
}
