using AWE.Math;

namespace AWE.Moving.Moving2D {

    public delegate void DTransform2DUpdate (Transform2DState resultantState);
    public delegate void DTransform2DTransformation (Transform2DState resultantState, Transformation2D transformation);
    public delegate void DTransform2DTranslation (Transform2DState resultantState, pair2f translation);
    public delegate void DTransform2DRotation (Transform2DState resultantState, angle rotation);
    public delegate void DTransform2DDilation (Transform2DState resultantState, pair2f dilation);

}
