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

        protected override ConvexPolygonCollider2DState CreateProjectedState (
            ITransformation transformation,
            float transformationScale = 1f
        ) {

            ConvexPolygonCollider2DState projected = null;

            if (transformation is Transformation2D transformation2d) {

                projected = this.CreateProjectedState (transformation2d, transformationScale);

            }

            return projected;

        }

        protected virtual ConvexPolygonCollider2DState CreateProjectedState (
            Transformation2D transformation,
            float transformationScale = 1f
        ) => this.currentState.Add (
            transformation * transformationScale
        );

    }
}