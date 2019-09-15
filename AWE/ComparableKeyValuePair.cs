using System;

namespace AWE {

    public class ComparableKeyValuePair<TKey, TValue> : IComparable<ComparableKeyValuePair<TKey, TValue>>, IComparable<TKey> where TKey : IComparable<TKey> {

        public TKey key { get; }
        public TValue value { get; }

        public ComparableKeyValuePair (TKey key, TValue value) {

            this.key = key;
            this.value = value;

        }

        public int CompareTo (object other) {

            int comparison;

            if (other is ComparableKeyValuePair<TKey, TValue> otherItem) {

                comparison = this.CompareTo (otherItem);

            } else if (other is TKey otherKey) {

                comparison = this.CompareTo (otherKey);

            } else {

                throw new ArgumentException (
                    "Object is not a binary search tree item or key. "
                    + other.GetType ().ToString () + " other"
                );

            }

            return comparison;

        }

        public int CompareTo (ComparableKeyValuePair<TKey, TValue> other) => this.key.CompareTo (other.key);

        public int CompareTo (TKey other) => this.key.CompareTo (other);

    }
}