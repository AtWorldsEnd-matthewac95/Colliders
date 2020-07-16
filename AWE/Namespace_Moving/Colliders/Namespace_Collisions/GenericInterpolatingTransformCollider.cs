using System;

namespace AWE.Moving.Collisions {

    public class GenericInterpolatingTransformCollider<TColliderState, TTransformState>
        : AInterpolatingTransformCollider<TColliderState, TTransformState>
        where TColliderState : ITransformColliderState<TTransformState>
        where TTransformState : ITransformState
    {

        protected Func<TColliderState> DelegateFindNextState;
        protected Func<TTransformState, float> DelegateFindTranslationDistance;
        protected Func<TTransformState, float, TColliderState> DelegateFindProjectedState;

        public GenericInterpolatingTransformCollider (
            ReadOnlyColliderProperties properties,
            ICollisionHandler handler,
            IColliderAgent<TTransformState> agent,
            ReadOnlyTransform<TTransformState> agentTransform,
            TColliderState currentState,
            Func<TColliderState> FindNextState,
            Func<TTransformState, float> FindTranslationDistance,
            Func<TTransformState, float, TColliderState> FindProjectedState
        ) : this (
            properties,
            handler,
            agent,
            agentTransform,
            currentState,
            default,
            FindNextState,
            FindTranslationDistance,
            FindProjectedState
        ) {
        }

        public GenericInterpolatingTransformCollider (
            ReadOnlyColliderProperties properties,
            ICollisionHandler handler,
            IColliderAgent<TTransformState> agent,
            ReadOnlyTransform<TTransformState> agentTransform,
            TColliderState currentState,
            TColliderState nextState,
            Func<TColliderState> FindNextState,
            Func<TTransformState, float> FindTranslationDistance,
            Func<TTransformState, float, TColliderState> FindProjectedState
        ) : base (properties, handler, agent, agentTransform, currentState, nextState) {

            this.DelegateFindNextState = FindNextState;
            this.DelegateFindTranslationDistance = FindTranslationDistance;
            this.DelegateFindProjectedState = FindProjectedState;

        }

        protected override TColliderState FindNextState () => this.DelegateFindNextState ();
        protected override float FindTranslationDistance (TTransformState destination) => this.DelegateFindTranslationDistance (destination);
        protected override TColliderState FindProjectedState (TTransformState destination, float projectionScale = 1f)
            => this.DelegateFindProjectedState (destination, projectionScale);

    }
}