using System;

namespace AWE.Moving.Collisions {

    public interface ICollider {

        ICollisionHandler handler { get; }
        IColliderState currentState { get; }
        bool isEnabled { get; }

    }

    public interface ICollider<out TColliderState> : ICollider where TColliderState : IColliderState {

        event DColliderStateChange<TColliderState> OnNextStateChange;

        new TColliderState currentState { get; }

    }
}