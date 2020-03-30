using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AWE.Math.FloatExtensions;

namespace AWE.Moving {

    public class TransformPathAnchors<TTransformState> : ICloneable, IReadOnlyList<WeightedTransformState<TTransformState>> where TTransformState : ITransformState {

        protected static List<WeightedTransformState<TTransformState>> CombineAnchorCollections (
            ReadOnlyCollection<WeightedTransformState<TTransformState>> existingAnchors,
            ReadOnlyCollection<float> anchorPositions,
            bool allowDuplicates = false
        ) {

            var combined = (
                this.count > 0
                ? new LinkedList<WeightedTransformState<TTransformState>> (existingAnchors)
                : new LinkedList<WeightedTransformState<TTransformState>> ()
            );
            var length = anchorPositions.Count;
            var current = combined.CopyFrontPosition ();

            // TODO - I am certain there is a more efficient way of setting up these loops...
            int index = 0;

            while (current.hasValue && (index < length)) {

                while ((anchorPositions[index] < current.value.weight) && (index < length)) {

                    // Notice the !. If the difference is NOT negligible...
                    if (allowDuplicates || !(anchorPositions[index] - current.value.weight).IsNegligible ()) {

                        combined.AddAsPrevious (
                            new WeightedTransformState<TTransformState> (
                                this.path[anchorPositions[index]],
                                anchorPositions[index]
                            ),
                            current
                        );

                    }

                    index++;

                }

                current.MoveNext ();

            }

            while (index < length) {

                combined.AddToEnd (
                    new WeightedTransformState<TTransformState> (
                        this.path[anchorPositions[index]],
                        anchorPositions[index]
                    )
                );

                index++;

            }

            return combined.AsList ();

        }

        protected List<WeightedTransformState<TTransformState>> anchors;
        protected DValuesInRange<float> anchorFunction;

        int IReadOnlyCollection<WeightedTransformState<TTransformState>>.Count => this.count;

        public WeightedTransformState<TTransformState> this [int index] => this.anchors?[index];
        public int count => ((this.anchors == null) ? 0 : this.anchors.Count);

        public TransformPath<TTransformState> path { get; }

        private TransformPathAnchors (
            TransformPath<TTransformState> path,
            ReadOnlyCollection<WeightedTransformState<TTransformState>> anchors,
            DValuesInRange<float> anchorFunction
        ) {

            this.path = path;
            this.anchorFunction = anchorFunction;

            this.anchors = new List<WeightedTransformState<TTransformState>> ();

            for (int i = 0; i < anchors.Count; i++) {

                this.anchors.Add (anchors[i].Clone ());

            }
        }

        private TransformPathAnchors (
            TransformPath<TTransformState> path,
            List<WeightedTransformState<TTransformState>> anchors,
            DValuesInRange<float> anchorFunction
        ) {

            this.path = path;
            this.anchors = anchors;
            this.anchorFunction = anchorFunction;

        }

        public TransformPathAnchors (TransformPath<TTransformState> path) {

            this.path = path;
            this.anchors = null;
            this.anchorFunction = null;

        }

        public TransformPathAnchors (
            TransformPath<TTransformState> path,
            ReadOnlyCollection<float> positions,
            bool sort = false,
            bool checkRange = false
        ) {

            this.path = path;

            var length = positions.Count;
            var range = path.range;
            this.anchors = new List<WeightedTransformState<TTransformState>> (length);

            if (checkRange) {

                for (int i = 0; i < length; i++) {

                    var position = positions[i];

                    if (range.IsInRange (position)) {

                        this.anchors.Add (new WeightedTransformState<TTransformState> (path[position], position));

                    }
                }

            } else {

                for (int i = 0; i < length; i++) {

                    var position = positions[i];
                    this.anchors.Add (new WeightedTransformState<TTransformState> (path[position], position));

                }
            }

            if (sort) {

                this.anchors.Sort ((a, b) => a.weight.CompareTo (b.weight));

            }
        }

        public TransformPathAnchors (TransformPath<TTransformState> path, DValuesInRange<float> anchorFunction) {

            this.path = path;
            this.anchors = new List<WeightedTransformState<TTransformState>> ();
            this.anchorFunction = anchorFunction;

        }

        protected List<WeightedTransformState<TTransformState>> FindAnchorsWithinRangeUsingList (
            float lower,
            float upper,
            bool includeEndpoints = true,
            int start = 0
        ) {

            var anchorsInRange = new List<WeightedTransformState<TTransformState>> ();
            var reachedLower = false;
            var length = this.count;

            for (int i = start; i < length; i++) {

                var anchor = this.anchors[i];

                if (anchor.weight >= lower) {

                    if (anchor.weight > upper) {

                        if (includeEndpoints) {

                            // If we haven't reached lower yet, add both endpoints now.
                            if (!reachedLower) {

                                anchorsInRange.Add (new WeightedTransformState<TTransformState> (this.path[lower], lower));

                            }

                            anchorsInRange.Add (new WeightedTransformState<TTransformState> (this.path[upper], upper));

                        }

                        break;

                    } else if ((upper - anchor.weight).IsNegligible ()) {

                        anchorsInRange.Add (anchor.Clone ());
                        break;

                    } else if (reachedLower) {

                        anchorsInRange.Add (anchor.Clone ());

                    } else {

                        // Notice the !. If weight - lower is NOT negligible...
                        if (includeEndpoints && !(anchor.weight - lower).IsNegligible ()) {

                            anchorsInRange.Add (new WeightedTransformState<TTransformState> (this.path[lower], lower));

                        }

                        anchorsInRange.Add (anchor.Clone ());
                        reachedLower = true;

                    }

                } else if (reachedLower) {

                    // This would be a very weird case, it's not clear what the user was intending. Just end the loop.
                    break;

                }
            }

            // If we got through the for loop without reaching the lower bound or adding anchors, add the endpoints now.
            if (includeEndpoints && !reachedLower && (anchorsInRange.Count <= 0)) {

                anchorsInRange.Add (new WeightedTransformState<TTransformState> (this.path[lower], lower));
                anchorsInRange.Add (new WeightedTransformState<TTransformState> (this.path[upper], upper));

            }

            return anchorsInRange;

        }

        protected virtual List<WeightedTransformState<TTransformState>> FindAnchorsWithinRangeUsingFunction (float lower, float upper, bool includeEndpoints = true) {

            var anchorsInRange = new List<WeightedTransformState<TTransformState>> ();
            var anchorPositionsInRange = this.anchorFunction (lower, upper);
            var positionsReturned = (anchorPositionsInRange?.Count > 0);

            if (positionsReturned) {

                // Notice the !. If it's NOT negligible...
                if (includeEndpoints && !(anchorPositionsInRange[0] - lower).IsNegligible ()) {

                    anchorsInRange.Add (new WeightedTransformState<TTransformState> (this.path[lower], lower));

                }

                int index;

                for (index = 0; index < anchorPositionsInRange.Count; index++) {

                    var position = anchorPositionsInRange[index];
                    anchorsInRange.Add (new WeightedTransformState<TTransformState> (this.path[position], position));

                }

                // Notice the !. If it's NOT negligible...
                if (includeEndpoints && !(anchorPositionsInRange[index - 1] - upper).IsNegligible ()) {

                    anchorsInRange.Add (new WeightedTransformState<TTransformState> (this.path[upper], upper));

                }

            } else if (includeEndpoints) {

                anchorsInRange.Add (new WeightedTransformState<TTransformState> (this.path[lower], lower));
                anchorsInRange.Add (new WeightedTransformState<TTransformState> (this.path[upper], upper));

            }

            return anchorsInRange;

        }

        public virtual List<WeightedTransformState<TTransformState>> FindAnchorsWithinRange (
            float lower,
            float upper,
            bool includeEndpoints = true,
            int start = 0
        ) {

            List<WeightedTransformState<TTransformState>> anchorsInRange;

            if (this.anchorFunction == null) {

                anchorsInRange = this.FindAnchorsWithinRangeUsingList (lower, upper, includeEndpoints, start);

            } else if (this.count <= 0) {

                anchorsInRange = this.FindAnchorsWithinRangeUsingFunction (lower, upper, includeEndpoints);

            } else {

                anchorsInRange = CombineAnchorCollections (
                    this.FindAnchorsWithinRangeUsingList (lower, upper, includeEndpoints, start).AsReadOnly (),
                    this.anchorFunction (lower, upper)
                );

            }

            return anchorsInRange;

        }

        public TransformPathAnchors<TTransformState> Add (params float[] anchorPositions) => this.Add (Array.AsReadOnly (anchorPositions), false);
        public TransformPathAnchors<TTransformState> Add (bool allowDuplicates, params float[] anchorPositions) => this.Add (Array.AsReadOnly (anchorPositions), allowDuplicates);
        public virtual TransformPathAnchors<TTransformState> Add (
            ReadOnlyCollection<float> anchorPositions,
            bool allowDuplicates = false
        ) => new TransformPathAnchors<TTransformState> (
            this.path,
            CombineAnchorCollections (this.anchors.AsReadOnly (), anchorPositions, allowDuplicates),
            this.anchorFunction
        );

        IEnumerator IEnumerable.GetEnumerator () => this.GetEnumerator ();
        public virtual IEnumerator<WeightedTransformState<TTransformState>> GetEnumerator () => this.anchors?.GetEnumerator ();

        object ICloneable.Clone () => this.Clone ();
        public TransformPathAnchors<TTransformState> Clone () => new TransformPathAnchors<TTransformState> (this.path, this.anchors.AsReadOnly (), this.anchorFunction);

    }
}
