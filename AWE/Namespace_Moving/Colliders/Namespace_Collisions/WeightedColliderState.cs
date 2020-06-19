using System;

namespace AWE.Moving.Collisions {

    public class WeightedColliderState<TColliderState> : ICloneable where TColliderState : IColliderState {

        public TColliderState state { get; }
        public float weight { get; }

        public WeightedColliderState (TColliderState state, float weight) {

            this.state = state;
            this.weight = weight;

        }

        object ICloneable.Clone () => this.Clone ();
        public WeightedColliderState<TColliderState> Clone () => new WeightedColliderState<TColliderState> (this.state, this.weight);

    }
}
