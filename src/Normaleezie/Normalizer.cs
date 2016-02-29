using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Normaleezie
{
    public class Normalizer
    {
        public virtual List<List<List<object>>> Normalize<T>(List<T> denormalizedList)
        {
            if (null == denormalizedList)
            {
                return new List<List<List<object>>>();
            }

            return ConvertToNormalizedForm(denormalizedList);
        }

        internal virtual List<List<List<object>>> ConvertToNormalizedForm<T>(List<T> denormalizedList)
        {
            if (null == denormalizedList)
            {
                throw new ArgumentException(nameof(denormalizedList) + " must not be null.", nameof(denormalizedList));
            }

            List<List<object>> normalizedDataList = CreateNormalizedDataList(denormalizedList);
            List<List<object>> normalizedStructureList = CreateNormalizedStructureList(denormalizedList, normalizedDataList);

            return new List<List<List<object>>>() {
                normalizedDataList, normalizedStructureList
            };
        }

        internal virtual List<List<object>> CreateNormalizedDataList<T>(List<T> denormalizedList)
        {
            if (null == denormalizedList)
            {
                throw new ArgumentException(nameof(denormalizedList) + " must not be null.", nameof(denormalizedList));
            }

            List<List<object>> normalizedDataList = new List<List<object>>();

            IEnumerable<PropertyInfo> propInfos = typeof(T).GetProperties();

            foreach (var propInfo in propInfos)
            {
                normalizedDataList.AddRange(GetNormalizedDataForProperty(denormalizedList, propInfo));
            }

            return normalizedDataList;
        }

        internal virtual List<List<object>> GetNormalizedDataForProperty<T>(List<T> denormalizedList, PropertyInfo property)
        {
            if (null == denormalizedList)
            {
                throw new ArgumentException(nameof(denormalizedList) + " must not be null.", nameof(denormalizedList));
            }

            if (null == property)
            {
                throw new ArgumentException(nameof(property) + " must not be null.", nameof(property));
            }

            List<object> normalizedPropertyData = new List<object>();

            normalizedPropertyData.Add(property.Name);

            List<object> uniquePropertyValues = GetUniquePropertyValues(denormalizedList, property);

            if (uniquePropertyValues.Count < denormalizedList.Count)
            {
                normalizedPropertyData.AddRange(uniquePropertyValues);
            }
            return new List<List<object>>() { normalizedPropertyData };
        }

        internal virtual List<object> GetUniquePropertyValues<T>(List<T> objects, PropertyInfo property)
        {
            if (null == objects)
            {
                throw new ArgumentException(nameof(objects) + " must not be null.", nameof(objects));
            }

            if (null == property)
            {
                throw new ArgumentException(nameof(property) + " must not be null.", nameof(property));
            }

            return objects.Select(t => Convert.ChangeType(property.GetValue(t, null), property.PropertyType))
                .Distinct()
                .ToList();
        }

        internal virtual List<List<object>> CreateNormalizedStructureList<T>(List<T> denormalizedList
            , List<List<object>> normalizedDataList)
        {
            if (null == denormalizedList)
            {
                throw new ArgumentException(nameof(denormalizedList) + " must not be null.", nameof(denormalizedList));
            }

            if (null == normalizedDataList)
            {
                throw new ArgumentException(nameof(normalizedDataList) + " must not be null.", nameof(normalizedDataList));
            }

            return denormalizedList.Select(denormalizedItem => CreateNormalizedStructureItem(denormalizedItem, normalizedDataList)).ToList();
        }

        internal virtual List<object> CreateNormalizedStructureItem<T>(T denormalizedItem
            , List<List<object>> normalizedDataList)
        {
            if (null == denormalizedItem)
            {
                throw new ArgumentException(nameof(denormalizedItem) + " must not be null.", nameof(denormalizedItem));
            }

            if (null == normalizedDataList)
            {
                throw new ArgumentException(nameof(normalizedDataList) + " must not be null.", nameof(normalizedDataList));
            }

            return normalizedDataList.Select(normalizedPropertyData => GetNormalizedItemPropertyObject(denormalizedItem, normalizedPropertyData)).ToList();
        }

        internal virtual object GetNormalizedItemPropertyObject<T>(T denormalizedItem, List<object> normalizedPropertyData)
        {
            if (null == denormalizedItem)
            {
                throw new ArgumentException(nameof(denormalizedItem) + " must not be null.", nameof(denormalizedItem));
            }

            if (null == normalizedPropertyData)
            {
                throw new ArgumentException(nameof(normalizedPropertyData) + " must not be null.", nameof(normalizedPropertyData));
            }

            string propName = (string)normalizedPropertyData[0];
            PropertyInfo propInfo = typeof(T).GetProperty(propName);
            object val = propInfo.GetValue(denormalizedItem, null);

            if (1 == normalizedPropertyData.Count)
            {
                return val;
            }

            int position = normalizedPropertyData.IndexOf(val);

            if (-1 == position)
            {
                throw new InvalidOperationException(val.ToString() + " is missing from the denormalized data list.");
            }

            return position;
        }
    }
}
