namespace AWE.Math {

    public static class SEPolygon2DTypeExtensions {

        public static bool isConvex (this EPolygon2DType polygonType) {

            var value = false;

            switch (polygonType) {

            case EPolygon2DType.IsoscelesTriangle:
            case EPolygon2DType.Radial:
            case EPolygon2DType.Rectangle:
            case EPolygon2DType.Regular:
            case EPolygon2DType.RightTriangle:
                value = true;
            break;

            }

            return value;

        }
    }
}