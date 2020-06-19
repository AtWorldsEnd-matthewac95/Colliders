using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AWE.CollectionExtensions;

namespace AWE.Math {

    public partial class ConvexHull2DBuilder {

        private const int MINIMUM_VERTEX_COUNT_FOR_ADD = 2;

        private readonly float colinearTolerance;
        private readonly CenterBounds2DBuilder centerBoundsBuilder;

        // Used in ConvexHull2DBuilder_AddSome.cs
        private readonly DirectionSpectrum2D directionalSearch;

        protected readonly DFindOrthogonal2D DelegateFindOrthogonal;

        private List<pair2f> hull;

        public float currentMinimalRadius { get; private set; }
        public float currentMaximalRadius { get; private set; }

        public List<pair2f> currentHull => this.ToHull ();
        public Bounds2D currentBounds => this.ToBounds ();
        public pair2f currentCenter => this.ToCenter ();
        public bool isEmpty => (this.hull.Count < 1);

        public ConvexHull2DBuilder (float colinearTolerance = -SFloatMath.MINIMUM_DIFFERENCE) : this (ConvexHull2DBuilderOptions.clockwise, colinearTolerance) {}
        public ConvexHull2DBuilder (ConvexHull2DBuilderOptions options, float colinearTolerance = -SFloatMath.MINIMUM_DIFFERENCE) {

            this.directionalSearch = options.directionalSearch;
            this.DelegateFindOrthogonal = options.DelegateFindOrthogonal;
            this.colinearTolerance = colinearTolerance;
            this.centerBoundsBuilder = new CenterBounds2DBuilder ();
            this.hull = new List<pair2f> ();

        }

        public ConvexHull2DBuilder (pair2f point) : this (ConvexHull2DBuilderOptions.clockwise, -SFloatMath.MINIMUM_DIFFERENCE) => this.Add (point);

        public ConvexHull2DBuilder Add (pair2f point) => this.Add (point, true);
        protected ConvexHull2DBuilder Add (pair2f point, bool updateAfter) {

            if (this.hull.Count < MINIMUM_VERTEX_COUNT_FOR_ADD) {

                this.hull.Add (point);

                if (updateAfter) {

                    this.AfterAddPoint (point);

                }

            } else {

                var diff = (point - this.currentCenter);

                if (diff.magnitude > this.currentMinimalRadius) {

                    var count = this.hull.Count;
                    this.hull = this.Project (point);

                    if (updateAfter && (count < this.hull.Count)) {

                        this.AfterAddPoint (point);

                    }
                }
            }

            return this;

        }

        public List<pair2f> Project (pair2f point) {

            var newHull = new List<pair2f> ();

            if (this.hull.Count < MINIMUM_VERTEX_COUNT_FOR_ADD) {

                newHull.AddRange (this.hull);
                newHull.Add (point);

                return newHull;

            }

            var isPointMissed = new ValueMonitor<bool> ();
            var iterator = this.hull.AsReadOnly ().GetCyclicIterator ();

            // This loop has two exit conditions.
            //   (1) iterator.cycles == 1, which means we've iterated through every vertex.
            //   (2) isPointMissed.transitionCount == 2, which means the value of isPointMissed
            //         has changed twice.
            //
            // If case (2) happens, that means although we haven't necessarily iterated through
            //   every vertex, it's pointless to run calculations on the rest of the verticies.
            //   We can save some time by simply exiting out of the loop and assuming the value
            //   of isPointMissed will no longer change (which, if the current hull is truly
            //   convex, it wouldn't).
            for (iterator.ResetCycles (); (isPointMissed.transitionCount < 2) && (iterator.cycles < 1); iterator++) {

                var dot = SFloatMath.GetDotProduct (
                    (point - iterator.current),
                    this.DelegateFindOrthogonal (iterator.next - iterator.current)
                );

                // If the value of isPointMissed is changing...
                if (isPointMissed.Set (dot < this.colinearTolerance)) {

                    // If the value of isPointMissed is changing, and the current value
                    //   is true, then the previous value was false.
                    // That means we're going from finding the point to not finding it.
                    // This is the moment to add the point.
                    if (isPointMissed.value) {

                        newHull.Add (point);

                    }

                    // Always add the current vertex during isPointMissed state transitions.
                    newHull.Add (iterator.current);

                } else if (isPointMissed.value) {

                    // If the value of isPointMissed is not changing, and the current value
                    //   is true, then we are continuing to not find the point.
                    // In these cases, add the current vertex since it's thereby guaranteed
                    //   to be on the convex hull.
                    newHull.Add (iterator.current);

                }
            }

            // If we exited out of the loop because of the transition count and the current
            //   value of isPointMissed is true, add the rest of the verticies.
            if ((isPointMissed.transitionCount > 1) && isPointMissed.value) {

                newHull.AddSome (this.hull.AsReadOnly (), iterator.currentIndex, this.hull.Count);

            } else if (isPointMissed.transitionCount == 1) {

                // If we exited out of the loop with the transition count only changing once,
                //   that means the final transition would have happened between the last
                //   vertex and the first vertex.
                // If the current value of isPointMissed is true, then the first vertex was
                //   never added because it's value would have been false.
                if (isPointMissed.value) {

                    // Note that we've already accounted for an empty hull at the beginning of
                    //   this function, so this operation is safe from out of bounds exceptions.
                    newHull.Add (this.hull[0]);

                } else {

                    // If the current value of isPointMissed is false, then the first vertex was
                    //   already added to the hull (since it's value must have been true), but
                    //   the point was never added (since the point is only added on a false->true
                    //   transition).
                    newHull.Add (point);

                }
            }

            return newHull;

        }

        public ConvexHull2DBuilder Clear () {

            this.hull.Clear ();
            this.centerBoundsBuilder.Clear ();
            this.currentMinimalRadius = 0f;
            this.currentMaximalRadius = 0f;

            return this;

        }

        private void AfterAddPoint (pair2f point) {

            this.centerBoundsBuilder.Add (point);
            this.UpdateRadiuses ();

        }

        private void UpdateRadiuses () {

            var radiuses = SShapeMath.FindMinimalAndMaximalRadius (this.hull.AsReadOnly (), this.currentCenter);
            this.currentMinimalRadius = radiuses.lower;
            this.currentMaximalRadius = radiuses.upper;

        }

        public List<pair2f> ToHull () {

            var value = new List<pair2f> ();

            for (int i = 0; i < this.hull.Count; i++) {

                value.Add (this.hull[i]);

            }

            return value;

        }

        public Bounds2D ToBounds () => this.centerBoundsBuilder.ToBounds ();
        public pair2f ToCenter () => this.centerBoundsBuilder.ToCenter ();
        public float ToMinimalRadius () => this.currentMinimalRadius;
        public float ToMaximalRadius () => this.currentMaximalRadius;

    }
}
