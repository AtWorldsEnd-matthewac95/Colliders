using System;

namespace AWE.Moving {

    public class WeightedTransformState<TTransformState> : ICloneable where TTransformState : ITransformState {

        public TTransformState state { get; }
        public float weight { get; }

        public WeightedTransformState (TTransformState state, float weight) {

            this.state = state;
            this.weight = weight;

        }

        object ICloneable.Clone () => this.Clone ();
        public WeightedTransformState<TTransformState> Clone () => new WeightedTransformState<TTransformState> (this.state, this.weight);

    }
}