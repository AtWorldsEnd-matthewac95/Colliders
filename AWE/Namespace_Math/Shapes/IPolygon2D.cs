using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AWE.Math {

    public interface IPolygon2D : IShape2D, IReadOnlyList<pair2f> {

        Bounds2D bounds { get; }
        ReadOnlyCollection<pair2f> unoffsetVerticies { get; }
        int count { get; }

        Polygon2DEdge this [IntPair indicies] { get; }
        Polygon2DEdge this [int one, int two] { get; }

        new IPolygon2D CreateOffset (pair2f offset);
        IPolygon2D CreateOffset (pair2f offset, angle rotation, pair2f center = default);
        IPolygon2D CreateOffset (angle offset, pair2f center = default);

        int GetNextIndex (int index);
        int GetPreviousIndex (int index);
        List<Line2D> CreateLineList ();
        List<Polygon2DEdge> CreateEdgeList ();
        List<IntPair> CreateEdgeIndexList ();
        ACyclicIndexIterator<pair2f> CreateVertexIterator (int startingIndex = 0);
        ACyclicIndexIterator<Polygon2DEdge> CreateEdgeIterator (int startingIndex = 0);

        bool AddRangeOfPointsToList (
            List<pair2f> list,
            int start,
            int end,
            DPointConditional SkipCondition = null,
            DPointConditional StartCondition = null,
            DPointConditional ExitCondition = null
        );
        bool IsIntersecting (IPolygon2D other);

    }
}
