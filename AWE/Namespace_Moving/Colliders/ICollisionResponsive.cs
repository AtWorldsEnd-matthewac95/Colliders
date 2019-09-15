using System.Collections.ObjectModel;

namespace AWE.Moving {

    public interface ICollisionResponsive {

        ReadOnlyCollection<ICollider> colliders { get; }

        bool RespondTo (Collision collision);

    }
}