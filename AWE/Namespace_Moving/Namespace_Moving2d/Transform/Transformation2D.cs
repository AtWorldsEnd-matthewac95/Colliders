using System;
using System.Collections.Generic;
using AWE.Math;

namespace AWE.Moving.Moving2D {

    public class Transformation2D : ATransformation<float, pair2f, angle, pair2f>, IEquatable<Transformation2D> {

        public static Transformation2D zero { get; }

        static Transformation2D () {

            zero = new Transformation2D ();

        }

        public static Transformation2D operator + (Transformation2D a, Transformation2D b) => new Transformation2D (
            (a.translation + b.translation),
            (a.rotation + b.rotation),
            new pair2f (
                (a.dilation.x * b.dilation.x),
                (a.dilation.y * b.dilation.y)
            )
        );

        public static Transformation2D operator - (Transformation2D a, Transformation2D b) => new Transformation2D (
            (a.translation + b.translation),
            (a.rotation + b.rotation),
            new pair2f (
                (a.dilation.x / b.dilation.x),
                (a.dilation.y / b.dilation.y)
            )
        );

        public static Transformation2D operator * (Transformation2D t, float s) => t.Scale (s);
        public static Transformation2D operator * (float s, Transformation2D t) => t.Scale (s);

        public Transformation2D () : base (pair2f.origin, new angle (0f, EAngleMode.Radian), pair2f.one) {}

        public Transformation2D (pair2f translation, angle rotation, pair2f dilation) : base (translation, rotation, dilation) {}

        public Transformation2D (Transform2DState current, Transform2DState next) : this (
            (next.position - current.position),
            (next.rotation - current.rotation),
            new pair2f (
                (next.dilation.x / current.dilation.x),
                (next.dilation.y / current.dilation.y)
            )
        ) {
        }

        public Transformation2D (pair2f delta, bool isDilation = false) : this (
            (isDilation ? pair2f.origin : delta),
            new angle (0f, EAngleMode.Radian),
            (isDilation ? delta : pair2f.one)
        ) {
        }

        public Transformation2D (angle rotation) : this (pair2f.origin, rotation, pair2f.one) {}

        public override ITransformation Add (ITransformation other) {

            ITransformation sum;

            if (other is Transformation2D transformation) {

                sum = this.Add (transformation);

            } else {

                sum = base.Add (other);

            }

            return sum;

        }

        protected override ATransformation<float, pair2f, angle, pair2f> _Add (
            pair2f translation,
            angle rotation,
            pair2f dilation
        ) => this.Add (translation, rotation, dilation);
        public Transformation2D Add (Transformation2D other) => this.Add (other.translation, other.rotation, other.dilation);

        public Transformation2D Add (pair2f translation, angle rotation, pair2f dilation) => new Transformation2D (
            (this.translation + translation),
            (this.rotation + rotation),
            new pair2f (
                (this.dilation.x * dilation.x),
                (this.dilation.y * dilation.y)
            )
        );

        public override ITransformation Subtract (ITransformation other) {

            ITransformation difference;

            if (other is Transformation2D transformation) {

                difference = this.Add (transformation);

            } else {

                difference = base.Add (other);

            }

            return difference;

        }

        protected override ATransformation<float, pair2f, angle, pair2f> _Subtract (
            pair2f translation,
            angle rotation,
            pair2f dilation
        ) => this.Subtract (translation, rotation, dilation);
        public Transformation2D Subtract (Transformation2D other) => this.Subtract (other.translation, other.rotation, other.dilation);

        public Transformation2D Subtract (pair2f translation, angle rotation, pair2f dilation) => new Transformation2D (
            (this.translation + translation),
            (this.rotation + rotation),
            new pair2f (
                (this.dilation.x / dilation.x),
                (this.dilation.y / dilation.y)
            )
        );

        public Transformation2D Scale (float scale, bool scaleDilation = false) => new Transformation2D (
            (this.translation * scale),
            (this.rotation * scale),
            (scaleDilation ? (this.dilation * scale) : this.dilation)
        );

        public override int GetHashCode () {

            int basePrime = 43;
            int multPrime = 61;

            unchecked {

                var hashCode = basePrime;
                hashCode = ((hashCode * multPrime) ^ this.translation.GetHashCode ());
                hashCode = ((hashCode * multPrime) ^ this.rotation.GetHashCode ());
                hashCode = ((hashCode * multPrime) ^ this.dilation.GetHashCode ());

                return hashCode;

            }
        }

        public override bool Equals (object other) {

            bool isEqual;

            if (other is Transformation2D otherTransformation2D) {

                isEqual = this.Equals (otherTransformation2D);

            } else {

                isEqual = base.Equals (other);

            }

            return isEqual;

        }

        public bool Equals (Transformation2D other) => this.Equals (other.translation, other.rotation, other.dilation);
        public override bool Equals (pair2f translation, angle rotation, pair2f dilation) => (
            this.translation.Equals (translation)
            && this.rotation.Equals (rotation)
            && this.dilation.Equals (dilation)
        );

    }
}