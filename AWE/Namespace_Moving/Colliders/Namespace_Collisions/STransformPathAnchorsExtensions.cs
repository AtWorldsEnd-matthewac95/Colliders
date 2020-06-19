namespace AWE.Moving.Collisions {

    public static class STransformPathAnchorsExtensions {

        public static ColliderAgentPathCursor<TTransformState> CreateColliderAgentCursor<TTransformState> (
            this TransformPathAnchors<TTransformState> pathAnchors,
            float speed,
            float position = 0f,
            bool evaluateCurrent = false
        ) where TTransformState : ITransformState => new ColliderAgentPathCursor<TTransformState> (
            pathAnchors,
            speed,
            position,
            evaluateCurrent
        );

    }
}
