namespace AWE.Moving.Collisions {

    public delegate void DColliderStateChange<in TColliderState> (
        TColliderState oldState,
        TColliderState newState
    ) where TColliderState : IColliderState;

}
