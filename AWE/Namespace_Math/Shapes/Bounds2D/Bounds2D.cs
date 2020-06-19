using System;
using System.Collections.ObjectModel;
using AWE.Math.FloatExtensions;

namespace AWE.Math {

    public struct Bounds2D : IBounds2D, IEquatable<Bounds2D> {

        public static Bounds2D operator + (Bounds2D b1, Bounds2D b2) => new Bounds2D (
            (b1.right + b2.right),
            (b1.top + b2.top),
            (b1.left + b2.left),
            (b1.bottom + b2.bottom)
        );

        public static Bounds2D operator + (Bounds2D bounds, pair2f offset) => new Bounds2D (
            (bounds.right + offset.x),
            (bounds.top + offset.y),
            (bounds.left + offset.x),
            (bounds.bottom + offset.y)
        );

        public float right { get; }
        public float top { get; }
        public float left { get; }
        public float bottom { get; }

        public float width => (this.right - this.left);
        public float height => (this.top - this.bottom);
        public pair2f centerOfBounds => new pair2f (
            ((this.right + this.left) / 2f),
            ((this.top + this.bottom) / 2f)
        );

        public bool isValid => (
            (this.width > SFloatMath.MINIMUM_DIFFERENCE) || (this.height > SFloatMath.MINIMUM_DIFFERENCE)
        );

        public float this [EDirection direction] {

            get {

                float value = Single.NaN;

                switch (direction) {

                case EDirection.Right:
                    value = this.right;
                break;

                case EDirection.Up:
                    value = this.top;
                break;

                case EDirection.Left:
                    value = this.left;
                break;

                case EDirection.Down:
                    value = this.bottom;
                break;

                default:
                    // TODO - Throw an exception
                break;

                }

                return value;

            }
        }

        public Bounds2D (float right, float top, float left, float bottom) : this () {

            this.right = right;
            this.top = top;
            this.left = left;
            this.bottom = bottom;

        }

        public Bounds2D (params pair2f[] shape) : this (Array.AsReadOnly (shape)) {}
        public Bounds2D (ReadOnlyCollection<pair2f> shape) : this () {

            float right = shape[0].x;
            float top = shape[0].y;
            float left = shape[0].x;
            float bottom = shape[0].y;

            for (int i = 1; i < shape.Count; i++) {

                right = System.Math.Max (right, shape[i].x);
                top = System.Math.Max (top, shape[i].y);
                left = System.Math.Min (left, shape[i].x);
                bottom = System.Math.Min (bottom, shape[i].y);

            }

            this.right = right;
            this.top = top;
            this.left = left;
            this.bottom = bottom;

        }

        public bool IsContaining (pair2f point) => (
            (this.right >= point.x)
            && (this.left <= point.x)
            && (this.top >= point.y)
            && (this.bottom <= point.y)
        );

        public bool IsContaining (Bounds2D other) => (
            (this.right >= other.right)
            && (this.left <= other.left)
            && (this.top >= other.top)
            && (this.bottom <= other.bottom)
        );

        public override bool Equals (object other) {

            var isEqual = false;

            if (other is Bounds2D otherBounds) {

                isEqual = this.Equals (otherBounds);

            }

            return isEqual;

        }

        public override int GetHashCode () {

            int basePrime = 89;
            int multPrime = 23;

            unchecked {

                var hashCode = basePrime;
                hashCode = ((hashCode * multPrime) ^ this.right.GetHashCode ());
                hashCode = ((hashCode * multPrime) ^ this.top.GetHashCode ());
                hashCode = ((hashCode * multPrime) ^ this.left.GetHashCode ());
                hashCode = ((hashCode * multPrime) ^ this.bottom.GetHashCode ());

                return hashCode;

            }
        }

        public bool Equals (Bounds2D other) => (
            (this.right - other.right).IsNegligible ()
            && (this.top - other.top).IsNegligible ()
            && (this.left - other.left).IsNegligible ()
            && (this.bottom - other.bottom).IsNegligible ()
        );
    }
}
