using AWE;

namespace AWE.Math {

    public class Polygon2DIntersection {

        public pair2f point { get; }
        public Polygon2DEdge incidentEdge { get; }
        public Line2DSegment incidentVector { get; }

        public Polygon2DIntersection (pair2f point, Polygon2DEdge incidentEdge, Line2DSegment incidentVector) {

            this.point = point;
            this.incidentEdge = incidentEdge;
            this.incidentVector = incidentVector;

        }
    }
}
