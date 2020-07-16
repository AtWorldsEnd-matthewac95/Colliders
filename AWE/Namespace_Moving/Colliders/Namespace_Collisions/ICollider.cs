using System;

namespace AWE.Moving.Collisions {

    public interface ICollider {

        ReadOnlyColliderProperties properties { get; }
        ICollisionHandler handler { get; }
        IColliderState currentState { get; }
        IColliderState nextState { get; }
        bool isEnabled { get; }

    }

    public interface ICollider<out TColliderState> : ICollider where TColliderState : IColliderState {

        event DColliderStateChange<TColliderState> OnNextStateChange;

        new TColliderState currentState { get; }
        new TColliderState nextState { get; }

    }
}
