using System;

namespace AWE {

    public abstract class ARange<T> where T : IComparable<T> {

        public T lower { get; private set; }
        public T upper { get; private set; }

        public abstract T diff { get; }

        public ARange (T lower, T upper) {

            if (upper.CompareTo (lower) <= 0) {

                throw new ArgumentException (
                    "upper must be greater than lower.\n" +
                    "lower: " + lower.ToString () + "\n" +
                    "upper: " + upper.ToString () + "\n"
                );

            }

            this.lower = lower;
            this.upper = upper;

        }

        protected abstract T CloneLower ();
        protected abstract T CloneUpper ();

        public abstract T WrapToRange (T value);

        public virtual T TrimToRange (T value) {

            var trimmed = value;

            if (value.CompareTo (this.upper) > 0) {

                trimmed = this.CloneUpper ();

            } else if (value.CompareTo (this.lower) < 0) {

                trimmed = this.CloneLower ();

            }

            return trimmed;

        }

        public virtual bool IsInRange (T value) => ((this.upper.CompareTo (value) >= 0) && (this.lower.CompareTo (value) <= 0));

    }
}