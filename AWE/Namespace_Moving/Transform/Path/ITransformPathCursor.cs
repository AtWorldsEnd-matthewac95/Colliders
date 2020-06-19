namespace AWE.Moving {

    public interface ITransformPathCursor<TTransformState> : IReadOnlyTransformPathCursor<TTransformState> where TTransformState : ITransformState {

        new float speed { get; set; }
        new float position { get; set; }

        IReadOnlyTransformPathCursor<TTransformState> AsReadOnly ();
        void MoveForward ();
        void MoveBackward ();
        TransformPathAnchors<TTransformState> UpdateAnchors (TransformPathAnchors<TTransformState> newAnchors, bool resetPosition = false);

    }
}
