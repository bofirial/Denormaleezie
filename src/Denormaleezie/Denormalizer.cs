using Denormaleezie.Denormalizers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Denormaleezie
{
    public class Denormalizer
    {
        IJSONDenormalizer jsonDenormalizer;

        public Denormalizer() : this(new JSONDenormalizer()) { }

        internal Denormalizer(IJSONDenormalizer jsonDenormalizer)
        {
            this.jsonDenormalizer = jsonDenormalizer;
        }

        public string DenormalizeToJSON<T>(List<T> objectToDenormalize)
        {
            return this.jsonDenormalizer.DenormalizeToJSON(objectToDenormalize);
        }
    }
}
