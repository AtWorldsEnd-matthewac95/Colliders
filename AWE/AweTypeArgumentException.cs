using System;
using System.Runtime.Serialization;
using AWE;

namespace AWE {

    [System.Serializable]
    public class AweTypeArgumentException : AweException {

        protected AweTypeArgumentException ()
            : base () {
        }

        protected AweTypeArgumentException (SerializationInfo info, StreamingContext context)
            : base (info, context) {
        }

        public AweTypeArgumentException (string message)
            : base (message) {
        }

        public AweTypeArgumentException (string message, Exception innerException)
            : base (message, innerException) {
        }
    }
}
