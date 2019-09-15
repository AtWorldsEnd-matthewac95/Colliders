using System;
using System.Collections.Generic;
using AWE.Math;

namespace AWE.Moving.Moving2D {

    public class Transform2DState : ATransformState<float, pair2f, angle, pair2f> {

        # region Operators
        public static Transformation2D operator - (Transform2DState current, Transform2DState next) => new Transformation2D (current, next);

        public static Transform2DState operator + (Transform2DState state, Transformation2D transformation) => new Transform2DState (
            state.transform,
            (state.position + transformation.translation),
            (state.rotation + transformation.rotation),
            new pair2f ((state.dilation.x * transformation.dilation.x), (state.dilation.y * transformation.dilation.y))
        );

        public static Transform2DState operator + (Transform2DState state, pair2f translation) => new Transform2DState (
            state.transform,
            (state.position + translation),
            state.rotation,
            state.dilation
        );

        public static Transform2DState operator + (Transform2DState state, angle rotation) => new Transform2DState (
            state.transform,
            state.position,
            (state.rotation + rotation),
            state.dilation
        );

        public static Transform2DState operator - (Transform2DState state, pair2f translation) => new Transform2DState (
            state.transform,
            (state.position - translation),
            state.rotation,
            state.dilation
        );

        public static Transform2DState operator - (Transform2DState state, angle rotation) => new Transform2DState (
            state.transform,
            state.position,
            (state.rotation - rotation),
            state.dilation
        );

        public static bool operator < (Transform2DState i, Transform2DState j) => ((i.transform == j.transform) ? (i.CompareTo (j) < 0) : false);

        public static bool operator > (Transform2DState i, Transform2DState j) => ((i.transform == j.transform) ? (i.CompareTo (j) > 0) : false);

        public static bool operator <= (Transform2DState i, Transform2DState j) => ((i.transform == j.transform) ? (i.CompareTo (j) <= 0) : false);

        public static bool operator >= (Transform2DState i, Transform2DState j) => ((i.transform == j.transform) ? (i.CompareTo (j) >= 0) : false);
        # endregion

        protected override ITransform<float, pair2f, angle, pair2f> _transform => this.transform;

        new public ITransform2D transform { get; }
        public override TransformStateIndex index { get; }
        public override pair2f position { get; }
        public override angle rotation { get; }
        public override pair2f dilation { get; }

        public Transform2DState (ITransform2D transform, pair2f position, angle rotation, pair2f dilation) {

            this.transform = transform;
            this.position = position;
            this.rotation = rotation;
            this.dilation = dilation;

            this.index = new TransformStateIndex (transform);

        }

        public Transform2DState (Transform2DState original) : this (original.transform, original.position, original.rotation, original.dilation) {}

    }
}