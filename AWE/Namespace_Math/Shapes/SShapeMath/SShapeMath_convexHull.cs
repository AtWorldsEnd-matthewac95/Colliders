using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AWE.CollectionExtensions;

namespace AWE.Math {

    public static partial class SShapeMath {

        public static List<pair2f> GetConvexHull (ReadOnlyCollection<pair2f> shape) => GetConvexHull (shape, pair2f.one, (point => {}));

        public static List<pair2f> GetConvexHull (ReadOnlyCollection<pair2f> shape, pair2f normal) => GetConvexHull (shape, normal, (point => {}));

        public static List<pair2f> GetConvexHull (ReadOnlyCollection<pair2f> shape, Action<pair2f> forEach) => GetConvexHull (shape, pair2f.one, forEach);

        public static List<pair2f> GetConvexHull (ReadOnlyCollection<pair2f> shape, pair2f normal, Action<pair2f> forEach) {

            LinkedList<pair2f> hull = null;

            if (shape.Count >= MINIMUM_VERTEX_COUNT) {

                hull = new LinkedList<pair2f> ();
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

                forEach (head);
                forEach (tail);

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
                    forEach (perpendicular);

                }

                if (!inverse.isNan) {

                    hull.AddAsNext (inverse, hull.end);
                    forEach (inverse);

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
                        forEach (greatest.position.value);
                        list.RemoveByCondition (removeByCondition, positives.AsReadOnly ());

                        iterator--;

                    }
                }
            }

            return hull?.AsList ();

        }
    }
}