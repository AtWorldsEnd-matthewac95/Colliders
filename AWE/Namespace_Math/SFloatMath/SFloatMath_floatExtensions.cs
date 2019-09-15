namespace AWE.Math {

    public static partial class SFloatMath {

        public static bool IsFloatInfinite (float value) {

            return (
                value >= float.PositiveInfinity
                || value <= float.NegativeInfinity
            );

        }

        public static bool IsFloatNegligible (
            float value,
            float minimumAbsoluteValue = MINIMUM_DIFFERENCE
        ) {

            float abs = System.Math.Abs (value);
            return (abs < minimumAbsoluteValue);

        }

        public static float GetSquareRoot (float value) {

            double dval = (double)value;

            return (
                (float)System.Math.Sqrt (dval)
            );

        }

        public static int GetSign (
            float value,
            bool isUsingMinimumValue = false,
            float minimumAbsoluteValue = MINIMUM_DIFFERENCE
        ) {

            int sign;

            if (isUsingMinimumValue) {

                if (IsFloatNegligible (value, minimumAbsoluteValue)) {

                    sign = 0;

                } else if (value > 0f) {

                    sign = 1;

                } else {

                    sign = -1;

                }

            } else {

                if (value > 0f) {

                    sign = 1;

                } else if (value < 0f) {

                    sign = -1;

                } else {

                    sign = 0;

                }
            }

            return sign;

        }
    }
}
