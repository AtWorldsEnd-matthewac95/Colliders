using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AWE.CollectionExtensions;

namespace AWE.Moving {

    public sealed class GenericTransformation<TValueType>
        : ATransformation<TValueType, ReadOnlyCollection<TValueType>, ReadOnlyCollection<TValueType>, ReadOnlyCollection<TValueType>>,
        IEquatable<GenericTransformation<TValueType>>
        where TValueType : struct
    {

        private readonly Func<TValueType, TValueType, TValueType> Addition;
        private readonly Func<TValueType, TValueType, TValueType> Subtraction;
        private readonly Func<TValueType, TValueType, TValueType> DilationAddition;
        private readonly Func<TValueType, TValueType, TValueType> DilationSubtraction;

        private GenericTransformation (
            Func<TValueType, TValueType, TValueType> Addition,
            Func<TValueType, TValueType, TValueType> Subtraction,
            Func<TValueType, TValueType, TValueType> DilationAddition,
            Func<TValueType, TValueType, TValueType> DilationSubtraction,
            ReadOnlyCollection<TValueType> translation,
            ReadOnlyCollection<TValueType> rotation,
            ReadOnlyCollection<TValueType> dilation
        ) : base (translation, rotation, dilation) {

            this.Addition = Addition;
            this.Subtraction = Subtraction;
            this.DilationAddition = DilationAddition;
            this.DilationSubtraction = DilationSubtraction;

        }

        public GenericTransformation (
            IEnumerable<TValueType> translation,
            IEnumerable<TValueType> rotation,
            IEnumerable<TValueType> dilation,
            Func<TValueType, TValueType, TValueType> Addition,
            Func<TValueType, TValueType, TValueType> Subtraction,
            Func<TValueType, TValueType, TValueType> DilationAddition,
            Func<TValueType, TValueType, TValueType> DilationSubtraction
        ) : base (
            (new List<TValueType> (translation)).AsReadOnly (),
            (new List<TValueType> (rotation)).AsReadOnly (),
            (new List<TValueType> (dilation)).AsReadOnly ()
        ) {

            this.Addition = Addition;
            this.Subtraction = Subtraction;
            this.DilationAddition = DilationAddition;
            this.DilationSubtraction = DilationSubtraction;

        }

        public GenericTransformation (
            ITransformation<TValueType> transformation1,
            ITransformation<TValueType> transformation2,
            Func<TValueType, TValueType, TValueType> Addition,
            Func<TValueType, TValueType, TValueType> Subtraction,
            Func<TValueType, TValueType, TValueType> DilationAddition,
            Func<TValueType, TValueType, TValueType> DilationSubtraction,
            Func<TValueType, TValueType, TValueType> CombinationOperation,
            Func<TValueType, TValueType, TValueType> DilationOperation
        ) : base (
            transformation1.translation.Combine (transformation2.translation, CombinationOperation),
            transformation1.rotation.Combine (transformation2.rotation, CombinationOperation),
            transformation1.dilation.Combine (transformation2.dilation, DilationOperation)
        ) {

            this.Addition = Addition;
            this.Subtraction = Subtraction;
            this.DilationAddition = DilationAddition;
            this.DilationSubtraction = DilationSubtraction;

        }

        public override ITransformation Add (ITransformation other) {

            ITransformation sum;

            if (other is GenericTransformation<TValueType> transformation) {

                sum = this.Add (transformation);

            } else {

                sum = base.Add (other);

            }

            return sum;

        }

        public override ITransformation<TValueType> Add (ITransformation<TValueType> other) => this.Add (
            other.translation,
            other.rotation,
            other.dilation
        );
        protected override ATransformation<TValueType, ReadOnlyCollection<TValueType>, ReadOnlyCollection<TValueType>, ReadOnlyCollection<TValueType>> _Add (
            ReadOnlyCollection<TValueType> translation,
            ReadOnlyCollection<TValueType> rotation,
            ReadOnlyCollection<TValueType> dilation
        ) => this.Add (translation, rotation, dilation);
        public GenericTransformation<TValueType> Add (GenericTransformation<TValueType> other) => this.Add (other.translation, other.rotation, other.dilation);

        public GenericTransformation<TValueType> Add (
            ReadOnlyCollection<TValueType> translation,
            ReadOnlyCollection<TValueType> rotation,
            ReadOnlyCollection<TValueType> dilation
        ) => new GenericTransformation<TValueType> (
            this.Addition,
            this.Subtraction,
            this.DilationAddition,
            this.DilationSubtraction,
            this.translation.Combine (translation, this.Addition),
            this.rotation.Combine (rotation, this.Addition),
            this.dilation.Combine (dilation, this.DilationAddition)
        );

        public GenericTransformation<TValueType> Add (
            IReadOnlyList<TValueType> translation,
            IReadOnlyList<TValueType> rotation,
            IReadOnlyList<TValueType> dilation
        ) => new GenericTransformation<TValueType> (
            this.Addition,
            this.Subtraction,
            this.DilationAddition,
            this.DilationSubtraction,
            this.translation.Combine (translation, this.Addition),
            this.rotation.Combine (rotation, this.Addition),
            this.dilation.Combine (dilation, this.DilationAddition)
        );

        public override ITransformation Subtract (ITransformation other) {

            ITransformation difference;

            if (other is GenericTransformation<TValueType> transformation) {

                difference = this.Add (transformation);

            } else {

                difference = base.Add (other);

            }

            return difference;

        }

        public override ITransformation<TValueType> Subtract (ITransformation<TValueType> other) => this.Subtract (
            other.translation,
            other.rotation,
            other.dilation
        );
        protected override ATransformation<TValueType, ReadOnlyCollection<TValueType>, ReadOnlyCollection<TValueType>, ReadOnlyCollection<TValueType>> _Subtract (
            ReadOnlyCollection<TValueType> translation,
            ReadOnlyCollection<TValueType> rotation,
            ReadOnlyCollection<TValueType> dilation
        ) => this.Subtract (translation, rotation, dilation);
        public GenericTransformation<TValueType> Subtract (GenericTransformation<TValueType> other) => this.Subtract (other.translation, other.rotation, other.dilation);

        public GenericTransformation<TValueType> Subtract (
            ReadOnlyCollection<TValueType> translation,
            ReadOnlyCollection<TValueType> rotation,
            ReadOnlyCollection<TValueType> dilation
        ) => new GenericTransformation<TValueType> (
            this.Addition,
            this.Subtraction,
            this.DilationAddition,
            this.DilationSubtraction,
            this.translation.Combine (translation, this.Subtraction),
            this.rotation.Combine (rotation, this.Subtraction),
            this.dilation.Combine (dilation, this.DilationSubtraction)
        );

        public GenericTransformation<TValueType> Subtract (
            IReadOnlyList<TValueType> translation,
            IReadOnlyList<TValueType> rotation,
            IReadOnlyList<TValueType> dilation
        ) => new GenericTransformation<TValueType> (
            this.Addition,
            this.Subtraction,
            this.DilationAddition,
            this.DilationSubtraction,
            this.translation.Combine (translation, this.Subtraction),
            this.rotation.Combine (rotation, this.Subtraction),
            this.dilation.Combine (dilation, this.DilationSubtraction)
        );

        public override int GetHashCode () {

            int basePrime = 73; // TODO - Get new primes
            int multPrime = 47; // TODO - Get new primes

            unchecked {

                var hashCode = basePrime;
                hashCode = ((hashCode * multPrime) ^ this.translation.GetHashCode ());
                hashCode = ((hashCode * multPrime) ^ this.rotation.GetHashCode ());
                hashCode = ((hashCode * multPrime) ^ this.dilation.GetHashCode ());

                return hashCode;

            }
        }

        public override bool Equals (object other) {

            bool isEqual;

            if (other is GenericTransformation<TValueType> otherTransformation) {

                isEqual = this.Equals (otherTransformation);

            } else {

                isEqual = base.Equals (other);

            }

            return isEqual;

        }

        public bool Equals (GenericTransformation<TValueType> other) => this.Equals (other.translation, other.rotation, other.dilation);
        public override bool Equals (
            ReadOnlyCollection<TValueType> translation,
            ReadOnlyCollection<TValueType> rotation,
            ReadOnlyCollection<TValueType> dilation
        ) {

            var translationCount = this.translation.Count;
            var rotationCount = this.rotation.Count;
            var dilationCount = this.dilation.Count;

            var isValueSame = true;
            var isDimensionSame = (
                (translationCount == translation.Count)
                && (rotationCount == rotation.Count)
                && (dilationCount == dilation.Count)
            );

            if (isDimensionSame) {

                var maxCount = System.Math.Max (
                    System.Math.Max (translationCount, rotationCount),
                    dilationCount
                );

                for (int i = 0; i < maxCount; i++) {

                    if ((i < translationCount) && !this.translation[i].Equals(translation[i])) {

                        isValueSame = false;
                        break;

                    }

                    if ((i < rotationCount) && !this.rotation[i].Equals(rotation[i])) {

                        isValueSame = false;
                        break;

                    }

                    if ((i < dilationCount) && !this.dilation[i].Equals(dilation[i])) {

                        isValueSame = false;
                        break;

                    }
                }
            }

            return (isDimensionSame && isValueSame);

        }
    }
}