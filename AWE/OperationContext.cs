namespace AWE {

    public class OperationContext<T> {

        public EOperationContext context { get; protected set; }
        public T value { get; protected set; }

        public OperationContext (EOperationContext context, T value) {

            this.context = context;
            this.value = value;

        }
    }
}