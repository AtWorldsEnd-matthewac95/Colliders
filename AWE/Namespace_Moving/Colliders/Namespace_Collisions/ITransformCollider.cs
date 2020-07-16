namespace AWE.Moving.Collisions {

    public interface ITransformCollider<TColliderState, TTransformState>
        : ICollider<TColliderState>,
        ITransformListener
        where TColliderState : ITransformColliderState<TTransformState>
        where TTransformState : ITransformState
    {

        IColliderAgent<TTransformState> agent { get; }
        ReadOnlyColliderAgentPathCursor<TTransformState> agentPathCursor { get; }
        ReadOnlyTransform<TTransformState> agentTransform { get; }

    }
}
