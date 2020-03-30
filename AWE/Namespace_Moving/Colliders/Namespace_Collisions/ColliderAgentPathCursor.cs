using System.Collections.Generic;
using System.Collections.ObjectModel;
using AWE.Math.FloatExtensions;

namespace AWE.Moving.Collisions {

    public class ColliderAgentPathCursor<TTransformState> : TransformPathCursor<TTransformState> where TTransformState : ITransformState {

        protected List<WeightedTransformState<TTransformState>> cachedInterpolations;

        public ColliderAgentPathCursor (TransformPathCursor<TTransformState> cursor, bool evaluateCurrent) : this (
            cursor.path,
            cursor.speed,
            cursor.position,
            evaluateCurrent
        ) {
        }

        public ColliderAgentPathCursor (
            TransformPath<TTransformState> path,
            float speed,
            float position = 0f,
            bool evaluateCurrent = false
        ) : base (path, speed, position, evaluateCurrent) {

            this.cachedInterpolations = null;

        }

        protected override void OnPositionChange () {

            this.OnChange ();
            base.OnPositionChange ();

        }

        protected override void OnSpeedChange () {

            this.OnChange ();
            base.OnSpeedChange ();

        }

        private void OnChange () => this.cachedInterpolations = null;

        public ReadOnlyCollection<WeightedTransformState<TTransformState>> CreateInterpolationCollection () => this.CreateInterpolationCollection (this.speed);
        public ReadOnlyCollection<WeightedTransformState<TTransformState>> CreateInterpolationCollection (float cursorSpeed) {

            var isCurrentSpeed = (this.speed - cursorSpeed).IsNegligible ();
            var interpolations = (isCurrentSpeed ? this.cachedInterpolations : null);

            if (interpolations == null) {

                var anchors = this.path.FindAnchorsInRange (this.position, (this.position + cursorSpeed), includeEndpoints: true);
                interpolations = new List<WeightedTransformState<TTransformState>> (anchors.Count);

                if (anchors.Count > 0) {

                    var firstPosition = anchors[0].weight;
                    var lastIndex = (anchors.Count - 1);
                    var diff = (anchors[lastIndex].weight - firstPosition);

                    if (diff > 0f) {

                        for (int i = 0; i <= lastIndex; i++) {

                            var anchor = anchors[i];
                            var state = anchor.state;
                            float interpolation;

                            if (i <= 0) {

                                interpolation = 0f;

                            } else if (i >= lastIndex) {

                                interpolation = 1f;

                            } else {

                                interpolation = ((anchor.weight - firstPosition) / diff);

                            }

                            interpolations.Add (new WeightedTransformState<TTransformState> (state, interpolation));

                        }
                    }
                }

                if (isCurrentSpeed) {

                    this.cachedInterpolations = interpolations;

                }
            }

            return interpolations.AsReadOnly ();

        }

        /*
         * TODO
         *
         * Should the cursor function return the closest anchor state?
         * Or the interpolation between the two closest anchor states?
         */
        public TTransformState CreateInterpolatedState (float interpolation) => this.CreateInterpolatedState (interpolation, this.speed);
        public TTransformState CreateInterpolatedState (float interpolation, float cursorSpeed) {

            var interpolations = this.CreateInterpolationCollection (cursorSpeed);
            var state = ((interpolations.Count > 0) ? interpolations[0].state : default);

            for (int i = 1; i < interpolations.Count; i++) {

                var current = interpolations[i];

                if (current.weight > interpolation) {

                    break;

                } else {

                    state = current.state;

                }
            }

            return state;

        }
    }
}
