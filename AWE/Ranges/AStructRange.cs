using System;

namespace AWE {

    public abstract class AStructRange<TStruct> : ARange<TStruct> where TStruct : struct, IComparable<TStruct> {

        public AStructRange (TStruct lower, TStruct upper) : base (lower, upper) {}

        protected override TStruct CloneLower () => this.lower;
        protected override TStruct CloneUpper () => this.upper;

    }
}