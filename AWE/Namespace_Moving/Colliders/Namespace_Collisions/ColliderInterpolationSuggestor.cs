using AWE.CollectionExtensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AWE.Moving.Collisions {

    public class ColliderInterpolationSuggestor<TColliderState> where TColliderState : IColliderState {

        // TODO

        // Keeps track of what collision frames a collider has gone through.
        // Also keeps track of what the current and desired final state of the collider were at the time of this object's creation.

        private readonly Queue<float> remainingInterpolations;
        private readonly Func<float, WeightedColliderState<TColliderState>> DelegateFindInterpolatedWeightedState;

        public bool isActive { get; private set; }

        public bool isEmpty => (this.remainingInterpolations.Count > 0);
        public float current => (isEmpty ? 1f : this.remainingInterpolations.Peek ());

        #region Constructors

        public ColliderInterpolationSuggestor (IInterpolatingCollider<TColliderState> collider)
            : this (collider, collider.FindInterpolatedState) {}
        public ColliderInterpolationSuggestor (
            ICollider<TColliderState> deactivationTriggerEmitter,
            Func<float, WeightedColliderState<TColliderState>> FindInterpolatedWeightedState
        ) : this (deactivationTriggerEmitter, FindInterpolatedWeightedState, () => new Queue<float> ()) {}

        public ColliderInterpolationSuggestor (
            IInterpolatingCollider<TColliderState> collider,
            Func<Queue<float>> CreateInterpolations
        ) : this (collider, collider.FindInterpolatedState, CreateInterpolations) {}
        public ColliderInterpolationSuggestor (
            ICollider<TColliderState> deactivationTriggerEmitter,
            Func<float, WeightedColliderState<TColliderState>> FindInterpolatedWeightedState,
            Func<Queue<float>> CreateInterpolations
        ) {

            this.DelegateFindInterpolatedWeightedState = FindInterpolatedWeightedState;
            this.remainingInterpolations = CreateInterpolations ();
            this.isActive = true;

            deactivationTriggerEmitter.OnNextStateChange += this.Deactivate;

        }

        public ColliderInterpolationSuggestor (
            IInterpolatingCollider<TColliderState> collider,
            params float[] interpolations
        ) : this (collider, collider.FindInterpolatedState, interpolations) {}
        public ColliderInterpolationSuggestor (
            ICollider<TColliderState> deactivationTriggerEmitter,
            Func<float, WeightedColliderState<TColliderState>> FindInterpolatedWeightedState,
            params float[] interpolations
        ) : this (deactivationTriggerEmitter, FindInterpolatedWeightedState, Array.AsReadOnly (interpolations)) {}

        public ColliderInterpolationSuggestor (
            IInterpolatingCollider<TColliderState> collider,
            ReadOnlyCollection<float> interpolations
        ) : this (collider, collider.FindInterpolatedState, interpolations) {}
        public ColliderInterpolationSuggestor (
            ICollider<TColliderState> deactivationTriggerEmitter,
            Func<float, WeightedColliderState<TColliderState>> FindInterpolatedWeightedState,
            ReadOnlyCollection<float> interpolations
        ) : this (deactivationTriggerEmitter, FindInterpolatedWeightedState, interpolations.GetCyclicIterator ()) {}

        public ColliderInterpolationSuggestor (
            IInterpolatingCollider<TColliderState> collider,
            ACyclicIndexIterator<float> interpolations
        ) : this (collider, collider.FindInterpolatedState, interpolations) {}
        public ColliderInterpolationSuggestor (
            ICollider<TColliderState> deactivationTriggerEmitter,
            Func<float, WeightedColliderState<TColliderState>> FindInterpolatedWeightedState,
            ACyclicIndexIterator<float> interpolations
        ) : this (deactivationTriggerEmitter, FindInterpolatedWeightedState, () => {

                var queue = new Queue<float> ();

                for (var i = interpolations.Copy (copyCycles: false, startingIndex: 0); i.cycles < 1; i++) {

                    queue.Enqueue (i.current);

                }

                return queue;

            }
        ) {}

        #endregion

        private void Deactivate (TColliderState o = default, TColliderState n = default) {

            this.isActive = false;

        }

        public TColliderState FindInterpolatedState (float interpolation) {

            this.DequeueUntilGreater (interpolation);
            var weightedState = this.DelegateFindInterpolatedWeightedState (interpolation);
            this.DequeueUntilGreater (weightedState.weight);

            return weightedState.state;

        }

        private float DequeueUntilGreater (float value) {

            var last = Single.NaN;

            while (!this.isEmpty && (this.current <= value)) {

                last = this.remainingInterpolations.Dequeue ();

            }

            return last;

        }
    }
}
