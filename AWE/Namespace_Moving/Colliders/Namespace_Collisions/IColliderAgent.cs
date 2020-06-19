using System;

namespace AWE.Moving.Collisions {

    public interface IColliderAgent<TTransformState> where TTransformState : ITransformState {

        event Action<ReadOnlyColliderAgentPathCursor<TTransformState>> OnFixedPathChange;

        ReadOnlyColliderAgentPathCursor<TTransformState> fixedPathCursor { get; }
        ACollisionResponder collisionResponder { get; }

        bool IsUsingTransform (IReadOnlyTransform transform);

    }
}
