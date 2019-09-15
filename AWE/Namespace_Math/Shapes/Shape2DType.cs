using System;

namespace AWE.Math {

    public class Shape2DType : IComparable, IComparable<Shape2DType>, IEquatable<Shape2DType> {

        public static readonly Shape2DType None;
        public static readonly Shape2DType Any;
        public static readonly Shape2DType Polygon;
        public static readonly Shape2DType Circle;
        public static readonly Shape2DType Quadratic;

        static Shape2DType () {

            None = new Shape2DType ("None", 0);
            Any = new Shape2DType ("Any", 1);
            Polygon = new Shape2DType ("Polygon", 2);
            Circle = new Shape2DType ("Circle", 3);
            Quadratic = new Shape2DType ("Quadratic", 4);

        }

        // TODO - Lotsa operators

        public string name { get; protected set; }
        public int index { get; protected set; }

        public Shape2DType (string name, int index) {

            this.name = name;
            this.index = index;

        }

        public override bool Equals (object other) {

            var isEqual = false;

            if (other is Shape2DType otherType) {

                isEqual = this.Equals (otherType);

            }

            return isEqual;

        }

        public override int GetHashCode () {

            int basePrime = 251;
            int multPrime = 67;

            unchecked {

                var hashCode = basePrime;
                hashCode = ((hashCode * multPrime) ^ this.name.GetHashCode ());
                hashCode = ((hashCode * multPrime) ^ this.index.GetHashCode ());

                return hashCode;

            }
        }

        public override string ToString () {

            return this.name;

        }

        public bool Equals (Shape2DType other) {

            return (
                this.name.Equals (other.name)
                && (this.index == other.index)
            );

        }

        public int CompareTo (object other) {

            if (other is Shape2DType otherType) {

                return this.CompareTo (otherType);

            } else {

                // Improve this exception message.
                throw new Exception ("object other is not a Shape2DType");

            }
        }

        public int CompareTo (Shape2DType other) {

            var comparison = this.index.CompareTo (other.index);

            if (comparison == 0) {

                comparison = this.name.CompareTo (other.name);

            }

            return comparison;

        }
    }
}
