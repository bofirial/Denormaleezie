using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Denormaleezie.Denormalizers
{
    internal class JSONDenormalizer : IJSONDenormalizer
    {
        internal JSONDenormalizer()
        {

        }

        public virtual string DenormalizeToJSON<T>(T objectToDenormalize) where T : IEnumerable
        {
            if (null == objectToDenormalize)
            {
                return string.Empty;
            }

            string json = JsonConvert.SerializeObject(objectToDenormalize);
            string denormalizedJson = JsonConvert.SerializeObject(ConvertToStringList(objectToDenormalize));

            if (json.Length < denormalizedJson.Length)
            {
                return json;
            }

            return denormalizedJson;
        }

        internal virtual List<string> ConvertToStringList<T>(T objectToDenormalize) where T : IEnumerable
        {
            throw new NotImplementedException();
        }
    }
}
