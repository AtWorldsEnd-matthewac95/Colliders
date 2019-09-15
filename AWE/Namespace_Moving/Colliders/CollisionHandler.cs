using System.Collections.Generic;
using System.Linq;

namespace AWE.Moving {

    public class Collision { }

    public class CollisionHandler : IFrameEmitter {

        protected readonly HashSet<ICollider> movingColliders;
        protected AColliderFrame[] stationaryColliders;

        public event DFrameEvent FrameBegin;
        public event DFrameEvent FrameIntermediate;
        public event DFrameEvent FrameEnd;

        public CollisionHandler (params AColliderFrame[] stationaryColliders) {

            this.movingColliders = new HashSet<ICollider> ();
            this.stationaryColliders = stationaryColliders;

        }

        public void AddStationaryCollider (AColliderFrame collider) {

            var newColliders = new AColliderFrame [this.stationaryColliders.Length + 1];

            int i;
            for (i = 0; i < this.stationaryColliders.Length; i++) {

                newColliders[i] = this.stationaryColliders[i];

            }
            newColliders[i] = collider;

            this.stationaryColliders = newColliders;

        }

        public BooleanNote AddCollider (ICollider collider) {

            var note = new BooleanNote (false, "Collider is disabled, only enabled colliders can be added.");

            if (collider.isEnabled) {

                var success = this.movingColliders.Add (collider);
                note = new BooleanNote (success, (success ? "" : "Collider has already been added."));

            }

            return note;

        }

        public BooleanNote RemoveCollider (ICollider collider, CollisionHandler newHandler = null) {

            var note = new BooleanNote (false, "Collider is enabled, only disabled colliders can be removed without providing a replacement handler.");

            if (!collider.isEnabled) {

                var success = this.movingColliders.Remove (collider);
                note = new BooleanNote (success, (success ? "" : "Collider was never added."));

            } else if (newHandler != null) {

                var success = newHandler.IsContainingCollider (collider);

                if (success) {

                    this.movingColliders.Remove (collider);

                }

                note = new BooleanNote (success, (
                    success
                    ? ""
                    : "The provided replacement handler was not tracking the collider, collider must already be added to another handler if collider is still enabled."
                ));

            }

            return note;

        }

        public bool IsContainingCollider (ICollider collider) => this.movingColliders.Contains (collider);

        protected List<Collision> FindCollisions () {

            this.FrameEnd ();

            var colliderFrames = this.movingColliders.Select (collider => collider.current);

            // Do stuff with the frames.

            return null;

        }
    }
}