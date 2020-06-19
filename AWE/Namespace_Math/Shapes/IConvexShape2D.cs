namespace AWE.Math {

    public interface IConvexShape2D : IShape2D {

        float minimalRadius { get; }
        float maximalRadius { get; }

        new IConvexShape2D CreateOffset (pair2f offset);

    }
}