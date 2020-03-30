using System.Collections.Generic;
using AWE.Math;

namespace AWE.Moving.Moving2D {

    public abstract class ATransform2D : ATransform<float, pair2f, angle, pair2f, Transformation2D, Transform2DState>, ITransform2D {

        public event DTransform2DUpdate OnAnyChange;
        public event DTransform2DTransformation OnTransformation;
        public event DTransform2DTranslation OnTranslation;
        public event DTransform2DRotation OnRotation;
        public event DTransform2DDilation OnDilation;

        IReadOnlyTransform2D ITransform2D.AsReadOnly () => this.AsReadOnly ();
        new public ReadOnlyTransform2D AsReadOnly () => new ReadOnlyTransform2D (this);

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

        public override void AddListener (ITransformListener<float, pair2f, angle, pair2f, Transformation2D, Transform2DState> listener) {

            if (listener.hasOnAnyChange) this.OnAnyChange += listener.OnAnyChange;
            if (listener.hasOnTransformation) this.OnTransformation += listener.OnTransformation;
            if (listener.hasOnTranslation) this.OnTranslation += listener.OnTranslation;
            if (listener.hasOnRotation) this.OnRotation += listener.OnRotation;
            if (listener.hasOnDilation) this.OnDilation += listener.OnDilation;

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

        public override BooleanNote TransformBy (Transformation2D transformation) {

            var note = this.Transform (transformation);

            this.OnTranslation?.Invoke (this.state, transformation.translation);
            this.OnRotation?.Invoke (this.state, transformation.rotation);
            this.OnDilation?.Invoke (this.state, transformation.dilation);
            this.OnTransformation?.Invoke (this.state, transformation);
            this.OnAnyChange?.Invoke (this.state);

            return note;

        }

        public override BooleanNote TranslateBy (pair2f translation) {

            var note = this.Translate (translation);

            this.OnTranslation?.Invoke (this.state, translation);
            this.OnTransformation?.Invoke (this.state, new Transformation2D (translation));
            this.OnAnyChange?.Invoke (this.state);

            return note;

        }

        public override BooleanNote RotateBy (angle rotation) {

            var note = this.Rotate (rotation);

            this.OnRotation?.Invoke (this.state, rotation);
            this.OnTransformation?.Invoke (this.state, new Transformation2D (rotation));
            this.OnAnyChange?.Invoke (this.state);

            return note;

        }

        public override BooleanNote TransformTo (Transform2DState state) {

            var transformation = (state - this.state);
            var note = this.Transform (transformation);

            this.OnTranslation?.Invoke (this.state, transformation.translation);
            this.OnRotation?.Invoke (this.state, transformation.rotation);
            this.OnDilation?.Invoke (this.state, transformation.dilation);
            this.OnTransformation?.Invoke (this.state, transformation);
            this.OnAnyChange?.Invoke (this.state);

            return note;

        }

        public override BooleanNote TransformTo (Transformation2D transformation) {

            var diff = (new Transform2DState (this, transformation) - this.state);
            var note = this.Transform (diff);

            this.OnTranslation?.Invoke (this.state, diff.translation);
            this.OnRotation?.Invoke (this.state, diff.rotation);
            this.OnDilation?.Invoke (this.state, diff.dilation);
            this.OnTransformation?.Invoke (this.state, diff);
            this.OnAnyChange?.Invoke (this.state);

            return note;

        }

        public override BooleanNote TranslateTo (pair2f position) {

            var diff = (position - this.position);
            var note = this.Translate (diff);

            this.OnTranslation?.Invoke (this.state, diff);
            this.OnTransformation?.Invoke (this.state, new Transformation2D (diff));
            this.OnAnyChange?.Invoke (this.state);

            return note;

        }

        public override BooleanNote RotateTo (angle rotation) {

            var diff = (rotation - this.rotation);
            var note = this.Rotate (diff);

            this.OnRotation?.Invoke (this.state, diff);
            this.OnTransformation?.Invoke (this.state, new Transformation2D (diff));
            this.OnAnyChange?.Invoke (this.state);

            return note;

        }

        public override BooleanNote DilateTo (pair2f dilation) {

            var transformation = (new Transform2DState (this, this.position, this.rotation, dilation) - this.state);
            var note = this.Transform (transformation);

            this.OnDilation?.Invoke (this.state, transformation.dilation);
            this.OnTransformation?.Invoke (this.state, transformation);
            this.OnAnyChange?.Invoke (this.state);

            return note;

        }
    }
}
