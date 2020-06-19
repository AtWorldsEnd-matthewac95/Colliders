using AWE.CollectionExtensions;
using System;
using System.Collections.ObjectModel;

namespace AWE.Math {

    public class CenterBounds2DBuilder : IGradient<Bounds2D> {

        public static CenterBounds2DBuilder operator + (CenterBounds2DBuilder builder, pair2f point) => builder.Add (point);
        public static CenterBounds2DBuilder operator + (CenterBounds2DBuilder builder, ReadOnlyCollection<pair2f> points) => builder.AddAll (points);

        private float xtotal;
        private float ytotal;
        private int count;

        protected float right { get; private set; }
        protected float top { get; private set; }
        protected float left { get; private set; }
        protected float bottom { get; private set; }

        Bounds2D IGradient<Bounds2D>.current => this.ToBounds ();
        public Bounds2D currentBounds => this.ToBounds ();
        public pair2f currentCenter => this.ToCenter ();
        public bool isEmpty { get; private set; }

        public CenterBounds2DBuilder () => this.Clear ();
        public CenterBounds2DBuilder (pair2f point) => this.Add (point);
        public CenterBounds2DBuilder (ReadOnlyCollection<pair2f> points) => this.AddAll (points);

        public Bounds2D ToBounds () => (this.isEmpty ? new Bounds2D (0f, 0f, 0f, 0f) : new Bounds2D (this.right, this.top, this.left, this.bottom));
        public pair2f ToCenter () => (this.isEmpty ? pair2f.origin : new pair2f ((this.xtotal / this.count), (this.ytotal / this.count)));

        public CenterBounds2DBuilder Add (pair2f point) {

            this.BeforeAdd (point);

            this.isEmpty = false;

            this.right = System.Math.Max (this.right, point.x);
            this.top = System.Math.Max (this.top, point.y);
            this.left = System.Math.Max (this.left, point.x);
            this.bottom = System.Math.Max (this.bottom, point.y);
            this.xtotal += point.x;
            this.ytotal += point.y;

            this.AfterAdd (point);

            return this;

        }

        public CenterBounds2DBuilder AddAll (ReadOnlyCollection<pair2f> points) => this.AddSome (points, 0, points.Count);
        public CenterBounds2DBuilder AddAll (ACyclicIndexIterator<pair2f> points) => this.AddSome (points, 0, points.count);
        public CenterBounds2DBuilder AddSome (ReadOnlyCollection<pair2f> points, int startAt, int stopBefore) => this.AddSome (points.GetCyclicIterator (), startAt, stopBefore);
        public CenterBounds2DBuilder AddSome (ACyclicIndexIterator<pair2f> points, int startAt, int stopBefore) {

            for (var iterator = points.Copy (startingIndex: startAt); iterator.currentIndex < stopBefore; iterator++) {

                this.Add (iterator.current);

            }

            return this;

        }

        public CenterBounds2DBuilder Clear () {

            this.BeforeClear ();

            this.isEmpty = true;

            this.right = Single.NegativeInfinity;
            this.top = Single.NegativeInfinity;
            this.left = Single.PositiveInfinity;
            this.bottom = Single.PositiveInfinity;
            this.xtotal = 0f;
            this.ytotal = 0f;
            this.count = 0;

            this.AfterClear ();

            return this;

        }

        protected virtual void BeforeAdd (pair2f point) {}
        protected virtual void AfterAdd (pair2f point) {}
        protected virtual void BeforeClear () {}
        protected virtual void AfterClear () {}

    }
}
