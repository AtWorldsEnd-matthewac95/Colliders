namespace AWE.Moving.Collisions {

    public interface IInterpolatingCollider<TColliderState> : ICollider<TColliderState> where TColliderState : IColliderState {

        ColliderInterpolationSuggestor<TColliderState> CreateInterpolationSuggestor ();
        WeightedColliderState<TColliderState> FindInterpolatedState (float interpolation);

    }
}
