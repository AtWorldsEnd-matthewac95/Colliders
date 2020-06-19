using AWE.Moving.Collisions;

namespace AWE.Moving.Moving2D {

    public class ConvexPolygonCollider2D : ACollider<Transform2DState, ReadOnlyTransform2D, ConvexPolygonCollider2DState> {

        public ConvexPolygonCollider2D (
            ACollisionHandler handler,
            IColliderAgent<Transform2DState> agent,
            ReadOnlyTransform2D agentTransform,
            ConvexPolygonCollider2DState currentState,
            ConvexPolygonCollider2DState nextState = default,
            bool isEnabled = true
        ) : base (handler, agent, agentTransform, currentState, nextState, isEnabled) {
        }

        protected override ConvexPolygonCollider2DState FindNextState () => this.currentState.Add (
            this.agentTransform.state - this.currentState.transformState
        );

        protected override ConvexPolygonCollider2DState FindProjectedState (
            Transform2DState destination,
            float projectionScale = 1f
        ) => this.currentState.Add (
            (destination - this.currentState.transformState) * projectionScale
        );

        public override ColliderInterpolationIterator<ConvexPolygonCollider2DState> CreateInterpolationIterator (Transform2DState destination) {

            var transformation = this.currentState.transformState - destination;

            //if (this.currentState.)

            return null;

        }
    }
}