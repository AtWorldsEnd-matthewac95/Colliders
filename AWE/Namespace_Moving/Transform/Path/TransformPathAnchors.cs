using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AWE.Math.FloatExtensions;

namespace AWE.Moving {

    public class TransformPathAnchors<TTransformState> : ICloneable, IReadOnlyList<WeightedTransformState<TTransformState>> where TTransformState : ITransformState {

        protected List<WeightedTransformState<TTransformState>> anchors;
        protected DValuesInRange<float> anchorFunction;

        int IReadOnlyCollection<WeightedTransformState<TTransformState>>.Count => this.count;

        public WeightedTransformState<TTransformState> this [int index] => this.anchors?[index];
        public int count => ((this.anchors == null) ? 0 : this.anchors.Count);

        public _TransformPath<TTransformState> path { get; protected set; }

        private TransformPathAnchors (
            _TransformPath<TTransformState> path,
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
            _TransformPath<TTransformState> path,
            List<WeightedTransformState<TTransformState>> anchors,
            DValuesInRange<float> anchorFunction
        ) {

            this.path = path;
            this.anchors = anchors;
            this.anchorFunction = anchorFunction;

        }

        public TransformPathAnchors (
            _TransformPath<TTransformState> path,
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

        public TransformPathAnchors (_TransformPath<TTransformState> path, DValuesInRange<float> anchorFunction) {

            this.path = path;
            this.anchors = new List<WeightedTransformState<TTransformState>> ();
            this.anchorFunction = anchorFunction;

        }

        protected List<WeightedTransformState<TTransformState>> FindAnchorsInRangeUsingList (
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

        IEnumerator IEnumerable.GetEnumerator () => this.GetEnumerator ();
        public virtual IEnumerator<WeightedTransformState<TTransformState>> GetEnumerator () => this.anchors?.GetEnumerator ();

        public virtual List<WeightedTransformState<TTransformState>> FindAnchorsWithinRange (
            float lower,
            float upper,
            bool includeEndpoints = true,
            int start = 0
        ) {

            List<WeightedTransformState<TTransformState>> anchorsInRange;

            if (this.count > 0) {

                anchorsInRange = this.FindAnchorsInRangeUsingList (lower, upper, includeEndpoints, start);

            } else if (this.anchorFunction != null) {

                anchorsInRange = this.FindAnchorsWithinRangeUsingFunction (lower, upper, includeEndpoints);

            } else {

                anchorsInRange = new List<WeightedTransformState<TTransformState>> ();

            }

            return anchorsInRange;

        }

        private void Test<TOne, TTwo> (TOne one, TTwo two) where TOne : IComparable<TTwo> { }

        public TransformPathAnchors<TTransformState> Add (params float[] anchorPositions) => this.Add (Array.AsReadOnly (anchorPositions), false);
        public TransformPathAnchors<TTransformState> Add (bool allowDuplicates, params float[] anchorPositions) => this.Add (Array.AsReadOnly (anchorPositions), allowDuplicates);
        public virtual TransformPathAnchors<TTransformState> Add (ReadOnlyCollection<float> anchorPositions, bool allowDuplicates) {

            var newAnchors = (
                this.count > 0
                ? new LinkedList<WeightedTransformState<TTransformState>> (this.anchors.AsReadOnly ())
                : new LinkedList<WeightedTransformState<TTransformState>> ()
            );
            var length = anchorPositions.Count;
            var current = newAnchors.CopyFrontPosition ();

            // TODO - I am certain there is a more efficient way of setting up these loops...
            int index = 0;

            while (current.hasValue && (index < length)) {

                while ((anchorPositions[index] < current.value.weight) && (index < length)) {

                    // Notice the !. If the difference is NOT negligible...
                    if (allowDuplicates || !(anchorPositions[index] - current.value.weight).IsNegligible ()) {

                        newAnchors.AddAsPrevious (
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

                newAnchors.AddToEnd (
                    new WeightedTransformState<TTransformState> (
                        this.path[anchorPositions[index]],
                        anchorPositions[index]
                    )
                );

                index++;

            }

            return new TransformPathAnchors<TTransformState> (this.path, newAnchors.AsList (), this.anchorFunction);

        }

        object ICloneable.Clone () => this.Clone ();
        public TransformPathAnchors<TTransformState> Clone () => new TransformPathAnchors<TTransformState> (this.path, this.anchors.AsReadOnly (), this.anchorFunction);

    }
}