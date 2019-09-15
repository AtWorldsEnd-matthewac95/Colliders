using System.Collections.ObjectModel;

namespace AWE.Moving {

    public abstract class AColliderFrame {

        public ICollider collider { get; }
        public ITransformation transformation { get; }
        public float elapsed { get; }
        public ReadOnlyCollection<float> suggestedInterpolations { get; }

        public abstract AColliderFrame FindInterpolated (float interpolation);
        public abstract bool IsCollidingWith (AColliderFrame other);

    }
}