using AWE.Math.FloatExtensions;

namespace AWE.Math {

    public static partial class SShapeMath {

        public const float MINIMUM_ORIENTATION = 0.001f;
        public const int ORIENT_CLOCK = 1;
        public const int ORIENT_COUNTER = -1;
        public const int ORIENT_LINEAR = 0;

        /*
         A positive number indicates a clockwise orientation.
         A negative number indicates a counter-clockwise orientation.
         Zero indicates the points are colinear.
         */
        public static float GetOrientation (pair2f start, pair2f mid, pair2f end) {

            var endSlope = (end - mid);
            var startSlope = (mid - start);

            return SFloatMath.GetDeterminant (endSlope, startSlope);

        }

        /*
         ORIENT_CLOCK indicates a clockwise orientation.
         ORIENT_COUNTER indicates a counter-clockwise orientation.
         ORIENT_LINEAR the points are colinear.
         */
        public static int GetOrientation (
            pair2f start,
            pair2f mid,
            pair2f end,
            bool isUsingMinimumValue,
            float minimumAbsoluteValue = MINIMUM_ORIENTATION
        ) {

            return GetOrientation (start, mid, end).sign (
                isUsingMinimumValue,
                minimumAbsoluteValue
            );

        }
    }
}
