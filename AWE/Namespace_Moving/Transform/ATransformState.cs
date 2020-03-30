using System;
using System.Collections.Generic;

namespace AWE.Moving {

    public abstract class ATransformState<TValueType, TTranslation, TRotation, TDilation, TTransformation>
        : ITransformState,
        IComparable<ATransformState<TValueType, TTranslation, TRotation, TDilation, TTransformation>>
        where TValueType : struct
        where TTranslation : IReadOnlyList<TValueType>
        where TRotation : IReadOnlyList<TValueType>
        where TDilation : IReadOnlyList<TValueType>
        where TTransformation : ATransformation<TValueType, TTranslation, TRotation, TDilation>
    {

        IReadOnlyTransform ITransformState.transform => this.GetParentTransform ();
        protected abstract IReadOnlyTransform GetParentTransform ();

        public abstract TransformStateIndex index { get; }
        public abstract TTranslation position { get; }
        public abstract TRotation rotation { get; }
        public abstract TDilation dilation { get; }

        int IComparable.CompareTo (object other) {

            if (other is ATransformState<TValueType, TTranslation, TRotation, TDilation, TTransformation> otherState) {

                return this.CompareTo (otherState);

            } else if (other is ITransformState otherIState) {

                return this.CompareTo (otherIState);

            } else {

                throw new ArgumentException (
                    "Object is not a transform state. ",
                    other.GetType ().ToString () + " other"
                );

            }
        }
        public int CompareTo (ITransformState other) => this.index.CompareTo (other.index);
        public int CompareTo (ATransformState<TValueType, TTranslation, TRotation, TDilation, TTransformation> other) => this.index.CompareTo (other.index);

        public bool IsEquivalent (ATransformState<TValueType, TTranslation, TRotation, TDilation, TTransformation> other) => this.IsEquivalent (
            other.position,
            other.rotation,
            other.dilation
        );
        public virtual bool IsEquivalent (TTranslation position, TRotation rotation, TDilation dilation) => (
            this.position.Equals (position)
            && this.rotation.Equals (rotation)
            && this.dilation.Equals (dilation)
        );

        ITransformState ITransformState.Add (ITransformation transformation) {

            var result = this;

            if (transformation is ATransformation<TValueType, TTranslation, TRotation, TDilation> atransformation) {

                result = this.Add (atransformation);

            }

            return result;

        }
        public abstract ATransformState<TValueType, TTranslation, TRotation, TDilation, TTransformation> Add (
            ATransformation<TValueType, TTranslation, TRotation, TDilation> transformation
        );

        ITransformState ITransformState.Subtract (ITransformation transformation) {

            var result = this;

            if (transformation is ATransformation<TValueType, TTranslation, TRotation, TDilation> atransformation) {

                result = this.Subtract (atransformation);

            }

            return result;

        }
        public abstract ATransformState<TValueType, TTranslation, TRotation, TDilation, TTransformation> Subtract (
            ATransformation<TValueType, TTranslation, TRotation, TDilation> transformation
        );

        ITransformation ITransformState.FindDifference (ITransformState other) {

            ITransformation difference = null;

            if (other is ATransformState<TValueType, TTranslation, TRotation, TDilation, TTransformation> otherState) {

                difference = this.FindDifference (otherState);

            }

            return difference;

        }
        public abstract TTransformation FindDifference (
            ATransformState<TValueType, TTranslation, TRotation, TDilation, TTransformation> other
        );

    }
}