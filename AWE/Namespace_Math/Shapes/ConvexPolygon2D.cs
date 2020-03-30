using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AWE.CollectionExtensions;

namespace AWE.Math {

    public class ConvexPolygon2D : APolygon2D, IConvexShape2D {

        private bool _isConvex;
        private Bounds2D _bounds;
        private pair2f _center;
        private List<pair2f> _unoffsetVerticies;

        public override ReadOnlyCollection<pair2f> unoffsetVerticies => this._unoffsetVerticies.AsReadOnly ();

        public sealed override bool isConvex {

            get {

                if (!this._isConvex) {

                    this._isConvex = this.ConfirmConvexity (ref this._unoffsetVerticies);

                }

                return this._isConvex;

            }
        }

        public override Bounds2D bounds {

            get {

                if (this._bounds.isValid) {

                    this._bounds = new Bounds2D (this.unoffsetVerticies);

                }

                return this._bounds;

            }
        }

        public override pair2f center {

            get {

                if (this._center.isNan) {

                    this._center = SShapeMath.GetCenter (this.unoffsetVerticies);

                }

                return this._center;

            }
        }

        #region Constructors

        private ConvexPolygon2D () {

            this._unoffsetVerticies = null;
            this._bounds = new Bounds2D (0f, 0f, 0f, 0f);
            this._center = pair2f.nan;
            this._isConvex = false;

        }

        private ConvexPolygon2D (ReadOnlyCollection<pair2f> verticies, bool isConvex, pair2f center) {

            if (verticies.Count < SShapeMath.MINIMUM_VERTEX_COUNT) {

                // TODO - Throw an exception

            }

            var first = verticies[0];
            float right = first.x, top = first.y, left = first.x, bottom = first.y;

            if (isConvex) {

                this._center = center;

                this._unoffsetVerticies = new List<pair2f> ();
                for (int i = 0; i < verticies.Count; i++) {

                    var point = verticies[i];
                    this._unoffsetVerticies.Add (point);

                    if (point.x > right) {

                        right = point.x;

                    }
                    if (point.x > top) {

                        top = point.y;

                    }
                    if (point.x < left) {

                        left = point.x;

                    }
                    if (point.y < bottom) {

                        bottom = point.y;

                    }
                }

                this._bounds = new Bounds2D (right, top, left, bottom);

            } else {

                float xcenter = 0f, ycenter = 0f;

                this._unoffsetVerticies = SShapeMath.GetConvexHull (verticies, (point => {

                    xcenter += point.x;
                    ycenter += point.y;

                    if (point.x > right) {

                        right = point.x;

                    }
                    if (point.x > top) {

                        top = point.y;

                    }
                    if (point.x < left) {

                        left = point.x;

                    }
                    if (point.y < bottom) {

                        bottom = point.y;

                    }

                }));

                this._bounds = new Bounds2D (right, top, left, bottom);
                this._center = new pair2f (xcenter, ycenter);

            }

            this._isConvex = true;

        }

        private ConvexPolygon2D (ReadOnlyCollection<pair2f> verticies, bool isConvex) {

            if (verticies.Count < SShapeMath.MINIMUM_VERTEX_COUNT) {

                // TODO - Throw an exception

            }

            var first = verticies[0];
            float right = first.x, top = first.y, left = first.x, bottom = first.y;

            if (isConvex) {

                this._unoffsetVerticies = new List<pair2f> ();

                this._center = SShapeMath.GetCenter (verticies, (point => {

                    this._unoffsetVerticies.Add (point);

                    if (point.x > right) {

                        right = point.x;

                    }
                    if (point.x > top) {

                        top = point.y;

                    }
                    if (point.x < left) {

                        left = point.x;

                    }
                    if (point.y < bottom) {

                        bottom = point.y;

                    }

                }));

                this._bounds = new Bounds2D (right, top, left, bottom);

            } else {

                float xcenter = 0f, ycenter = 0f;

                this._unoffsetVerticies = SShapeMath.GetConvexHull (verticies, (point => {

                    xcenter += point.x;
                    ycenter += point.y;

                    if (point.x > right) {

                        right = point.x;

                    }
                    if (point.x > top) {

                        top = point.y;

                    }
                    if (point.x < left) {

                        left = point.x;

                    }
                    if (point.y < bottom) {

                        bottom = point.y;

                    }

                }));

                this._bounds = new Bounds2D (right, top, left, bottom);
                this._center = new pair2f (xcenter, ycenter);

            }

            this._isConvex = true;

        }

        internal ConvexPolygon2D (Polygon2DTemplate template) {

            if (!template.polygonType.isConvex()) {

                // TODO - Throw an expection.

            }

            this._unoffsetVerticies = SShapeMath.CreatePolygon2DVerticies (template, out this._center, out this._bounds);
            this._isConvex = true;

        }

        public ConvexPolygon2D (ReadOnlyCollection<pair2f> verticies) : this (verticies, SShapeMath.IsConvex (verticies)) {}

        public ConvexPolygon2D (ConvexPolygon2D polygon) {

            this._unoffsetVerticies = new List<pair2f> (polygon._unoffsetVerticies);
            this._center = polygon._center;
            this._bounds = polygon._bounds;
            this._isConvex = true;

        }

        public ConvexPolygon2D (IShape2D shape) {

            ReadOnlyCollection<pair2f> verticies = null;

            if (shape is IPolygon2D polygon) {

                verticies = polygon.unoffsetVerticies;

            } else {

                verticies = shape.CreateVertexList ().AsReadOnly ();

            }

            if (verticies.Count < SShapeMath.MINIMUM_VERTEX_COUNT) {

                // TODO - Throw an exception

            }

            var first = verticies[0];
            float right = first.x, top = first.y, left = first.x, bottom = first.y;

            if (shape.isConvex) {

                this._unoffsetVerticies = new List<pair2f> ();

                this._center = SShapeMath.GetCenter (verticies, (point => {

                    this._unoffsetVerticies.Add (point);

                    if (point.x > right) {

                        right = point.x;

                    }
                    if (point.x > top) {

                        top = point.y;

                    }
                    if (point.x < left) {

                        left = point.x;

                    }
                    if (point.y < bottom) {

                        bottom = point.y;

                    }

                }));

                this._bounds = new Bounds2D (right, top, left, bottom);
                this._isConvex = false;

            } else {

                float xcenter = 0f, ycenter = 0f;

                this._unoffsetVerticies = SShapeMath.GetConvexHull (verticies, (point => {

                    xcenter += point.x;
                    ycenter += point.y;

                    if (point.x > right) {

                        right = point.x;

                    }
                    if (point.x > top) {

                        top = point.y;

                    }
                    if (point.x < left) {

                        left = point.x;

                    }
                    if (point.y < bottom) {

                        bottom = point.y;

                    }

                }));

                this._bounds = new Bounds2D (right, top, left, bottom);
                this._center = new pair2f (xcenter, ycenter);
                this._isConvex = true;

            }
        }

        #endregion

        #region Constructor Helpers

        private void PopulateBoundsAndCenter (ReadOnlyCollection<pair2f> verticies) {

            var point = verticies[0];
            float right = point.x, top = point.y, left = point.x, bottom = point.y;
            float xcenter = 0f, ycenter = 0f;

            for (int i = 1; i < verticies.Count; i++) {

                point = verticies[i];

                xcenter += point.x;
                ycenter += point.y;

                if (point.x > right) {

                    right = point.x;

                }
                if (point.x > top) {

                    top = point.y;

                }
                if (point.x < left) {

                    left = point.x;

                }
                if (point.y < bottom) {

                    bottom = point.y;

                }

            };

            this._bounds = new Bounds2D (right, top, left, bottom);
            this._center = new pair2f (xcenter, ycenter);

        }

        private bool ConfirmConvexity (ref List<pair2f> verticies) {

            var isConvex = SShapeMath.IsConvex (verticies);

            if (!isConvex) {

                verticies = SShapeMath.GetConvexHull (verticies.AsReadOnly ());
                isConvex = (verticies?.Count > 0);

            }

            return isConvex;

        }

        #endregion

        public override bool IsContainingPoint (pair2f point) => this.IsContainingPoint (point, true);

        public bool IsContainingPoint (pair2f point, bool allowBoundary, float tolerance = SShapeMath.MINIMUM_ORIENTATION)
            => SShapeMath.IsPointInConvexShape (point, this.CreateVertexIterator (), allowBoundary, tolerance);

        public Pair<pair2f> FindSegmentIntersections (Line2DSegment segment, int start = 0, int end = -1, bool returnOnFirst = false)
            => this.FindSegmentIntersections (segment, out _, start, end, returnOnFirst);

        public Pair<pair2f> FindSegmentIntersections (
            Line2DSegment segment,
            out bool isParallel,
            int start = 0,
            int end = -1,
            bool returnOnFirst = false
        ) {

            var intersection1 = pair2f.nan;
            var intersection2 = pair2f.nan;

            // This value normally would be set below, but we assume false here to make sure isParallel is written to at least once.
            isParallel = false;

            if (end < 0) {

                end = start;

            }

            var edgeIterator = this.CreateEdgeIterator (start);
            var distance = Single.PositiveInfinity;
            var skipNextIntersection = false;

            do {

                var current = edgeIterator.current;

                if (segment.IsIntersecting (current, disallowParallel: false)) {

                    if (intersection1.isNan) {

                        intersection1 = segment.FindIntersection (current);
                        isParallel = intersection1.isNan;

                        // If the intersection is parallel...
                        if (isParallel) {

                            // Resolve it, then break because parallel intersection means there are no other intersections.
                            (intersection1, intersection2) = current.FindParallelIntersections (segment, out _);

                            break;

                        }

                        if (returnOnFirst) {

                            break;

                        } else {

                            // If the segment intersected on a vertex and is not parallel...
                            if (intersection1 == current.head) {

                                // Then skip the next intersection, because it will intersect on the same vertex.
                                skipNextIntersection = true;

                            } else if ((start == end) && (intersection1 == current.tail)) {

                                // If the intersection is on current's tail, then this must be the first segment on the polygon.
                                // Skip the last segment on the polygon for the same reason as above.
                                end = this.GetPreviousIndex (end);

                            }

                            distance = (intersection1 - segment.tail).magnitude;

                        }

                    } else {

                        var intersection = segment.FindIntersection (current);

                        // Note that I intentionally do not use isParallel here, because I don't want to change the value of the out parameter.
                        if (intersection.isNan) {

                            /*
                             * This must be a parallel intersection.
                             *
                             * We can discard the first element returned here because
                             * it must have already been found as current.head last iteration.
                             */

                            (_, intersection2) = current.FindParallelIntersections (segment, out _);

                            break;

                        } else {

                            if (skipNextIntersection) {

                                // We've skipped the next intersection. Reset the flag so it doesn't happen again.
                                skipNextIntersection = false;

                            } else {

                                if ((intersection - segment.tail).magnitude < distance) {

                                    intersection2 = intersection1;
                                    intersection1 = intersection;

                                } else {

                                    intersection2 = intersection;

                                }

                                break;

                            }
                        }
                    }
                }

                edgeIterator++;

            } while (edgeIterator.currentIndex != end);

            return new Pair<pair2f> (intersection1, intersection2);

        }

        public Pair<Polygon2DIntersection> FindSegmentIntersections (Polygon2DEdge otherEdge, int start = 0, int end = -1, bool returnOnFirst = false)
            => this.FindSegmentIntersections (otherEdge, out _, start, end, returnOnFirst);

        public Pair<Polygon2DIntersection> FindSegmentIntersections (
            Polygon2DEdge otherEdge,
            out bool isParallel,
            int start = 0,
            int end = -1,
            bool returnOnFirst = false
        ) {

            Polygon2DIntersection intersection1 = null;
            Polygon2DIntersection intersection2 = null;

            // This value normally would be set below, but we assume false here to make sure isParallel is written to at least once.
            isParallel = false;

            if (end < 0) {

                end = start;

            }

            var edgeIterator = this.CreateEdgeIterator (start);
            var distance = Single.PositiveInfinity;
            var skipNextIntersection = false;

            do {

                var current = edgeIterator.current;

                if (otherEdge.IsIntersecting (current, disallowParallel: false)) {

                    var point = otherEdge.FindIntersection (current);

                    if (intersection1 == null) {

                        // If the intersection is parallel...
                        isParallel = point.isNan;

                        if (isParallel) {

                            // Resolve it, then break because parallel intersection means there are no other intersections.
                            (var point1, var point2) = current.FindParallelIntersections (otherEdge);

                            intersection1 = new Polygon2DIntersection (
                                point1,
                                current,
                                otherEdge
                            );
                            intersection2 = new Polygon2DIntersection (
                                point2,
                                current,
                                otherEdge
                            );

                            break;

                        } else {

                            intersection1 = new Polygon2DIntersection (
                                point,
                                current,
                                otherEdge
                            );

                        }

                        if (returnOnFirst) {

                            break;

                        } else {

                            // If the segment intersected on a vertex and is not parallel...
                            if (point == current.head) {

                                // Then skip the next intersection, because it will intersect on the same vertex.
                                skipNextIntersection = true;

                            } else if ((start == end) && (point == current.tail)) {

                                // If the intersection is on current's tail, then this must be the first segment on the polygon.
                                // Skip the last segment on the polygon for the same reason as above.
                                end = this.GetPreviousIndex (end);

                            }

                            distance = (point - otherEdge.tail).magnitude;

                        }

                    } else {

                        // Note that I intentionally do not use isParallel here, because I don't want to change the value of the out parameter.
                        if (point.isNan) {

                            /*
                             * This must be a parallel intersection.
                             *
                             * We can discard the first element returned here because
                             * it must have already been found as current.head last iteration.
                             */

                            (_, var point2) = current.FindParallelIntersections (otherEdge);

                            intersection2 = new Polygon2DIntersection (
                                point2,
                                current,
                                otherEdge
                            );

                            break;

                        } else {

                            if (skipNextIntersection) {

                                // We've skipped the next intersection. Reset the flag so it doesn't happen again.
                                skipNextIntersection = false;

                            } else {

                                intersection2 = new Polygon2DIntersection (
                                    point,
                                    current,
                                    otherEdge
                                );

                                if ((point - otherEdge.tail).magnitude < distance) {

                                    var intersection = intersection1;
                                    intersection1 = intersection2;
                                    intersection2 = intersection;

                                }

                                break;

                            }
                        }
                    }
                }

                edgeIterator++;

            } while (edgeIterator.currentIndex != end);

            return new Pair<Polygon2DIntersection> (intersection1, intersection2);

        }

        public ConvexPolygon2D FindOverlap (ConvexPolygon2D other, bool qualifyImmediately = false) {

            var overlap = new List<pair2f> ();
            var intersections = new List<Pair<Polygon2DIntersection>> ();
            var otherIndex = 0;

            for (var edges = this.CreateEdgeIterator (); edges.cycles == 0; edges++) {

                var incidents = other.FindSegmentIntersections (
                    edges.current,
                    start: otherIndex
                );

                if (incidents.first != null) {

                    otherIndex = incidents.first.incidentEdge.indicies.a;

                    if (incidents.first.point != edges.current.tail) {

                        intersections.Add (incidents);

                    } else if (incidents.second != null) {

                        intersections.Add (new Pair<Polygon2DIntersection> (incidents.second, null));

                    }
                }
            }

            if (intersections.Count > 0) {

                for (var iterator = intersections.AsReadOnly ().GetCyclicIterator (); iterator.cycles == 0; iterator++) {

                    var intersection = iterator.current;
                    overlap.Add (intersection.first.point);

                    if (intersection.second != null) {

                        overlap.Add (intersection.second.point);

                    }

                    var furthest = (intersection.second ?? intersection.first);
                    var next = iterator.next.first;

                    var notSkipped = true;

                    if (!this.AddRangeOfPointsToList (
                        overlap,
                        ((Polygon2DEdge)furthest.incidentVector).indicies.b,
                        ((Polygon2DEdge)next.incidentVector).indicies.b,
                        SkipCondition: p => {

                            notSkipped &= (p == furthest.point);
                            return notSkipped;

                        },
                        StartCondition: p => other.IsContainingPoint (p, false)
                    )) {

                        notSkipped = true;

                        other.AddRangeOfPointsToList (
                            overlap,
                            furthest.incidentEdge.indicies.b,
                            next.incidentEdge.indicies.b,
                            SkipCondition: p => {

                                notSkipped &= (p == furthest.point);
                                return notSkipped;

                            },
                            StartCondition: p => this.IsContainingPoint (p, false)
                        );

                    }
                }

            } else if (this.IsContainingPoint (other[0])) {

                overlap = new List<pair2f> (other);

            } else if (other.IsContainingPoint (this[0])) {

                overlap = new List<pair2f> (this);

            }

            ConvexPolygon2D polygon = null;

            if (overlap.Count >= SShapeMath.MINIMUM_VERTEX_COUNT) {

                polygon = new ConvexPolygon2D {
                    _unoffsetVerticies = overlap,
                    _isConvex = true
                };

                if (qualifyImmediately) {

                    polygon.PopulateBoundsAndCenter (overlap.AsReadOnly ());

                }
            }

            return polygon;

        }

        protected override APolygon2D _CreateOffset (pair2f offset, angle rotation, pair2f center = default) => this.CreateOffset (offset, rotation, center);
        new public virtual ConvexPolygon2D CreateOffset (pair2f offset, angle rotation, pair2f center = default) {

            if (center.isZero) {

                center = this.center;

            }

            var points = new List<pair2f> ();

            for (int i = 0; i < this.count; i++) {

                var temp = (this[i] - center);
                points.Add ((temp + rotation) + offset);

            }

            return new ConvexPolygon2D (points.AsReadOnly (), true, center);

        }

        protected override APolygon2D _CreateOffset (angle offset, pair2f center = default) => this.CreateOffset (offset, center);
        new public virtual ConvexPolygon2D CreateOffset (angle offset, pair2f center = default) {

            if (center.isZero) {

                center = this.center;

            }

            var points = new List<pair2f> ();

            for (int i = 0; i < this.count; i++) {

                var temp = (this[i] - center);
                points.Add (temp + offset);

            }

            return new ConvexPolygon2D (points.AsReadOnly (), true, center);

        }

        protected override APolygon2D _CreateOffset (pair2f offset) => this.CreateOffset (offset);
        IConvexShape2D IConvexShape2D.CreateOffset(pair2f offset) => this.CreateOffset (offset);
        new public virtual ConvexPolygon2D CreateOffset (pair2f offset) => new ConvexPolygon2DOffset (this, offset);

        private class ConvexPolygon2DOffset : ConvexPolygon2D {

            private readonly pair2f offset;

            public override pair2f this[int index] => (this._unoffsetVerticies[index] + offset);

            internal ConvexPolygon2DOffset (ConvexPolygon2D source, pair2f offset) {

                this.offset = offset;

                if (source is ConvexPolygon2DOffset otherOffset) {

                    this.offset += otherOffset.offset;

                }

                this._unoffsetVerticies = source._unoffsetVerticies;
                this._bounds = (source._bounds + offset);
                this._center = (source._center + offset);
                this._isConvex = source._isConvex;

            }
        }
    }
}