using System;

namespace AWE.Moving {

    public struct TransformStateIndex : IComparable, IComparable<TransformStateIndex> {

        public static bool operator < (TransformStateIndex i, TransformStateIndex j) => ((i.transform == j.transform) ? (i.CompareTo (j) < 0) : false);

        public static bool operator > (TransformStateIndex i, TransformStateIndex j) => ((i.transform == j.transform) ? (i.CompareTo (j) > 0) : false);

        public static bool operator <= (TransformStateIndex i, TransformStateIndex j) => ((i.transform == j.transform) ? (i.CompareTo (j) <= 0) : false);

        public static bool operator >= (TransformStateIndex i, TransformStateIndex j) => ((i.transform == j.transform) ? (i.CompareTo (j) >= 0) : false);

        private readonly IReadOnlyTransform transform;
        private readonly DateTime time;

        public TransformStateIndex (IReadOnlyTransform transform) : this () {

            this.transform = transform;
            this.time = DateTime.Now;

        }

        public int CompareTo (object other) {

            int result;

            if (other is TransformStateIndex otherId) {

                result = this.CompareTo (otherId);

            } else {

                throw new ArgumentException (
                    "Object is not a TransformStateIndex. "
                    + other.GetType ().ToString () + " other"
                );

            }

            return result;

        }

        public int CompareTo (TransformStateIndex other) => this.time.CompareTo (other.time);

        public bool IsSameTransform (IReadOnlyTransform transform) => (this.transform.Equals (transform));

    }
}