using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Normaleezie.Helpers;

namespace Normaleezie.NormalizedData
{
    internal class NormalizedDataManager
    {
        private readonly ReflectionHelper _reflectionHelper;

        public NormalizedDataManager() : this(new ReflectionHelper()) {}

        public NormalizedDataManager(ReflectionHelper reflectionHelper)
        {
            this._reflectionHelper = reflectionHelper;
        }

        internal virtual List<List<object>> CreateNormalizedDataList<T>(List<T> denormalizedList, List<string> previousDataNames = null, string dataName = "")
        {
            if (null == denormalizedList)
            {
                throw new ArgumentException(nameof(denormalizedList) + " must not be null.", nameof(denormalizedList));
            }

            if (!denormalizedList.Any())
            {
                return new List<List<object>>() { new List<object>() { dataName } };
            }

            if (previousDataNames == null)
            {
                previousDataNames = new List<string>();
            }

            previousDataNames.Add(dataName);

            var result = from id in previousDataNames
                         group id by id into g
                         orderby g.Count() descending
                         select new { Id = g.Key, Count = g.Count() };

            if (result.ToList()[0].Count > 20)
            {
                throw new Exception("Circular Reference Detected in object.");
            }

            if (null != typeof(T).GetInterface("IEnumerable") && typeof(T) != typeof(string))
            {
                //return (List<List<object>>)this.GetType()
                //    .GetMethod("GetNormalizedDataForList")
                //    .MakeGenericMethod(typeof (T).GetGenericArguments().First())
                //    .Invoke(this, BindingFlags.NonPublic | BindingFlags.Instance, null, 
                //    new object[] {denormalizedList.SelectMany(i => (IEnumerable<object>) i).ToList(), dataName + "~"}, null);

                var propValues = denormalizedList.SelectMany(i => (IEnumerable<object>)i).ToList();


                var thisType2 = _reflectionHelper.GetType();
                var method2 = thisType2.GetMethod("ConvertList", BindingFlags.NonPublic | BindingFlags.Instance);
                var genMeth2 = method2.MakeGenericMethod(typeof(T).GetGenericArguments().First());


                var typedPropValues = genMeth2.Invoke(_reflectionHelper, new object[] { propValues });

                var thisType = this.GetType();
                var method = thisType.GetMethod("CreateNormalizedDataList", BindingFlags.NonPublic | BindingFlags.Instance);
                var genMeth = method.MakeGenericMethod(typeof(T).GetGenericArguments().First());

                var normalizedDataForList = new List<object>() { dataName + "~" };

                normalizedDataForList.AddRange(((List<List<object>>)genMeth.Invoke(this, new object[] { typedPropValues, previousDataNames, null })));

                return new List<List<object>>() { normalizedDataForList };

                //return (List<List<object>>) genMeth.Invoke(this, new object[] { typedPropValues, dataName + "~"});

                //return GetNormalizedDataForList(denormalizedList.SelectMany(i => (IEnumerable<dynamic>)i).ToList(), dataName + "~");
            }

            if (!_reflectionHelper.IsSimpleType(typeof(T)))
            {
                List<List<object>> normalizedDataForProperties = new List<List<object>>();

                var props = typeof(T).GetProperties().ToList();

                props.Sort((a, b) => string.CompareOrdinal(a.Name, b.Name));

                foreach (var subProperty in props)
                {
                    var propValues = denormalizedList.Select(t =>
                        Convert.ChangeType(subProperty.GetValue(t, null), subProperty.PropertyType)).ToList();


                    var thisType2 = _reflectionHelper.GetType();
                    var method2 = thisType2.GetMethod("ConvertList", BindingFlags.NonPublic | BindingFlags.Instance);
                    var genMeth2 = method2.MakeGenericMethod(subProperty.PropertyType);


                    var typedPropValues = genMeth2.Invoke(_reflectionHelper, new object[] { propValues });

                    var thisType = this.GetType();
                    var method = thisType.GetMethod("CreateNormalizedDataList", BindingFlags.NonPublic | BindingFlags.Instance);
                    var genMeth = method.MakeGenericMethod(subProperty.PropertyType);

                    normalizedDataForProperties.AddRange(((List<List<object>>)genMeth.Invoke(this, new object[] { typedPropValues, previousDataNames, subProperty.Name })));

                    //normalizedDataForList.AddRange(
                    //    GetNormalizedDataForList(denormalizedList.Select(t => (dynamic)Convert.ChangeType(subProperty.GetValue(t, null), subProperty.PropertyType)).ToList(),
                    //    dataNameForProperty));
                }

                if (string.IsNullOrEmpty(dataName))
                {
                    return normalizedDataForProperties;
                }
                List<object> normalizedDataForList = new List<object>() { dataName + "." };

                normalizedDataForList.AddRange(normalizedDataForProperties);

                return new List<List<object>>() { normalizedDataForList };
            }

            List<object> normalizedData = new List<object>() { dataName };
            List<object> uniqueValues = denormalizedList.Select(i => (object)i).Distinct().ToList();

            if (uniqueValues.Count < denormalizedList.Count)
            {
                normalizedData.AddRange(uniqueValues);
            }

            return new List<List<object>>() { normalizedData };
        }
    }
}
