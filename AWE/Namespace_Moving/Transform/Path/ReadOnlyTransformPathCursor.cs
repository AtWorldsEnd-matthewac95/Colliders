namespace AWE.Moving {

    public class ReadOnlyTransformPathCursor<TTransformState> : IReadOnlyTransformPathCursor<TTransformState> where TTransformState : ITransformState {

        private readonly TransformPathCursor<TTransformState> cursor;

        public float speed => this.cursor.speed;
        public float position => this.cursor.position;
        public TransformPathAnchors<TTransformState> pathAnchors => this.cursor.pathAnchors;
        public TTransformState current => this.cursor.current;

        public ReadOnlyTransformPathCursor (TransformPathCursor<TTransformState> cursor) => this.cursor = cursor;

        public override bool Equals (object other) {

            var isEqual = false;

            if (other is ReadOnlyTransformPathCursor<TTransformState> otherReadOnly) {

                isEqual = this.Equals (otherReadOnly);

            } else if (other is TransformPathCursor<TTransformState> otherCursor) {

                isEqual = this.Equals (otherCursor);

            }

            return isEqual;

        }
        public override int GetHashCode () => this.cursor.GetHashCode ();
        public virtual bool Equals (ReadOnlyTransformPathCursor<TTransformState> other) => (this.cursor == other.cursor);
        public virtual bool Equals (TransformPathCursor<TTransformState> other) => (this.cursor == other);

    }
}
