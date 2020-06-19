using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AWE.CollectionExtensions {

    public static class SCollectionExtensions {

        public static bool IsNullOrEmpty (this IEnumerable enumerable) {

            bool isNullOrEmpty;

            if (enumerable == null) {

                isNullOrEmpty = true;

            } else {

                if (enumerable is ICollection) {

                    var collection = (enumerable as ICollection);
                    isNullOrEmpty = (collection.Count <= 0);

                } else {

                    isNullOrEmpty = !((IEnumerable<object>)enumerable).Any ();

                }
            }

            return isNullOrEmpty;

        }

        public static List<T> AddAll<T> (this List<T> destination, ReadOnlyCollection<T> source) => AddSome (destination, source, 0, source.Count);

        public static List<T> AddSome<T> (this List<T> destination, ReadOnlyCollection<T> source, int startAt, int stopBefore) {

            for (int i = startAt; i < stopBefore; i++) {

                destination.Add (source[i]);

            }

            return destination;

        }

        internal static List<T> RemoveSeveralAt<T> (this List<T> list, params int[] indicies) {

            var removed = new List<T> ();

            Array.Sort (indicies, (x, y) => -x.CompareTo (y));
            int index = -1, previndex = -1;

            for (int i = 0; i < indicies.Length; i++) {

                index = indicies[i];

                if ((index < list.Count) && (index != previndex)) {

                    removed.Add (list[index]);
                    list.RemoveAt (index);

                }

                previndex = index;

            }

            return removed;

        }

        internal static List<T> RemoveSeveralAt<T> (this List<T> list, List<int> indicies) {

            var removed = new List<T> ();

            indicies.Sort ((x, y) => -x.CompareTo (y));
            int index = -1, previndex = -1;

            for (int i = 0; i < indicies.Count; i++) {

                index = indicies[i];

                if ((index < list.Count) && (index != previndex)) {

                    removed.Add (list[index]);
                    list.RemoveAt (index);

                }

                previndex = index;

            }

            return removed;

        }

        public static string ToCommaSeperatedString<T> (this IEnumerable<T> enumerable)
            => String.Format ("[{0}]", String.Join (", ", enumerable));

        public static void Push<T> (this List<T> list, T item) => list.Add (item);

        public static T Pop<T> (this List<T> list) {

            var item = default(T);
            var lastIndex = (list.Count - 1);

            if (lastIndex >= 0) {

                item = list[lastIndex];
                list.RemoveAt (lastIndex);

            }

            return item;

        }

        public static T Peek<T> (this List<T> list) {

            var item = default(T);
            var lastIndex = (list.Count - 1);

            if (lastIndex >= 0) {

                item = list[lastIndex];

            }

            return item;

        }

        public static CyclicCollectionIterator<T> GetCyclicIterator<T> (this ReadOnlyCollection<T> collection)
            => new CyclicCollectionIterator<T> (collection);

        public static ReadOnlyCollection<T> Combine<T> (
            this IReadOnlyList<T> collection,
            IReadOnlyList<T> other,
            Func<T, T, T> CombinationOperation
        ) {

            var combined = new List<T> ();
            var myCount = collection.Count;
            var otherCount = other.Count;

            for (int i = 0; ((i < myCount) || (i < otherCount)); i++) {

                if (i < myCount) {

                    if (i < otherCount) {

                        combined.Add (CombinationOperation (collection[i], other[i]));

                    } else {

                        combined.Add (collection[i]);

                    }

                } else {

                    combined.Add (other[i]);

                }
            }

            return combined.AsReadOnly ();

        }

        public static ReadOnlyCollection<T> Combine<T> (
            this ReadOnlyCollection<T> collection,
            ReadOnlyCollection<T> other,
            Func<T, T, T> CombinationOperation
        ) {

            var combined = new List<T> ();
            var myCount = collection.Count;
            var otherCount = other.Count;

            for (int i = 0; ((i < myCount) || (i < otherCount)); i++) {

                if (i < myCount) {

                    if (i < otherCount) {

                        combined.Add (CombinationOperation (collection[i], other[i]));

                    } else {

                        combined.Add (collection[i]);

                    }

                } else {

                    combined.Add (other[i]);

                }
            }

            return combined.AsReadOnly ();

        }
    }
}
