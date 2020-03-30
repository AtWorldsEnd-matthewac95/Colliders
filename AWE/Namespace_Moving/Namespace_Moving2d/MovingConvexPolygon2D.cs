using System.Collections.ObjectModel;
using AWE.Math;

namespace AWE.Moving.Moving2D {

    public class MovingConvexPolygon2D : AMovingPolygon2D<ConvexPolygon2D> {
        
        public static MovingConvexPolygon2D operator + (MovingConvexPolygon2D m, Transformation2D t) => m.Add (t);
        public static MovingConvexPolygon2D operator - (MovingConvexPolygon2D m, Transformation2D t) => m.Subtract (t);

        private MovingConvexPolygon2D (AMovingPolygon2D<ConvexPolygon2D> polygon) : base (polygon.current, polygon.center) {}

        public MovingConvexPolygon2D (ConvexPolygon2D current, pair2f center) : base (current, center) {}

        protected override AMovingPolygon2D<ConvexPolygon2D> CreateMovingPolygon (APolygon2D current, pair2f center) {

            AMovingPolygon2D<ConvexPolygon2D> polygon;

            if (current is ConvexPolygon2D convex) {

                polygon = new MovingConvexPolygon2D (convex, center);

            } else {

                polygon = CreatePolygonFromPoints (
                    current.unoffsetVerticies,
                    center
                );

            }

            return polygon;

        }

        protected override AMovingPolygon2D<ConvexPolygon2D> CreatePolygonFromPoints (
            ReadOnlyCollection<pair2f> points,
            pair2f center
        ) => new MovingConvexPolygon2D (
            new ConvexPolygon2D (points),
            center
        );

        new public MovingConvexPolygon2D Add (
            Transformation2D transformation,
            pair2f centerOfTransformation = default,
            pair2f centerAfterTransformation = default
        ) => new MovingConvexPolygon2D (base.Add (
            transformation,
            centerOfTransformation,
            centerAfterTransformation
        ));

        new public MovingConvexPolygon2D Subtract (
            Transformation2D transformation,
            pair2f centerOfTransformation = default,
            pair2f centerAfterTransformation = default
        ) => new MovingConvexPolygon2D (base.Subtract (
            transformation,
            centerOfTransformation,
            centerAfterTransformation
        ));

    }
}