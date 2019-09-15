using System.Collections.Generic;

namespace AWE.Math {

    public interface IShape2D {

        pair2f center { get; }
        bool isConvex { get; }

        bool IsContainingPoint (pair2f point);
        List<pair2f> CreateVertexList ();
        List<ICurve2D> CreateCurveList ();
        List<ICurve2DSegment> CreateCurveSegmentList ();
        IShape2D CreateOffset (pair2f offset);

    }
}
