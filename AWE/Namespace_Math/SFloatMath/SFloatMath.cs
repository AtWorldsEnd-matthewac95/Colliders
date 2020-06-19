using AWE.Math.FloatExtensions;

namespace AWE.Math {

    public static partial class SFloatMath {

        public const float MINIMUM_DIFFERENCE = 0.000001f;

        public static pair2f GetRotatedPosition2D (pair2f position, angle theta) {

            float sine = Sine (theta);
            float cosine = Cosine (theta);

            float x = (
                (position.x * cosine)
                - (position.y * sine)
            );

            float y = (
                (position.x * sine)
                + (position.y * cosine)
            );

            return new pair2f (x, y);

        }

        public static float GetDotProduct (pair2f p, pair2f q) {

            return (
                (p.x * q.x)
                + (p.y * q.y)
            );

        }

        public static float GetDotProduct (Vector2D v, Vector2D w) {

            return GetDotProduct (v.components, w.components);

        }

        public static float TrimToRange (
            float value,
            float lower,
            float upper,
            bool lowerInclusive = true,
            bool upperInclusive = false
        ) {

            float trimmedValue = value;
            float diff = (upper - lower);

            if (!IsFloatNegligible (diff)) {

                if (lowerInclusive) {

                    while (trimmedValue < lower) {

                        trimmedValue += diff;

                    }

                } else {

                    while (trimmedValue <= lower) {

                        trimmedValue += diff;

                    }
                }

                if (upperInclusive) {

                    while (trimmedValue > upper) {

                        trimmedValue -= diff;

                    }

                } else {

                    while (trimmedValue >= upper) {

                        trimmedValue -= diff;

                    }
                }
            }

            return trimmedValue;

        }

        public static float TrimToRange (
            float value,
            FloatRange range,
            bool lowerInclusive = true,
            bool upperInclusive = false
        ) {

            return TrimToRange (
                value,
                range.lower,
                range.upper,
                lowerInclusive,
                upperInclusive
            );

        }

        public static int NonzeroSign (float value) {

            int sign;

            if (value < 0f) {

                sign = -1;

            } else {

                sign = 1;

            }

            return sign;

        }

        public static pair2f GetClosestOnLineSegmentToPoint (pair2f lineHead, pair2f lineTail, pair2f point) {

            pair2f closest;

            var lineDiff = lineHead - lineTail;

            if (lineDiff.isNegligible) {

                closest = lineHead;

            } else {

                var t = GetTOfClosestOnLine (lineTail, lineDiff.y, lineDiff.x, point);

                if (t < 0f) {

                    closest = lineTail;

                } else if (t > 1f) {

                    closest = lineHead;

                } else {

                    closest = (lineTail + (lineDiff * t));

                }
            }

            return closest;

        }

        public static pair2f GetClosestOnLineToPoint (pair2f pointOnLine, float slope, pair2f point) {

            var slopeVector = new pair2f (1f, slope);
            return (pointOnLine + (slopeVector * GetTOfClosestOnLine (pointOnLine, slopeVector.y, slopeVector.x, point)));

        }

        private static float GetTOfClosestOnLine (pair2f lineBasePoint, float rise, float run, pair2f point) {

            var pointDiff = point - lineBasePoint;
            return (
                ((rise * pointDiff.y) + (run * pointDiff.x))
                / ((rise * rise) + (run * run))
            );

        }
    }
}
