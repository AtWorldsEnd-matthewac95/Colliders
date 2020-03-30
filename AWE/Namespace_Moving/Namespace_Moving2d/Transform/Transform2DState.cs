using System;
using System.Collections.Generic;
using AWE.Math;

namespace AWE.Moving.Moving2D {

    public class Transform2DState : ATransformState<float, pair2f, angle, pair2f, Transformation2D>, IComparable<Transform2DState> {

        # region Operators
        public static Transformation2D operator - (Transform2DState current, Transform2DState next) => current.FindDifference (next);

        public static Transform2DState operator + (Transform2DState state, Transformation2D transformation) => state.Add (transformation);
        public static Transform2DState operator + (Transform2DState state, pair2f translation) => state.Add (translation);
        public static Transform2DState operator + (Transform2DState state, angle rotation) => state.Add (rotation);

        public static Transform2DState operator - (Transform2DState state, Transformation2D transformation) => state.Subtract (transformation);
        public static Transform2DState operator - (Transform2DState state, pair2f translation) => state.Subtract (translation);
        public static Transform2DState operator - (Transform2DState state, angle rotation) => state.Subtract (rotation);

        public static bool operator < (Transform2DState i, Transform2DState j) => ((i.transform == j.transform) ? (i.CompareTo (j) < 0) : false);
        public static bool operator > (Transform2DState i, Transform2DState j) => ((i.transform == j.transform) ? (i.CompareTo (j) > 0) : false);
        public static bool operator <= (Transform2DState i, Transform2DState j) => ((i.transform == j.transform) ? (i.CompareTo (j) <= 0) : false);
        public static bool operator >= (Transform2DState i, Transform2DState j) => ((i.transform == j.transform) ? (i.CompareTo (j) >= 0) : false);
        # endregion

        protected override IReadOnlyTransform GetParentTransform () => this.transform;

        public ReadOnlyTransform2D transform { get; }
        public override TransformStateIndex index { get; }
        public override pair2f position { get; }
        public override angle rotation { get; }
        public override pair2f dilation { get; }

        public Transform2DState (ReadOnlyTransform2D transform, pair2f position, angle rotation, pair2f dilation) {

            this.transform = transform;
            this.position = position;
            this.rotation = rotation;
            this.dilation = dilation;

            this.index = new TransformStateIndex (transform);

        }

        public Transform2DState (ATransform2D transform, pair2f position, angle rotation, pair2f dilation) : this (transform.AsReadOnly (), position, rotation, dilation) {}
        public Transform2DState (ReadOnlyTransform2D transform) : this (transform, pair2f.origin, new angle (0f, EAngleMode.Radian), pair2f.one) {}
        public Transform2DState (ATransform2D transform) : this (transform.AsReadOnly (), pair2f.origin, new angle (0f, EAngleMode.Radian), pair2f.one) {}
        public Transform2DState (ReadOnlyTransform2D transform, Transformation2D transformation) : this (transform, transformation.translation, transformation.rotation, transformation.dilation) {}
        public Transform2DState (ATransform2D transform, Transformation2D transformation) : this (transform.AsReadOnly (), transformation.translation, transformation.rotation, transformation.dilation) {}
        public Transform2DState (Transform2DState original) : this (original.transform, original.position, original.rotation, original.dilation) {}

        public int CompareTo (Transform2DState other) => this.index.CompareTo (other.index);

        public override ATransformState<float, pair2f, angle, pair2f, Transformation2D> Add (
            ATransformation<float, pair2f, angle, pair2f> transformation
        ) {

            ATransformState<float, pair2f, angle, pair2f, Transformation2D> newState;

            if (transformation is Transformation2D transformation2d) {

                newState = this.Add (transformation2d);

            } else {

                newState = this.Copy ();

            }

            return newState;

        }

        public virtual Transform2DState Add (Transformation2D transformation) => new Transform2DState (
            this.transform,
            (this.position + transformation.translation),
            (this.rotation + transformation.rotation),
            new pair2f ((this.dilation.x * transformation.dilation.x), (this.dilation.y * transformation.dilation.y))
        );
        public Transform2DState Add (pair2f delta, bool isDilation = false) => new Transform2DState (
            this.transform,
            (isDilation ? this.position : (this.position + delta)),
            this.rotation,
            (isDilation ? new pair2f ((this.dilation.x * delta.x), (this.dilation.y * delta.y)) : this.dilation)
        );
        public Transform2DState Add (angle rotation) => new Transform2DState (
            this.transform,
            this.position,
            (this.rotation + rotation),
            this.dilation
        );

        public override ATransformState<float, pair2f, angle, pair2f, Transformation2D> Subtract (
            ATransformation<float, pair2f, angle, pair2f> transformation
        ) {

            ATransformState<float, pair2f, angle, pair2f, Transformation2D> newState;

            if (transformation is Transformation2D transformation2D) {

                newState = this.Subtract (transformation2D);

            } else {

                newState = this.Copy ();

            }

            return newState;

        }

        public virtual Transform2DState Subtract (Transformation2D transformation) => new Transform2DState (
            this.transform,
            (this.position - transformation.translation),
            (this.rotation - transformation.rotation),
            new pair2f ((this.dilation.x / transformation.dilation.x), (this.dilation.y / transformation.dilation.y))
        );
        public Transform2DState Subtract (pair2f delta, bool isDilation = false) => new Transform2DState (
            this.transform,
            (isDilation ? this.position : (this.position - delta)),
            this.rotation,
            (isDilation ? new pair2f ((this.dilation.x / delta.x), (this.dilation.y / delta.y)) : this.dilation)
        );
        public Transform2DState Subtract (angle rotation) => new Transform2DState (
            this.transform,
            this.position,
            (this.rotation - rotation),
            this.dilation
        );

        public override Transformation2D FindDifference (ATransformState<float, pair2f, angle, pair2f, Transformation2D> other) {

            Transformation2D difference;

            if (other is Transform2DState state2D) {

                difference = this.FindDifference (state2D);

            } else {

                difference = Transformation2D.zero;

            }

            return difference;

        }
        public virtual Transformation2D FindDifference (Transform2DState other) => new Transformation2D (this, other);

        public Transform2DState Copy () => new Transform2DState (this);

    }
}