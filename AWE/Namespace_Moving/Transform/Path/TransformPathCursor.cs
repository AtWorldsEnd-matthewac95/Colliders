﻿using AWE.Math.FloatExtensions;
using System;

namespace AWE.Moving {

    public class TransformPathCursor<TTransformState> : ITransformPathCursor<TTransformState>, ICloneable where TTransformState : ITransformState {

        private bool isCurrentOutdated;
        private float _position;
        private float _speed;
        private TransformPathAnchors<TTransformState> _pathAnchors;

        protected TTransformState _current;

        public float next => (this._position + this.speed);

        float IReadOnlyTransformPathCursor<TTransformState>.speed => this._speed;
        public float speed {

            get => this._speed;

            set {

                this._speed = value;

                this.OnSpeedChange ();

            }
        }

        float IReadOnlyTransformPathCursor<TTransformState>.position => this._position;
        public float position {

            get => this._position;

            set {

                this.isCurrentOutdated = true;
                this._position = value;

                this.OnPositionChange ();

            }
        }

        public TransformPathAnchors<TTransformState> pathAnchors {

            get => this._pathAnchors;

            protected set {

                this._pathAnchors = value;

                this.OnAnchorsChange ();

            }
        }

        public virtual TTransformState current {

            get {

                if (this.isCurrentOutdated) {

                    this._current = this.pathAnchors.path[this._position];
                    this.isCurrentOutdated = false;

                }

                return this._current;

            }
        }

        private TransformPathCursor (TransformPathAnchors<TTransformState> pathAnchors, float speed, float position, TTransformState current) {

            this._speed = speed;
            this._position = position;
            this._pathAnchors = pathAnchors;
            this._current = current;
            this.isCurrentOutdated = (current != null);

        }

        public TransformPathCursor (TransformPathAnchors<TTransformState> pathAnchors, float speed, float position = 0f, bool evaluateCurrent = false) {

            this.isCurrentOutdated = true;

            this._speed = speed;
            this._position = position;
            this._pathAnchors = pathAnchors;

            if (evaluateCurrent) {

                this._current = pathAnchors.path[position];
                this.isCurrentOutdated = false;

            }
        }

        IReadOnlyTransformPathCursor<TTransformState> ITransformPathCursor<TTransformState>.AsReadOnly () => this.AsReadOnly ();
        public ReadOnlyTransformPathCursor<TTransformState> AsReadOnly () => new ReadOnlyTransformPathCursor<TTransformState> (this);

        protected virtual void OnSpeedChange () {}
        protected virtual void OnPositionChange () {}
        protected virtual void OnAnchorsChange () {}

        public void MoveForward () => this.position += this.speed;

        public void MoveBackward () => this.position -= this.speed;

        public TransformPathAnchors<TTransformState> UpdateAnchors (
            TransformPathAnchors<TTransformState> newAnchors,
            bool resetPosition = false
        ) {

            var oldAnchors = this.pathAnchors;

            if ((oldAnchors == null) || (newAnchors.path == oldAnchors.path)) {

                this.pathAnchors = newAnchors;

                // Notice the !. If the current position is NOT negligible...
                if (resetPosition && !this.position.IsNegligible ()) {

                    this.position = 0f;

                }

            } else {

                // TODO - Throw an exception

            }

            return oldAnchors;

        }

        object ICloneable.Clone () => this.Clone ();
        public TransformPathCursor<TTransformState> Clone (bool evaluateCurrent = false) {

            TransformPathCursor<TTransformState> clone;

            if (this.isCurrentOutdated) {

                clone = new TransformPathCursor<TTransformState> (this.pathAnchors, this.speed, this._position, evaluateCurrent);

            } else {

                clone = new TransformPathCursor<TTransformState> (this.pathAnchors, this.speed, this._position, this._current);

            }

            return clone;

        }
    }
}
