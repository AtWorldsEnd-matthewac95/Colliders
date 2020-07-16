namespace AWE.Moving.Collisions {

    public interface IInterpolatingTransformCollider<TColliderState, TTransformState>
        : IInterpolatingCollider<TColliderState>,
        ITransformCollider<TColliderState, TTransformState>
        where TColliderState : ITransformColliderState<TTransformState>
        where TTransformState : ITransformState
    {

        ColliderInterpolationSuggestor<TColliderState> CreateInterpolationSuggestor (TTransformState destination);

    }
}
