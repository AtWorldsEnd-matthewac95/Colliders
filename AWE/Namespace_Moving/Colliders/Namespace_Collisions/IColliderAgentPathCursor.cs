namespace AWE.Moving.Collisions {

    public interface IColliderAgentPathCursor<TTransformState> : IReadOnlyColliderAgentPathCursor<TTransformState>, ITransformPathCursor<TTransformState> where TTransformState : ITransformState {

        new bool isInterpolatingWithAnchors { get; set; }

        new IReadOnlyColliderAgentPathCursor<TTransformState> AsReadOnly ();

    }
}
