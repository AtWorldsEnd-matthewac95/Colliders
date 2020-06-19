using AWE.Math.FloatExtensions;

namespace AWE.Math {

    public static partial class SFloatMath {

        public const float TAU = (2f * (float)System.Math.PI);
        public const float DEGREE_MAX_VALUE = 360f;
        public const float RADIAN_MAX_VALUE = TAU;
        public const float DEGREE_TO_RADIAN = (RADIAN_MAX_VALUE / DEGREE_MAX_VALUE);
        public const float RADIAN_TO_DEGREE = (DEGREE_MAX_VALUE / RADIAN_MAX_VALUE);

        public static float Sine (angle theta) {

            float sine;

            if (theta.mode == EAngleMode.Auto) {

                sine = float.NaN;

            } else {

                sine = (float)System.Math.Sin (theta.GetValueUsing (EAngleMode.Radian));

            }

            return sine;

        }

        public static float Cosine (angle theta) {

            float cosine;

            if (theta.mode == EAngleMode.Auto) {

                cosine = float.NaN;

            } else {

                cosine = (float)System.Math.Cos (theta.GetValueUsing (EAngleMode.Radian));

            }

            return cosine;

        }

        public static float Cosine (pair2f vector, float minimumMagnitude = MINIMUM_DIFFERENCE) {

            var magnitude = vector.magnitude;

            return (
                magnitude.IsNegligible (minimumMagnitude)
                ? 1f
                : (vector.x / vector.magnitude)
            );

        }

        public static float Tangent (angle theta) {

            float tangent = 0f;

            if (theta.mode == EAngleMode.Auto) {

                tangent = float.NaN;

            } else {

                tangent = (float)System.Math.Tan (theta.GetValueUsing (EAngleMode.Radian));

            }

            return tangent;

        }

        public static angle Arctangent (float tangent) => new angle (
            (float)System.Math.Atan (tangent),
            EAngleMode.Radian
        );

        public static angle Arctangent (float rise, float run) => new angle (
            (float)System.Math.Atan2 (rise, run),
            EAngleMode.Radian
        );

        public static angle Arctangent (pair2f pair) => Arctangent (pair.y, pair.x);

        public static float TrimToRange (
            float value,
            EAngleMode angleMode,
            bool lowerInclusive = true,
            bool upperInclusive = false
        ) {

            float trimmedValue = value;

            switch (angleMode) {

            case EAngleMode.Degree:

                trimmedValue = TrimToRange (
                    value,
                    0f,
                    DEGREE_MAX_VALUE,
                    lowerInclusive,
                    upperInclusive
                );

            break;

            case EAngleMode.Radian:

                trimmedValue = TrimToRange (
                    value,
                    0f,
                    RADIAN_MAX_VALUE,
                    lowerInclusive,
                    upperInclusive
                );

            break;

            case EAngleMode.Percent:

                trimmedValue = TrimToRange (
                    value,
                    0f,
                    1f,
                    lowerInclusive,
                    upperInclusive
                );

            break;

            }

            return trimmedValue;

        }
    }
}
