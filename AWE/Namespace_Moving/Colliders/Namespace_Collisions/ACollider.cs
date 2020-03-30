using System;

namespace AWE.Moving.Collisions {

    public abstract class ACollider<TTransformState, TReadOnlyTransform, TColliderState>
        : ICollider<TColliderState>,
        ITransformListener
        where TTransformState : ITransformState
        where TReadOnlyTransform : IReadOnlyTransform<TTransformState>
        where TColliderState : IColliderState<TTransformState>
    {

        protected bool isStateCurrent;

        IColliderState ICollider.currentState => this.currentState;
        public TColliderState currentState { get; protected set; }
        public TColliderState nextState { get; protected set; }

        ICollisionHandler ICollider.handler => this.handler;
        public ACollisionHandler handler { get; protected set; }
        public bool isEnabled { get; protected set; }

        public IColliderAgent<TTransformState> agent { get; }
        public TReadOnlyTransform agentTransform { get; }

        public ACollider (
            ACollisionHandler handler,
            IColliderAgent<TTransformState> agent,
            TReadOnlyTransform agentTransform,
            TColliderState currentState,
            TColliderState nextState = default,
            bool isEnabled = true
        ) {

            if (!agent.IsUsingTransform (agentTransform)) {

                // TODO - Create a message for this argument exception.
                //        agentTransform needs to be a transform used by agent.
                throw new ArgumentException ();

            }

            // A collider is always considered enabled during the constructor.
            this.isEnabled = true;

            this.isStateCurrent = false;
            this.currentState = currentState;
            this.nextState = nextState;

            this.agent = agent;
            this.agentTransform = agentTransform;
            this.ListenToTransform (this.agentTransform);

            this.handler = handler;
            this.handler.OnBeginCollisionAnalysis += this.OnBeginCollisionAnalysis;
            this.handler.OnEndCollisionAnalysis += this.OnEndCollisionAnalysis;

            this.handler.Add (this);

            // After the constructor is finished, then enable or disable based on the parameter.
            this.isEnabled = isEnabled;

        }

        protected virtual void ListenToTransform (TReadOnlyTransform transform) => transform.AddListener (this);
        protected virtual void UpdateCurrentState () {

            this.currentState = this.nextState;
            this.isStateCurrent = true;

        }

        public virtual bool hasOnAnyChange => true;
        protected virtual void OnAnyChange (ITransformState resultantState) {

            this.isStateCurrent = false;

        }

        #region Trivial ITransformListener Implementations
        void ITransformListener.OnAnyChange (ITransformState resultantState) => this.OnAnyChange (resultantState);

        public virtual bool hasOnTransformation => false;
        void ITransformListener.OnTransformation (ITransformState resultantState, ITransformation transformation) => this.OnTransformation (resultantState, transformation);
        protected virtual void OnTransformation (ITransformState resultantState, ITransformation transformation) { }

        public virtual bool hasOnTranslation => false;
        void ITransformListener.OnTranslation (ITransformState resultantState, ITransformation transformation) => this.OnTranslation (resultantState, transformation);
        protected virtual void OnTranslation (ITransformState resultantState, ITransformation transformation) { }

        public virtual bool hasOnRotation => false;
        void ITransformListener.OnRotation (ITransformState resultantState, ITransformation transformation) => this.OnRotation (resultantState, transformation);
        protected virtual void OnRotation (ITransformState resultantState, ITransformation transformation) { }

        public virtual bool hasOnDilation => false;
        void ITransformListener.OnDilation (ITransformState resultantState, ITransformation transformation) => this.OnDilation (resultantState, transformation);
        protected virtual void OnDilation (ITransformState resultantState, ITransformation transformation) { }
        #endregion

        protected virtual void OnBeginCollisionAnalysis () {

            if (!this.isStateCurrent) {

                this.nextState = this.FindNextState ();

            }
        }
        protected virtual void OnEndCollisionAnalysis () {

            if (!this.isStateCurrent) {

                this.UpdateCurrentState ();

            }
        }

        protected abstract TColliderState FindNextState ();
        protected abstract TColliderState CreateProjectedState (ITransformation transformation, float transformationScale = 1f);

        public virtual TColliderState CreateInterpolatedState (float interpolation) {

            TColliderState interpolated;
            var agentCursor = this.agent.fixedPathCursor;

            if (agentCursor == null) {

                interpolated = this.CreateProjectedState (
                    this.nextState.transformState.FindDifference (
                        this.currentState.transformState
                    ),
                    interpolation
                );

            } else {

                /*
                 * TODO
                 *
                 * Should the cursor function return the closest anchor state?
                 * Or the interpolation between the two closest anchor states?
                 */
                interpolated = this.CreateProjectedState (
                    agentCursor.CreateInterpolatedState (interpolation).FindDifference (
                        this.currentState.transformState
                    )
                );

            }

            return interpolated;

        }
    }
}
