namespace AWE.Math {

    // TODO - Find something to put in this interface, eventually. For now this is fine.
    public interface IConvexShape2D : IShape2D {

        new IConvexShape2D CreateOffset (pair2f offset);

    }
}