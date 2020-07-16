using System;

namespace AWE.Moving.Collisions {

    public abstract class ATransformCollider<TColliderState, TTransformState>
        : ACollider<TColliderState>,
        ITransformCollider<TColliderState, TTransformState>
        where TColliderState : ITransformColliderState<TTransformState>
        where TTransformState : ITransformState
    {

        protected bool isUpToDateWithTransform;

        public IColliderAgent<TTransformState> agent { get; }
        public ReadOnlyColliderAgentPathCursor<TTransformState> agentPathCursor { get; protected set; }
        public ReadOnlyTransform<TTransformState> agentTransform { get; }

        public ATransformCollider (
            ReadOnlyColliderProperties properties,
            ICollisionHandler handler,
            IColliderAgent<TTransformState> agent,
            ReadOnlyTransform<TTransformState> agentTransform,
            TColliderState currentState,
            TColliderState nextState = default,
            bool isEnabled = true
        ) : base (properties, handler, currentState, nextState, isEnabled) {

            if (!agent.IsUsingTransform (agentTransform)) {

                // TODO - Create a message for this argument exception.
                //        agentTransform needs to be a transform used by agent.
                throw new ArgumentException ();

            }

            // A collider is always considered enabled during the constructor.
            this.isEnabled = true;

            this.agent = agent;
            this.agentPathCursor = agent.fixedPathCursor;
            this.agent.OnFixedPathChange += this.OnAgentFixedPathChange;
            this.agentTransform = agentTransform;
            this.ListenToTransform (this.agentTransform);

            // Initial state is assumed not to be up to date with transform.
            this.isUpToDateWithTransform = false;

            // After the constructor is finished, then enable or disable based on the parameter.
            this.isEnabled = isEnabled;

        }

        protected virtual void ListenToTransform (ReadOnlyTransform<TTransformState> transform) => transform.AddListener (this);

        public virtual bool hasOnAnyChange => true;
        protected virtual void OnAnyChange (ITransformState resultantState) {

            this.isUpToDateWithTransform = false;

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

        protected override void OnBeginCollisionAnalysis () {

            if (!this.isUpToDateWithTransform) {

                this.nextState = this.FindNextState ();

            }
        }

        protected override void OnEndCollisionAnalysis () {

            if (!this.isUpToDateWithTransform) {

                this.UpdateCurrentState ();

            }
        }

        protected override void UpdateCurrentState () {

            base.UpdateCurrentState ();
            this.isUpToDateWithTransform = true;

        }
    }
}
