using AWE.CollectionExtensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AWE.Moving.Collisions {

    public class ColliderInterpolationIterator<TColliderState> where TColliderState : IColliderState {

        // TODO

        // Keeps track of what collision frames a collider has gone through.
        // Also keeps track of what the current and desired final state of the collider were at the time of this object's creation.

        private readonly Queue<float> remainingInterpolations;
        private readonly Func<float, WeightedColliderState<TColliderState>> DelegateFindInterpolatedWeightedState;

        public bool isActive { get; private set; }

        public bool isEmpty => (this.remainingInterpolations.Count > 0);
        public float currentInterpolation => (isEmpty ? 1f : this.remainingInterpolations.Peek ());

        public ColliderInterpolationIterator (
            ICollider<TColliderState> collider,
            Func<float, WeightedColliderState<TColliderState>> FindInterpolatedWeightedState
        ) : this (collider, FindInterpolatedWeightedState, () => new Queue<float> ()) {}

        public ColliderInterpolationIterator (
            ICollider<TColliderState> collider,
            Func<float, WeightedColliderState<TColliderState>> FindInterpolatedWeightedState,
            Func<Queue<float>> CreateInterpolations
        ) {

            this.DelegateFindInterpolatedWeightedState = FindInterpolatedWeightedState;
            this.remainingInterpolations = CreateInterpolations ();
            this.isActive = true;

            collider.OnNextStateChange += this.Deactivate;

        }

        public ColliderInterpolationIterator (
            ICollider<TColliderState> collider,
            Func<float, WeightedColliderState<TColliderState>> FindInterpolatedWeightedState,
            params float[] interpolations
        ) : this (collider, FindInterpolatedWeightedState, Array.AsReadOnly (interpolations)) {}

        public ColliderInterpolationIterator (
            ICollider<TColliderState> collider,
            Func<float, WeightedColliderState<TColliderState>> FindInterpolatedWeightedState,
            ReadOnlyCollection<float> interpolations
        ) : this (collider, FindInterpolatedWeightedState, interpolations.GetCyclicIterator ()) {}

        public ColliderInterpolationIterator (
            ICollider<TColliderState> collider,
            Func<float, WeightedColliderState<TColliderState>> FindInterpolatedWeightedState,
            ACyclicIndexIterator<float> interpolations
        ) : this (collider, FindInterpolatedWeightedState, () => {

                var queue = new Queue<float> ();

                for (var i = interpolations.Copy (copyCycles: false, startingIndex: 0); i.cycles < 1; i++) {

                    queue.Enqueue (i.current);

                }

                return queue;

            }
        ) {}

        private void Deactivate (TColliderState o = default, TColliderState n = default) {

            this.isActive = false;

        }

        public TColliderState FindInterpolatedState (float interpolation) {

            var weightedState = this.DelegateFindInterpolatedWeightedState (interpolation);

            // TODO the rest
            return this.DelegateFindInterpolatedWeightedState (interpolation).state;

        }
    }
}
