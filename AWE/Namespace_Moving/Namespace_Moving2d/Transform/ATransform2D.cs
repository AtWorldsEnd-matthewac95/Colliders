using System.Collections.Generic;
using AWE.Math;

namespace AWE.Moving.Moving2D {

    public abstract class ATransform2D : ATransform<float, pair2f, angle, pair2f>, ITransform2D {

        public abstract Transform2DState state { get; }

        ITransformState ITransform.state => this.state;
        ITransformState<float> ITransform<float>.state => this.state;
        ITransformState<float, pair2f, angle, pair2f> ITransform<float, pair2f, angle, pair2f>.state => this.state;
        IReadOnlyList<float> ITransform<float>.position => this.position;
        IReadOnlyList<float> ITransform<float>.rotation => this.rotation;
        IReadOnlyList<float> ITransform<float>.dilation => this.dilation;

        public event DTransform2DUpdate OnAnyChange;
        public event DTransform2DTransformation OnTransformation;
        public event DTransform2DTranslation OnTranslation;
        public event DTransform2DRotation OnRotation;
        public event DTransform2DDilation OnDilation;

        #region AddListener
        public void AddListener (ATransform2DListener listener) {

            if (listener.hasOnAnyChange) this.OnAnyChange += listener.OnAnyChange;
            if (listener.hasOnTransformation) this.OnTransformation += listener.OnTransformation;
            if (listener.hasOnTranslation) this.OnTranslation += listener.OnTranslation;
            if (listener.hasOnRotation) this.OnRotation += listener.OnRotation;
            if (listener.hasOnDilation) this.OnDilation += listener.OnDilation;

        }

        public void AddListener (ITransform2DListener listener) {

            if (listener.hasOnAnyChange) this.OnAnyChange += listener.OnAnyChange;
            if (listener.hasOnTransformation) this.OnTransformation += listener.OnTransformation;
            if (listener.hasOnTranslation) this.OnTranslation += listener.OnTranslation;
            if (listener.hasOnRotation) this.OnRotation += listener.OnRotation;
            if (listener.hasOnDilation) this.OnDilation += listener.OnDilation;

        }

        public override void AddListener (ITransformListener<float, pair2f, angle, pair2f> listener) {

            if (listener.hasOnAnyChange) this.OnAnyChange += listener.OnAnyChange;
            if (listener.hasOnTransformation) this.OnTransformation += listener.OnTransformation;
            if (listener.hasOnTranslation) this.OnTranslation += listener.OnTranslation;
            if (listener.hasOnRotation) this.OnRotation += listener.OnRotation;
            if (listener.hasOnDilation) this.OnDilation += listener.OnDilation;

        }

        public override void AddListener (ITransformListener<float> listener) {

            if (listener.hasOnAnyChange) this.OnAnyChange += listener.OnAnyChange;
            if (listener.hasOnTransformation) this.OnTransformation += listener.OnTransformation;
            if (listener.hasOnTranslation) this.OnTranslation += ((resultantState, translation) => listener.OnTranslation (
                resultantState,
                translation
            ));
            if (listener.hasOnRotation) this.OnRotation += ((resultantState, rotation) => listener.OnRotation (
                resultantState,
                rotation
            ));
            if (listener.hasOnDilation) this.OnDilation += ((resultantState, dilation) => listener.OnDilation (
                resultantState,
                dilation
            ));

        }

        public override void AddListener (ITransformListener listener) {

            if (listener.hasOnAnyChange) this.OnAnyChange += listener.OnAnyChange;
            if (listener.hasOnTransformation) this.OnTransformation += listener.OnTransformation;
            if (listener.hasOnTranslation) this.OnTranslation += ((resultantState, translation) => listener.OnTranslation (
                resultantState,
                new Transformation2D (translation)
            ));
            if (listener.hasOnRotation) this.OnRotation += ((resultantState, rotation) => listener.OnRotation (
                resultantState,
                new Transformation2D (rotation)
            ));
            if (listener.hasOnDilation) this.OnDilation += ((resultantState, dilation) => listener.OnDilation (
                resultantState,
                new Transformation2D (dilation, isDilation: true)
            ));

        }
        # endregion

        public abstract BooleanNote TransformBy (Transformation2D transformation);
        public abstract BooleanNote TransformTo (Transform2DState state);
        public abstract BooleanNote TransformTo (Transformation2D transformation);

        BooleanNote ITransform<float, pair2f, angle, pair2f>.TransformBy (ITransformation<float, pair2f, angle, pair2f> transformation)
            => this.TransformBy (new Transformation2D (transformation.translation, transformation.rotation, transformation.dilation));
        BooleanNote ITransform<float, pair2f, angle, pair2f>.TransformTo (ITransformState<float, pair2f, angle, pair2f> state)
            => this.TransformTo (new Transform2DState (this, state.position, state.rotation, state.dilation));
        BooleanNote ITransform<float, pair2f, angle, pair2f>.TransformTo (ITransformation<float, pair2f, angle, pair2f> transformation)
            => this.TransformTo (new Transformation2D (transformation.translation, transformation.rotation, transformation.dilation));

        protected override ITransformation<float, pair2f, angle, pair2f> ConvertToDerivedTransformation (ITransformation<float> genericTransformation) => new Transformation2D (
            this.ConvertToTranslation (genericTransformation.translation),
            this.ConvertToRotation (genericTransformation.rotation),
            this.ConvertToDilation (genericTransformation.dilation)
        );

        protected override ITransformState<float, pair2f, angle, pair2f> ConvertToDerivedState (ITransformState<float> genericState) => new Transform2DState (
            this,
            this.ConvertToTranslation (genericState.position),
            this.ConvertToRotation (genericState.rotation),
            this.ConvertToDilation (genericState.dilation)
        );

        protected override pair2f ConvertToTranslation (IReadOnlyList<float> values) => new pair2f (
            ((values.Count > 0) ? values[0] : 0f),
            ((values.Count > 1) ? values[1] : 0f)
        );

        protected override angle ConvertToRotation (IReadOnlyList<float> values) => new angle (((values.Count > 0) ? values[0] : 0f), EAngleMode.Radian);

        protected override pair2f ConvertToDilation (IReadOnlyList<float> values) => new pair2f (
            ((values.Count > 0) ? values[0] : 0f),
            ((values.Count > 1) ? values[1] : 0f)
        );

    }
}
