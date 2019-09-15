using System;
using System.Collections.Generic;

namespace AWE.Moving {

    public abstract class ATransformState<TValueType, TTranslation, TRotation, TDilation>
        : ITransformState<TValueType, TTranslation, TRotation, TDilation>,
        IComparable<ATransformState<TValueType, TTranslation, TRotation, TDilation>>
        where TValueType : struct
        where TTranslation : IReadOnlyList<TValueType>
        where TRotation : IReadOnlyList<TValueType>
        where TDilation : IReadOnlyList<TValueType>
    {

        protected abstract ITransform<TValueType, TTranslation, TRotation, TDilation> _transform { get; }

        public ITransform<TValueType, TTranslation, TRotation, TDilation> transform => this._transform;
        public abstract TransformStateIndex index { get; }
        public abstract TTranslation position { get; }
        public abstract TRotation rotation { get; }
        public abstract TDilation dilation { get; }

        IReadOnlyList<TValueType> ITransformState<TValueType>.position => this.position;
        IReadOnlyList<TValueType> ITransformState<TValueType>.rotation => this.rotation;
        IReadOnlyList<TValueType> ITransformState<TValueType>.dilation => this.dilation;

        int IComparable.CompareTo (object other) {

            if (other is ATransformState<TValueType, TTranslation, TRotation, TDilation> otherState) {

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
        public int CompareTo (ATransformState<TValueType, TTranslation, TRotation, TDilation> other) => this.index.CompareTo (other.index);

        bool ITransformState<TValueType>.IsEquivalent (ITransformState<TValueType> other) {

            var positionCount = this.position.Count;
            var rotationCount = this.rotation.Count;
            var dilationCount = this.dilation.Count;

            var isValueSame = true;
            var isDimensionSame = (
                (positionCount == other.position.Count)
                && (rotationCount == other.rotation.Count)
                && (dilationCount == other.dilation.Count)
            );

            if (isDimensionSame) {

                var maxCount = System.Math.Max (
                    System.Math.Max (positionCount, rotationCount),
                    dilationCount
                );

                for (int i = 0; i < maxCount; i++) {

                    if ((i < positionCount) && !this.position[i].Equals(other.position[i])) {

                        isValueSame = false;
                        break;

                    }

                    if ((i < rotationCount) && !this.rotation[i].Equals(other.rotation[i])) {

                        isValueSame = false;
                        break;

                    }

                    if ((i < dilationCount) && !this.dilation[i].Equals(other.dilation[i])) {

                        isValueSame = false;
                        break;

                    }
                }
            }

            return (isDimensionSame && isValueSame);

        }

        bool ITransformState<TValueType, TTranslation, TRotation, TDilation>.IsEquivalent (
            ITransformState<TValueType, TTranslation, TRotation, TDilation> other
        ) => this.IsEquivalent (other.position, other.rotation, other.dilation);
        public bool IsEquivalent (ATransformState<TValueType, TTranslation, TRotation, TDilation> other) => this.IsEquivalent (
            other.position,
            other.rotation,
            other.dilation
        );
        public bool IsEquivalent (TTranslation position, TRotation rotation, TDilation dilation) => (
            this.position.Equals (position)
            && this.rotation.Equals (rotation)
            && this.dilation.Equals (dilation)
        );

    }
}