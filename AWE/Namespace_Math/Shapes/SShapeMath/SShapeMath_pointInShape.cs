using System;
using System.Collections.ObjectModel;
using AWE;

namespace AWE.Math {

    public static partial class SShapeMath {

        public static bool IsPointInConvexShape (pair2f point, params pair2f[] shape)
            => IsPointInConvexShape (point, Array.AsReadOnly (shape), false);

        public static bool IsPointInConvexShape (pair2f point, bool allowBoundary, params pair2f[] shape)
            => IsPointInConvexShape (point, Array.AsReadOnly (shape), allowBoundary);

        public static bool IsPointInConvexShape (pair2f point, bool allowBoundary, float tolerance, params pair2f[] shape)
            => IsPointInConvexShape (point, Array.AsReadOnly (shape), allowBoundary, tolerance);

        public static bool IsPointInConvexShape (
            pair2f point,
            ReadOnlyCollection<pair2f> shape,
            bool allowBoundary = true,
            float tolerance = MINIMUM_ORIENTATION
        ) {

            var isPointInShape = false;

            int length = shape.Count;
            var isShapeValid = (length >= MINIMUM_VERTEX_COUNT);
            var previousVertex = shape[length - 1];

            if (isShapeValid) {

                int orientation;
                int? previousOrientation = null;

                for (int i = 0; i < length; i++) {

                    orientation = GetOrientation (
                        point,
                        previousVertex,
                        shape[i],
                        allowBoundary,
                        tolerance
                    );

                    if (orientation == 0) {

                        isPointInShape = allowBoundary;
                        break;

                    }

                    if (previousOrientation.HasValue) {

                        isPointInShape = (orientation == previousOrientation.Value);

                        if (!isPointInShape) {

                            break;

                        }
                    }

                    previousOrientation = orientation;
                    previousVertex = shape[i];

                }
            }

            return (isShapeValid && isPointInShape);

        }

        public static bool IsPointInConvexShape (
            pair2f point,
            LinkedList<pair2f> shape,
            bool allowBoundary = true,
            float tolerance = MINIMUM_ORIENTATION
        ) {

            var isPointInShape = false;
            var isShapeValid = !shape.CopyFrontPosition ().Equals (shape.CopyEndPosition ());

            if (isShapeValid) {

                int orientation;
                int? previousOrientation = null;

                for (var iterator = shape.CopyFrontPosition (); iterator.hasValue; iterator++) {

                    orientation = GetOrientation (
                        point,
                        iterator.value,
                        shape.GetNextOrFront (iterator),
                        allowBoundary,
                        tolerance
                    );

                    if (orientation == 0) {

                        isPointInShape = allowBoundary;
                        break;

                    }

                    if (previousOrientation.HasValue) {

                        isPointInShape = (orientation == previousOrientation.Value);

                        if (!isPointInShape) {

                            break;

                        }
                    }

                    previousOrientation = orientation;

                }
            }

            return (isShapeValid && isPointInShape);

        }

        public static bool IsPointInConvexShape (
            pair2f point,
            ACyclicIndexIterator<pair2f> shape,
            bool allowBoundary = true,
            float tolerance = MINIMUM_ORIENTATION
        ) {

            var isPointInShape = false;
            var isShapeValid = !shape.current.Equals (shape.next);

            if (isShapeValid) {

                int orientation;
                int? previousOrientation = null;
                var iterator = shape.Copy (startingIndex: 0);

                while (iterator.MoveNext ()) {

                    orientation = GetOrientation (
                        point,
                        iterator.current,
                        iterator.next,
                        allowBoundary,
                        tolerance
                    );

                    if (orientation == 0) {

                        isPointInShape = allowBoundary;
                        break;

                    }

                    if (previousOrientation.HasValue) {

                        isPointInShape = (orientation == previousOrientation.Value);

                        if (!isPointInShape) {

                            break;

                        }
                    }

                    previousOrientation = orientation;

                }
            }

            return (isShapeValid && isPointInShape);

        }
    }
}
