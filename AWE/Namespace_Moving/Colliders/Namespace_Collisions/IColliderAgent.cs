namespace AWE.Moving.Collisions {

    public interface ICollision { }

    public interface IColliderAgent<TTransformState> where TTransformState : ITransformState {

        ColliderAgentPathCursor<TTransformState> fixedPathCursor { get; }

        bool IsUsingTransform (IReadOnlyTransform transform);
        void RespondTo (ICollision collision);

    }
}