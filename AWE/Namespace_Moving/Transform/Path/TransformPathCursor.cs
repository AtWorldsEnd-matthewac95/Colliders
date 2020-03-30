using System;

namespace AWE.Moving {

    public class TransformPathCursor<TTransformState> : ICloneable where TTransformState : ITransformState {

        private bool isCurrentOutdated;
        private float _position;
        private float _speed;

        protected TTransformState _current;

        public TransformPath<TTransformState> path { get; protected set; }

        public float next => (this._position + this.speed);

        public float speed {

            get => this._speed;

            set {

                this._speed = value;
                this.OnSpeedChange ();

            }

        }

        public float position {

            get => this._position;

            set {

                this.isCurrentOutdated = true;
                this._position = value;

                this.OnPositionChange ();

            }
        }

        public virtual TTransformState current {

            get {

                if (this.isCurrentOutdated) {

                    this._current = this.path.GetState (this._position);
                    this.isCurrentOutdated = false;

                }

                return this._current;

            }
        }

        private TransformPathCursor (TransformPath<TTransformState> path, float speed, float position, TTransformState current) {

            this.path = path;
            this.speed = speed;
            this._position = position;
            this._current = current;
            this.isCurrentOutdated = false;

        }

        public TransformPathCursor (TransformPath<TTransformState> path, float speed, float position = 0f, bool evaluateCurrent = false) {

            this.isCurrentOutdated = true;

            this.path = path;
            this.speed = speed;
            this._position = position;

            if (evaluateCurrent) {

                this._current = path.GetState (position);
                this.isCurrentOutdated = false;

            }
        }

        protected virtual void OnSpeedChange () {}
        protected virtual void OnPositionChange () {}

        public void MoveForward () => this.position += this.speed;

        public void MoveBackward () => this.position -= this.speed;

        object ICloneable.Clone () => this.Clone ();
        public TransformPathCursor<TTransformState> Clone (bool evaluateCurrent = false) {

            TransformPathCursor<TTransformState> clone;

            if (this.isCurrentOutdated) {

                clone = new TransformPathCursor<TTransformState> (this.path, this.speed, this._position, evaluateCurrent);

            } else {

                clone = new TransformPathCursor<TTransformState> (this.path, this.speed, this._position, this._current);

            }

            return clone;

        }
    }
}