using System;
using AWE.Math;

namespace AWE.Math.FloatExtensions {

    internal static class SFloatExtensions {

        internal static bool IsInfinite (this float value) => SFloatMath.IsFloatInfinite (value);

        internal static bool IsNegligible (
            this float value,
            float minimumAbsoluteValue = SFloatMath.MINIMUM_DIFFERENCE
        ) {

            return SFloatMath.IsFloatNegligible (value, minimumAbsoluteValue);

        }

        internal static bool IsNaN (this float value) => Single.IsNaN (value);

        internal static float abs (this float value) => System.Math.Abs (value);

        internal static float sqrt (this float value) => SFloatMath.GetSquareRoot (value);

        internal static int sign (
            this float value,
            bool isUsingMinimumValue = false,
            float minimumAbsoluteValue = SFloatMath.MINIMUM_DIFFERENCE
        ) {

            return SFloatMath.GetSign (
                value,
                isUsingMinimumValue,
                minimumAbsoluteValue
            );

        }
    }
}
