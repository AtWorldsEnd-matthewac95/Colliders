using AWE.Math;
using AWE.Moving.Collisions;

namespace AWE.Moving.Moving2D {

    public class ConvexPolygonCollider2DState : IColliderState<Transform2DState> {

        private readonly MovingConvexPolygon2D polygon;

        ICollider IColliderState.collider => this.collider;
        public ConvexPolygonCollider2D collider { get; }
        public Transform2DState transformState { get; }

        public ConvexPolygonCollider2DState (
            ConvexPolygonCollider2D collider,
            ConvexPolygon2D polygon,
            Transform2DState transformState
        ) {

            this.collider = collider;
            this.polygon = new MovingConvexPolygon2D (polygon, transformState.position);
            this.transformState = transformState;

        }

        public bool IsCollidingWith (IColliderState other) {

            var isColliding = false;

            if (other is ConvexPolygonCollider2DState otherConvex) {

                isColliding = this.IsCollidingWith (otherConvex);

            }

            return isColliding;

        }

        public bool IsCollidingWith (ConvexPolygonCollider2DState other) => this.polygon.current.IsIntersecting (
            other.polygon.current
        );

        IColliderState IColliderState.Add (ITransformation transformation) {

            ConvexPolygonCollider2DState state = null;

            if (transformation is Transformation2D transformation2D) {

                state = this.Add (transformation2D);

            }

            return state;

        }
        public ConvexPolygonCollider2DState Add (Transformation2D transformation) => new ConvexPolygonCollider2DState (
            this.collider,
            this.polygon.Add (transformation).current,
            (this.transformState + transformation)
        );

        IColliderState IColliderState.Subtract (ITransformation transformation) {

            ConvexPolygonCollider2DState state = null;

            if (transformation is Transformation2D transformation2D) {

                state = this.Subtract (transformation2D);

            }

            return state;

        }
        public ConvexPolygonCollider2DState Subtract (Transformation2D transformation) => new ConvexPolygonCollider2DState (
            this.collider,
            this.polygon.Add (transformation).current,
            (this.transformState + transformation)
        );

    }
}