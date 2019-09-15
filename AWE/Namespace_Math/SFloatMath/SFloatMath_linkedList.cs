using System;
using System.Collections.ObjectModel;

namespace AWE.Math {

    public static partial class SFloatMath {

        public static pair2f RemoveFromLinkedListByDotProduct (
            LinkedList<pair2f> list,
            pair2f vector
        ) {

            var iterator = list.CopyFrontPosition ();
            var dot = GetDotProduct (iterator.value, vector);
            var furthest = (dot, position: iterator.Copy ());

            while (iterator.MoveNext ()) {

                dot = GetDotProduct (iterator.value, vector);

                if (dot > furthest.dot) {

                    furthest.dot = dot;
                    furthest.position = iterator.Copy ();

                }
            }

            list.Remove (furthest.position);
            return furthest.position.value;

        }

        public static pair2f RemoveFromLinkedListByDotProduct (
            LinkedList<pair2f> list,
            pair2f vector,
            out pair2f opposite
        ) {

            var iterator = list.CopyFrontPosition ();
            var dot = GetDotProduct (iterator.value, vector);
            var furthest = (dot, position: iterator.Copy ());
            var inverse = (dot, position: iterator.Copy ());

            while (iterator.MoveNext ()) {

                dot = GetDotProduct (iterator.value, vector);

                if (dot > furthest.dot) {

                    furthest.dot = dot;
                    furthest.position = iterator.Copy ();

                } else if (dot < inverse.dot) {

                    inverse.dot = dot;
                    inverse.position = iterator.Copy ();

                }
            }

            opposite = inverse.position.value;
            list.Remove (inverse.position);

            list.Remove (furthest.position);
            return furthest.position.value;

        }

        /*
        public static pair2f RemoveFromLinkedListByDotProduct (
            LinkedList<pair2f> list,
            pair2f vector
        ) {

            pair2f value;
            var current = list.head;
            var dot = GetDotProduct (list.head.value, vector);
            (float dot, LinkedListNode<pair2f> node) furthest = (dot, null);

            while (current.next != null) {

                dot = GetDotProduct (current.next.value, vector);

                if (dot > furthest.dot) {

                    furthest.node = current;
                    furthest.dot = dot;

                }
            }

            if (furthest.node == null) {

                value = list.RemoveHead ();

            } else {

                value = list.RemoveNext (furthest.node);

            }

            return value;

        }

        public static pair2f RemoveFromLinkedListByDotProduct (
            LinkedList<pair2f> list,
            pair2f vector,
            out pair2f inverse
        ) {

            pair2f value;
            var current = list.head;
            var dot = GetDotProduct (list.head.value, vector);
            (float dot, LinkedListNode<pair2f> node) furthest = (dot, null);
            (float dot, LinkedListNode<pair2f> node) furthestInverse = (dot, null);

            while (current.next != null) {

                dot = GetDotProduct (current.next.value, vector);

                if (dot > furthest.dot) {

                    furthest.node = current;
                    furthest.dot = dot;

                } else if (dot < furthestInverse.dot) {

                    furthestInverse.node = current;
                    furthestInverse.dot = dot;

                }

                current = current.next;

            }

            if (furthest.node == null) {

                value = list.RemoveHead ();

            } else {

                value = list.RemoveNext (furthest.node);

            }

            if (furthest.node == furthestInverse.node) {

                inverse = value;

            } else {

                if (furthestInverse.node == null) {

                    inverse = list.RemoveHead ();

                } else {

                    inverse = list.RemoveNext (furthestInverse.node);

                }
            }

            return value;

        }

        public static LinkedList<pair2f> RemoveFromLinkedListByDotProduct (
            ReadOnlyCollection<pair2f> values,
            pair2f vector,
            out pair2f removed
        ) {

            var current = values[0];
            var list = new LinkedList<pair2f> (current);
            var dot = GetDotProduct (current, vector);
            (float dot, LinkedListNode<pair2f> node) furthest = (dot, null);

            for (int i = 1; i < values.Count; i++) {

                current = values[i];
                dot = GetDotProduct (current, vector);

                if (dot > furthest.dot) {

                    furthest.node = list.tail;
                    furthest.dot = dot;

                }

                list.AddToEnd (current);

            }

            if (furthest.node == null) {

                removed = list.RemoveHead ();

            } else {

                removed = list.RemoveNext (furthest.node);

            }

            return list;

        }

        public static LinkedList<pair2f> RemoveFromLinkedListByDotProduct (
            ReadOnlyCollection<pair2f> values,
            pair2f vector,
            out pair2f removed,
            out pair2f inverse
        ) {

            var current = values[0];
            var list = new LinkedList<pair2f> (current);
            var dot = GetDotProduct (current, vector);
            (float dot, LinkedListNode<pair2f> node) furthest = (dot, null);
            (float dot, LinkedListNode<pair2f> node) furthestInverse = (dot, null);

            for (int i = 1; i < values.Count; i++) {

                current = values[i];
                dot = GetDotProduct (current, vector);

                if (dot > furthest.dot) {

                    furthest.node = list.tail;
                    furthest.dot = dot;

                } else if (dot < furthestInverse.dot) {

                    furthestInverse.node = list.tail;
                    furthestInverse.dot = dot;

                }

                list.AddToEnd (current);

            }

            if (furthest.node == null) {

                removed = list.RemoveHead ();

            } else {

                removed = list.RemoveNext (furthest.node);

            }

            if (furthest.node == furthestInverse.node) {

                inverse = removed;

            } else {

                if (furthestInverse.node == null) {

                    inverse = list.RemoveHead ();

                } else {

                    inverse = list.RemoveNext (furthestInverse.node);

                }
            }

            return list;

        }
        */
    }
}