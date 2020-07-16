using AWE.Moving.Collisions;

namespace AWE.Moving.Moving2D {

    public class ConvexPolygonCollider2D : AInterpolatingTransformCollider<ConvexPolygonCollider2DState, Transform2DState> {

        new public ReadOnlyTransform2D agentTransform { get; }

        public ConvexPolygonCollider2D (
            ReadOnlyColliderProperties properties,
            ACollisionHandler handler,
            IColliderAgent<Transform2DState> agent,
            ReadOnlyTransform2D agentTransform,
            ConvexPolygonCollider2DState currentState,
            ConvexPolygonCollider2DState nextState = default,
            bool isEnabled = true
        ) : base (properties, handler, agent, agentTransform, currentState, nextState, isEnabled) {

            this.agentTransform = agentTransform;

        }

        protected override ConvexPolygonCollider2DState FindNextState () => this.currentState.Add (
            this.agentTransform.state - this.currentState.transformState
        );
        protected override float FindTranslationDistance (Transform2DState destination)
            => (destination.position - this.currentState.transformState.position).magnitude;

        protected override ConvexPolygonCollider2DState FindProjectedState (
            Transform2DState destination,
            float projectionScale = 1f
        ) => this.currentState.Add (
            (destination - this.currentState.transformState) * projectionScale
        );

    }
}