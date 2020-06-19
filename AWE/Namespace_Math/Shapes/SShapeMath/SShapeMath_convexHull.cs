using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AWE.CollectionExtensions;

namespace AWE.Math {

    public static partial class SShapeMath {

        public static FloatRange FindMinimalAndMaximalRadius (ReadOnlyCollection<pair2f> shape, pair2f center) {

            var minimalRadius = Single.PositiveInfinity;
            var maximalRadius = 0f;
            var iterator = shape.GetCyclicIterator ();

            if (iterator.count < MINIMUM_VERTEX_COUNT) {

                minimalRadius = 0f;

                // If our current shape is a line segment...
                if (iterator.count == 2) {

                    // ...then the maximal radius is equal to half the segment's length.
                    maximalRadius = ((iterator.next - iterator.current).magnitude / 2f);

                }

            } else {

                for (iterator.ResetCycles (); iterator.cycles < 1; iterator++) {

                    var diff = (center - SFloatMath.GetClosestOnLineSegmentToPoint (iterator.next, iterator.current, center));
                    minimalRadius = System.Math.Min (
                        minimalRadius,
                        ((diff.x * diff.x) + (diff.y * diff.y))
                    );

                    diff = (center - iterator.current);
                    maximalRadius = System.Math.Max (
                        maximalRadius,
                        ((diff.x * diff.x) + (diff.y * diff.y))
                    );

                }

                minimalRadius = SFloatMath.GetSquareRoot (minimalRadius);
                maximalRadius = SFloatMath.GetSquareRoot (maximalRadius);

            }

            return new FloatRange (minimalRadius, maximalRadius);

        }

        public static List<pair2f> GetConvexHull (ReadOnlyCollection<pair2f> shape) => GetConvexHull (shape, pair2f.one, new CenterBounds2DBuilder ());

        public static List<pair2f> GetConvexHull (ReadOnlyCollection<pair2f> shape, pair2f normal) => GetConvexHull (shape, normal, new CenterBounds2DBuilder ());

        public static List<pair2f> GetConvexHull (ReadOnlyCollection<pair2f> shape, CenterBounds2DBuilder centerBoundsBuilder)
            => GetConvexHull (shape, pair2f.one, centerBoundsBuilder);

        public static List<pair2f> GetConvexHull (ReadOnlyCollection<pair2f> shape, pair2f normal, CenterBounds2DBuilder centerBoundsBuilder) {

            if (shape.Count < MINIMUM_VERTEX_COUNT) {

                return null;

            }

            var hull = new LinkedList<pair2f> ();
            var list = new LinkedList<pair2f> (shape[0]);
            var current = list.CopyFrontPosition ();

            var dot = SFloatMath.GetDotProduct (current.value, normal);
            var greatest = (dot, position: current.Copy ());
            var least = (dot, position: current.Copy ());

            for (int i = 1; i < shape.Count; i++) {

                current = list.AddToEnd (shape[i]);
                dot = SFloatMath.GetDotProduct (current.value, normal);

                if (dot > greatest.dot) {

                    greatest.dot = dot;
                    greatest.position = current.Copy ();

                } else if (dot < least.dot) {

                    least.dot = dot;
                    least.position = current.Copy ();

                }
            }

            var head = hull.AddToEnd (list.Remove (greatest.position)).value;
            var tail = hull.AddToEnd (list.Remove (least.position)).value;

            centerBoundsBuilder.Add (head);
            centerBoundsBuilder.Add (tail);

            normal = GetNormalComponents (tail, head);

            var perpendicular = list.RemoveExtrema (
                (point => SFloatMath.GetDotProduct (point, normal)),
                out var inverse,
                maximaThreshold: 0f,
                minimaThreshold: 0f,
                nullval: pair2f.nan
            );

            if (!perpendicular.isNan) {

                hull.AddAsNext (perpendicular, hull.front);
                centerBoundsBuilder.Add (perpendicular);

            }

            if (!inverse.isNan) {

                hull.AddAsNext (inverse, hull.end);
                centerBoundsBuilder.Add (inverse);

            }

            Func<pair2f, bool> removeByCondition = (point => IsPointInConvexShape (point, hull));
            list.RemoveByCondition (removeByCondition);

            var positives = new List<LinkedListPosition<pair2f>> ();

            for (var iterator = hull.CopyFrontPosition (); (!list.isEmpty && iterator.hasValue); iterator++) {

                positives.Clear ();
                normal = GetNormalComponents (hull.GetNextOrFront (iterator), iterator.value);
                greatest.dot = 0f;
                greatest.position = null;

                for (current = list.CopyFrontPosition (); current.hasValue; current++) {

                    dot = SFloatMath.GetDotProduct ((current.value - iterator.value), normal);

                    if (dot > SFloatMath.MINIMUM_DIFFERENCE) {

                        var position = current.Copy ();
                        positives.Add (position);

                        if (dot > greatest.dot) {

                            greatest.dot = dot;
                            greatest.position = position;

                        }
                    }
                }

                if (greatest.position != null) {

                    hull.AddAsNext (greatest.position.value, iterator);
                    centerBoundsBuilder.Add (greatest.position.value);
                    list.RemoveByCondition (removeByCondition, positives.AsReadOnly ());

                    iterator--;

                }
            }

            return hull?.AsList ();

        }
    }
}