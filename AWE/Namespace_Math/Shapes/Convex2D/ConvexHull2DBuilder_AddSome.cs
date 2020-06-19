using AWE.CollectionExtensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AWE.Math {

    public partial class ConvexHull2DBuilder {

        private const int MINIMUM_VERTEX_COUNT_FOR_BULK = 9;

        public ConvexHull2DBuilder (ACyclicIndexIterator<pair2f> points) : this (ConvexHull2DBuilderOptions.clockwise, -SFloatMath.MINIMUM_DIFFERENCE) => this.AddAll (points);
        public ConvexHull2DBuilder (ReadOnlyCollection<pair2f> points) : this (points.GetCyclicIterator ()) {}
        public ConvexHull2DBuilder (params pair2f[] points) : this (Array.AsReadOnly (points)) {}
        public ConvexHull2DBuilder (APolygon2D polygon) : this (ConvexHull2DBuilderOptions.clockwise, -SFloatMath.MINIMUM_DIFFERENCE) => this.AddAll (polygon);
        public ConvexHull2DBuilder (IShape2D shape) : this (ConvexHull2DBuilderOptions.clockwise, -SFloatMath.MINIMUM_DIFFERENCE) => this.AddAll (shape);

        #region AddAll

        public ConvexHull2DBuilder AddAll (params pair2f[] points) => this.AddAll (Array.AsReadOnly (points));

        public ConvexHull2DBuilder AddAll (IEnumerable<pair2f> points) {

            if (points is APolygon2D polygon) {

                return this.AddAll (polygon);

            } else if (points is IShape2D shape) {

                return this.AddAll (shape);

            } else if (points is ReadOnlyCollection<pair2f> collection) {

                return this.AddAll (collection);

            } else if (points is List<pair2f> list) {

                return this.AddAll (list.AsReadOnly ());

            } else {

                foreach (var point in points) {

                    this.Add (point);

                }

                return this;

            }
        }

        public ConvexHull2DBuilder AddAll (IShape2D shape) {

            if (shape is APolygon2D apolygon) {

                return this.AddAll (apolygon);

            } else if (shape is IPolygon2D ipolygon) {

                return this.AddSome(ipolygon.CreateVertexIterator (), 0, ipolygon.count);

            } else {

                return this.AddAll (shape.CreateVertexList ().AsReadOnly ());

            }
        }

        public ConvexHull2DBuilder AddAll (APolygon2D polygon) => this.AddSome (polygon.CreateVertexIterator (), 0, polygon.count);

        public ConvexHull2DBuilder AddAll (ReadOnlyCollection<pair2f> points) => this.AddSome (points.GetCyclicIterator (), 0, points.Count);

        public ConvexHull2DBuilder AddAll (ACyclicIndexIterator<pair2f> points) => this.AddSome (points, 0, points.count);

        #endregion

        public ConvexHull2DBuilder AddSome (ReadOnlyCollection<pair2f> points, int startAt, int stopBefore)
            => this.AddSome (points.GetCyclicIterator (), startAt, stopBefore);

        public ConvexHull2DBuilder AddSome (ACyclicIndexIterator<pair2f> points, int startAt, int stopBefore) {

            if ((stopBefore - startAt) < MINIMUM_VERTEX_COUNT_FOR_BULK) {

                return this.AddSome_Sequential (points, startAt, stopBefore);

            } else {

                return this.AddSome_Bulk (points, startAt, stopBefore);

            }
        }

        private ConvexHull2DBuilder AddSome_Sequential (ACyclicIndexIterator<pair2f> points, int startAt, int stopBefore) {

            for (var iterator = points.Copy (startingIndex: startAt); iterator.currentIndex < stopBefore; iterator++) {

                this.Add (iterator.current, false);

            }

            this.AfterAddPoint (points);

            return this;

        }

        protected virtual ConvexHull2DBuilder AddSome_Bulk (ACyclicIndexIterator<pair2f> points, int startAt, int stopBefore) {

            var list = new LinkedList<pair2f> ();

            for (var iterator = points.Copy (startingIndex: startAt); iterator.currentIndex < stopBefore; iterator++) {

                list.AddToEnd (iterator.current);

            }

            return this.AddSome_Bulk_LinkedList (list, true);

        }

        private ConvexHull2DBuilder AddSome_Bulk_LinkedList (LinkedList<pair2f> points, bool addCurrentHull) {

            if (addCurrentHull) {

                for (int i = 0; i < this.hull.Count; i++) {

                    points.AddToEnd (this.hull[i]);

                }
            }

            points = this.AddSome_Bulk_LinkedList_Initialize (points, this.directionalSearch);
            this.hull = this.AddSome_Bulk_LinkedList_CompleteHull (points);
            this.AfterAddPoint (this.hull, true);

            return this;

        }

        private LinkedList<pair2f> AddSome_Bulk_LinkedList_Initialize (LinkedList<pair2f> points, DirectionSpectrum2D searchBy) {

            // List to hold the points furthest along in the directions in searchBy.
            var extrema = new List<KeyValuePair<float, LinkedListPosition<pair2f>>> ();

            // Initialize the extrema list.
            for (var i = 0; i < searchBy.count; i++) {

                extrema.Add (new KeyValuePair<float, LinkedListPosition<pair2f>> (
                    Single.NegativeInfinity,
                    new LinkedListPosition<pair2f> ()
                ));

            }

            // For each point...
            for (var p = points.CopyFrontPosition (); p.hasValue; p++) {

                // For each direction we're searching...
                for (var i = 0; i < searchBy.count; i++) {

                    // Find how far along the point is in the direction.
                    var dot = SFloatMath.GetDotProduct (p.value, searchBy[i]);

                    // If it's the furthest we've found in this direction so far, overwrite the previous record.
                    if (dot > extrema[i].Key) {

                        extrema[i] = new KeyValuePair<float, LinkedListPosition<pair2f>> (dot, p.Copy ());

                    }
                }
            }

            // Now we have all extrema. Clear the current hull to accept the new points.
            this.hull.Clear ();

            //For each extreme...
            for (var e = extrema.AsReadOnly ().GetCyclicIterator (); e.cycles < 1; e++) {

                // Attempt to extract the furthest point from the linked list.
                // Note that since it's possible to have the same point be the furthest for multiple directions,
                //   we use a nullval of pair2f.nan to ensure we only extract that point once from the list.
                var point = points.Remove (e.current.Value, nullval: pair2f.nan);

                if (!point.isNan) {

                    // Since the hull was cleared, accept the extremes as new points into the hull.
                    this.hull.Add (point);

                }
            }

            // Finally, remove all points in the linked list that are in the current convex hull.
            points.RemoveByCondition (point => SShapeMath.IsPointInConvexShape (point, this.hull.AsReadOnly ()));

            return points;

        }

        private List<pair2f> AddSome_Bulk_LinkedList_CompleteHull (LinkedList<pair2f> pointsToAdd) {

            // Temporarily move the hull to a linked list to allow for easier intermediate adding.
            var workingHull = new LinkedList<pair2f> (this.hull.AsReadOnly ());

            for (var current = workingHull.CopyFrontPosition (); !pointsToAdd.isEmpty && current.hasValue; current++) {

                var normal = this.DelegateFindOrthogonal (workingHull.GetNextOrFront (current) - current.value);
                var furthest = new KeyValuePair<float, LinkedListPosition<pair2f>> (0f, null);

                // This loop will find the furthest point in the direction normal to the current hull line segment.
                // If the point is in that direction from the shape, but is not the furthest, we can still record
                //   it as a candidate for removal from the list.
                var otherPositives = new List<LinkedListPosition<pair2f>> ();

                for (var a = pointsToAdd.CopyFrontPosition (); a.hasValue; a++) {

                    // Find how far along the point is.
                    var dot = SFloatMath.GetDotProduct ((a.value - current.value), normal);

                    // If it's further than our current furthest recorded...
                    if (dot > furthest.Key) {

                        // Add the current furthest to the deletion candidates list.
                        otherPositives.Add (furthest.Value);

                        // Then replace the current furthest.
                        furthest = new KeyValuePair<float, LinkedListPosition<pair2f>> (dot, a.Copy ());

                    } else if (dot > 0f) {

                        // If the point is not further than the furthest, but is positive, add it to the
                        //   candidate deletion list.
                        otherPositives.Add (a.Copy ());

                    }
                }

                // If we found a furthest point...
                if (furthest.Value != null) {

                    var oldNext = workingHull.GetNextOrFront (current);

                    // Remove the furthest point from the list and add it to the hull.
                    var newNext = workingHull.AddAsNext (pointsToAdd.Remove (furthest.Value), current).value;

                    // Then remove any deletion candidates we found which are contained in the new section
                    //   of the null.
                    pointsToAdd.RemoveByCondition (
                        point => SShapeMath.IsPointInConvexShape (
                            point,
                            // Following three points forms a triangle.
                            current.value,
                            newNext,
                            oldNext
                        ),
                        otherPositives.AsReadOnly ()
                    );

                    // Finally, set the current index back a step so we can analyse the new line segment
                    //   which was just created.
                    current--;

                }
            }

            return workingHull.AsList ();

        }

        private void AfterAddPoint (List<pair2f> points, bool clearBounds = false) => this.AfterAddPoint (points.AsReadOnly (), clearBounds);
        private void AfterAddPoint (ReadOnlyCollection<pair2f> points, bool clearBounds = false) => this.AfterAddPoint (points.GetCyclicIterator (), clearBounds);
        private void AfterAddPoint (ACyclicIndexIterator<pair2f> points, bool clearBounds = false) {

            if (clearBounds) {

                this.centerBoundsBuilder.Clear ();

            }

            this.centerBoundsBuilder.AddAll (points);
            this.UpdateRadiuses ();

        }
    }
}
