using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AWE {

    public class LinkedList<T> {

        internal LinkedListPosition<T> front { get; private set; }
        internal LinkedListPosition<T> end { get; private set; }

        public T frontValue => this.front.value;
        public T endValue => this.end.value;
        public bool isEmpty => !this.front.hasValue;

        public T this [LinkedListPosition<T> position] => this.Get (position);

        public LinkedList () {

            // TODO - I don't think this is needed actually? Because all types are wrapped by Node classes, which are nullable?...
            if (default(T) != null) {

                throw new AweTypeArgumentException ("T must be a nullable type for LinkedList<T>. ");

            }

            var nullpos = new LinkedListPosition<T> ();

            this.front = nullpos;
            this.end = nullpos;

        }

        internal LinkedList (LinkedListNode<T> node) : this () {

            var position = node.AsPosition ();

            this.front = position;
            this.end = position;

        }

        public LinkedList (params T[] values) : this (Array.AsReadOnly (values)) {}

        public LinkedList (ReadOnlyCollection<T> values) : this () {

            for (int i = 0; i < values.Count; i++) {

                this.AddToEnd (values[i]);

            }
        }

        public LinkedList (ReadOnlyCollection<T> values, Func<T, bool> addCondition) : this () {

            for (int i = 0; i < values.Count; i++) {

                var value = values[i];

                if (addCondition (value)) {

                    this.AddToEnd (value);

                }
            }
        }

        public LinkedList (IEnumerable<T> values) : this () {

            foreach (var value in values) {

                this.AddToEnd (value);

            }
        }

        public LinkedList (IEnumerable<T> values, Func<T, bool> addCondition) : this () {

            foreach (var value in values) {

                if (addCondition (value)) {

                    this.AddToEnd (value);

                }
            }
        }

        public LinkedListPosition<T> CopyFrontPosition () => new LinkedListPosition<T> (this.front.current);
        public LinkedListPosition<T> CopyEndPosition () => new LinkedListPosition<T> (this.end.current);

        public LinkedListPosition<T> AddToEnd (T value) {

            var node = new LinkedListNode<T> (this, value);

            if (this.end.current == null) {

                if (this.end.previous == null) {

                    this.front.current = node;

                }

                this.end.current = node;

            } else {

                this.end.current.AddNext (node, out bool updateEnd);

                if (updateEnd) {

                    this.end = node.AsPosition ();

                }
            }

            return node.AsPosition ();

        }

        public LinkedListPosition<T> AddToFront (T value) {

            var node = new LinkedListNode<T> (this, value);

            if (this.front.current == null) {

                if (this.front.next == null) {

                    this.end.current = node;

                }

                this.front.current = node;

            } else {

                this.front.current.AddPrevious (node, out bool updateFront);

                if (updateFront) {

                    this.front = node.AsPosition ();

                }
            }

            return node.AsPosition ();

        }

        public LinkedListPosition<T> AddAsNext (T value, LinkedListPosition<T> position) {

            LinkedListNode<T> node = null;

            if (position.current.parent == this) {

                node = new LinkedListNode<T> (this, value);
                position.current.AddNext (node, out bool updateEnd);

                if (updateEnd) {

                    this.end = node.AsPosition ();

                }
            }

            return node?.AsPosition ();

        }

        public LinkedListPosition<T> AddAsPrevious (T value, LinkedListPosition<T> position) {

            LinkedListNode<T> node = null;

            if (position.current.parent == this) {

                node = new LinkedListNode<T> (this, value);
                position.current.AddPrevious (node, out bool updateFront);

                if (updateFront) {

                    this.front = node.AsPosition ();

                }
            }

            return node?.AsPosition ();

        }

        public bool Contains (LinkedListPosition<T> position) => ((position != null) && (position.current.parent == this));

        public T Get (LinkedListPosition<T> position, T nullval = default) {

            var value = nullval;

            if ((position.current.parent == this) && position.hasValue) {

                value = position.value;

            }

            return value;

        }

        public T GetNextOrFront (LinkedListPosition<T> position, T nullval = default) {

            var value = nullval;

            if (position.current.parent == this) {

                value = position.current.GetNextOrFront ().value;

            }

            return value;

        }

        public T GetPreviousOrEnd (LinkedListPosition<T> position, T nullval = default) {

            var value = nullval;

            if (position.current.parent == this) {

                value = position.current.GetPreviousOrEnd ().value;

            }

            return value;

        }

        public T RemoveFront (T nullval = default) {

            var value = nullval;

            if (this.front.current != null) {

                value = this.front.current.value;
                this.front.current.Remove ();

            }

            return value;

        }

        public T RemoveEnd (T nullval = default) {

            var value = nullval;

            if (this.end.current != null) {

                value = this.end.current.value;
                this.end.current.Remove ();

            }

            return value;

        }

        public T Remove (LinkedListPosition<T> position, T nullval = default) {

            var value = nullval;

            if ((position != null) && position.hasValue && (position.current.parent == this)) {

                value = position.current.value;
                position.current.Remove ();

            }

            return value;

        }

        public void RemoveAll (ReadOnlyCollection<LinkedListPosition<T>> positions) {

            for (int i = 0; i < positions.Count; i++) {

                if (this.Contains (positions[i])) {

                    this.Remove (positions[i]);

                }
            }
        }

        public void RemoveByCondition (
            Func<T, bool> condition,
            ReadOnlyCollection<LinkedListPosition<T>> positionsToCheck = null
        ) {

            if (positionsToCheck == null) {

                for (var iterator = this.CopyFrontPosition (); iterator.hasValue; iterator++) {

                    if (condition (iterator.current.value)) {

                        var current = iterator.current;
                        iterator.current = current.previous;
                        current.Remove ();

                    }
                }

            } else {

                var count = positionsToCheck.Count;

                for (int i = 0; i < count; i++) {

                    var node = positionsToCheck[i].current;

                    if ((node != null)
                        && (node.parent == this)
                        && (condition (node.value))
                    ) {

                        node.Remove ();

                    }
                }
            }
        }

        public void RemoveByCondition (
            Func<T, bool> condition,
            out List<T> removed,
            ReadOnlyCollection<LinkedListPosition<T>> positionsToCheck = null
        ) {

            if (positionsToCheck == null) {

                removed = new List<T> ();

                for (var iterator = this.CopyFrontPosition (); iterator.hasValue; iterator++) {

                    if (condition (iterator.current.value)) {

                        var current = iterator.current;
                        iterator.current = current.previous;

                        removed.Add (current.value);
                        current.Remove ();

                    }
                }

            } else {

                var count = positionsToCheck.Count;
                removed = new List<T> (count);

                for (int i = 0; i < count; i++) {

                    var node = positionsToCheck[i].current;

                    if ((node != null)
                        && (node.parent == this)
                        && (condition (node.value))
                    ) {

                        removed.Add (node.value);
                        node.Remove ();

                    } else {

                        removed.Add (default);

                    }
                }
            }
        }

        public T RemoveExtrema<TComparable> (
            Func<T, TComparable> function
        ) where TComparable : IComparable {

            var iterator = this.CopyFrontPosition ();
            var maxima = (key: function (iterator.value), position: iterator.Copy ());

            for (iterator++; iterator.hasValue; iterator++) {

                var key = function (iterator.value);

                if (key.CompareTo (maxima.key) > 0) {

                    maxima.key = key;
                    maxima.position = iterator.Copy ();

                }
            }

            maxima.position.current.Remove ();
            return maxima.position.value;

        }

        public T RemoveExtrema<TComparable> (
            Func<T, TComparable> function,
            out T inverse,
            T nullval = default
        ) where TComparable : IComparable {

            var iterator = this.CopyFrontPosition ();
            var greatest = (key: function (iterator.value), position: iterator.Copy ());
            var least = (key: function (iterator.value), position: iterator.Copy ());

            for (iterator++; iterator.hasValue; iterator++) {

                var key = function (iterator.value);

                if (key.CompareTo (greatest.key) > 0) {

                    greatest.key = key;
                    greatest.position = iterator.Copy ();

                } else if (key.CompareTo (least.key) < 0) {

                    least.key = key;
                    least.position = iterator.Copy ();

                }
            }

            if (greatest.position.Equals (least.position)) {

                inverse = nullval;

            } else {

                inverse = least.position.value;
                least.position.current.Remove ();

            }

            greatest.position.current.Remove ();
            return greatest.position.value;

        }

        public T RemoveExtrema<TComparable> (
            Func<T, TComparable> function,
            TComparable maximaThreshold,
            T nullval = default
        ) where TComparable : IComparable {

            var value = nullval;
            (TComparable key, LinkedListPosition<T> position) maxima = (maximaThreshold, null);

            for (var iterator = this.CopyFrontPosition (); iterator.hasValue; iterator++) {

                var key = function (iterator.value);

                if (key.CompareTo (maxima.key) > 0) {

                    maxima.key = key;
                    maxima.position = iterator.Copy ();

                }
            }

            if (maxima.position != null) {

                value = maxima.position.current.value;
                maxima.position.current.Remove ();

            }

            return value;

        }

        public T RemoveExtrema<TComparable> (
            Func<T, TComparable> function,
            out T inverse,
            TComparable maximaThreshold,
            TComparable minimaThreshold,
            T nullval = default
        ) where TComparable : IComparable {

            var value = nullval;
            (TComparable key, LinkedListPosition<T> position) maxima = (maximaThreshold, null);
            (TComparable key, LinkedListPosition<T> position) minima = (minimaThreshold, null);

            for (var iterator = this.CopyFrontPosition (); iterator.hasValue; iterator++) {

                var key = function (iterator.value);

                if (key.CompareTo (maxima.key) > 0) {

                    maxima.key = key;
                    maxima.position = iterator.Copy ();

                } else if (key.CompareTo (minima.key) < 0) {

                    minima.key = key;
                    minima.position = iterator.Copy ();

                }
            }

            if ((minima.position == null) || (minima.position.Equals (maxima.position))) {

                inverse = nullval;

            } else {

                inverse = minima.position.value;
                minima.position.current.Remove ();

            }

            if (maxima.position != null) {

                value = maxima.position.value;
                maxima.position.current.Remove ();

            }

            return value;

        }

        protected virtual void Sort (Comparison<T> comparison, LinkedListPosition<T> sortBegin, LinkedListPosition<T> sortEnd) {

            

        }

        public List<T> AsList () {

            List<T> list = null;

            if (this.front.current != null) {

                list = new List<T> ();

                for (var iterator = this.CopyFrontPosition (); iterator.hasValue; iterator++) {

                    list.Add (iterator.current.value);

                }
            }

            return list;

        }

        public CyclicLinkedListIterator<T> GetCyclicIterator () => new CyclicLinkedListIterator<T> (this);

    }
}