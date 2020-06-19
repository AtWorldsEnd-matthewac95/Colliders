using System;

namespace AWE {

    internal class LinkedListNode<T> : IEquatable<LinkedListNode<T>> {

        private const string DEFAULT_ORPHAN_EXCEPTION_MESSAGE = "This node does not have a parent. ";

        public LinkedList<T> parent { get; protected set; }

        public virtual T value { get; internal set; }
        public virtual LinkedListNode<T> next { get; protected set; }
        public virtual LinkedListNode<T> previous { get; protected set; }

        public LinkedListNode (LinkedList<T> parent, T value = default) {

            this.parent = parent;
            this.value = value;
            this.next = null;
            this.previous = null;

        }

        internal LinkedListNode (
            LinkedList<T> parent,
            T value,
            LinkedListNode<T> next,
            LinkedListNode<T> previous
        ) {

            this.parent = parent;
            this.value = value;

            this.SetNext (next);
            this.SetPrevious (previous);

        }

        public LinkedListNode<T> GetNextOrFront () => (next ?? parent.front.current);

        public LinkedListNode<T> GetPreviousOrEnd () => (previous ?? parent.end.current);

        internal void AddNext (T value) => this.AddNext (value, out _);
        internal void AddNext (T value, out bool updateParentEnd) => this.AddNext (
            new LinkedListNode<T> (this.parent, value),
            out updateParentEnd
        );
        internal void AddNext (LinkedListNode<T> node) => this.AddNext (node, out _);
        internal void AddNext (LinkedListNode<T> node, out bool updateParentEnd) {

            if (this.parent == null) {

                throw new AweOrphanedNodeException (DEFAULT_ORPHAN_EXCEPTION_MESSAGE);

            }

            if (this.next != null) {

                this.next.previous = node;
                node.next = this.next;

            }

            this.next = node;
            node.previous = this;

            updateParentEnd = (this.parent.end.current == this);

        }

        internal void AddPrevious (T value) => this.AddPrevious (value, out _);
        internal void AddPrevious (T value, out bool updateParentFront) => this.AddPrevious (
            new LinkedListNode<T> (this.parent, value),
            out updateParentFront
        );
        internal void AddPrevious (LinkedListNode<T> node) => this.AddPrevious (node, out _);
        internal void AddPrevious (LinkedListNode<T> node, out bool updateParentFront) {

            if (this.parent == null) {

                throw new AweOrphanedNodeException (DEFAULT_ORPHAN_EXCEPTION_MESSAGE);

            }

            if (this.previous != null) {

                this.previous.next = node;
                node.previous = this.previous;

            }

            this.previous = node;
            node.next = this;

            updateParentFront = (this.parent.front.current == this);

        }

        internal void SetNext (LinkedListNode<T> node) => this.SetNext (node, out _);
        internal void SetNext (LinkedListNode<T> node, out bool updateParentEnd) {

            if (this.parent == null) {

                throw new AweOrphanedNodeException (DEFAULT_ORPHAN_EXCEPTION_MESSAGE);

            }

            if (this.next == null) {

                this.AddNext (node, out updateParentEnd);

            } else {

                this.next.AddNext (node, out updateParentEnd);
                this.next.Remove ();

            }
        }

        internal void SetPrevious (LinkedListNode<T> node) => this.SetPrevious (node, out _);
        internal void SetPrevious (LinkedListNode<T> node, out bool updateParentFront) {

            if (this.parent == null) {

                throw new AweOrphanedNodeException (DEFAULT_ORPHAN_EXCEPTION_MESSAGE);

            }

            if (this.previous == null) {

                this.AddPrevious (node, out updateParentFront);

            } else {

                this.previous.AddPrevious (node, out updateParentFront);
                this.previous.Remove ();

            }
        }

        internal BooleanNote SetParent (LinkedList<T> parent) {

            var note = new BooleanNote (false, "This node already has a parent. ");

            if (this.parent == null) {

                this.parent = parent;
                note = new BooleanNote (true);

            }

            return note;

        }

        internal LinkedList<T> Remove () {

            if (this.next != null) {

                this.next.previous = this.previous;

            }

            if (this.previous != null) {

                this.previous.next = this.next;

            }

            if (this.parent != null) {

                if (this.parent.front.current == this) {

                    this.parent.front.current = (this.previous ?? this.next);

                }

                if (this.parent.end.current == this) {

                    this.parent.end.current = (this.next ?? this.previous);

                }
            }

            this.next = null;
            this.previous = null;

            var parent = this.parent;
            this.parent = null;
            return parent;

        }

        public LinkedListPosition<T> AsPosition () => new LinkedListPosition<T> (this);

        public override bool Equals (object other) {

            var isEqual = false;

            if (other is LinkedListNode<T> otherNode) {

                isEqual = this.Equals (otherNode);

            }

            return isEqual;

        }

        public override int GetHashCode () {

            int basePrime = 127;
            int multPrime = 199;

            unchecked {

                var hashCode = basePrime;
                hashCode = ((hashCode * multPrime) ^ base.GetHashCode ());
                hashCode = ((hashCode * multPrime) ^ this.parent.GetHashCode ());
                hashCode = ((hashCode * multPrime) ^ this.value.GetHashCode ());

                return hashCode;

            }
        }

        public bool Equals (LinkedListNode<T> other) {

            var isParentEqual = (this.parent == other.parent);
            var isValueEqual = (this.value.Equals (other.value));
            var isNextEqual = (this.next == other.next);
            var isPreviousEqual = (this.previous == other.previous);

            return (
                isParentEqual
                && isValueEqual
                && isNextEqual
                && isPreviousEqual
            );

        }
    }
}