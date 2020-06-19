namespace AWE.Moving {

    public interface IReadOnlyTransformPathCursor<TTransformState> where TTransformState : ITransformState {

        float speed { get; }
        float position { get; }
        TransformPathAnchors<TTransformState> pathAnchors { get; }
        TTransformState current { get; }

    }
}
