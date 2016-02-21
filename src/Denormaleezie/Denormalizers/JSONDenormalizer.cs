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

        public virtual string DenormalizeToJSON<T>(List<T> objectToDenormalize)
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

        internal virtual List<List<List<object>>> ConvertToDenormalizedLists<T>(List<T> objectToDenormalize)
        {
            if (null == objectToDenormalize)
            {
                throw new ArgumentException(nameof(objectToDenormalize) + " must not be null.", nameof(objectToDenormalize));
            }

            List<List<object>> denormalizedDataLists = CreateDenormalizedDataLists(objectToDenormalize);
            List<List<object>> dataStructureLists = CreateDataStructureLists(objectToDenormalize, denormalizedDataLists);

            return new List<List<List<object>>>() {
                denormalizedDataLists, dataStructureLists
            };
        }

        internal virtual List<List<object>> CreateDenormalizedDataLists<T>(List<T> objectToDenormalize)
        {
            if (null == objectToDenormalize)
            {
                throw new ArgumentException(nameof(objectToDenormalize) + " must not be null.", nameof(objectToDenormalize));
            }

            List<List<object>> denormalizedDataLists = new List<List<object>>();

            IEnumerable<PropertyInfo> propInfos = typeof(T).GetProperties();

            foreach (var propInfo in propInfos)
            {
                List<object> dataList = new List<object>();

                dataList.Add(propInfo.Name);

                List<object> denormalizedDataListForProp = CreateDenormalizedDataListForProperty(objectToDenormalize, propInfo);

                if (denormalizedDataListForProp.Count < objectToDenormalize.Count)
                {
                    dataList.AddRange(denormalizedDataListForProp);
                }

                denormalizedDataLists.Add(dataList);
            }

            return denormalizedDataLists;
        }

        internal virtual List<object> CreateDenormalizedDataListForProperty<T>(List<T> objectToDenormalize, PropertyInfo property)
        {
            if (null == objectToDenormalize)
            {
                throw new ArgumentException(nameof(objectToDenormalize) + " must not be null.", nameof(objectToDenormalize));
            }

            if (null == property)
            {
                throw new ArgumentException(nameof(property) + " must not be null.", nameof(property));
            }

            return objectToDenormalize.Select(t => Convert.ChangeType(property.GetValue(t, null), property.PropertyType)).Distinct().ToList();
        }

        internal virtual List<List<object>> CreateDataStructureLists<T>(List<T> objectToDenormalize
            , List<List<object>> denormalizedData)
        {
            if (null == objectToDenormalize)
            {
                throw new ArgumentException(nameof(objectToDenormalize) + " must not be null.", nameof(objectToDenormalize));
            }

            if (null == denormalizedData)
            {
                throw new ArgumentException(nameof(denormalizedData) + " must not be null.", nameof(denormalizedData));
            }

            List<List<object>> dataStructureLists = new List<List<object>>();

            foreach (var denormalizeItem in objectToDenormalize)
            {
                dataStructureLists.Add(CreateDataStructureList(denormalizeItem, denormalizedData));
            }

            return dataStructureLists;
        }

        internal virtual List<object> CreateDataStructureList<T>(T objectToDenormalize
            , List<List<object>> denormalizedData)
        {
            if (null == objectToDenormalize)
            {
                throw new ArgumentException(nameof(objectToDenormalize) + " must not be null.", nameof(objectToDenormalize));
            }

            if (null == denormalizedData)
            {
                throw new ArgumentException(nameof(denormalizedData) + " must not be null.", nameof(denormalizedData));
            }

            List<object> dataStructureList = new List<object>();

            foreach (var denormalizedPropertyValues in denormalizedData)
            {
                dataStructureList.Add(GetDataStructureObject(objectToDenormalize, denormalizedPropertyValues));
            }

            return dataStructureList;
        }

        internal virtual object GetDataStructureObject<T>(T objectToDenormalize, List<object> denormalizedData)
        {
            if (null == objectToDenormalize)
            {
                throw new ArgumentException(nameof(objectToDenormalize) + " must not be null.", nameof(objectToDenormalize));
            }

            if (null == denormalizedData)
            {
                throw new ArgumentException(nameof(denormalizedData) + " must not be null.", nameof(denormalizedData));
            }

            string propName = (string)denormalizedData[0];
            PropertyInfo propInfo = typeof(T).GetProperty(propName);
            object val = propInfo.GetValue(objectToDenormalize, null);

            if (1 == denormalizedData.Count)
            {
                return val;
            }

            int position = denormalizedData.IndexOf(val);

            if (-1 == position)
            {
                throw new InvalidOperationException(val.ToString() + " is missing from the denormalized data list.");
            }

            return position;
        }
    }
}
