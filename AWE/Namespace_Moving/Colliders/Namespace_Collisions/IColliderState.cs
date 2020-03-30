namespace AWE.Moving.Collisions {

    public interface IColliderState {

        ICollider collider { get; }

        bool IsCollidingWith (IColliderState other);
        IColliderState Add (ITransformation transformation);
        IColliderState Subtract (ITransformation transformation);

    }

    public interface IColliderState<out TTransformState> : IColliderState where TTransformState : ITransformState {

        TTransformState transformState { get; }

    }
}