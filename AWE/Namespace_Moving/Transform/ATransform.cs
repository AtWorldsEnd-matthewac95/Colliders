using System.Collections.Generic;

namespace AWE.Moving {

    public abstract class ATransform<TValueType, TTranslation, TRotation, TDilation>
        : ITransform<TValueType, TTranslation, TRotation, TDilation>
        where TValueType : struct
        where TTranslation : IReadOnlyList<TValueType>
        where TRotation : IReadOnlyList<TValueType>
        where TDilation : IReadOnlyList<TValueType>
    {

        protected ITransformState<TValueType, TTranslation, TRotation, TDilation> _state { get; }

        public virtual TTranslation position { get; }
        public virtual TRotation rotation { get; }
        public virtual TDilation dilation { get; }

        ITransformState ITransform.state => this._state;
        ITransformState<TValueType> ITransform<TValueType>.state => this._state;
        ITransformState<TValueType, TTranslation, TRotation, TDilation> ITransform<TValueType, TTranslation, TRotation, TDilation>.state => this._state;
        IReadOnlyList<TValueType> ITransform<TValueType>.position => this.position;
        IReadOnlyList<TValueType> ITransform<TValueType>.rotation => this.rotation;
        IReadOnlyList<TValueType> ITransform<TValueType>.dilation => this.dilation;

        public abstract void AddListener (ITransformListener<TValueType, TTranslation, TRotation, TDilation> listener);
        public abstract void AddListener (ITransformListener<TValueType> listener);
        public abstract void AddListener (ITransformListener listener);

        public abstract BooleanNote TransformBy (ITransformation<TValueType, TTranslation, TRotation, TDilation> transformation);
        public abstract BooleanNote TranslateBy (TTranslation translation);
        public abstract BooleanNote RotateBy (TRotation rotation);
        public abstract BooleanNote TransformTo (ITransformState<TValueType, TTranslation, TRotation, TDilation> state);
        public abstract BooleanNote TransformTo (ITransformation<TValueType, TTranslation, TRotation, TDilation> transformation);
        public abstract BooleanNote TranslateTo (TTranslation position);
        public abstract BooleanNote RotateTo (TRotation rotation);
        public abstract BooleanNote DilateTo (TDilation dilation);

        protected abstract ITransformation<TValueType, TTranslation, TRotation, TDilation> ConvertToDerivedTransformation (ITransformation<TValueType> genericTransformation);
        protected abstract ITransformState<TValueType, TTranslation, TRotation, TDilation> ConvertToDerivedState (ITransformState<TValueType> genericState);
        protected abstract TTranslation ConvertToTranslation (IReadOnlyList<TValueType> values);
        protected abstract TRotation ConvertToRotation (IReadOnlyList<TValueType> values);
        protected abstract TDilation ConvertToDilation (IReadOnlyList<TValueType> values);

        BooleanNote ITransform<TValueType>.TransformBy (ITransformation<TValueType> transformation) => this.TransformBy (this.ConvertToDerivedTransformation (transformation));
        BooleanNote ITransform<TValueType>.TranslateBy (IReadOnlyList<TValueType> translation) => this.TranslateBy (this.ConvertToTranslation (translation));
        BooleanNote ITransform<TValueType>.RotateBy (IReadOnlyList<TValueType> rotation) => this.RotateBy (this.ConvertToRotation (rotation));
        BooleanNote ITransform<TValueType>.TransformTo (ITransformState<TValueType> state) => this.TransformTo (this.ConvertToDerivedState (state));
        BooleanNote ITransform<TValueType>.TransformTo (ITransformation<TValueType> transformation) => this.TransformTo (this.ConvertToDerivedTransformation (transformation));
        BooleanNote ITransform<TValueType>.TranslateTo (IReadOnlyList<TValueType> translation) => this.TranslateTo (this.ConvertToTranslation (translation));
        BooleanNote ITransform<TValueType>.RotateTo (IReadOnlyList<TValueType> rotation) => this.RotateTo (this.ConvertToRotation (rotation));
        BooleanNote ITransform<TValueType>.DilateTo (IReadOnlyList<TValueType> dilation) => this.DilateTo (this.ConvertToDilation (dilation));

    }
}