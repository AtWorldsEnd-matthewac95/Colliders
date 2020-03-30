namespace AWE.Moving.Collisions {

    /*
     * TCA Collider
     *
     * TCA stands for Transient Collision Analysis.
     * Essentially, these are "dummy" colliders that only provide a current and next state. As the name implies,
     * this is useful for collision analysis since you can take as much time as is needed to analyze a specific
     * state transition without locking down the collider whose state you're looking at.
     */
    public class TempCollider<TColliderState> : ICollider<TColliderState> where TColliderState : IColliderState {

        IColliderState ICollider.currentState => this.currentState;

        public ICollisionHandler handler { get; }
        public TColliderState currentState { get; }
        public TColliderState nextState { get; }
        public TColliderState collidedState { get; }
        public float collisionInterpolation { get; }

        public bool isEnabled => false;

        public TempCollider (
            ICollisionHandler handler,
            TColliderState currentState,
            TColliderState nextState
        ) : this (
            handler,
            currentState,
            nextState,
            nextState,
            1f
        ) {
        }

        public TempCollider (
            ICollisionHandler handler,
            TColliderState currentState,
            TColliderState nextState,
            TColliderState collidedState,
            float collisionInterpolation
        ) {

            this.handler = handler;
            this.currentState = currentState;
            this.nextState = nextState;
            this.collidedState = collidedState;
            this.collisionInterpolation = collisionInterpolation;

        }
    }
}
