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

        internal virtual List<List<List<string>>> ConvertToDenormalizedLists<T>(IEnumerable<T> objectToDenormalize)
        {
            if (null == objectToDenormalize)
            {
                throw new ArgumentException(nameof(objectToDenormalize) + " must not be null.", nameof(objectToDenormalize));
            }

            List<List<string>> denormalizedDataLists = CreateDenormalizedDataLists(objectToDenormalize);
            List<List<string>> dataStructureLists = CreateDataStructureLists(objectToDenormalize, denormalizedDataLists);

            return new List<List<List<string>>>()
            {
                denormalizedDataLists,
                dataStructureLists
            };
        }

        internal virtual List<List<string>> CreateDenormalizedDataLists<T>(IEnumerable<T> objectToDenormalize)
        {
            if (null == objectToDenormalize)
            {
                throw new ArgumentException(nameof(objectToDenormalize) + " must not be null.", nameof(objectToDenormalize));
            }

            List<List<string>> denormalizedDataLists = new List<List<string>>();

            IEnumerable<PropertyInfo> propInfos = typeof(T).GetProperties();

            foreach (var propInfo in propInfos)
            {
                List<string> dataList = new List<string>();

                dataList.Add(propInfo.Name);

                List<string> denormalizedDataListForProp = CreateDenormalizedDataListForProperty(objectToDenormalize, propInfo);

                if (denormalizedDataListForProp.Count() < objectToDenormalize.Count())
                {
                    dataList.AddRange(denormalizedDataListForProp);
                }

                denormalizedDataLists.Add(dataList);
            }

            return denormalizedDataLists;
        }

        internal virtual List<string> CreateDenormalizedDataListForProperty<T>(IEnumerable<T> objectToDenormalize, PropertyInfo property)
        {
            throw new NotImplementedException();
        }

        internal virtual List<List<string>> CreateDataStructureLists<T>(IEnumerable<T> objectToDenormalize, List<List<string>> denormalizedData)
        {
            throw new NotImplementedException();
        }

        internal virtual List<List<List<string>>> ConvertToStringListTest<T>(IEnumerable<T> objectToDenormalize)
        {
            IEnumerable<PropertyInfo> propInfos = typeof(T).GetProperties();

            List<List<string>> denormalizedObject = new List<List<string>>();

            foreach (var propInfo in propInfos)
            {
                List<string> dataList = new List<string>();

                dataList.Add(propInfo.Name);
                
                dataList.AddRange(objectToDenormalize.Select(t => propInfo.GetValue(t, null).ToString()).GroupBy(k => k).Select(k => k.Key));

                denormalizedObject.Add(dataList);
            }

            throw new NotImplementedException();
        }
    }
}
