using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Denormaleezie.Denormalizers
{
    internal class JSONDenormalizer : IJSONDenormalizer
    {
        internal JSONDenormalizer()
        {

        }

        public virtual string DenormalizeToJSON<T>(IEnumerable<T> objectToDenormalize)
        {
            if (null == objectToDenormalize)
            {
                return string.Empty;
            }

            string json = JsonConvert.SerializeObject(objectToDenormalize);
            string denormalizedJson = JsonConvert.SerializeObject(ConvertToDenormalizedLists(objectToDenormalize));

            if (json.Length < denormalizedJson.Length)
            {
                return json;
            }

            return denormalizedJson;
        }

        internal virtual IEnumerable<IEnumerable<IEnumerable<object>>> ConvertToDenormalizedLists<T>(IEnumerable<T> objectToDenormalize)
        {
            if (null == objectToDenormalize)
            {
                throw new ArgumentException(nameof(objectToDenormalize) + " must not be null.", nameof(objectToDenormalize));
            }

            IEnumerable<IEnumerable<object>> denormalizedDataLists = CreateDenormalizedDataLists(objectToDenormalize);
            IEnumerable<IEnumerable<object>> dataStructureLists = CreateDataStructureLists(objectToDenormalize, denormalizedDataLists);

            return new List<IEnumerable<IEnumerable<object>>>() {
                denormalizedDataLists, dataStructureLists
            };
        }

        internal virtual IEnumerable<IEnumerable<object>> CreateDenormalizedDataLists<T>(IEnumerable<T> objectToDenormalize)
        {
            if (null == objectToDenormalize)
            {
                throw new ArgumentException(nameof(objectToDenormalize) + " must not be null.", nameof(objectToDenormalize));
            }

            List<IEnumerable<object>> denormalizedDataLists = new List<IEnumerable<object>>();

            IEnumerable<PropertyInfo> propInfos = typeof(T).GetProperties();

            foreach (var propInfo in propInfos)
            {
                List<object> dataList = new List<object>();

                dataList.Add(propInfo.Name);

                IEnumerable<object> denormalizedDataListForProp = CreateDenormalizedDataListForProperty(objectToDenormalize, propInfo);

                if (denormalizedDataListForProp.Count() < objectToDenormalize.Count())
                {
                    dataList.AddRange(denormalizedDataListForProp);
                }

                denormalizedDataLists.Add(dataList);
            }

            return denormalizedDataLists;
        }

        internal virtual IEnumerable<object> CreateDenormalizedDataListForProperty<T>(IEnumerable<T> objectToDenormalize, PropertyInfo property)
        {
            if (null == objectToDenormalize)
            {
                throw new ArgumentException(nameof(objectToDenormalize) + " must not be null.", nameof(objectToDenormalize));
            }

            if (null == property)
            {
                throw new ArgumentException(nameof(property) + " must not be null.", nameof(property));
            }

            return objectToDenormalize.Select(t => Convert.ChangeType(property.GetValue(t, null), property.PropertyType)).GroupBy(k => k).Select(k => k.Key);
        }

        internal virtual IEnumerable<IEnumerable<object>> CreateDataStructureLists<T>(IEnumerable<T> objectToDenormalize
            , IEnumerable<IEnumerable<object>> denormalizedData)
        {
            return null;
        }
    }
}
