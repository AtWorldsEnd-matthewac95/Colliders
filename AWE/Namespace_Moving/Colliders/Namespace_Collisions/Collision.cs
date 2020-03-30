using System;
using System.Collections;
using System.Collections.Generic;

namespace AWE.Moving.Collisions {

    public class Collision<TColliderState> : IPair<TempCollider<TColliderState>> where TColliderState : IColliderState {

        public TempCollider<TColliderState> first { get; }
        public TempCollider<TColliderState> second { get; }

        int IReadOnlyCollection<TempCollider<TColliderState>>.Count => 2;

        TempCollider<TColliderState> IReadOnlyList<TempCollider<TColliderState>>.this [int index] {

            get {

                TempCollider<TColliderState> value;

                if (index == 0) {

                    value = this.first;

                } else if (index == 1) {

                    value = this.second;

                } else {

                    throw new IndexOutOfRangeException ();

                }

                return value;

            }
        }
        public TempCollider<TColliderState> this [bool getSecond] => (getSecond ? second : first);

        TempCollider<TColliderState> IPair<TempCollider<TColliderState>>.opposite (int index) {

            TempCollider<TColliderState> value;

            if (index == 0) {

                value = this.second;

            } else if (index == 1) {

                value = this.first;

            } else {

                throw new IndexOutOfRangeException ();

            }

            return value;

        }

        public Collision (TempCollider<TColliderState> first, TempCollider<TColliderState> second) {

            this.first = first;
            this.second = second;

        }

        IEnumerator IEnumerable.GetEnumerator () => this.GetEnumerator ();
        public IEnumerator<TempCollider<TColliderState>> GetEnumerator () {

            yield return this.first;
            yield return this.second;

        }

        public void Deconstruct (out TempCollider<TColliderState> first, out TempCollider<TColliderState> second) {

            first = this.first;
            second = this.second;

        }
    }
}