using System;

namespace AWE.Moving.Collisions {

    public interface ICollisionHandler {

        event Action OnBeginCollisionAnalysis;
        event Action OnEndCollisionAnalysis;

        void HandleCollisions ();
        BooleanNote Add (ICollider collider);
        BooleanNote Remove (ICollider collider);
        bool IsTracking (ICollider collider);

    }
}