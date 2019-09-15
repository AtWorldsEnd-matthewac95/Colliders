using System;
using System.Collections;
using System.Collections.Generic;
using AWE.Math.FloatExtensions;

namespace AWE.Math {

    public class Line2DSegment : ILine2DSegment, IPair<pair2f> {

        private readonly Line2DSegment parentSegment;

        pair2f IPair<pair2f>.first => this.tail;
        pair2f IPair<pair2f>.second => this.head;

        public pair2f tail { get; }
        public pair2f head { get; }
        public Line2D parentLine => new Line2D (this.tail, this.head);

        public ICurve2D parent => (
            (this.parentSegment == null)
            ? (ICurve2D)(this.parentLine)
            : (ICurve2D)(this.parentSegment)
        );

        public pair2f difference => (this.head - this.tail);

        public bool isHorizontal => (this.head.y - this.tail.y).IsNegligible ();

        public bool isVertical => (this.head.x - this.tail.x).IsNegligible ();

        int IReadOnlyCollection<pair2f>.Count => 2;

        public pair2f this [int index] {

            get {

                pair2f value;

                if (index == 0) {

                    value = tail;

                } else if (index == 1) {

                    value = head;

                } else {

                    throw new IndexOutOfRangeException ();

                }

                return value;

            }
        }

        public pair2f this [bool getHead] => (getHead ? this.head : this.tail);

        public Line2DSegment (pair2f tail, pair2f head, Line2DSegment parent = null) {

            this.tail = tail;
            this.head = head;
            this.parentSegment = parent;

        }

        public void Deconstruct (out pair2f one, out pair2f two) {

            one = this.tail;
            two = this.head;

        }

        public pair2f GetPoint (float interpolation) => (this.tail + (this.difference * interpolation));

        public bool IsIntersecting (Line2DSegment other, bool disallowParallel = false)
            => SShapeMath.IsLineSegmentIntersectingAnother (this.tail, this.head, other.tail, other.head, disallowParallel);

        public pair2f FindIntersection (Line2DSegment other)
            => SShapeMath.FindIntersectionOfLineSegments (this.tail, this.head, other.tail, other.head);

        public Pair<pair2f> FindParallelIntersections (Line2DSegment other)
            => this.FindParallelIntersections (other, out _);

        // TODO - Why does this function live in Line2DSegment while the other functions live in SFloatMath?
        public Pair<pair2f> FindParallelIntersections (Line2DSegment other, out Pair<EOperationContext> enders) {

            EOperationContext tailend, headend;

            var taildiff = (this.tail - other.tail);
            var headdiff = (this.tail - other.head);
            tailend = ((taildiff.ToDirection () == headdiff.ToDirection ()) ? EOperationContext.Target : EOperationContext.Source);

            taildiff = (this.head - other.tail);
            headdiff = (this.head - other.head);
            headend = ((taildiff.ToDirection () == headdiff.ToDirection ()) ? EOperationContext.Target : EOperationContext.Source);

            enders = new Pair<EOperationContext> (tailend, headend);
            return new Pair<pair2f> (
                ((tailend == EOperationContext.Source) ? this.tail : other.tail),
                ((headend == EOperationContext.Source) ? this.head : other.head)
            );

        }

        public bool IsPointOnLine (pair2f point) {

            bool isWithinSegment = false;
            bool isColinear = false;

            if (this.isVertical) {

                isColinear = (this.head.x - point.x).IsNegligible ();

                if (isColinear) {

                    isWithinSegment = ((this.head.y - point.y).sign() == (point.y - this.tail.y).sign());

                }

            } else {

                var xheadpoint = (this.head.x - point.x);
                var xpointtail = (point.x - this.tail.x);

                isWithinSegment = (xheadpoint.sign() == xpointtail.sign());

                if (isWithinSegment) {

                    var slopehead = ((this.head.y - point.y) / xheadpoint);
                    var slopetail = ((point.y - this.tail.y) / xpointtail);

                    isColinear = (slopehead - slopetail).IsNegligible ();

                }
            }

            return (isWithinSegment && isColinear);

        }


        bool ICurve2D.IsPointOnCurve (pair2f point) => this.IsPointOnLine (point);

        public override string ToString () => String.Format ("({0}, {1})", this.tail.ToString (), this.head.ToString ());

        pair2f IPair<pair2f>.opposite (int index) {

            pair2f value;

            if (index == 0) {

                value = this.head;

            } else if (index == 1) {

                value = this.tail;

            } else {

                throw new IndexOutOfRangeException ();

            }

            return value;

        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator ();
        public IEnumerator<pair2f> GetEnumerator () {

            yield return this.tail;
            yield return this.head;

        }
    }
}