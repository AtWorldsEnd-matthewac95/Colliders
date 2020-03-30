using System;
using System.Collections.Generic;

namespace AWE.Moving {

    public abstract class ATransformation<TValueType, TTranslation, TRotation, TDilation>
        : ITransformation,
        IEquatable<ATransformation<TValueType, TTranslation, TRotation, TDilation>>
        where TValueType : struct
        where TTranslation : IReadOnlyList<TValueType>
        where TRotation : IReadOnlyList<TValueType>
        where TDilation : IReadOnlyList<TValueType>
    {

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

            if (other is ATransformation<TValueType, TTranslation, TRotation, TDilation> otherTransformation) {

                sum = this.Add (otherTransformation);

            }

            return sum;

        }

        protected abstract ATransformation<TValueType, TTranslation, TRotation, TDilation> _Add (
            TTranslation translation,
            TRotation rotation,
            TDilation dilation
        );

        public virtual ITransformation Subtract (ITransformation other) {

            ITransformation difference = null;

            if (other is ATransformation<TValueType, TTranslation, TRotation, TDilation> otherTransformation) {

                difference = this.Subtract (otherTransformation);

            }

            return difference;

        }

        protected abstract ATransformation<TValueType, TTranslation, TRotation, TDilation> _Subtract (
            TTranslation translation,
            TRotation rotation,
            TDilation dilation
        );

        public override bool Equals (object other) {

            var isEqual = false;

            if (other is ATransformation<TValueType, TTranslation, TRotation, TDilation> otherTransformation) {

                isEqual = this.Equals (otherTransformation);

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

        public bool Equals (ITransformation other) {

            var isEqual = false;

            if (other is ATransformation<TValueType, TTranslation, TRotation, TDilation> atransformation) {

                isEqual = this.Equals (atransformation);

            }

            return isEqual;

        }
        public bool Equals (ATransformation<TValueType, TTranslation, TRotation, TDilation> other) => (
            this.translation.Equals (other.translation)
            && this.rotation.Equals (other.rotation)
            && this.dilation.Equals (other.dilation)
        );
        public abstract bool Equals (TTranslation translation, TRotation rotation, TDilation dilation);

    }
}