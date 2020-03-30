using System;

namespace AWE.Moving.Collisions {

    public class GenericCollider<TTransformState, TReadOnlyTransform, TColliderState> : ACollider<TTransformState, TReadOnlyTransform, TColliderState>
        where TTransformState : ITransformState
        where TReadOnlyTransform : IReadOnlyTransform<TTransformState>
        where TColliderState : IColliderState<TTransformState>
    {

        protected Func<TColliderState> _FindNextState;
        protected Func<ITransformation, float, TColliderState> _CreateProjectedState;

        public GenericCollider (
            ACollisionHandler handler,
            IColliderAgent<TTransformState> agent,
            TReadOnlyTransform agentTransform,
            TColliderState currentState,
            Func<TColliderState> FindNextState,
            Func<ITransformation, float, TColliderState> CreateProjectedState
        ) : base (handler, agent, agentTransform, currentState, default) {

            this._FindNextState = FindNextState;
            this._CreateProjectedState = CreateProjectedState;

        }

        public GenericCollider (
            ACollisionHandler handler,
            IColliderAgent<TTransformState> agent,
            TReadOnlyTransform agentTransform,
            TColliderState currentState,
            TColliderState nextState,
            Func<TColliderState> FindNextState,
            Func<ITransformation, float, TColliderState> CreateProjectedState
        ) : base (handler, agent, agentTransform, currentState, nextState) {

            this._FindNextState = FindNextState;
            this._CreateProjectedState = CreateProjectedState;

        }

        protected override TColliderState FindNextState () => this._FindNextState ();
        protected override TColliderState CreateProjectedState (ITransformation transformation, float transformationScale = 1f)
            => this._CreateProjectedState (transformation, transformationScale);

    }
}