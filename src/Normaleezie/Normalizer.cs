using Normaleezie.Denormalizers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Normaleezie
{
    public class Normalizer
    {
        IJSONDenormalizer jsonDenormalizer;

        public Normalizer() : this(new JSONDenormalizer()) { }

        internal Normalizer(IJSONDenormalizer jsonDenormalizer)
        {
            this.jsonDenormalizer = jsonDenormalizer;
        }

        public string DenormalizeToJSON<T>(List<T> objectToDenormalize)
        {
            return this.jsonDenormalizer.DenormalizeToJSON(objectToDenormalize);
        }
    }
}
