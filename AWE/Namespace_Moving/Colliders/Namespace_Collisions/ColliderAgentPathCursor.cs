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

        public ReadOnlyCollection<WeightedTransformState<TTransformState>> CreateInterpolationCollection (bool includeEndpoints = true)
            => this.CreateInterpolationCollection (this.position, (this.position + this.speed), includeEndpoints);
        public ReadOnlyCollection<WeightedTransformState<TTransformState>> CreateInterpolationCollection (float cursorSpeed, bool includeEndpoints = true)
            => this.CreateInterpolationCollection (this.position, (this.position + cursorSpeed), includeEndpoints);
        public ReadOnlyCollection<WeightedTransformState<TTransformState>> CreateInterpolationCollection (float start, float end, bool includeEndpoints = true) {

            var isCurrentPositionAndSpeed = ((start - this.position).IsNegligible () && (end - start - this.speed).IsNegligible ());
            var interpolations = (isCurrentPositionAndSpeed ? this.ReadCache (includeEndpoints) : null);

            if (interpolations == null) {

                var anchors = this.pathAnchors.FindAnchorsWithinRange (start, end, includeEndpoints);
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

                    this.WriteCache (interpolations, endpointsIncluded: includeEndpoints);

                }
            }

            return interpolations.AsReadOnly ();

        }

        private List<WeightedTransformState<TTransformState>> ReadCache (bool includeEndpoints = true) {

            List<WeightedTransformState<TTransformState>> cache;

            if (includeEndpoints) {

                cache = this.cachedInterpolations;

            } else {

                cache = new List<WeightedTransformState<TTransformState>> ();

                for (int i = 1; i < (this.cachedInterpolations.Count - 1); i++) {

                    cache.Add (this.cachedInterpolations[i]);

                }
            }

            return cache;

        }

        private ReadOnlyCollection<WeightedTransformState<TTransformState>> WriteCache (
            List<WeightedTransformState<TTransformState>> interpolations,
            bool endpointsIncluded
        ) {

            if (endpointsIncluded) {

                this.cachedInterpolations = interpolations;

            } else {

                var tmp = new List<WeightedTransformState<TTransformState>> ();
                tmp.Add (new WeightedTransformState<TTransformState> (
                    this.pathAnchors.path[this.position],
                    this.position
                ));

                for (int i = 0; i < interpolations.Count; i++) {

                    tmp.Add (interpolations[i]);

                }

                tmp.Add (new WeightedTransformState<TTransformState> (
                    this.pathAnchors.path[this.next],
                    this.next
                ));
                this.cachedInterpolations = tmp;

            }

            return this.cachedInterpolations.AsReadOnly ();

        }

        /*
         * TODO
         *
         * Should the cursor function return the closest anchor state?
         * Or the interpolation between the two closest anchor states?
         * Or the anchor state just after the given interpolation?     <-- Currently implementing this option.
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

                    state = interpolations[i];

                    if (state.weight > interpolation) {

                        break;

                    }
                }
            }

            return state;

        }
    }
}
