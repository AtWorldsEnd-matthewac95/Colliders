using System;
using System.Collections.ObjectModel;

namespace AWE.Math {

    public class Bounds2DBuilder : IBounds2DBuilder<Bounds2D> {

        public static Bounds2DBuilder operator + (Bounds2DBuilder builder, pair2f point) => builder.Add (point);
        public static Bounds2DBuilder operator + (Bounds2DBuilder builder, Bounds2D bounds) => builder.Add (bounds);
        public static Bounds2DBuilder operator + (Bounds2DBuilder builder, ReadOnlyCollection<pair2f> points) => builder.AddAll (points);
        public static Bounds2DBuilder operator + (Bounds2DBuilder builder, ReadOnlyCollection<Bounds2D> bounds) => builder.AddAll (bounds);

        protected float right { get; private set; }
        protected float top { get; private set; }
        protected float left { get; private set; }
        protected float bottom { get; private set; }

        public Bounds2D current => this.ToBounds ();
        public bool isEmpty { get; private set; }

        public Bounds2DBuilder () => this.Clear ();
        public Bounds2DBuilder (pair2f point) => this.Add (point);
        public Bounds2DBuilder (Bounds2D bounds) => this.Add (bounds);
        public Bounds2DBuilder (ReadOnlyCollection<pair2f> points) => this.AddAll (points);
        public Bounds2DBuilder (ReadOnlyCollection<Bounds2D> bounds) => this.AddAll (bounds);

        public Bounds2D ToBounds () => (this.isEmpty ? new Bounds2D (0f, 0f, 0f, 0f) : new Bounds2D (this.right, this.top, this.left, this.bottom));

        public Bounds2DBuilder Add (pair2f point) {

            this.BeforeAdd (point);

            this.isEmpty = false;

            this.right = System.Math.Max (this.right, point.x);
            this.top = System.Math.Max (this.top, point.y);
            this.left = System.Math.Max (this.left, point.x);
            this.bottom = System.Math.Max (this.bottom, point.y);

            this.AfterAdd (point);

            return this;

        }

        IBounds2DBuilder<Bounds2D> IBounds2DBuilder<Bounds2D>.Add (Bounds2D bounds) => this.Add (bounds);
        public Bounds2DBuilder Add (Bounds2D bounds) {

            this.BeforeAdd (bounds);

            this.isEmpty = false;

            this.right = System.Math.Max (this.right, bounds.right);
            this.top = System.Math.Max (this.top, bounds.top);
            this.left = System.Math.Max (this.left, bounds.left);
            this.bottom = System.Math.Max (this.bottom, bounds.bottom);

            this.AfterAdd (bounds);

            return this;

        }

        public Bounds2DBuilder AddAll (ReadOnlyCollection<pair2f> points) {

            for (int i = 0; i < points.Count; i++) {

                this.Add (points[i]);

            }

            return this;

        }

        IBounds2DBuilder<Bounds2D> IBounds2DBuilder<Bounds2D>.AddAll (ReadOnlyCollection<Bounds2D> bounds) => this.AddAll (bounds);
        public Bounds2DBuilder AddAll (ReadOnlyCollection<Bounds2D> bounds) {

            for (int i = 0; i < bounds.Count; i++) {

                this.Add (bounds[i]);

            }

            return this;

        }

        IBounds2DBuilder<Bounds2D> IBounds2DBuilder<Bounds2D>.Clear () => this.Clear ();
        public Bounds2DBuilder Clear () {

            this.BeforeClear ();

            this.isEmpty = true;

            this.right = Single.NegativeInfinity;
            this.top = Single.NegativeInfinity;
            this.left = Single.PositiveInfinity;
            this.bottom = Single.PositiveInfinity;

            this.AfterClear ();

            return this;

        }

        protected virtual void BeforeAdd (pair2f point) {}
        protected virtual void AfterAdd (pair2f point) {}
        protected virtual void BeforeAdd (Bounds2D bounds) {}
        protected virtual void AfterAdd (Bounds2D bounds) {}
        protected virtual void BeforeClear () {}
        protected virtual void AfterClear () {}

    }
}
