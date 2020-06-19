namespace AWE.Moving.Collisions {

    public abstract class ACollisionResponder {

        public static DefaultCollisionResponder defaultResponder => new DefaultCollisionResponder ();

        // TODO

        // Module of (usually) ColliderAgents that responds to a collision.
        // Used to make collision response behavior more extendable across different implementations of IColliderAgent,
        //     as well as reduce interface calls.

        // Collision response method is (for now) mainly expected to tell whether a collision is a stopping collision or not.
        public abstract void RespondTo (Collision collision);

    }
}
