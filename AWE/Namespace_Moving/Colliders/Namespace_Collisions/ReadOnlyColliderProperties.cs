using System;

namespace AWE.Moving.Collisions {

    public class ReadOnlyColliderProperties : IEquatable<ReadOnlyColliderProperties>, IEquatable<ColliderProperties> {

        private readonly ColliderProperties properties;

        public bool isSolid => this.properties.isSolid;

        public string this [string key] => this.properties[key];

        public ReadOnlyColliderProperties (ColliderProperties properties) => this.properties = properties;

        public BooleanNote GetMappableProperty (string key) => this.properties.GetMappableProperty (key);

        public override bool Equals (object other) {

            var isEqual = false;

            if (other is ReadOnlyColliderProperties otherReadonly) {

                isEqual = this.Equals (otherReadonly);

            } else if (other is ColliderProperties otherProperties) {

                isEqual = this.Equals (otherProperties);

            }

            return isEqual;

        }
        public override int GetHashCode () => this.properties.GetHashCode ();
        public virtual bool Equals (ReadOnlyColliderProperties other) => this.properties.Equals (other.properties);
        public virtual bool Equals (ColliderProperties other) => this.properties.Equals (other);

    }
}
