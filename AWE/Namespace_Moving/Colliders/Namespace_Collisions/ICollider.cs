namespace AWE.Moving.Collisions {

    public interface ICollider {

        ICollisionHandler handler { get; }
        IColliderState currentState { get; }
        bool isEnabled { get; }

    }

    public interface ICollider<out TColliderState> : ICollider where TColliderState : IColliderState {

        new TColliderState currentState { get; }

    }
}