namespace AWE.Moving {

    public interface ICollider : IGradient<AColliderFrame> {

        ICollisionResponsive responder { get; }
        bool isEnabled { get; }
        CollisionHandler handler { get; }

    }
}