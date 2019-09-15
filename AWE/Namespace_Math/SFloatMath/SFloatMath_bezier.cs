using System.Collections.Generic;

namespace AWE.Math {

    public static partial class SFloatMath {

        public static pair2f GetBezierPoint (float interpolation, params pair2f[] curve) {

            var result = pair2f.origin;
            var length = curve.Length;

            if (length > 0) {

                while (length > 1) {

                    for (int i = 1; i < length; i++) {

                        var previous = curve[i - 1];
                        var current = curve[i];
                        var diff = (current - previous);
                        curve[i - 1] = (previous + (diff * interpolation));

                    }

                    length--;

                }

                result = curve[0];

            }

            return result;

        }

        public static pair2f GetBezierPoint (float interpolation, List<pair2f> curve) {

            var result = pair2f.origin;
            var length = curve.Count;

            if (length > 0) {

                while (length > 1) {

                    for (int i = 1; i < length; i++) {

                        var previous = curve[i - 1];
                        var current = curve[i];
                        var diff = (current - previous);
                        curve[i - 1] = (previous + (diff * interpolation));

                    }

                    length--;

                }

                result = curve[0];

            }

            return result;

        }
    }
}