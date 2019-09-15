using System;
using AWE.Math.DirectionMathExtensions;
using AWE.Math.FloatExtensions;

namespace AWE.Math {

    public class Line2D : ICurve2D {

        public float slope { get; }
        public float yIntercept { get; }
        public float xIntercept { get; }

        public bool isHorizontal => slope.IsNegligible ();

        public bool isVertical => slope.IsInfinite ();

        public Line2D (pair2f point1, pair2f point2) {

            float xdiff = (point1.x - point2.x);
            float ydiff = (point1.y - point2.y);

            if (xdiff.IsNegligible ()
                && ydiff.IsNegligible ()
            ) {

                // TODO - Throw an exception.

            }

            this.slope = (ydiff / xdiff);
            this.yIntercept = (point1.y - (this.slope * point1.x));
            this.xIntercept = (point1.x - (point1.y / this.slope));

        }

        public Line2D (float x1, float y1, float x2, float y2) : this (
            new pair2f (x1, y1),
            new pair2f (x2, y2)
        ) {
        }

        public Line2D (Line2DSegment segment) : this (segment.tail, segment.head) {}

        public Line2D (float slope, pair2f point = default) {

            if (slope.IsInfinite ()) {

                this.xIntercept = point.x;
                this.yIntercept = float.NegativeInfinity;

            } else if (slope.IsNegligible ()) {

                this.xIntercept = float.NegativeInfinity;
                this.yIntercept = point.y;

            } else {

                this.yIntercept = (point.y - (this.slope * point.x));
                this.xIntercept = (point.x - (point.y / this.slope));

            }

            this.slope = slope;

        }

        public Line2D (angle slopeAngle, pair2f point = default) : this (
            SFloatMath.Tangent (slopeAngle),
            point
        ) {
        }

        public (pair2f first, pair2f second) GetTwoPoints () {

            pair2f first, second;

            if (this.isVertical) {

                first = new pair2f (this.xIntercept, 0f);
                second = new pair2f (this.xIntercept, 1f);

            } else {

                first = new pair2f (0f, this.yIntercept);

                if (this.isHorizontal || this.yIntercept.IsNegligible ()) {

                    second = new pair2f (1f, this.slope);

                } else {

                    second = new pair2f (this.xIntercept, 0f);

                }
            }

            return (first, second);

        }

        public bool IsParallelTo (float otherSlope) => (this.slope - otherSlope).IsNegligible ();

        public bool IsParallelTo (Line2D other) => this.IsParallelTo (other.slope);

        // TODO - Cache point?
        public pair2f GetIntersection (Line2D otherLine, bool cachePoint = false) {

            pair2f intersection;

            if (this.isVertical) {

                if (otherLine.isVertical) {

                    intersection = new pair2f (0f, float.PositiveInfinity);

                } else {

                    intersection = new pair2f (
                        this.xIntercept,
                        otherLine.y (this.xIntercept)
                    );

                }

            } else if (this.isHorizontal) {

                if (otherLine.isHorizontal) {

                    intersection = new pair2f (float.PositiveInfinity, 0f);

                } else {

                    intersection = new pair2f (
                        otherLine.x (this.yIntercept),
                        this.yIntercept
                    );

                }

            } else {

                if (this.IsParallelTo (otherLine.slope)) {

                    intersection = new pair2f (
                        float.PositiveInfinity,
                        (this.slope * float.PositiveInfinity)
                    );

                } else {

                    float x = (
                        (otherLine.yIntercept - this.yIntercept)
                        / (this.slope - otherLine.slope)
                    );

                    intersection = new pair2f (x, this.y (x));

                }
            }

            return intersection;

        }

        public float GetDistanceToPoint (pair2f point) {

            float distance;

            if (this.isVertical) {

                distance = (point.x - this.xIntercept).abs();

            } else if (this.isHorizontal) {

                distance = (point.y - this.yIntercept).abs();

            } else {

                float numerator = (
                    (point.x * this.slope)
                    - point.y
                    + this.yIntercept
                ).abs();

                float denominator = SFloatMath.GetSquareRoot (
                    (this.slope * this.slope) + 1f
                );

                distance = (numerator / denominator);

            }

            return distance;

        }

        public float x (float y) => (this.xIntercept + (y / this.slope));

        public float y (float x) => (this.yIntercept + (x * this.slope));

        public EDirection GetDirectionToPoint (pair2f point) {

            var direction = new DirectionGradient ();

            if (this.isVertical) {

                direction.AddHorizontal (point.x - this.xIntercept);

            } else if (this.isHorizontal) {

                direction.AddVertical (point.y - this.yIntercept);

            } else {

                float x = this.x (point.y);
                float y = this.y (point.x);

                direction.AddHorizontal (point.x - x);
                direction.AddVertical (point.y - y);

            }

            return direction.current;

        }

        public EDirection GetHorizontalDirectionToPoint (pair2f point) {

            var direction = new DirectionGradient ();

            if (!this.isHorizontal) {

                float x = this.x (point.y);
                direction.AddHorizontal (point.x - x);

            }

            return direction.current;

        }

        public EDirection GetVerticalDirectionToPoint (pair2f point) {

            var direction = new DirectionGradient ();

            if (!this.isVertical) {

                float y = this.y (point.x);
                direction.AddVertical (point.y - y);

            }

            return direction.current;

        }

        public bool IsPointOnLine (pair2f point) => (
            (this.isVertical && (point.x - this.xIntercept).IsNegligible ())
            || (point.y - this.y (point.x)).IsNegligible ()
        );

        bool ICurve2D.IsPointOnCurve (pair2f point) => this.IsPointOnLine (point);
    }
}
