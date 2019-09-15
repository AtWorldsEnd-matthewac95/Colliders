using System;
using System.Runtime.Serialization;
using AWE;

namespace AWE {

    [System.Serializable]
    public class AweOrphanedNodeException : AweException {

        protected AweOrphanedNodeException ()
            : base () {
        }

        protected AweOrphanedNodeException (SerializationInfo info, StreamingContext context)
            : base (info, context) {
        }

        public AweOrphanedNodeException (string message)
            : base (message) {
        }

        public AweOrphanedNodeException (string message, Exception innerException)
            : base (message, innerException) {
        }
    }
}
