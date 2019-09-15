using System;
using System.Collections;
using System.Collections.Generic;

namespace AWE {

    public struct IntPair : IPair<int> {

        public static readonly IntPair zero;

        static IntPair () {

            zero = new IntPair ();

        }

        int IPair<int>.first => this.a;
        int IPair<int>.second => this.b;

        public int a { get; }
        public int b { get; }

        int IReadOnlyCollection<int>.Count => 2;

        public int this [int index] {

            get {

                int value;

                if (index == 0) {

                    value = a;

                } else if (index == 1) {

                    value = b;

                } else {

                    throw new IndexOutOfRangeException ();

                }

                return value;

            }
        }

        public int this [bool getB] => (getB ? this.b : this.a);

        public IntPair (int a, int b) : this () {

            this.a = a;
            this.b = b;

        }

        public int opposite (int index) {

            int value;

            if (index == 0) {

                value = this.b;

            } else if (index == 1) {

                value = this.a;

            } else {

                throw new IndexOutOfRangeException ();

            }

            return value;

        }

        public void Deconstruct (out int a, out int b) {

            a = this.a;
            b = this.b;

        }

        public override string ToString () => String.Format ("({0}, {1})", this.a.ToString (), this.b.ToString ());

        IEnumerator IEnumerable.GetEnumerator () => this.GetEnumerator ();
        public IEnumerator<int> GetEnumerator () {

            yield return this.a;
            yield return this.b;

        }

        // TODO - Give this class the full stuff it needs

    }
}