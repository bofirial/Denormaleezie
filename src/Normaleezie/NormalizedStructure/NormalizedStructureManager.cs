using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Normaleezie.NormalizedStructure
{
    internal class NormalizedStructureManager
    {
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
        internal virtual List<object> CreateNormalizedStructureItem(object denormalizedItem
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

        internal virtual object GetNormalizedItemPropertyObject(object denormalizedItem, List<object> normalizedPropertyData)
        {
            if (null == denormalizedItem)
            {
                throw new ArgumentException(nameof(denormalizedItem) + " must not be null.", nameof(denormalizedItem));
            }

            if (null == normalizedPropertyData || !normalizedPropertyData.Any())
            {
                throw new ArgumentException(nameof(normalizedPropertyData) + " must not be null.", nameof(normalizedPropertyData));
            }

            string propertyName = (string)normalizedPropertyData[0];

            int index = propertyName.IndexOfAny(new char[] { '.', '~' });

            if (index > -1)
            {
                if (propertyName[index] == '.')
                {
                    return GetNormalizedItemPropertyObjectForComplexProperty(denormalizedItem, normalizedPropertyData, propertyName);
                }

                return GetNormalizedItemPropertyObjectForListProperty(denormalizedItem, normalizedPropertyData, propertyName);
            }

            return GetNormalizedItemPropertyObjectForSimpleProperty(denormalizedItem, normalizedPropertyData, propertyName);
        }

        internal virtual object GetNormalizedItemPropertyObjectForSimpleProperty(object denormalizedItem,
    List<object> normalizedPropertyData, string propertyName)
        {
            if (null == denormalizedItem)
            {
                throw new ArgumentException(nameof(denormalizedItem) + " must not be null.", nameof(denormalizedItem));
            }

            if (null == normalizedPropertyData)
            {
                throw new ArgumentException(nameof(normalizedPropertyData) + " must not be null.", nameof(normalizedPropertyData));
            }

            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException(nameof(propertyName) + " must not be null.", nameof(propertyName));
            }

            PropertyInfo propInfo = denormalizedItem.GetType().GetProperty(propertyName);
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

        internal virtual object GetNormalizedItemPropertyObjectForListProperty(object denormalizedItem,
            List<object> normalizedPropertyData, string propertyName)
        {
            if (null == denormalizedItem)
            {
                throw new ArgumentException(nameof(denormalizedItem) + " must not be null.", nameof(denormalizedItem));
            }

            if (null == normalizedPropertyData)
            {
                throw new ArgumentException(nameof(normalizedPropertyData) + " must not be null.", nameof(normalizedPropertyData));
            }

            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException(nameof(propertyName) + " must not be null.", nameof(propertyName));
            }

            string[] propertyNameParts = propertyName.Split('~');

            object list = denormalizedItem;

            if (!string.IsNullOrEmpty(propertyNameParts[0]))
            {
                PropertyInfo propInfo = denormalizedItem.GetType().GetProperty(propertyNameParts[0]);
                list = propInfo.GetValue(denormalizedItem, null);
            }

            List<List<object>> normalizedItemsPropertyObjectList = new List<List<object>>();

            foreach (var val in (IEnumerable)list)
            {
                var normalizedItemPropertyObject = new List<object>();

                foreach (var normalizedSubPropertyData in normalizedPropertyData.Skip(1))
                {
                    normalizedItemPropertyObject.Add(GetNormalizedItemPropertyObject(val, (List<object>)normalizedSubPropertyData));
                }

                normalizedItemsPropertyObjectList.Add(normalizedItemPropertyObject);
            }

            return normalizedItemsPropertyObjectList;
        }

        internal virtual object GetNormalizedItemPropertyObjectForComplexProperty(object denormalizedItem,
            List<object> normalizedPropertyData, string propertyName)
        {
            if (null == denormalizedItem)
            {
                throw new ArgumentException(nameof(denormalizedItem) + " must not be null.", nameof(denormalizedItem));
            }

            if (null == normalizedPropertyData)
            {
                throw new ArgumentException(nameof(normalizedPropertyData) + " must not be null.", nameof(normalizedPropertyData));
            }

            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException(nameof(propertyName) + " must not be null.", nameof(propertyName));
            }

            string[] propertyNameParts = propertyName.Split('.');

            object val = denormalizedItem;

            if (!string.IsNullOrEmpty(propertyNameParts[0]))
            {
                PropertyInfo propInfo = denormalizedItem.GetType().GetProperty(propertyNameParts[0]);
                val = propInfo.GetValue(denormalizedItem, null);
            }

            List<object> normalizedItemPropertyObject = new List<object>();

            foreach (var normalizedSubPropertyData in normalizedPropertyData.Skip(1))
            {
                normalizedItemPropertyObject.Add(GetNormalizedItemPropertyObject(val, (List<object>)normalizedSubPropertyData));
            }

            return normalizedItemPropertyObject;

            //string[] propertyNameParts = propertyName.Split('.');
            //string subPropName = string.Join(".", propertyNameParts.Skip(1));

            //PropertyInfo propInfo = denormalizedItem.GetType().GetProperty(propertyNameParts[0]);
            //object val = propInfo.GetValue(denormalizedItem, null);

            //List<object> subPropertyNormalizedPropertyData = new List<object>() { subPropName };

            //subPropertyNormalizedPropertyData.AddRange(normalizedPropertyData.Skip(1));

            //return GetNormalizedItemPropertyObject(val, subPropertyNormalizedPropertyData);
        }
    }
}
