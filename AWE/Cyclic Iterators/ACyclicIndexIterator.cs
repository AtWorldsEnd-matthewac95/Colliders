namespace AWE {

    public abstract class ACyclicIndexIterator<T> : ICyclicIndexIterator<T> {

        public static ACyclicIndexIterator<T> operator++ (ACyclicIndexIterator<T> iterator) {

            iterator.MoveNext ();
            return iterator;

        }

        public static ACyclicIndexIterator<T> operator-- (ACyclicIndexIterator<T> iterator) {

            iterator.MovePrevious ();
            return iterator;

        }

        protected int _index;

        public abstract int count { get; }
        public abstract T current { get; }
        public abstract T next { get; }
        public abstract T previous { get; }

        public virtual int cycles { get; protected set; }

        public int currentIndex {

            get => this._index;

            set {

                var count = this.count;
                var index = (value % count);

                if (index < 0) {

                    index += count;

                }

                this._index = index;

            }
        }

        public int nextIndex => ((this.currentIndex + 1) % this.count);
        public int previousIndex => (((this.currentIndex <= 0) ? this.count : this.currentIndex) - 1);

        public virtual bool MoveNext () {

            var isNextIntermediate = true;
            var next = this.nextIndex;

            if (next == 0) {

                isNextIntermediate = false;
                this.cycles++;

            }

            this._index = next;

            return isNextIntermediate;

        }

        public virtual bool MovePrevious () {

            var isPreviousIntermediate = true;

            if (this._index == 0) {

                isPreviousIntermediate = false;
                this.cycles--;

            }

            this._index = this.previousIndex;

            return isPreviousIntermediate;

        }

        ICyclicIterator<T> ICyclicIterator<T>.Copy (bool copyCycles) => this.Copy (copyCycles, -1);
        ICyclicIndexIterator<T> ICyclicIndexIterator<T>.Copy (bool copyCycles, int startingIndex) => this.Copy (copyCycles, startingIndex);
        public abstract ACyclicIndexIterator<T> Copy (bool copyCycles = false, int startingIndex = -1);

        public int ResetCycles () {

            var cycles = this.cycles;
            this.cycles = 0;
            return cycles;

        }
    }
}