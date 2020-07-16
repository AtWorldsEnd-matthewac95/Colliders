namespace AWE.Moving.Collisions {

    public interface IColliderState {

        ICollider collider { get; }

        bool IsCollidingWith (IColliderState other);
        IColliderState Add (ITransformation transformation);
        IColliderState Subtract (ITransformation transformation);

    }
}