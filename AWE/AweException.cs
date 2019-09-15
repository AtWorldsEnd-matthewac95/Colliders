using System;
using System.Runtime.Serialization;

namespace AWE {

    [System.Serializable]
    public class AweException : Exception {

        protected AweException ()
            : base () {
        }

        protected AweException (SerializationInfo info, StreamingContext context)
            : base (info, context) {
        }

        public AweException (string message)
            : base (message) {
        }

        public AweException (string message, Exception innerException)
            : base (message, innerException) {
        }
    }
}
