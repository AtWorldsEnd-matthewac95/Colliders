using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AWE.Math {

    public static partial class SShapeMath {

        public const int MINIMUM_VERTEX_COUNT = 3;

        public static pair2f GetCenter (ReadOnlyCollection<pair2f> shape) => GetCenter (shape, (point => {}));

        public static pair2f GetCenter (ReadOnlyCollection<pair2f> shape, Action<pair2f> forEach) {

            float xcenter = 0f, ycenter = 0f;

            for (int i = 0; i < shape.Count; i++) {

                xcenter += shape[i].x;
                ycenter += shape[i].y;

                forEach (shape[i]);

            }

            return new pair2f (
                (xcenter / shape.Count),
                (ycenter / shape.Count)
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

        public static pair2f GetFurthestPointInDirection (pair2f[] points, pair2f direction) {

            var furthest = pair2f.origin;
            var distance = float.NaN;

            for (int i = 0; i < points.Length; i++) {

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

        public static pair2f GetFurthestPointInDirection (List<pair2f> points, pair2f direction) {

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

        public static pair2f GetFurthestPointInDirection (IEnumerable<pair2f> points, pair2f direction) {

            var furthest = pair2f.origin;
            var distance = Single.NaN;

            foreach (var point in points) {

                var current = (
                    (point.x * direction.x)
                    + (point.y * direction.y)
                );

                if (!(current <= distance)) {

                    furthest = point;
                    distance = current;

                }
            }

            return furthest;

        }
    }
}