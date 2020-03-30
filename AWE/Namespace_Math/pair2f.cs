using System;
using System.Collections;
using System.Collections.Generic;
// UnityEngine needed for Vector2 and Vector3
//using UnityEngine;
using AWE.Math.FloatExtensions;
using AWE.Math.DirectionMathExtensions;

namespace AWE.Math {

    public struct pair2f : IPair<float>, IEquatable<pair2f> {

        public static readonly pair2f origin;
        public static readonly pair2f one;
        public static readonly pair2f nan;

        static pair2f () {

            origin = new pair2f ();
            one = new pair2f () {
                x = 1f,
                y = 1f
            };
            nan = new pair2f () {
                x = float.NaN,
                y = float.NaN
            };

        }

        public static bool operator == (pair2f p, pair2f q) => p.Equals (q);

        public static bool operator != (pair2f p, pair2f q) => !p.Equals (q);

        /*
        public static implicit operator Vector2 (pair2f p) {

            return new Vector2 (p.x, p.y);

        }

        public static implicit operator pair2f (Vector2 v) {

            return new pair2f (v.x, v.y);

        }

        public static implicit operator Vector3 (pair2f p) {

            return new Vector3 (p.x, p.y, 0f);

        }

        public static implicit operator pair2f (Vector3 v) {

            return new pair2f (v.x, v.y);

        }
        */

        public static pair2f operator + (pair2f p, pair2f q) => new pair2f ((p.x + q.x), (p.y + q.y));

        public static pair2f operator - (pair2f p, pair2f q) => new pair2f ((p.x - q.x), (p.y - q.y));

        public static pair2f operator * (pair2f p, float f) => new pair2f ((p.x * f), (p.y * f));

        public static pair2f operator * (float f, pair2f p) => new pair2f ((p.x * f), (p.y * f));

        public static pair2f operator / (pair2f p, float f) => new pair2f ((p.x / f), (p.y / f));

        public static pair2f operator - (pair2f p) => new pair2f (-p.x, -p.y);

        public static pair2f operator + (pair2f p, angle a) => p.Rotate (a, clockwise: true);

        public static pair2f operator - (pair2f p, angle a) => p.Rotate (a, clockwise: false);

        public float x { get; private set; }
        public float y { get; private set; }

        float IPair<float>.first => this.x;
        float IPair<float>.second => this.y;
        int IReadOnlyCollection<float>.Count => 2;

        public float this [int index] {

            get {

                float value;

                if (index == 0) {

                    value = x;

                } else if (index == 1) {

                    value = y;

                } else {

                    throw new IndexOutOfRangeException ();

                }

                return value;

            }
        }

        public float this [bool getY] => (getY ? this.y : this.x);

        public float magnitude {

            get {

                return ((this.x * this.x) + (this.y * this.y)).sqrt ();

            }
        }

        public EQuadrant quadrant {

            get {

                EQuadrant quad;

                if (this.x < 0f) {

                    if (this.y < 0f) {

                        quad = EQuadrant.NegativeNegative;

                    } else {

                        quad = EQuadrant.NegativePositive;

                    }

                } else if (this.y < 0f) {

                    quad = EQuadrant.PositiveNegative;

                } else {

                    quad = EQuadrant.PositivePositive;

                }

                return quad;

            }
        }

        public bool isZero => ((this.x == 0f) && (this.y == 0f));
        public bool isOne => ((this.x - 1f).IsNegligible () && (this.y - 1f).IsNegligible ());
        public bool isNan => (Single.IsNaN (this.x) || Single.IsNaN (this.y));

        float IPair<float>.opposite (int index) {

            float value;

            if (index == 0) {

                value = this.y;

            } else if (index == 1) {

                value = this.x;

            } else {

                throw new IndexOutOfRangeException ();

            }

            return value;

        }

        public pair2f (float x, float y) : this () {

            this.x = x;
            this.y = y;

        }

        IEnumerator IEnumerable.GetEnumerator () => this.GetEnumerator ();
        public IEnumerator<float> GetEnumerator () {

            yield return this.x;
            yield return this.y;

        }

        public override bool Equals (object other) {

            var isEqual = false;

            if (other is pair2f otherPair) {

                isEqual = this.Equals (otherPair);

            }

            return isEqual;

        }

        public override int GetHashCode () {

            int basePrime = 61;
            int multPrime = 229;

            unchecked {

                var hashCode = basePrime;
                hashCode = ((hashCode * multPrime) ^ this.x.GetHashCode ());
                hashCode = ((hashCode * multPrime) ^ this.y.GetHashCode ());

                return hashCode;

            }
        }

        public bool Equals (pair2f other) {

            float xdiff = (this.x - other.x);
            float ydiff = (this.y - other.y);

            return (xdiff.IsNegligible () && ydiff.IsNegligible ());

        }

        public EDirection ToDirection () => EDirection.None.Add (this);

        public void Deconstruct (out float first, out float second) {

            first = this.x;
            second = this.y;

        }

        public override string ToString () => String.Format ("({0}, {1})", this.x, this.y);

        public pair2f Rotate (angle rotation, bool clockwise = true) {

            var a = (clockwise ? (SFloatMath.Arctangent (this) + rotation) : (SFloatMath.Arctangent (this) - rotation));
            return (this.magnitude * new pair2f (SFloatMath.Cosine (a), SFloatMath.Sine (a)));

        }
    }
}
