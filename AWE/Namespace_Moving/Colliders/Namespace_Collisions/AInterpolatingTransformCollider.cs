using AWE.Math.FloatExtensions;
using System.Collections.Generic;

namespace AWE.Moving.Collisions {

    public abstract class AInterpolatingTransformCollider<TColliderState, TTransformState>
        : ATransformCollider<TColliderState, TTransformState>,
        IInterpolatingTransformCollider<TColliderState, TTransformState>
        where TColliderState : ITransformColliderState<TTransformState>
        where TTransformState : ITransformState
    {

        public AInterpolatingTransformCollider (
            ReadOnlyColliderProperties properties,
            ICollisionHandler handler,
            IColliderAgent<TTransformState> agent,
            ReadOnlyTransform<TTransformState> agentTransform,
            TColliderState currentState,
            TColliderState nextState = default,
            bool isEnabled = true
        ) : base (properties, handler, agent, agentTransform, currentState, nextState, isEnabled) {
        }

        protected abstract TColliderState FindProjectedState (TTransformState destination, float projectionScale = 1f);
        protected abstract float FindTranslationDistance (TTransformState destination);

        public virtual ColliderInterpolationSuggestor<TColliderState> CreateInterpolationSuggestor ()
            => this.CreateInterpolationSuggestor (this.FindNextState ().transformState);
        public virtual ColliderInterpolationSuggestor<TColliderState> CreateInterpolationSuggestor (TTransformState destination)
            => new ColliderInterpolationSuggestor<TColliderState> (this, () => {

                var interpolations = new Queue<float> ();

                if (this.agentPathCursor == null) {

                    var translationDistance = this.FindTranslationDistance (destination);
                    var collisionDistance = this.currentState.FindTranslationCollisionDistance (destination);

                    if ((System.Math.Max (0f, collisionDistance) - System.Math.Min (collisionDistance, translationDistance)).IsNegligible ()) {

                        var boundaryDistance = (translationDistance - collisionDistance);

                        for (var interpolationDistance = boundaryDistance; interpolationDistance < translationDistance; interpolationDistance += boundaryDistance) {

                            interpolations.Enqueue (interpolationDistance / translationDistance);

                        }
                    }

                } else {

                    var pathInterpolations = this.agentPathCursor.CreateInterpolationCollection (includeEndpoints: false);

                    for (int i = 0; i < pathInterpolations.Count; i++) {

                        interpolations.Enqueue (pathInterpolations[i].weight);

                    }
                }

                return interpolations;

            }
        );

        public virtual WeightedColliderState<TColliderState> FindInterpolatedState (float interpolation) {

            var interpolated = new WeightedColliderState<TColliderState> (this.currentState, 1f);

            if (this.nextState != null) {

                var agentCursor = this.agentPathCursor;

                if (agentCursor == null) {

                    interpolated = new WeightedColliderState<TColliderState> (
                        this.FindProjectedState (this.nextState.transformState, interpolation),
                        interpolation
                    );

                } else {

                    /*
                     * In this case we don't have to pass the interpolation as a seperate parameter
                     * to FindProjectedState, since the interpolation is resolved in the agent cursor's
                     * CreateInterpolatedState function.
                     *
                     * For default implementation, see ColliderAgentPathCursor.CreateInterpolatedState
                     */
                    var cursorState = agentCursor.CreateInterpolatedState (interpolation);
                    interpolated = new WeightedColliderState<TColliderState> (
                        this.FindProjectedState (cursorState.state),
                        cursorState.weight
                    );

                }
            }

            return interpolated;

        }
    }
}
