namespace AWE.Moving.Collisions {

    public static class STransformPathExtensions {

        public static ColliderAgentPathCursor<TTransformState> CreateColliderAgentCursor<TTransformState> (
            this TransformPath<TTransformState> path,
            float speed,
            float position = 0f,
            bool evaluateCurrent = false
        ) where TTransformState : ITransformState => new ColliderAgentPathCursor<TTransformState> (
            path,
            speed,
            position,
            evaluateCurrent
        );

    }
}