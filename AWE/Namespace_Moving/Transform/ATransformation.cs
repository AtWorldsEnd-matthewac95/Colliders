using System;
using System.Collections.Generic;

namespace AWE.Moving {

    public abstract class ATransformation<TValueType, TTranslation, TRotation, TDilation>
        : ITransformation<TValueType, TTranslation, TRotation, TDilation>,
        IEquatable<ATransformation<TValueType, TTranslation, TRotation, TDilation>>
        where TValueType : struct
        where TTranslation : IReadOnlyList<TValueType>
        where TRotation : IReadOnlyList<TValueType>
        where TDilation : IReadOnlyList<TValueType>
    {

        IReadOnlyList<TValueType> ITransformation<TValueType>.translation => this.translation;
        IReadOnlyList<TValueType> ITransformation<TValueType>.rotation => this.rotation;
        IReadOnlyList<TValueType> ITransformation<TValueType>.dilation => this.dilation;

        public TTranslation translation { get; protected set; }
        public TRotation rotation { get; protected set; }
        public TDilation dilation { get; protected set; }

        protected ATransformation () {

            this.translation = default;
            this.rotation = default;
            this.dilation = default;

        }

        public ATransformation (TTranslation translation, TRotation rotation, TDilation dilation) {

            this.translation = translation;
            this.rotation = rotation;
            this.dilation = dilation;

        }

        public virtual ITransformation Add (ITransformation other) {

            ITransformation sum = null;

            if (other is ITransformation<TValueType, TTranslation, TRotation, TDilation> otherITransformation) {

                sum = this.Add (otherITransformation);

            } else if (other is ITransformation<TValueType> otherITransformationV) {

                sum = this.Add (otherITransformationV);

            }

            return sum;

        }

        public abstract ITransformation<TValueType> Add (ITransformation<TValueType> other);

        ITransformation<TValueType, TTranslation, TRotation, TDilation> ITransformation<TValueType, TTranslation, TRotation, TDilation>.Add (
            ITransformation<TValueType, TTranslation, TRotation, TDilation> other
        ) => this._Add (other.translation, other.rotation, other.dilation);
        protected abstract ATransformation<TValueType, TTranslation, TRotation, TDilation> _Add (
            TTranslation translation,
            TRotation rotation,
            TDilation dilation
        );

        public virtual ITransformation Subtract (ITransformation other) {

            ITransformation difference = null;

            if (other is ITransformation<TValueType, TTranslation, TRotation, TDilation> otherITransformation) {

                difference = this.Subtract (otherITransformation);

            } else if (other is ITransformation<TValueType> otherITransformationV) {

                difference = this.Subtract (otherITransformationV);

            }

            return difference;

        }

        public abstract ITransformation<TValueType> Subtract (ITransformation<TValueType> other);

        ITransformation<TValueType, TTranslation, TRotation, TDilation> ITransformation<TValueType, TTranslation, TRotation, TDilation>.Subtract (
            ITransformation<TValueType, TTranslation, TRotation, TDilation> other
        ) => this._Subtract (other.translation, other.rotation, other.dilation);
        protected abstract ATransformation<TValueType, TTranslation, TRotation, TDilation> _Subtract (
            TTranslation translation,
            TRotation rotation,
            TDilation dilation
        );

        public override bool Equals (object other) {

            var isEqual = false;

            if (other is ATransformation<TValueType, TTranslation, TRotation, TDilation> otherTransformation) {

                isEqual = this.Equals (otherTransformation);

            } else if (other is ITransformation<TValueType, TTranslation, TRotation, TDilation> otherITransformation) {

                isEqual = this.Equals (otherITransformation);

            } else if (other is ITransformation<TValueType> otherITransformationV) {

                isEqual = this.Equals (otherITransformationV);

            }

            return isEqual;

        }

        public override int GetHashCode () {

            int basePrime = 73;
            int multPrime = 47;

            unchecked {

                var hashCode = basePrime;
                hashCode = ((hashCode * multPrime) ^ this.translation.GetHashCode ());
                hashCode = ((hashCode * multPrime) ^ this.rotation.GetHashCode ());
                hashCode = ((hashCode * multPrime) ^ this.dilation.GetHashCode ());

                return hashCode;

            }
        }

        public bool Equals (ITransformation<TValueType> other) {

            var translationCount = this.translation.Count;
            var rotationCount = this.rotation.Count;
            var dilationCount = this.dilation.Count;

            var isValueSame = true;
            var isDimensionSame = (
                (translationCount == other.translation.Count)
                && (rotationCount == other.rotation.Count)
                && (dilationCount == other.dilation.Count)
            );

            if (isDimensionSame) {

                var maxCount = System.Math.Max (
                    System.Math.Max (translationCount, rotationCount),
                    dilationCount
                );

                for (int i = 0; i < maxCount; i++) {

                    if ((i < translationCount) && !this.translation[i].Equals(other.translation[i])) {

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

        public bool Equals (ITransformation<TValueType, TTranslation, TRotation, TDilation> other) => (
            this.translation.Equals (other.translation)
            && this.rotation.Equals (other.rotation)
            && this.dilation.Equals (other.dilation)
        );
        public bool Equals (ATransformation<TValueType, TTranslation, TRotation, TDilation> other) => (
            this.translation.Equals (other.translation)
            && this.rotation.Equals (other.rotation)
            && this.dilation.Equals (other.dilation)
        );
        public abstract bool Equals (TTranslation translation, TRotation rotation, TDilation dilation);

    }
}