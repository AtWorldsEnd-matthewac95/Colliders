using System;
using System.Runtime.Serialization;
using AWE;

namespace AWE.Math {

    [System.Serializable]
    public class AweMathException : AweException {

        protected AweMathException ()
            : base () {
        }

        protected AweMathException (SerializationInfo info, StreamingContext context)
            : base (info, context) {
        }

        public AweMathException (string message)
            : base (message) {
        }

        public AweMathException (string message, Exception innerException)
            : base (message, innerException) {
        }
    }
}
