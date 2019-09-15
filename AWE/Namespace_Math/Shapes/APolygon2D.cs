using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AWE.CollectionExtensions;

namespace AWE.Math {

    public abstract class APolygon2D : IPolygon2D {

        public abstract ReadOnlyCollection<pair2f> unoffsetVerticies { get; }
        public abstract Bounds2D bounds { get; }
        public abstract pair2f center { get; }

        public virtual bool isConvex => SShapeMath.IsConvex (this.unoffsetVerticies);

        int IReadOnlyCollection<pair2f>.Count => this.count;
        public int count => this.unoffsetVerticies.Count;


        public virtual pair2f this [int index] => this.unoffsetVerticies[index];
        public Polygon2DEdge this [int a, int b] => this[new IntPair (a, b)];
        public virtual Polygon2DEdge this [IntPair indicies]=> new Polygon2DEdge (
            this,
            indicies
        );

        protected abstract APolygon2D _CreateOffset (pair2f offset);
        public abstract bool IsContainingPoint (pair2f point);

        IShape2D IShape2D.CreateOffset (pair2f offset) => this.CreateOffset (offset);
        IPolygon2D IPolygon2D.CreateOffset (pair2f offset) => this.CreateOffset (offset);
        public APolygon2D CreateOffset (pair2f offset) => this._CreateOffset (offset);

        public bool IsIndexValid (int index) => ((index >= 0) && (index < this.count));

        public int GetNextIndex (int index) => (((index + 1) < this.count) ? (index + 1) : 0);

        public int GetPreviousIndex (int index) => ((index == 0) ? (this.count - 1) : (index - 1));

        public Polygon2DEdge GetEdge (int index) => this[index, this.GetNextIndex (index)];

        public virtual List<pair2f> CreateVertexList () => new List<pair2f> (unoffsetVerticies);

        public virtual List<Line2D> CreateLineList () {

            var lines = new List<Line2D> ();

            for (var edgeIterator = this.CreateEdgeIterator (); edgeIterator.cycles == 0; edgeIterator++) {

                lines.Add (new Line2D (edgeIterator.current));

            }

            return lines;

        }

        List<ICurve2D> IShape2D.CreateCurveList () {

            var curves = new List<ICurve2D> ();
            var lines = this.CreateLineList ();

            for (int i = 0; i < lines.Count; i++) {

                curves.Add (lines[i] as ICurve2D);

            }

            return curves;

        }

        public virtual List<Polygon2DEdge> CreateEdgeList () {

            var edges = new List<Polygon2DEdge> ();

            for (var edgeIterator = this.CreateEdgeIterator (); edgeIterator.cycles == 0; edgeIterator++) {

                edges.Add (edgeIterator.current);

            }

            return edges;

        }

        List<ICurve2DSegment> IShape2D.CreateCurveSegmentList () {

            var csegments = new List<ICurve2DSegment> ();
            var lsegments = this.CreateEdgeList ();

            for (int i = 0; i < lsegments.Count; i++) {

                csegments.Add (lsegments[i] as ICurve2DSegment);

            }

            return csegments;

        }

        public virtual List<IntPair> CreateEdgeIndexList () {

            var indicies = new List<IntPair> ();

            int i;
            for (i = 1; i < this.count; i++) {

                indicies.Add (new IntPair ((i - 1), i));

            }
            indicies.Add (new IntPair ((i - 1), 0));

            return indicies;

        }

        public virtual ACyclicIndexIterator<pair2f> CreateVertexIterator (int startingIndex = 0) => new CyclicDelegateIterator<pair2f> (
            (index => this[index]),
            (() => this.count),
            startingIndex
        );

        public virtual ACyclicIndexIterator<Polygon2DEdge> CreateEdgeIterator (int startingIndex = 0) => new CyclicDelegateIterator<Polygon2DEdge> (
            (index => this.GetEdge (index)),
            (() => this.count),
            startingIndex
        );

        public virtual List<pair2f> GetRangeOfPoints (int start, int end, List<pair2f> pointList = null) {

            if (pointList == null) {

                pointList = new List<pair2f> ();

            }

            for (int i = start; i != end; i = this.GetNextIndex (i)) {

                pointList.Add (this[i]);

            }

            return pointList;

        }

        public virtual bool AddRangeOfPointsToList (
            List<pair2f> list,
            int start,
            int end,
            DPointConditional SkipCondition = null,
            DPointConditional StartCondition = null,
            DPointConditional ExitCondition = null
        ) {

            var success = false;

            bool IsInRange (int i) => (i != end);

            SkipCondition = (SkipCondition ?? (p => false));
            StartCondition = (StartCondition ?? (p => true));
            ExitCondition = (ExitCondition ?? (p => false));

            if (this.IsIndexValid (start) && this.IsIndexValid (end)) {

                var index = start;

                while (IsInRange (index) && SkipCondition (this[index])) {

                    index = this.GetNextIndex (index);

                }

                if (IsInRange (index) && StartCondition (this[index])) {

                    success = true;

                    while (IsInRange (index) && !ExitCondition (this[index])) {

                        if (!SkipCondition (this[index])) {

                            list.Add (this[index]);

                        }

                        index = this.GetNextIndex (index);

                    }
                }
            }

            return success;

        }

        public virtual bool IsIntersecting (IPolygon2D other) {

            var isIntersecting = false;

            for (var myIterator = this.CreateEdgeIterator (); myIterator.cycles == 0; myIterator++) {

                for (var otherIterator = other.CreateEdgeIterator (); otherIterator.cycles == 0; otherIterator++) {

                    isIntersecting = myIterator.current.IsIntersecting (otherIterator.current);

                    if (isIntersecting) {

                        break;

                    }
                }

                if (isIntersecting) {

                    break;

                }
            }

            if (!isIntersecting) {

                isIntersecting = this.IsContainingPoint (other[0]);

                if (!isIntersecting) {

                    isIntersecting = other.IsContainingPoint (this[0]);

                }
            }

            return isIntersecting;

        }

        public override string ToString () => this.ToCommaSeperatedString ();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator ();
        public IEnumerator<pair2f> GetEnumerator () {

            for (int i = 0; i < this.count; i++) {

                yield return this[i];

            }
        }
    }
}