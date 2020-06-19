using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AWE.Math {

    public static partial class SShapeMath {

        public const int MINIMUM_VERTEX_COUNT = 3;

        public static pair2f GetCenter (ReadOnlyCollection<pair2f> shape) {

            var count = shape.Count;
            float xcenter = 0f, ycenter = 0f;

            for (int i = 0; i < count; i++) {

                xcenter += shape[i].x;
                ycenter += shape[i].y;

            }

            return new pair2f (
                (xcenter / count),
                (ycenter / count)
            );

        }

        public static bool IsConvex (List<pair2f> polygon) => IsConvex (polygon.AsReadOnly ());

        public static bool IsConvex (ReadOnlyCollection<pair2f> polygon) {

            var isConvex = false;

            if (polygon.Count >= MINIMUM_VERTEX_COUNT) {

                int i, orientation = GetOrientation (
                    polygon[0],
                    polygon[1],
                    polygon[2],
                    isUsingMinimumValue: true
                );

                isConvex = (orientation != 0);

                if (isConvex) {

                    for (i = 3; isConvex && (i < polygon.Count); i++) {

                        isConvex = (orientation == GetOrientation (
                            polygon[i - 2],
                            polygon[i - 1],
                            polygon[i],
                            isUsingMinimumValue: true
                        ));

                    }

                    if (isConvex) {

                        isConvex = (orientation == GetOrientation (
                            polygon[i - 2],
                            polygon[i - 1],
                            polygon[0],
                            isUsingMinimumValue: true
                        ));

                        if (isConvex) {

                            isConvex = (orientation == GetOrientation (
                                polygon[i - 1],
                                polygon[0],
                                polygon[1],
                                isUsingMinimumValue: true
                            ));

                        }
                    }
                }

            }

            return isConvex;

        }

        public static pair2f GetFurthestPointInDirection (ReadOnlyCollection<pair2f> points, pair2f direction) {

            var furthest = pair2f.origin;
            var distance = float.NaN;

            for (int i = 0; i < points.Count; i++) {

                var current = (
                    (points[i].x * direction.x)
                    + (points[i].y * direction.y)
                );

                if (!(current <= distance)) {

                    furthest = points[i];
                    distance = current;

                }
            }

            return furthest;

        }

        public static FloatRange GetExtremaDistancesFromPoint (ReadOnlyCollection<pair2f> shape, pair2f point) {

            var furthest = 0f;
            var closest = Single.PositiveInfinity;
            var previousIndex = (shape.Count - 1);

            for (int index = 0; index < shape.Count; index++) {

                var furthestDiff = (point - shape[index]);
                var closestDiff = (point - SFloatMath.GetClosestOnLineSegmentToPoint (
                    shape[previousIndex],
                    shape[index],
                    point
                ));

                furthest = System.Math.Max (furthest, ((furthestDiff.x * furthestDiff.x) * (furthestDiff.y * furthestDiff.y)));
                closest = System.Math.Min (closest, ((closestDiff.x * closestDiff.x) * (closestDiff.y * closestDiff.y)));

                previousIndex = index;

            }

            return new FloatRange (((float)System.Math.Sqrt(closest)), ((float)System.Math.Sqrt(furthest)));

        }
    }
}