using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Normaleezie.Denormalizers
{
    public interface IJSONDenormalizer
    {
        string DenormalizeToJSON<T>(List<T> objectToDenormalize);
    }
}
