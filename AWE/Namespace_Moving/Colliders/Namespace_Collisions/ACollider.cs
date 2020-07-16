namespace AWE.Moving.Collisions {

    public abstract class ACollider<TColliderState> : ICollider<TColliderState> where TColliderState : IColliderState {

        public event DColliderStateChange<TColliderState> OnNextStateChange;

        IColliderState ICollider.currentState => this.currentState;
        public TColliderState currentState { get; protected set; }

        private TColliderState _nextState;
        IColliderState ICollider.nextState => this.nextState;
        public TColliderState nextState {

            get => this._nextState;

            protected set {

                this.OnNextStateChange (this._nextState, value);
                this._nextState = value;

            }
        }

        public ReadOnlyColliderProperties properties { get; protected set; }
        public ICollisionHandler handler { get; protected set; }

        public bool isEnabled { get; protected set; }

        public ACollider (
            ReadOnlyColliderProperties properties,
            ICollisionHandler handler,
            TColliderState currentState,
            TColliderState nextState = default,
            bool isEnabled = true
        ) {

            // A collider is always considered enabled during the constructor.
            this.isEnabled = true;

            this.properties = properties;

            this.currentState = currentState;
            this.nextState = nextState;

            this.handler = handler;
            this.handler.OnBeginCollisionAnalysis += this.OnBeginCollisionAnalysis;
            this.handler.OnEndCollisionAnalysis += this.OnEndCollisionAnalysis;

            this.handler.Add (this);

            // After the constructor is finished, then enable or disable based on the parameter.
            this.isEnabled = isEnabled;

        }

        protected virtual void OnBeginCollisionAnalysis () {

            this.nextState = this.FindNextState ();

        }

        protected virtual void OnEndCollisionAnalysis () {

            this.UpdateCurrentState ();

        }

        protected virtual void UpdateCurrentState () {

            this.currentState = this.nextState;

        }

        protected abstract TColliderState FindNextState ();

    }
}
