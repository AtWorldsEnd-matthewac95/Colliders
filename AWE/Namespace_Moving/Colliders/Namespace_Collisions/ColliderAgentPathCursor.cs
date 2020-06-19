using System.Collections.Generic;
using System.Collections.ObjectModel;
using AWE.Math.FloatExtensions;

namespace AWE.Moving.Collisions {

    public class ColliderAgentPathCursor<TTransformState> : TransformPathCursor<TTransformState>, IColliderAgentPathCursor<TTransformState> where TTransformState : ITransformState {

        protected List<WeightedTransformState<TTransformState>> cachedInterpolations;

        bool IReadOnlyColliderAgentPathCursor<TTransformState>.isInterpolatingWithAnchors => this.isInterpolatingWithAnchors;
        public bool isInterpolatingWithAnchors { get; set; }

        public ColliderAgentPathCursor (TransformPathCursor<TTransformState> cursor, bool evaluateCurrent, bool isInterpolatingWithAnchors = true) : this (
            cursor.pathAnchors,
            cursor.speed,
            cursor.position,
            isInterpolatingWithAnchors,
            evaluateCurrent
        ) {
        }

        public ColliderAgentPathCursor (
            TransformPathAnchors<TTransformState> pathAnchors,
            float speed,
            float position = 0f,
            bool isInterpolatingWithAnchors = true,
            bool evaluateCurrent = false
        ) : base (pathAnchors, speed, position, evaluateCurrent) {

            this.isInterpolatingWithAnchors = isInterpolatingWithAnchors;
            this.cachedInterpolations = null;

        }

        IReadOnlyColliderAgentPathCursor<TTransformState> IColliderAgentPathCursor<TTransformState>.AsReadOnly () => this.AsReadOnly ();
        new public ReadOnlyColliderAgentPathCursor<TTransformState> AsReadOnly () => new ReadOnlyColliderAgentPathCursor<TTransformState> (this);

        protected override void OnPositionChange () {

            this.OnChange ();
            base.OnPositionChange ();

        }

        protected override void OnSpeedChange () {

            this.OnChange ();
            base.OnSpeedChange ();

        }

        protected override void OnAnchorsChange () {

            this.OnChange ();
            base.OnAnchorsChange ();

        }

        private void OnChange () => this.cachedInterpolations = null;

        public ReadOnlyCollection<WeightedTransformState<TTransformState>> CreateInterpolationCollection ()
            => this.CreateInterpolationCollection (this.position, (this.position + this.speed));
        public ReadOnlyCollection<WeightedTransformState<TTransformState>> CreateInterpolationCollection (float cursorSpeed)
            => this.CreateInterpolationCollection (this.position, (this.position + cursorSpeed));
        public ReadOnlyCollection<WeightedTransformState<TTransformState>> CreateInterpolationCollection (float start, float end) {

            var isCurrentPositionAndSpeed = ((start - this.position).IsNegligible () && (end - start - this.speed).IsNegligible ());
            var interpolations = (isCurrentPositionAndSpeed ? this.cachedInterpolations : null);

            if (interpolations == null) {

                var anchors = this.pathAnchors.FindAnchorsWithinRange (start, end, includeEndpoints: true);
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

                if (isCurrentPositionAndSpeed) {

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
         * Or the anchor state just after the given interpolation?
         */
        public WeightedTransformState<TTransformState> CreateInterpolatedState (float interpolation)
            => this.CreateInterpolatedState (interpolation, this.speed);
        public WeightedTransformState<TTransformState> CreateInterpolatedState (float interpolation, float cursorSpeed) {

            var state = new WeightedTransformState<TTransformState> (
                this.pathAnchors.path[this.position + (interpolation * cursorSpeed)],
                interpolation
            );

            if (this.isInterpolatingWithAnchors) {

                var interpolations = this.CreateInterpolationCollection (cursorSpeed);

                if (interpolations.Count > 0) {

                    state = interpolations[0];

                }

                for (int i = 1; i < interpolations.Count; i++) {

                    var current = interpolations[i];

                    if (current.weight > interpolation) {

                        break;

                    } else {

                        state = current;

                    }
                }
            }

            return state;

        }
    }
}
