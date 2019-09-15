namespace AWE.Math {

    public interface ICurve2DSegment : ICurve2D {

        pair2f head { get; }
        pair2f tail { get; }
        ICurve2D parent { get; }

        pair2f GetPoint (float interpolation);

    }
}
