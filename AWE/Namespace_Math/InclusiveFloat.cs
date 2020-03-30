using System;
using AWE.Math.FloatExtensions;

namespace AWE.Math {

    public struct InclusiveFloat :
        IEquatable<float>,
        IEquatable<InclusiveFloat>,
        IComparable,
        IComparable<float>,
        IComparable<InclusiveFloat>
    {

        public static implicit operator float (InclusiveFloat f) {

            return f.value;

        }

        public static bool operator == (InclusiveFloat f, float v) {

            return f.Equals (v);

        }

        public static bool operator != (InclusiveFloat f, float v) {

            return !f.Equals (v);

        }

        public static bool operator == (InclusiveFloat f, InclusiveFloat g) {

            return f.Equals (g);

        }

        public static bool operator != (InclusiveFloat f, InclusiveFloat g) {

            return !f.Equals (g);

        }

        public static bool operator >= (InclusiveFloat f, float v) {

            bool result = (f.value > v);

            if (!result) {

                result = f.Equals (v);

            }

            return result;

        }

        public static bool operator <= (InclusiveFloat f, float v) {

            bool result = (f.value < v);

            if (!result) {

                result = f.Equals (v);

            }

            return result;

        }

        public float value { get; }
        public bool isInclusive { get; }
        public float tolerance { get; }

        public InclusiveFloat (
            float value,
            bool isInclusive,
            float tolerance = SFloatMath.MINIMUM_DIFFERENCE
        ) : this () {

            this.value = value;
            this.isInclusive = isInclusive;
            this.tolerance = tolerance;

        }

        public override bool Equals (object other) {

            var isEqual = false;

            switch (other) {

            case InclusiveFloat f:
                isEqual = this.Equals (f);
            break;

            case float v:
                isEqual = this.Equals (v);
            break;

            }

            return isEqual;

        }

        public override int GetHashCode () {

            int basePrime = 139;
            int multPrime = 229;

            unchecked {

                var hashCode = basePrime;
                hashCode = ((hashCode * multPrime) ^ this.value.GetHashCode ());
                hashCode = ((hashCode * multPrime) ^ this.isInclusive.GetHashCode ());
                hashCode = ((hashCode * multPrime) ^ this.tolerance.GetHashCode ());

                return hashCode;

            }
        }

        public bool Equals (float other) {

            var isEqual = this.isInclusive;

            if (isEqual) {

                isEqual = (this.value - other).IsNegligible (this.tolerance);

            }

            return isEqual;

        }

        public bool Equals (InclusiveFloat other) {

            return (
                (this.value == other.value)
                && (this.isInclusive == other.isInclusive)
                && (this.tolerance == other.tolerance)
            );

        }

        public int CompareTo (object other) {

            int comparison;

            switch (other) {

            case InclusiveFloat f:
                comparison = this.CompareTo (f);
            break;

            case float v:
                comparison = this.CompareTo (v);
            break;

            default:
                throw new ArgumentException (
                    "Object is not an InclusiveFloat or float. ",
                    other.GetType ().ToString () + " other"
                );
#pragma warning disable CS0162 // Unreachable code detected
            break;
#pragma warning restore CS0162 // Unreachable code detected

            }

            return comparison;

        }

        public int CompareTo (float other) {

            int comparison = this.value.CompareTo (other);

            if ((comparison == 0) && !isInclusive) {

                comparison = -1;

            }

            return comparison;

        }

        public int CompareTo (InclusiveFloat other) {

            int comparison = this.value.CompareTo (other.value);

            if (comparison == 0) {

                comparison = this.isInclusive.CompareTo (other.isInclusive);

                if (comparison == 0) {

                    comparison -= this.tolerance.CompareTo (other.tolerance);

                }
            }

            return comparison;

        }
    }
}