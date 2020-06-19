// UnityEngine needed for Mathf.Sqrt
//using UnityEngine;
using System;
using AWE.Math.FloatExtensions;

namespace AWE.Math {

    public struct Vector2D : IEquatable<Vector2D> {

        public static readonly Vector2D zero;

        static Vector2D () {

            zero = new Vector2D () {
                head = pair2f.origin,
                tail = pair2f.origin,
                components = pair2f.origin
            };

        }

        public static bool operator == (Vector2D v, Vector2D w) => v.Equals (w);

        public static bool operator != (Vector2D v, Vector2D w) => !v.Equals (w);

        public static Vector2D operator + (Vector2D v, Vector2D w) => new Vector2D (
            (v.head + w.components),
            v.tail
        );

        public static Vector2D operator + (Vector2D v, pair2f p) => new Vector2D (
            (v.head + p),
            v.tail
        );

        public static Vector2D operator - (Vector2D v, Vector2D w) => (v + (-w));

        public static Vector2D operator - (Vector2D v, pair2f p) => (v + (-p));

        public static Vector2D operator * (Vector2D v, float f) => new Vector2D (
            (v.tail + (v.components * f)),
            v.tail
        );

        public static Vector2D operator / (Vector2D v, float f) => new Vector2D (
            (v.tail + (v.components / f)),
            v.tail
        );

        public static Vector2D operator - (Vector2D v) => new Vector2D (
                (v.tail + (-v.components)),
                v.tail
            );

        public pair2f head { get; private set; }
        public pair2f tail { get; private set; }
        public pair2f components { get; private set; }
        public bool isPointingDown { get; private set; }
        public bool isPointingUp { get; private set; }
        public bool isPointingLeft { get; private set; }
        public bool isPointingRight { get; private set; }

        public bool isHorizontal => (!this.isPointingDown && !this.isPointingUp);

        public bool isVertical => (!this.isPointingLeft && !this.isPointingRight);

        public float xComponent => this.components.x;

        public float yComponent => this.components.y;

        public float magnitude => this.components.magnitude;

        public Vector2D (float x1, float y1, float x2 = 0f, float y2 = 0f) : this () {

            float xdiff = (x1 - x2);
            float ydiff = (y1 - y2);

            var isHorizontal = ydiff.IsNegligible ();
            var isVertical = xdiff.IsNegligible ();

            if (!(isHorizontal && isVertical)) {

                this.head = new pair2f (x1, y1);
                this.tail = new pair2f (x2, y2);
                this.components = new pair2f (xdiff, ydiff);

                this.SetDirections (
                    xdiff,
                    ydiff,
                    isHorizontal,
                    isVertical
                );

            }
        }

        public Vector2D (pair2f head, pair2f tail) : this (head.x, head.y, tail.x, tail.y) {}

        public Vector2D (pair2f position) : this () {

            var isHorizontal = position.y.IsNegligible ();
            var isVertical = position.x.IsNegligible ();

            if (!(isHorizontal && isVertical)) {

                this.head = position;
                this.tail = pair2f.origin;
                this.components = position;

                this.SetDirections (
                    position.x,
                    position.y,
                    isHorizontal,
                    isVertical
                );

            }
        }

        private void SetDirections (float xdiff, float ydiff) =>
            this.SetDirections (xdiff, ydiff, ydiff.IsNegligible (), xdiff.IsNegligible ());

        private void SetDirections (
            float xdiff,
            float ydiff,
            bool isHorizontal,
            bool isVertical
        ) {

            if (!(isHorizontal || isVertical)) {

                if (xdiff > 0f) {

                    if (ydiff > 0f) {

                        this.isPointingDown = false;
                        this.isPointingUp = true;
                        this.isPointingLeft = false;
                        this.isPointingRight = true;

                    } else {

                        this.isPointingDown = true;
                        this.isPointingUp = false;
                        this.isPointingLeft = false;
                        this.isPointingRight = true;

                    }

                } else {

                    if (ydiff > 0f) {

                        this.isPointingDown = false;
                        this.isPointingUp = true;
                        this.isPointingLeft = true;
                        this.isPointingRight = false;

                    } else {

                        this.isPointingDown = true;
                        this.isPointingUp = false;
                        this.isPointingLeft = true;
                        this.isPointingRight = false;

                    }
                }

            } else if (isHorizontal) {

                this.isPointingDown = false;
                this.isPointingUp = false;
                this.isPointingLeft = (xdiff < 0f);
                this.isPointingRight = (xdiff > 0f);

            } else if (isVertical) {

                this.isPointingDown = (ydiff < 0f);
                this.isPointingUp = (ydiff > 0f);
                this.isPointingLeft = false;
                this.isPointingRight = false;

            }
        }

        public override bool Equals (object other) {

            var isEqual = false;

            if (other is Vector2D otherVec) {

                isEqual = this.Equals (otherVec);

            }

            return isEqual;

        }

        public override int GetHashCode () {

            int basePrime = 181;
            int multPrime = 113;

            unchecked {

                var hashCode = basePrime;
                hashCode = ((hashCode * multPrime) ^ this.head.GetHashCode ());
                hashCode = ((hashCode * multPrime) ^ this.tail.GetHashCode ());

                return hashCode;

            }
        }

        public bool Equals (Vector2D other) {

            float headxdiff = (this.head.x - other.head.x);
            float headydiff = (this.head.y - other.head.y);

            var isTailEqual = false;
            var isHeadEqual = (
                headxdiff.IsNegligible ()
                && headydiff.IsNegligible ()
            );

            if (isHeadEqual) {

                float tailxdiff = (this.tail.x - other.tail.x);
                float tailydiff = (this.tail.y - other.tail.y);

                isTailEqual = (
                    tailxdiff.IsNegligible ()
                    && tailydiff.IsNegligible ()
                );

            }

            return (isHeadEqual && isTailEqual);

        }

        public angle ToAngle () => SFloatMath.Arctangent (this.components.y / this.components.x);

        public bool IsIntersecting (pair2f point) {

            var isColinear = false;
            var isBounded = false;

            if (this.isHorizontal) {

                float ydiff = (point.y - this.tail.y);
                isColinear = ydiff.IsNegligible ();

                if (isColinear) {

                    float tailxdiff = (point.x - this.tail.x);

                    if (!tailxdiff.IsNegligible ()) {

                        float headxdiff = (this.head.x - point.x);
                        isBounded = ((headxdiff * tailxdiff) > 0f);

                    }
                }

            } else if (this.isVertical) {

                float xdiff = (point.x - this.tail.x);
                isColinear = xdiff.IsNegligible ();

                if (isColinear) {

                    float tailydiff = (point.y - this.tail.y);

                    if (!tailydiff.IsNegligible ()) {

                        float headydiff = (this.head.y - point.y);
                        isBounded = ((headydiff * tailydiff) > 0f);

                    }
                }

            } else {

                float orientation = SShapeMath.GetOrientation (point, this.head, this.tail);
                isColinear = orientation.IsNegligible (SShapeMath.MINIMUM_ORIENTATION);

                if (isColinear) {

                    var taildiff = (point - this.tail);

                    if (!pair2f.origin.Equals (taildiff)) {

                        var headdiff = (this.head - point);

                        isBounded = (
                            ((headdiff.x * taildiff.x) > 0f)
                            && ((headdiff.y * taildiff.y) > 0f)
                        );

                    }
                }
            }

            return (isColinear && isBounded);

        }

        public bool IsIntersecting (Line2D line) {

            pair2f linePoint1;
            pair2f linePoint2;

            if (line.isHorizontal) {

                linePoint1 = new pair2f (
                    this.head.x,
                    line.yIntercept
                );
                linePoint2 = new pair2f (
                    this.tail.x,
                    line.yIntercept
                );

            } else {

                linePoint1 = new pair2f (
                    line.x (this.head.y),
                    this.head.y
                );
                linePoint2 = new pair2f (
                    line.x (this.tail.y),
                    this.tail.y
                );

            }

            return SShapeMath.IsLineSegmentIntersectingAnother (
                this.head,
                this.tail,
                linePoint1,
                linePoint2
            );

        }

        public bool IsIntersecting (Vector2D other) => SShapeMath.IsLineSegmentIntersectingAnother (
            this.head,
            this.tail,
            other.head,
            other.tail
        );

    }
}
