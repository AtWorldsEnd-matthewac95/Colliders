using System.Collections.ObjectModel;

namespace AWE.Moving {

    public class _TransformPath<TTransformState> where TTransformState : ITransformState {

        protected DTransformPath<TTransformState> path;

        public TransformPathAnchors<TTransformState> defaultAnchors { get; protected set; }
        public bool isLooping { get; protected set; }
        public bool isBacktracking { get; protected set; }
        public FloatRange range { get; protected set; }

        public bool isInfinite => (this.range == null);

        public TTransformState this [float position] => this.FindState (position);

        public _TransformPath (
            DTransformPath<TTransformState> path,
            bool isLooping,
            bool isBacktracking,
            ReadOnlyCollection<float> defaultAnchors,
            FloatRange range,
            bool sortDefaultAnchors = false,
            bool checkDefaultAnchorRange = false
        ) {

            this.path = path;
            this.isLooping = isLooping;
            this.isBacktracking = isBacktracking;
            this.range = range;

            this.defaultAnchors = new TransformPathAnchors<TTransformState> (
                this,
                defaultAnchors,
                sortDefaultAnchors,
                checkDefaultAnchorRange
            );

        }

        public _TransformPath (
            DTransformPath<TTransformState> path,
            bool isLooping,
            bool isBacktracking,
            DValuesInRange<float> anchorFunction,
            FloatRange range = null
        ) {

            this.path = path;
            this.isLooping = isLooping;
            this.isBacktracking = isBacktracking;

            if (range == null) {

                this.defaultAnchors = new TransformPathAnchors<TTransformState> (this, anchorFunction);

            } else {

                this.range = range;
                this.defaultAnchors = new TransformPathAnchors<TTransformState> (this, anchorFunction (range.lower, range.upper));

            }
        }

        public virtual TTransformState FindState (float position) {

            if (!this.isInfinite && !this.range.IsInRange (position)) {

                if (this.isLooping) {

                    position = this.range.WrapToRange (position, this.isBacktracking);

                } else {

                    position = this.range.TrimToRange (position, this.isBacktracking);

                }
            }

            return this.path (position);

        }
    }
}