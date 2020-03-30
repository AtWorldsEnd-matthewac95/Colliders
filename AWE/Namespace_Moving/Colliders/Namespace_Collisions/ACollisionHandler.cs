using System;

namespace AWE.Moving.Collisions {

    public abstract class ACollisionHandler : ICollisionHandler {

        public event Action OnBeginCollisionAnalysis;
        public event Action OnEndCollisionAnalysis;

        public abstract BooleanNote Add (ICollider collider);
        public abstract BooleanNote Remove (ICollider collider);
        public abstract bool IsTracking (ICollider collider);

        public virtual void HandleCollisions () {



        }
    }
}
