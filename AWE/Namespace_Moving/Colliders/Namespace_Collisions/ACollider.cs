using System;

namespace AWE.Moving.Collisions {

    // TODO - Make an ACollider class which has less generic constraints, preferably only on ColliderState.

    public abstract class ACollider<TTransformState, TReadOnlyTransform, TColliderState>
        : ICollider<TColliderState>,
        ITransformListener
        where TTransformState : ITransformState
        where TReadOnlyTransform : IReadOnlyTransform<TTransformState>
        where TColliderState : IColliderState<TTransformState>
    {

        public event DColliderStateChange<TColliderState> OnNextStateChange;

        private TColliderState _nextState;

        protected bool isStateCurrent;

        IColliderState ICollider.currentState => this.currentState;
        public TColliderState currentState { get; protected set; }
        public TColliderState nextState {

            get => this._nextState;

            protected set {

                this.OnNextStateChange (this._nextState, value);
                this._nextState = value;

            }
        }

        ICollisionHandler ICollider.handler => this.handler;
        public ACollisionHandler handler { get; protected set; }
        public bool isEnabled { get; protected set; }

        public IColliderAgent<TTransformState> agent { get; }
        public ReadOnlyColliderAgentPathCursor<TTransformState> agentPathCursor { get; protected set; }
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
            this.agentPathCursor = agent.fixedPathCursor;
            this.agent.OnFixedPathChange += this.OnAgentFixedPathChange;
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

        // The transform will call always this function when anything changes.
        void ITransformListener.OnAnyChange (ITransformState resultantState) => this.OnAnyChange (resultantState);

        // The transform will call OnAnyChange, then this function specifically if any transformation occurred.
        public virtual bool hasOnTransformation => false;
        void ITransformListener.OnTransformation (ITransformState resultantState, ITransformation transformation) => this.OnTransformation (resultantState, transformation);
        protected virtual void OnTransformation (ITransformState resultantState, ITransformation transformation) { }

        // The transform will call OnAnyChange, then OnTransformation, then this function specifically if a translation occurred.
        public virtual bool hasOnTranslation => false;
        void ITransformListener.OnTranslation (ITransformState resultantState, ITransformation transformation) => this.OnTranslation (resultantState, transformation);
        protected virtual void OnTranslation (ITransformState resultantState, ITransformation transformation) { }

        // The transform will call OnAnyChange, then OnTransformation, then this function specifically if a rotation occurred.
        public virtual bool hasOnRotation => false;
        void ITransformListener.OnRotation (ITransformState resultantState, ITransformation transformation) => this.OnRotation (resultantState, transformation);
        protected virtual void OnRotation (ITransformState resultantState, ITransformation transformation) { }

        // The transform will call OnAnyChange, then OnTransformation, then this function specifically if a dilation occurred.
        public virtual bool hasOnDilation => false;
        void ITransformListener.OnDilation (ITransformState resultantState, ITransformation transformation) => this.OnDilation (resultantState, transformation);
        protected virtual void OnDilation (ITransformState resultantState, ITransformation transformation) { }

        #endregion

        protected virtual void OnAgentFixedPathChange (ReadOnlyColliderAgentPathCursor<TTransformState> agentPathCursor) {

            this.agentPathCursor = agentPathCursor;

        }

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
        protected abstract TColliderState FindProjectedState (TTransformState destination, float projectionScale = 1f);

        // TODO Shoudn't be abstract, can just implement it here.
        public abstract ColliderInterpolationIterator<TColliderState> CreateInterpolationIterator (TTransformState destination);

        public virtual WeightedColliderState<TColliderState> FindInterpolatedState (float interpolation) {

            var interpolated = new WeightedColliderState<TColliderState> (this.currentState, 1f);

            if (this.nextState != null) {

                var agentCursor = this.agentPathCursor;

                if (agentCursor == null) {

                    interpolated = new WeightedColliderState<TColliderState> (
                        this.FindProjectedState (this.nextState.transformState, interpolation),
                        interpolation
                    );

                } else {

                    /*
                     * In this case we don't have to pass the interpolation as a seperate parameter
                     * to FindProjectedState, since the interpolation is resolved in the agent cursor's
                     * CreateInterpolatedState function.
                     *
                     * For default implementation, see ColliderAgentPathCursor.CreateInterpolatedState
                     */
                    var cursorState = agentCursor.CreateInterpolatedState (interpolation);
                    interpolated = new WeightedColliderState<TColliderState> (
                        this.FindProjectedState (cursorState.state),
                        cursorState.weight
                    );

                }
            }

            return interpolated;

        }
    }
}
