namespace AWE.Math {

    public class Polygon2DEdge : Line2DSegment {

        public APolygon2D parentPolygon { get; }
        public IntPair indicies { get; }

        public Polygon2DEdge (APolygon2D polygon, IntPair vertexIndicies) : base (
            polygon[vertexIndicies.a],
            polygon[vertexIndicies.b]
        ) {

            this.parentPolygon = polygon;
            this.indicies = vertexIndicies;

        }
    }
}