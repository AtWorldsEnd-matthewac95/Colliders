using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using AWE.Math.FloatExtensions;

namespace AWE.Math {

    public struct angle : IEquatable<angle>, IComparable, IComparable<angle>, IReadOnlyList<float> {

        public const float DIRECTION_DEGREE_INCREMENT = (SFloatMath.DEGREE_MAX_VALUE / 8f);
        public const float DIRECTION_RADIAN_INCREMENT = (SFloatMath.RADIAN_MAX_VALUE / 8f);

        public static List<angle> CreateQuarterAngles () {

            var list = new List<angle> () {
                new angle (0f, EAngleMode.Degree),
                new angle (90f, EAngleMode.Degree),
                new angle (180f, EAngleMode.Degree),
                new angle (270f, EAngleMode.Degree)
            };

            return list;

        }

        public static bool operator == (angle a, angle b) => a.Equals (b);

        public static bool operator != (angle a, angle b) => !a.Equals (b);

        public static angle operator - (angle a, angle b) {

            float diff = (a.value - b.GetValueUsing (a.mode));
            return new angle (diff, a.mode);

        }

        public static angle operator + (angle a, angle b) {

            float sum = (a.value + b.GetValueUsing (a.mode));
            return new angle (sum, a.mode);

        }

        public static angle operator * (angle a, float s) => new angle ((a.value * s), a.mode);

        public static angle operator * (float s, angle a) => new angle ((a.value * s), a.mode);

        public float value { get; }
        public EAngleMode mode { get; }
        public float trimmedValue { get; }

        public float degree => this.GetValueUsing (EAngleMode.Degree);
        public float radian => this.GetValueUsing (EAngleMode.Radian);
        public float percent => this.GetValueUsing (EAngleMode.Percent);
        public angle trimmed => (new angle (this.trimmedValue, this.mode));

        int IReadOnlyCollection<float>.Count => 1;

        float IReadOnlyList<float>.this [int index] {

            get {

                float value;

                if (index == 0) {

                    value = trimmedValue;

                } else {

                    throw new IndexOutOfRangeException ();

                }

                return value;

            }
        }

        public angle (float value, EAngleMode mode) : this () {

            this.mode = mode;

            if (mode == EAngleMode.Auto) {

                this.value = float.NaN;
                this.trimmedValue = float.NaN;

            } else {

                this.value = value;
                this.trimmedValue = SFloatMath.TrimToRange (value, mode);

            }
        }

        public angle (EDirection direction, EAngleMode mode) : this () {

            this.mode = mode;

            if ((mode == EAngleMode.Auto)
                || (direction == EDirection.None)
            ) {

                this.value = float.NaN;
                this.trimmedValue = float.NaN;

            } else {

                switch (mode) {

                case EAngleMode.Degree:
                    this.value = (DIRECTION_DEGREE_INCREMENT * ((int)direction) - 1);
                break;

                case EAngleMode.Radian:
                    this.value = (DIRECTION_RADIAN_INCREMENT * ((int)direction) - 1);
                break;

                case EAngleMode.Percent:
                    this.value = ((((float)direction) - 1f) / 8f);
                break;

                }

                this.trimmedValue = this.value;

            }

        }

        public override bool Equals (object other) {

            var isEqual = false;

            if (other is angle otherAngle) {

                isEqual = this.Equals (otherAngle);

            }

            return isEqual;

        }

        public override int GetHashCode () {

            if (this.mode == EAngleMode.Auto) {

                return 0;

            }

            int basePrime = 113;
            int multPrime = 157;

            unchecked {

                var hashCode = basePrime;
                hashCode = ((hashCode * multPrime) ^ this.value.GetHashCode ());
                hashCode = ((hashCode * multPrime) ^ this.mode.GetHashCode ());

                return hashCode;

            }
        }

        public bool Equals (angle other) {

            if (other.mode == EAngleMode.Auto) {

                return (this.mode == EAngleMode.Auto);

            }

            if (this.mode == EAngleMode.Auto) {

                return false;

            }

            float diff = (
                this.trimmedValue
                - other.trimmed.GetValueUsing (this.mode)
            );

            return diff.IsNegligible ();

        }

        public int CompareTo (object other) {

            if (other is angle otherAngle) {

                return this.CompareTo (otherAngle);

            } else {

                throw new ArgumentException (
                    "Object is not an angle. ",
                    other.GetType ().ToString () + " other"
                );

            }
        }

        public int CompareTo (angle other) {

            if (other.mode == EAngleMode.Auto) {

                if (this.mode == EAngleMode.Auto) {

                    return 0;

                } else {

                    return 1;

                }

            } else {

                if (this.mode == EAngleMode.Auto) {

                    return -1;

                }
            }

            float otherValue = other.trimmed.GetValueUsing (this.mode);
            return this.trimmedValue.CompareTo (otherValue);

        }

        public override string ToString () {

            var message = new StringBuilder (20);

            switch (this.mode) {

            case EAngleMode.Auto:
                message.Append ("ERROR: This angle has an EAngleMode of Auto, which is invalid for angle instances! ");
            break;

            case EAngleMode.Degree:
                message.Append (this.value.ToString ());
                message.Append (" degrees");
            break;

            case EAngleMode.Radian:
                message.Append (this.value.ToString ());
                message.Append (" radians");
            break;

            case EAngleMode.Percent:
                message.AppendFormat ("{0:P2}", this.value);
            break;

            }

            return message.ToString ();

        }

        public angle GetAs (EAngleMode otherMode) {

            if (otherMode == EAngleMode.Auto) {

                otherMode = this.mode;

            }

            return new angle (
                this.GetValueUsing (otherMode),
                otherMode
            );

        }

        public float GetValueUsing (EAngleMode otherMode) {

            if (otherMode == EAngleMode.Auto) {

                otherMode = this.mode;

                if (otherMode == EAngleMode.Auto) {

                    return float.NaN;

                }
            }

            if (otherMode == this.mode) {

                return this.value;

            } else {

                float conversionFactor = 1f;

                switch (this.mode) {

                case EAngleMode.Degree:

                    switch (otherMode) {

                    case EAngleMode.Radian:
                        conversionFactor = SFloatMath.DEGREE_TO_RADIAN;
                    break;

                    case EAngleMode.Percent:
                        conversionFactor = (1f / SFloatMath.DEGREE_MAX_VALUE);
                    break;

                    }

                break;

                case EAngleMode.Radian:

                    switch (otherMode) {

                    case EAngleMode.Degree:
                        conversionFactor = SFloatMath.RADIAN_TO_DEGREE;
                    break;

                    case EAngleMode.Percent:
                        conversionFactor = (1f / SFloatMath.RADIAN_MAX_VALUE);
                    break;

                    }

                break;

                case EAngleMode.Percent:

                    switch (otherMode) {

                    case EAngleMode.Degree:
                        conversionFactor = SFloatMath.DEGREE_MAX_VALUE;
                    break;

                    case EAngleMode.Radian:
                        conversionFactor = SFloatMath.RADIAN_MAX_VALUE;
                    break;

                    }

                break;

                }

                return (this.value * conversionFactor);

            }
        }

        IEnumerator IEnumerable.GetEnumerator() {

            yield return this.value;

        }

        IEnumerator<float> IEnumerable<float>.GetEnumerator() {

            yield return this.value;

        }
    }
}
