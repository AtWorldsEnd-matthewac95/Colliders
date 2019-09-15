using System.Collections.Generic;
using AWE;

namespace AWE.Math {

    public sealed class Shape2DTypeCatalog {

        public static readonly Shape2DTypeCatalog singleton;

        static Shape2DTypeCatalog () {

            singleton = new Shape2DTypeCatalog (
                Shape2DType.None,
                Shape2DType.Any,
                Shape2DType.Polygon,
                Shape2DType.Circle,
                Shape2DType.Quadratic
            );

        }

        private readonly Dictionary<string, Shape2DType> catalog;
        private readonly HashSet<int> indicies;

        public int count => this.catalog.Count;

        public Shape2DType this [string name] => this.catalog[name];

        public Shape2DTypeCatalog (params Shape2DType[] types) {

            this.catalog = new Dictionary<string, Shape2DType> (types.Length);
            this.indicies = new HashSet<int> ();

            for (int i = 0; i < types.Length; i++) {

                this.Register (types[i]);

            }
        }

        public BooleanNote Register (Shape2DType type) {

            var result = new BooleanNote (true);

            if (this.indicies.Contains (type.index)) {

                result = new BooleanNote (
                    false,
                    "This catalog already has a type with the given index. "
                );

            } else {

                if (this.catalog.ContainsKey (type.name)) {

                    this.catalog[type.name] = type;

                } else {

                    this.catalog.Add (type.name, type);

                }
            }

            return result;

        }

        public Shape2DType Register (string name) {

            if (this.catalog.ContainsKey (name)) {

                // TODO - Throw an exception.

            }

            var index = this.count;

            while (!this.indicies.Contains (index)) {

                index++;

            }

            var type = new Shape2DType (name, index);
            this.catalog.Add (name, type);
            return type;

        }

        public bool Contains (string name) => this.catalog.ContainsKey (name);

        public bool Contains (int index) => this.indicies.Contains (index);

        public bool Contains (Shape2DType type) => this.catalog.ContainsKey (type.name);
    }
}
