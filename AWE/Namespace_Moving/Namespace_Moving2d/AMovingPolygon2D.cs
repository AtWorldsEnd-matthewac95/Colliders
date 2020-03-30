using System.Collections.Generic;
using System.Collections.ObjectModel;
using AWE.Math;

namespace AWE.Moving.Moving2D {

    public abstract class AMovingPolygon2D<TPolygon2D> where TPolygon2D : APolygon2D {

        public static AMovingPolygon2D<TPolygon2D> operator + (AMovingPolygon2D<TPolygon2D> m, Transformation2D t) => m.Add (t);
        public static AMovingPolygon2D<TPolygon2D> operator - (AMovingPolygon2D<TPolygon2D> m, Transformation2D t) => m.Subtract (t);

        public TPolygon2D current { get; }
        public pair2f center { get; }

        public AMovingPolygon2D (TPolygon2D current, pair2f center) {

            this.current = current;
            this.center = center;

        }

        protected abstract AMovingPolygon2D<TPolygon2D> CreateMovingPolygon (APolygon2D current, pair2f center);
        protected abstract AMovingPolygon2D<TPolygon2D> CreatePolygonFromPoints (ReadOnlyCollection<pair2f> points, pair2f center);

        public virtual AMovingPolygon2D<TPolygon2D> Add (
            Transformation2D transformation,
            pair2f centerOfTransformation = default,
            pair2f centerAfterTransformation = default
        ) {

            if (centerOfTransformation.isZero) {

                centerOfTransformation = this.center;

            }

            if (centerAfterTransformation.isZero) {

                centerAfterTransformation = (this.center + transformation.translation);

            }

            AMovingPolygon2D<TPolygon2D> polygon;

            if (transformation.dilation.isOne) {

                polygon = this.CreateMovingPolygon (
                    current.CreateOffset (transformation.translation, transformation.rotation),
                    centerOfTransformation
                );

            } else {

                var points = new List<pair2f> ();

                for (int i = 0; i < this.current.count; i++) {

                    var temp = (this.current[i] - centerOfTransformation);
                    temp = new pair2f ((temp.x * transformation.dilation.x), (temp.y * transformation.dilation.y));
                    points.Add ((temp + transformation.rotation) + transformation.translation);

                }

                polygon = this.CreatePolygonFromPoints (points.AsReadOnly (), centerAfterTransformation);

            }

            return polygon;

        }

        public virtual AMovingPolygon2D<TPolygon2D> Subtract (
            Transformation2D transformation,
            pair2f centerOfTransformation = default,
            pair2f centerAfterTransformation = default
        ) {

            if (centerOfTransformation.isZero) {

                centerOfTransformation = this.center;

            }

            if (centerAfterTransformation.isZero) {

                centerAfterTransformation = (this.center - transformation.translation);

            }

            var points = new List<pair2f> ();

            for (int i = 0; i < this.current.count; i++) {

                var temp = (this.current[i] - centerOfTransformation);
                temp = new pair2f ((temp.x / transformation.dilation.x), (temp.y / transformation.dilation.y));
                points.Add ((temp - transformation.rotation) - transformation.translation);

            }

            return this.CreatePolygonFromPoints (points.AsReadOnly (), centerAfterTransformation);

        }
    }
}