using AWE.CollectionExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AWE.Math {

    public sealed class DirectionSpectrum2D : IReadOnlyCollection<pair2f> {

        static DirectionSpectrum2D () {

            clockwise = new DirectionSpectrum2D (
                new pair2f (1f, 0f),
                new pair2f (1f, -1f),
                new pair2f (0f, -1f),
                new pair2f (-1f, -1f),
                new pair2f (-1f, 0f),
                new pair2f (-1f, 1f),
                new pair2f (0f, 1f),
                new pair2f (1f, 1f)
            );

            counterclockwise = new DirectionSpectrum2D (
                new pair2f (1f, 0f),
                new pair2f (1f, 1f),
                new pair2f (0f, 1f),
                new pair2f (-1f, 1f),
                new pair2f (-1f, 0f),
                new pair2f (-1f, -1f),
                new pair2f (0f, -1f),
                new pair2f (1f, -1f)
            );

        }

        public static DirectionSpectrum2D clockwise { get; }
        public static DirectionSpectrum2D counterclockwise { get; }

        public ReadOnlyCollection<pair2f> collection { get; private set; }
        int IReadOnlyCollection<pair2f>.Count => this.count;
        public int count => this.collection.Count;

        public pair2f this[int index] => this.collection[index];

        internal DirectionSpectrum2D (params pair2f[] array) : this (Array.AsReadOnly (array)) {}
        internal DirectionSpectrum2D (List<pair2f> list) : this (list.AsReadOnly ()) {}
        internal DirectionSpectrum2D (ReadOnlyCollection<pair2f> collection) {

            this.collection = collection;

        }

        IEnumerator IEnumerable.GetEnumerator () => this.GetEnumerator ();
        public IEnumerator<pair2f> GetEnumerator () => this.collection.GetEnumerator ();

        public CyclicCollectionIterator<pair2f> CreateCyclicIterator () => this.collection.GetCyclicIterator ();

    }
}
