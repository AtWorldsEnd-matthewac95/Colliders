using System;
using System.Collections.Generic;

namespace AWE.Moving {

    public class ReadOnlyTransform<TValueType, TPosition, TRotation, TDilation, TTransformation, TTransformState>
        : IReadOnlyTransform<TValueType, TPosition, TRotation, TDilation, TTransformation, TTransformState>,
        IEquatable<ReadOnlyTransform<TValueType, TPosition, TRotation, TDilation, TTransformation, TTransformState>>
        where TValueType : struct
        where TPosition : IReadOnlyList<TValueType>
        where TRotation : IReadOnlyList<TValueType>
        where TDilation : IReadOnlyList<TValueType>
        where TTransformation : ATransformation<TValueType, TPosition, TRotation, TDilation>
        where TTransformState : ATransformState<TValueType, TPosition, TRotation, TDilation, TTransformation>
    {

        private readonly ATransform<TValueType, TPosition, TRotation, TDilation, TTransformation, TTransformState> transform;

        ITransformState IReadOnlyTransform.state => this.state;
        public TTransformState state => this.transform.state;
        public TPosition position => this.transform.position;
        public TRotation rotation => this.transform.rotation;
        public TDilation dilation => this.transform.dilation;

        public ReadOnlyTransform (ATransform<TValueType, TPosition, TRotation, TDilation, TTransformation, TTransformState> transform) => this.transform = transform;

        public void AddListener (ITransformListener listener) => this.transform.AddListener (listener);
        public void AddListener (ITransformListener<TValueType, TPosition, TRotation, TDilation, TTransformation, TTransformState> listener) => this.transform.AddListener (listener);

        public override bool Equals (object other) {

            var isEqual = false;

            if (other is ReadOnlyTransform<TValueType, TPosition, TRotation, TDilation, TTransformation, TTransformState> otherReadOnly) {

                isEqual = this.Equals (otherReadOnly);

            } else if (other is ATransform<TValueType, TPosition, TRotation, TDilation, TTransformation, TTransformState> otherTransform) {

                isEqual = this.Equals (otherTransform);

            }

            return isEqual;

        }
        public override int GetHashCode () => this.transform.GetHashCode ();
        public virtual bool Equals (ReadOnlyTransform<TValueType, TPosition, TRotation, TDilation, TTransformation, TTransformState> other) => (this.transform == other.transform);
        public virtual bool Equals (ATransform<TValueType, TPosition, TRotation, TDilation, TTransformation, TTransformState> other) => (this.transform == other);

    }
}