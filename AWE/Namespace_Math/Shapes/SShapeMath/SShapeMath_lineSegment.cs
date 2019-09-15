using AWE.Math.FloatExtensions;

namespace AWE.Math {

    public static partial class SShapeMath {

        /// This function returns the normal vector that's clockwise to the given points.
        public static pair2f GetNormalComponents (pair2f tail, pair2f head) => new pair2f (
            (head.y - tail.y),
            (tail.x - head.x)
        );

        public static float GetDistanceFromPointToLine (
            pair2f point,
            pair2f linePoint1,
            pair2f linePoint2
        ) {

            float distance;
            var xdiff = (linePoint2.x - linePoint1.x);
            var ydiff = (linePoint2.y - linePoint1.y);

            if (xdiff == 0f) {

                distance = System.Math.Abs (point.x - linePoint1.x);

            } else if (ydiff == 0f) {

                distance = System.Math.Abs (point.y - linePoint1.y);

            } else {

                var slope = (ydiff / xdiff);

                var numerator = System.Math.Abs (
                    (slope * (point.x - linePoint1.x))
                    - (point.y - linePoint1.y)
                );

                var denominator = ((slope * slope) + 1f).sqrt();

                distance = (numerator / denominator);

            }

            return distance;

        }

        public static bool IsLineIntersectingAnother (
            pair2f line1point1,
            pair2f line1point2,
            pair2f line2point1,
            pair2f line2point2
        ) {

            var diff1 = (line1point2 - line1point1);
            var diff2 = (line2point2 - line2point1);

            var mult1 = (diff1.y * diff2.x);
            var mult2 = (diff2.y * diff1.x);

            return !(mult1 - mult2).IsNegligible ();

        }

        public static bool IsLineSegmentIntersectingAnother (
            pair2f line1point1,
            pair2f line1point2,
            pair2f line2point1,
            pair2f line2point2,
            bool disallowParallel = true,
            bool isIntersectionLenient = false,
            float tolerance = MINIMUM_ORIENTATION
        ) {

            var isIntersecting = false;

            var orientation1a = GetOrientation (
                line2point1,
                line1point1,
                line1point2,
                isIntersectionLenient,
                tolerance
            );

            var isParallel = (orientation1a == 0);

            if (!(isParallel && disallowParallel)) {

                var orientation1b = GetOrientation (
                    line2point2,
                    line1point1,
                    line1point2,
                    isIntersectionLenient,
                    tolerance
                );

                isParallel &= (orientation1b == 0);

                if (orientation1a != orientation1b) {

                    var orientation2a = GetOrientation (
                        line1point1,
                        line2point1,
                        line2point2,
                        isIntersectionLenient,
                        tolerance
                    );

                    isIntersecting = (orientation2a == 0);

                    if (!isIntersecting) {

                        var orientation2b = GetOrientation (
                            line1point2,
                            line2point1,
                            line2point2,
                            isIntersectionLenient,
                            tolerance
                        );

                        isIntersecting = (orientation2a != orientation2b);

                    }

                } else if (isParallel && !disallowParallel) {

                    var direction1a = (line1point1 - line2point1).ToDirection ();
                    var direction1b = (line1point1 - line2point2).ToDirection ();

                    isIntersecting = (direction1a != direction1b);

                    if (!isIntersecting) {

                        var direction2a = (line1point2 - line2point1).ToDirection ();
                        var direction2b = (line1point2 - line2point2).ToDirection ();

                        isIntersecting = (
                            (direction2a != direction2b)
                            || (direction1a != direction2a)
                        );

                    }
                }
            }

            return isIntersecting;

        }

        public static pair2f FindIntersectionOfLineSegments (
            pair2f line1point1,
            pair2f line1point2,
            pair2f line2point1,
            pair2f line2point2
        ) {

            var intersection = pair2f.nan;

            var slope1 = (line1point1 - line1point2);
            var slope2 = (line2point1 - line2point2);

            var x1diff = (line1point1.x - line2point1.x);
            var y1diff = (line1point1.y - line2point1.y);

            var denominator = ((slope1.x * slope2.y) - (slope1.y * slope2.x));
            var interpolation1 = (((slope2.y * x1diff) - (slope2.x * y1diff)) / denominator);

            if ((interpolation1 >= 0f) && (interpolation1 <= 1f)) {

                var interpolation2 = (((slope1.y * x1diff) - (slope1.x * y1diff)) / denominator);

                if ((interpolation2 >= 0f) && (interpolation2 <= 1f)) {

                    intersection = (
                        line1point1 + (
                            interpolation1 * (
                                line1point2
                                - line1point1
                            )
                        )
                    );

                }
            }

            return intersection;

        }
    }
}