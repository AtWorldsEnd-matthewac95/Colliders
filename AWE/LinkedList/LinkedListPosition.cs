using System;

namespace AWE {

    public class LinkedListPosition<T> : IEquatable<LinkedListPosition<T>> {

        public static LinkedListPosition<T> operator++ (LinkedListPosition<T> position) {

            position.MoveNext ();
            return position;

        }

        public static LinkedListPosition<T> operator-- (LinkedListPosition<T> position) {

            position.MovePrevious ();
            return position;

        }

        private LinkedListNode<T> _previous;
        private LinkedListNode<T> _next;

        public bool hasValue => (this.current != null);
        public T value => this.current.value;

        internal LinkedListNode<T> current { get; set; }

        internal LinkedListNode<T> previous {

            get {

                if (this.current != null) {

                    this._previous = this.current.previous;

                }

                return this._previous;

            }
        }

        internal LinkedListNode<T> next {

            get {

                if (this.current != null) {

                    this._next = this.current.next;

                }

                return this._next;

            }
        }

        public LinkedListPosition () {

            this.current = null;
            this._previous = null;
            this._next = null;

        }

        internal LinkedListPosition (LinkedListNode<T> node) {

            this.current = node;

            // the public getters will assign the values to these fields as needed.
            this._previous = null;
            this._next = null;

        }

        public LinkedListPosition<T> Copy () => new LinkedListPosition<T> (this.current);

        public override bool Equals (object other) {

            var isEqual = false;

            if (other is LinkedListPosition<T> otherPosition) {

                isEqual = this.Equals (otherPosition);

            }

            return isEqual;

        }

        public override int GetHashCode () {

            int basePrime = 67;
            int multPrime = 211;

            unchecked {

                var hashCode = basePrime;
                hashCode = ((hashCode * multPrime) ^ this.current.GetHashCode ());
                hashCode = ((hashCode * multPrime) ^ this.value.GetHashCode ());

                return hashCode;

            }
        }

        public bool Equals (LinkedListPosition<T> other) {

            bool isEqual = false;

            if (other != null) {

                if (this.current == null) {

                    isEqual = (other.current == null);

                } else {

                    isEqual = this.current.Equals (other.current);

                }
            }

            return isEqual;

        }

        public bool MoveNext () {

            this._previous = this.current;

            if (this.current == null) {

                this.current = this._next;

            } else {

                this.current = this.current.next;

            }

            this._next = this.current?.next;

            return (this.current != null);

        }

        public bool MovePrevious () {

            this._next = this.current;

            if (this.current == null) {

                this.current = this._previous;

            } else {

                this.current = this.current.previous;

            }

            this._previous = this.current?.previous;

            return (this.current != null);

        }
    }
}