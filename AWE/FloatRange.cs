using System;

namespace AWE {

    public class FloatRange : AStructRange<float>, ICloneable {

        public static bool operator < (FloatRange range, float value) => (range.upper < value);
        public static bool operator > (FloatRange range, float value) => (range.lower > value);

        public static bool operator <= (FloatRange range, float value) => ((range.upper < value) || (range.lower <= value));
        public static bool operator >= (FloatRange range, float value) => ((range.lower > value) || (range.upper >= value));

        protected float _diff;
        public override float diff => this._diff;

        public FloatRange (float lower, float upper) : base (lower, upper) => this._diff = (upper - lower);

        public override float WrapToRange (float value) => this.WrapToRange (value, false);
        public float WrapToRange (float value, bool backtrack) {

            var wrapped = value;

            if (backtrack) {

                var upper = (this.upper + this._diff);
                var lower = this.lower;
                var diff = (2f * this._diff);

                while (wrapped > upper) {

                    wrapped -= diff;

                }

                while (wrapped < lower) {

                    wrapped += diff;

                }

                wrapped = this.TrimToRange (wrapped, true);

            } else {

                if (wrapped > this.upper) {

                    var multiple = (1 + (int)((value - this.upper) / this._diff));
                    wrapped -= (multiple * this._diff);

                } else if (wrapped < this.lower) {

                    var multiple = (1 + (int)((this.lower - value) / this._diff));
                    wrapped += (multiple * this._diff);

                }
            }

            return wrapped;

        }

        public override float TrimToRange (float value) => this.TrimToRange (value, false);
        public float TrimToRange (float value, bool backtrack) {

            var trimmed = value;

            if (backtrack) {

                if ((trimmed < this.lower) || (trimmed > (this.upper + this._diff))) {

                    trimmed = this.lower;

                } else if (trimmed > this.upper) {

                    trimmed = ((2f * this.upper) - trimmed);

                }

            } else {

                if (trimmed < this.lower) {

                    trimmed = this.lower;

                } else if (trimmed > this.upper) {

                    trimmed = this.upper;

                }
            }

            return trimmed;

        }

        public override bool IsInRange (float value) => ((this.upper >= value) && (this.lower <= value));

        object ICloneable.Clone () => this.Clone ();
        public FloatRange Clone () => new FloatRange (this.lower, this.upper);

    }
}