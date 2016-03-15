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
        internal virtual List<List<object>> CreateNormalizedStructure<T>(List<T> denormalizedList
            , List<List<object>> normalizedData)
        {
            if (null == denormalizedList)
            {
                throw new ArgumentNullException(nameof(denormalizedList));
            }

            return denormalizedList.Select(denormalizedItem => CreateNormalizedStructureItem(denormalizedItem, normalizedData)).ToList();
        }
        internal virtual List<object> CreateNormalizedStructureItem(object denormalizedItem, List<List<object>> normalizedData)
        {
            if (null == normalizedData)
            {
                throw new ArgumentNullException(nameof(normalizedData));
            }

            return normalizedData.Select(normalizedDataItem => GetNormalizedField(denormalizedItem, normalizedDataItem)).ToList();
        }

        internal virtual object GetNormalizedField(object denormalizedItem, List<object> normalizedDataItem)
        {
            if (null == normalizedDataItem || !normalizedDataItem.Any())
            {
                throw new ArgumentNullException(nameof(normalizedDataItem));
            }

            string dataName = (string)normalizedDataItem[0];

            if (dataName.EndsWith("."))
            {
                return GetNormalizedFieldForComplexType(denormalizedItem, normalizedDataItem, dataName);
            }

            if (dataName.EndsWith("~"))
            {
                return GetNormalizedFieldForIEnumerableType(denormalizedItem, normalizedDataItem, dataName);
            }

            return GetNormalizedFieldForSimpleType(denormalizedItem, normalizedDataItem, dataName);
        }

        internal virtual object GetNormalizedFieldForSimpleType(object denormalizedItem, List<object> normalizedDataItem, string dataName)
        {
            if (null == denormalizedItem)
            {
                throw new ArgumentNullException(nameof(denormalizedItem) + " must not be null.", nameof(denormalizedItem));
            }

            if (null == normalizedDataItem)
            {
                throw new ArgumentNullException(nameof(normalizedDataItem) + " must not be null.", nameof(normalizedDataItem));
            }
            
            object val = denormalizedItem;

            if (!string.IsNullOrEmpty(dataName))
            {
                PropertyInfo propInfo = denormalizedItem.GetType().GetProperty(dataName);
                val = propInfo.GetValue(denormalizedItem, null);
            }

            if (1 == normalizedDataItem.Count)
            {
                return val;
            }

            int position = normalizedDataItem.IndexOf(val);

            if (-1 == position)
            {
                throw new InvalidOperationException($"{val} is missing from the denormalized data item for {dataName}.");
            }

            return position;
        }

        internal virtual object GetNormalizedFieldForIEnumerableType(object denormalizedItem, List<object> normalizedDataItem, string dataName)
        {
            if (null == normalizedDataItem)
            {
                throw new ArgumentNullException(nameof(normalizedDataItem));
            }

            IEnumerable<object> list = (IEnumerable<object>)GetTargetDenormalizedItemByDataName(denormalizedItem, dataName, '~');

            List<List<object>> normalizedDataForListItem = normalizedDataItem.Skip(1).Cast<List<object>>().ToList();

            return list.Select(listItem => CreateNormalizedStructureItem(listItem, normalizedDataForListItem)).ToList();
        }

        internal virtual object GetNormalizedFieldForComplexType(object denormalizedItem, List<object> normalizedDataItem, string dataName)
        {
            if (null == normalizedDataItem)
            {
                throw new ArgumentNullException(nameof(normalizedDataItem));
            }
            
            object targetDenormalizedItem = GetTargetDenormalizedItemByDataName(denormalizedItem, dataName, '.');

            List<List<object>> normalizedDataForListItem = normalizedDataItem.Skip(1).Cast<List<object>>().ToList();

            return CreateNormalizedStructureItem(targetDenormalizedItem, normalizedDataForListItem);
        }

        internal virtual object GetTargetDenormalizedItemByDataName(object denormalizedItem, string dataName, char suffixSymbol)
        {
            if (null == denormalizedItem)
            {
                throw new ArgumentNullException(nameof(denormalizedItem));
            }

            if (string.IsNullOrEmpty(dataName))
            {
                throw new ArgumentNullException(nameof(dataName));
            }

            object targetDenormalizedItem;

            if (string.IsNullOrEmpty(dataName.TrimEnd(suffixSymbol)))
            {
                targetDenormalizedItem = denormalizedItem;
            }
            else
            {
                PropertyInfo propInfo = denormalizedItem.GetType().GetProperty(dataName.TrimEnd(suffixSymbol));
                targetDenormalizedItem = propInfo.GetValue(denormalizedItem, null);
            }
            return targetDenormalizedItem;
        }
    }
}
