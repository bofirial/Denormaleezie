using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Denormaleezie.Denormalizers
{
    public interface IJSONDenormalizer
    {
        string DenormalizeToJSON<T>(IEnumerable<T> objectToDenormalize);
    }
}
