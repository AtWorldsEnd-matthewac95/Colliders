using System;
using System.Collections;
using System.Collections.Generic;

namespace AWE {

    public class Pair<T> : IPair<T> {

        public T first { get; set; }
        public T second { get; set; }

        int IReadOnlyCollection<T>.Count => 2;

        public T this [int index] {

            get {

                T value;

                if (index == 0) {

                    value = first;

                } else if (index == 1) {

                    value = second;

                } else {

                    throw new IndexOutOfRangeException ();

                }

                return value;

            }
        }

        public T this [bool getSecond] => (getSecond ? this.second : this.first);

        public Pair (T first = default, T second = default) {

            this.first = first;
            this.second = second;

        }

        public T opposite (int index) {

            T value;

            if (index == 0) {

                value = this.second;

            } else if (index == 1) {

                value = this.first;

            } else {

                throw new IndexOutOfRangeException ();

            }

            return value;

        }

        public void Deconstruct (out T first, out T second) {

            first = this.first;
            second = this.second;

        }

        public override string ToString () => String.Format ("({0}, {1})", this.first.ToString (), this.second.ToString ());

        IEnumerator IEnumerable.GetEnumerator () => this.GetEnumerator ();
        public IEnumerator<T> GetEnumerator () {

            yield return this.first;
            yield return this.second;

        }

        // TODO - Give this class the full stuff it needs

    }
}