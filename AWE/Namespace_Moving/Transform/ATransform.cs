using System;
using System.Collections.Generic;

namespace AWE.Moving {

    public abstract class ATransform<TTransformState> : ITransform<TTransformState> where TTransformState : ITransformState {

        ITransformState IReadOnlyTransform.state => this.state;
        public abstract TTransformState state { get; }

        IReadOnlyTransform ITransform.AsReadOnly () => this.AsReadOnly ();
        IReadOnlyTransform<TTransformState> ITransform<TTransformState>.AsReadOnly () => this.AsReadOnly ();
        public virtual ReadOnlyTransform<TTransformState> AsReadOnly () => new ReadOnlyTransform<TTransformState> (this);

        public abstract void AddListener (ITransformListener<TTransformState> listener);
        public abstract void AddListener (ITransformListener listener);

        public BooleanNote TransformTo (ITransformState state) {

            var note = new BooleanNote (false, "Given transform state was not of the right type.");

            if (state is TTransformState tstate) {

                note = this.TransformTo (tstate);

            }

            return note;

        }
        public abstract BooleanNote TransformTo (TTransformState state);

        public abstract BooleanNote TransformBy (ITransformation transformation);
        public abstract BooleanNote TransformTo (ITransformation transformation);

    }

    public abstract class ATransform<TValueType, TTranslation, TRotation, TDilation, TTransformation, TTransformState>
        : ATransform<TTransformState>,
        ITransform<TValueType, TTranslation, TRotation, TDilation, TTransformation, TTransformState>
        where TValueType : struct
        where TTranslation : IReadOnlyList<TValueType>
        where TRotation : IReadOnlyList<TValueType>
        where TDilation : IReadOnlyList<TValueType>
        where TTransformation : ATransformation<TValueType, TTranslation, TRotation, TDilation>
        where TTransformState : ATransformState<TValueType, TTranslation, TRotation, TDilation, TTransformation>
    {

        ITransformState IReadOnlyTransform.state => this.state;
        public virtual TTranslation position => this.state.position;
        public virtual TRotation rotation => this.state.rotation;
        public virtual TDilation dilation => this.state.dilation;

        IReadOnlyTransform ITransform.AsReadOnly () => this.AsReadOnly ();
        IReadOnlyTransform<TTransformState> ITransform<TTransformState>.AsReadOnly () => this.AsReadOnly ();
        IReadOnlyTransform<TValueType, TTranslation, TRotation, TDilation, TTransformation, TTransformState> ITransform<TValueType, TTranslation, TRotation, TDilation, TTransformation, TTransformState>.AsReadOnly ()
            => this.AsReadOnly ();
        public virtual ReadOnlyTransform<TValueType, TTranslation, TRotation, TDilation, TTransformation, TTransformState> AsReadOnly ()
            => new ReadOnlyTransform<TValueType, TTranslation, TRotation, TDilation, TTransformation, TTransformState> (this);

        public abstract void AddListener (
            ITransformListener<TValueType, TTranslation, TRotation, TDilation, TTransformation, TTransformState> listener
        );

        public override BooleanNote TransformBy (ITransformation transformation) {

            var note = new BooleanNote (false, "Given transformation was not of the right type.");

            if (transformation is TTransformation ttransformation) {

                note = this.TransformBy (ttransformation);

            }

            return note;

        }
        public override BooleanNote TransformTo (ITransformation transformation) {

            var note = new BooleanNote (false, "Given transformation was not of the right type.");

            if (transformation is TTransformation ttransformation) {

                note = this.TransformTo (ttransformation);

            }

            return note;

        }

        public abstract BooleanNote TransformTo (TTransformation transformation);
        public abstract BooleanNote TranslateTo (TTranslation position);
        public abstract BooleanNote RotateTo (TRotation rotation);
        public abstract BooleanNote DilateTo (TDilation dilation);

        protected abstract BooleanNote Transform (TTransformation transformation);
        protected abstract BooleanNote Translate (TTranslation translation);
        protected abstract BooleanNote Rotate (TRotation rotation);

        public virtual BooleanNote TransformBy (TTransformation transformation) => this.Transform (transformation);
        public virtual BooleanNote TranslateBy (TTranslation translation) => this.Translate (translation);
        public virtual BooleanNote RotateBy (TRotation rotation) => this.Rotate (rotation);

    }
}
