using System;

namespace AWE {

    public abstract class ACloneableRange<TCloneable> :  ARange<TCloneable> where TCloneable : ICloneable, IComparable<TCloneable> {

        public ACloneableRange (TCloneable lower, TCloneable upper) : base (lower, upper) {}

        protected override TCloneable CloneLower () => (TCloneable)this.lower.Clone ();
        protected override TCloneable CloneUpper () => (TCloneable)this.upper.Clone ();

    }

}