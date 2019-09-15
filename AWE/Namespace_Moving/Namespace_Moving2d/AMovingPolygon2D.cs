using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AWE.Math;

namespace AWE.Moving.Moving2D {

    public abstract class AMovingPolygon2D<TPolygon2D> : ATransform2DListener, IGradient<TPolygon2D>, IPolygon2D where TPolygon2D : class, IPolygon2D {

        protected ATransform2D transform;
        protected TPolygon2D _current;

        public virtual TPolygon2D current {

            get {

                if (this._current == null) {

                    this._current = this.CreateCurrent ();

                }

                return this._current;

            }
        }

        public AMovingPolygon2D (ATransform2D transform) {

            transform.AddListener (this);
            this.transform = transform;

        }

# region IReadOnlyList
        int IReadOnlyCollection<pair2f>.Count => this.current.count;

        pair2f IReadOnlyList<pair2f>.this[int index] => this.current[index];

        IEnumerator IEnumerable.GetEnumerator () => this.current.GetEnumerator ();
        IEnumerator<pair2f> IEnumerable<pair2f>.GetEnumerator () => this.current.GetEnumerator ();
# endregion

# region IShape2D
        pair2f IShape2D.center => this.current.center;
        bool IShape2D.isConvex => this.current.isConvex;

        IShape2D IShape2D.CreateOffset (pair2f offset) => this.CreateOffset (offset);
        List<pair2f> IShape2D.CreateVertexList () => this.current.CreateVertexList ();
        List<ICurve2D> IShape2D.CreateCurveList () => this.current.CreateCurveList ();
        List<ICurve2DSegment> IShape2D.CreateCurveSegmentList () => this.current.CreateCurveSegmentList ();
        bool IShape2D.IsContainingPoint (pair2f point) => this.current.IsContainingPoint (point);
# endregion

# region IPolygon2D
        Bounds2D IPolygon2D.bounds => this.current.bounds;
        ReadOnlyCollection<pair2f> IPolygon2D.unoffsetVerticies => this.current.unoffsetVerticies;
        int IPolygon2D.count => this.current.count;

        Polygon2DEdge IPolygon2D.this[int a, int b] => this.current[new IntPair (a, b)];
        Polygon2DEdge IPolygon2D.this[IntPair indicies] => this.current[indicies];

        bool IPolygon2D.AddRangeOfPointsToList(
            List<pair2f> list,
            int start,
            int end,
            DPointConditional SkipCondition,
            DPointConditional StartCondition,
            DPointConditional ExitCondition
        ) => this.current.AddRangeOfPointsToList (list, start, end, SkipCondition, StartCondition, ExitCondition);
        IPolygon2D IPolygon2D.CreateOffset (pair2f offset) => this.CreateOffset (offset);
        List<Line2D> IPolygon2D.CreateLineList () => this.current.CreateLineList ();
        List<Polygon2DEdge> IPolygon2D.CreateEdgeList () => this.current.CreateEdgeList ();
        List<IntPair> IPolygon2D.CreateEdgeIndexList () => this.current.CreateEdgeIndexList ();
        ACyclicIndexIterator<Polygon2DEdge> IPolygon2D.CreateEdgeIterator (int startingIndex) => this.current.CreateEdgeIterator (startingIndex);
        ACyclicIndexIterator<pair2f> IPolygon2D.CreateVertexIterator (int startingIndex) => this.current.CreateVertexIterator (startingIndex);
        int IPolygon2D.GetNextIndex (int index) => this.current.GetNextIndex (index);
        int IPolygon2D.GetPreviousIndex (int index) => this.current.GetPreviousIndex (index);
        bool IPolygon2D.IsIntersecting (IPolygon2D other) => this.current.IsIntersecting (other);
# endregion

        protected abstract TPolygon2D CreateCurrent ();

        public abstract TPolygon2D CreateOffset (pair2f offset);

    }
}