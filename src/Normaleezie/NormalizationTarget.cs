using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Normaleezie
{
    public class NormalizationTarget
    {
        public Type Type { get; set; }

        public string DataName { get; set; }

        public Func<List<object>, List<object>> GetNormalizationDataFunction { get; set; }
    }
}
