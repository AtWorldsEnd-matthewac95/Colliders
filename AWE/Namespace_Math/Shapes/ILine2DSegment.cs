namespace AWE.Math {

    public interface ILine2DSegment : ICurve2DSegment {

        bool isHorizontal { get; }
        bool isVertical { get; }
        Line2D parentLine { get; }

    }
}