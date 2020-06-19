namespace AWE.Math {

    public interface IBounds2D {

        float right { get; }
        float top { get; }
        float left { get; }
        float bottom { get; }
        float width { get; }
        float height { get; }

        float this [EDirection direction] { get; }

        bool IsContaining (pair2f point);

    }
}