using System;

namespace AWE.Moving.Collisions {

    public class GenericCollider<TTransformState, TReadOnlyTransform, TColliderState> : ACollider<TTransformState, TReadOnlyTransform, TColliderState>
        where TTransformState : ITransformState
        where TReadOnlyTransform : IReadOnlyTransform<TTransformState>
        where TColliderState : IColliderState<TTransformState>
    {

        protected Func<TColliderState> DelegateFindNextState;
        protected Func<TTransformState, float, TColliderState> DelegateFindProjectedState;
        protected Func<TTransformState, ColliderInterpolationIterator<TColliderState>> DelegateCreateInterpolationIterator;

        public GenericCollider (
            ACollisionHandler handler,
            IColliderAgent<TTransformState> agent,
            TReadOnlyTransform agentTransform,
            TColliderState currentState,
            Func<TColliderState> FindNextState,
            Func<TTransformState, float, TColliderState> FindProjectedState,
            Func<TTransformState, ColliderInterpolationIterator<TColliderState>> CreateInterpolationIterator
        ) : base (handler, agent, agentTransform, currentState, default) {

            this.DelegateFindNextState = FindNextState;
            this.DelegateFindProjectedState = FindProjectedState;
            this.DelegateCreateInterpolationIterator = CreateInterpolationIterator;

        }

        public GenericCollider (
            ACollisionHandler handler,
            IColliderAgent<TTransformState> agent,
            TReadOnlyTransform agentTransform,
            TColliderState currentState,
            TColliderState nextState,
            Func<TColliderState> FindNextState,
            Func<TTransformState, float, TColliderState> FindProjectedState,
            Func<TTransformState, ColliderInterpolationIterator<TColliderState>> CreateInterpolationIterator
        ) : base (handler, agent, agentTransform, currentState, nextState) {

            this.DelegateFindNextState = FindNextState;
            this.DelegateFindProjectedState = FindProjectedState;
            this.DelegateCreateInterpolationIterator = CreateInterpolationIterator;

        }

        protected override TColliderState FindNextState () => this.DelegateFindNextState ();
        protected override TColliderState FindProjectedState (TTransformState destination, float projectionScale = 1f)
            => this.DelegateFindProjectedState (destination, projectionScale);
        public override ColliderInterpolationIterator<TColliderState> CreateInterpolationIterator (TTransformState destination)
            => this.DelegateCreateInterpolationIterator (destination);

    }
}