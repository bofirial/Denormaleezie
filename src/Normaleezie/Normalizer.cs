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
            if (null == denormalizedList || !denormalizedList.Any())
            {
                return new List<List<List<object>>>();
            }

            return ConvertToNormalizedForm(denormalizedList);
        }

        internal virtual List<List<List<object>>> ConvertToNormalizedForm<T>(List<T> denormalizedList)
        {
            List<List<object>> normalizedDataList = CreateNormalizedDataList(denormalizedList);
            List<List<object>> normalizedStructureList = CreateNormalizedStructureList(denormalizedList.Select(i => (object)i).ToList(), normalizedDataList);

            return new List<List<List<object>>>() {
                normalizedDataList, normalizedStructureList
            };
        }

        internal virtual List<List<object>> CreateNormalizedDataList<T>(List<T> denormalizedList)
        {
            List<List<object>> normalizedDataList = GetNormalizedDataForList(denormalizedList);

            //normalizedDataList.Sort((a, b) => string.CompareOrdinal(a[0].ToString(), b[0].ToString()));

            return normalizedDataList;
        }

        internal virtual List<List<object>> GetNormalizedDataForList<T>(List<T> denormalizedList, string dataName = "")
        {
            if (null == denormalizedList)
            {
                throw new ArgumentException(nameof(denormalizedList) + " must not be null.", nameof(denormalizedList));
            }

            if (!denormalizedList.Any())
            {
                return new List<List<object>>() { new List<object>() { dataName } };
            }

            if (null != typeof(T).GetInterface("IEnumerable") && typeof(T) != typeof(string))
            {
                //return (List<List<object>>)this.GetType()
                //    .GetMethod("GetNormalizedDataForList")
                //    .MakeGenericMethod(typeof (T).GetGenericArguments().First())
                //    .Invoke(this, BindingFlags.NonPublic | BindingFlags.Instance, null, 
                //    new object[] {denormalizedList.SelectMany(i => (IEnumerable<object>) i).ToList(), dataName + "~"}, null);

                var propValues = denormalizedList.SelectMany(i => (IEnumerable<object>)i).ToList();


                var thisType2 = this.GetType();
                var method2 = thisType2.GetMethod("ConvertList", BindingFlags.NonPublic | BindingFlags.Instance);
                var genMeth2 = method2.MakeGenericMethod(typeof(T).GetGenericArguments().First());


                var typedPropValues = genMeth2.Invoke(this, new object[] { propValues });

                var thisType = this.GetType();
                var method = thisType.GetMethod("GetNormalizedDataForList", BindingFlags.NonPublic | BindingFlags.Instance);
                var genMeth = method.MakeGenericMethod(typeof(T).GetGenericArguments().First());

                var normalizedDataForList = new List<object>() {dataName + "~"};

                normalizedDataForList.AddRange(((List<List<object>>)genMeth.Invoke(this, new object[] { typedPropValues, null })));

                return new List<List<object>>() { normalizedDataForList };

                //return (List<List<object>>) genMeth.Invoke(this, new object[] { typedPropValues, dataName + "~"});

                //return GetNormalizedDataForList(denormalizedList.SelectMany(i => (IEnumerable<dynamic>)i).ToList(), dataName + "~");
            }

            if (!IsSimpleType(typeof(T)))
            {
                List<List<object>> normalizedDataForProperties = new List<List<object>>();

                var props = typeof (T).GetProperties().ToList();

                props.Sort((a, b) => string.CompareOrdinal(a.Name, b.Name));
                
                foreach (var subProperty in props)
                {
                    var propValues = denormalizedList.Select(t =>
                        Convert.ChangeType(subProperty.GetValue(t, null), subProperty.PropertyType)).ToList();


                    var thisType2 = this.GetType();
                    var method2 = thisType2.GetMethod("ConvertList", BindingFlags.NonPublic | BindingFlags.Instance);
                    var genMeth2 = method2.MakeGenericMethod(subProperty.PropertyType);

                    
                    var typedPropValues = genMeth2.Invoke(this, new object[] { propValues });

                    var thisType = this.GetType();
                    var method = thisType.GetMethod("GetNormalizedDataForList", BindingFlags.NonPublic | BindingFlags.Instance);
                    var genMeth = method.MakeGenericMethod(subProperty.PropertyType);

                    normalizedDataForProperties.AddRange(((List<List<object>>)genMeth.Invoke(this, new object[] { typedPropValues, subProperty.Name })));

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

        internal virtual List<T> ConvertList<T>(List<object> list)
        {
            List<T> newlist = list.Cast<T>().ToList();
            return newlist;
        } 

        //internal virtual List<List<object>> GetNormalizedDataForList(List<object> denormalizedList, string dataNamePrefix = null)
        //{
        //    if (null == denormalizedList)
        //    {
        //        throw new ArgumentException(nameof(denormalizedList) + " must not be null.", nameof(denormalizedList));
        //    }

        //    if (!denormalizedList.Any())
        //    {
        //        return new List<List<object>>() { new List<object>() { dataNamePrefix } };
        //    }

        //    object firstItem = denormalizedList.FirstOrDefault(o => o != null);

        //    if (firstItem == null)
        //    {
        //        return new List<List<object>>() { new List<object>() { dataNamePrefix, null } };
        //    }

        //    Type listItemType = firstItem.GetType();

        //    if (null != listItemType.GetInterface("IEnumerable") && listItemType != typeof(string))
        //    {
        //        return GetNormalizedDataForList(denormalizedList.SelectMany(i => (IEnumerable<object>)i).ToList(), dataNamePrefix + "~");
        //    }

        //    List<List<object>> normalizedDataForList = new List<List<object>>();

        //    foreach (var subProperty in listItemType.GetProperties())
        //    {
        //        normalizedDataForList.AddRange(GetNormalizedDataForProperty(denormalizedList, subProperty, dataNamePrefix));
        //    }

        //    return normalizedDataForList;
        //}

        internal virtual List<List<object>> GetNormalizedDataForProperty(List<object> denormalizedList, PropertyInfo property, string propertyNamePrefix = null)
        {
            if (null == denormalizedList)
            {
                throw new ArgumentException(nameof(denormalizedList) + " must not be null.", nameof(denormalizedList));
            }

            if (null == property)
            {
                throw new ArgumentException(nameof(property) + " must not be null.", nameof(property));
            }

            if (!IsSimpleType(property.PropertyType))
            {
                if (null != property.PropertyType.GetInterface("IEnumerable") && property.PropertyType != typeof(string))
                {
                    return GetNormalizedDataForListProperty(denormalizedList, property, propertyNamePrefix);
                }

                return GetNormalizedDataForComplexProperty(denormalizedList, property, propertyNamePrefix);
            }

            return GetNormalizedDataForSimpleProperty(denormalizedList, property, propertyNamePrefix);
        }

        internal virtual List<List<object>> GetNormalizedDataForListProperty(List<object> denormalizedList, PropertyInfo property, string propertyNamePrefix = null)
        {
            if (null == denormalizedList)
            {
                throw new ArgumentException(nameof(denormalizedList) + " must not be null.", nameof(denormalizedList));
            }
            
            if (null == property)
            {
                throw new ArgumentException(nameof(property) + " must not be null.", nameof(property));
            }
            
            List<object> childList = denormalizedList.Select(t => Convert.ChangeType(property.GetValue(t, null), property.PropertyType)).ToList();
            string subPropertyNamePrefix = propertyNamePrefix + property.Name + "~";
            
            childList = childList.SelectMany(i => (IEnumerable<object>)i).ToList();

            return GetNormalizedDataForList(childList, subPropertyNamePrefix);
        }

        internal virtual List<List<object>> GetNormalizedDataForComplexProperty(List<object> denormalizedList, PropertyInfo property, string propertyNamePrefix = null)
        {
            if (null == denormalizedList)
            {
                throw new ArgumentException(nameof(denormalizedList) + " must not be null.", nameof(denormalizedList));
            }

            if (null == property)
            {
                throw new ArgumentException(nameof(property) + " must not be null.", nameof(property));
            }

            List<List<object>> normalizedDataForProperty = new List<List<object>>();
            List<object> childList = denormalizedList.Select(t => Convert.ChangeType(property.GetValue(t, null), property.PropertyType)).ToList();
            string subPropertyNamePrefix = propertyNamePrefix + property.Name + ".";

            foreach (var subProperty in property.PropertyType.GetProperties())
            {
                normalizedDataForProperty.AddRange(GetNormalizedDataForProperty(childList, subProperty, subPropertyNamePrefix));
            }

            return normalizedDataForProperty;
        }

        internal virtual List<List<object>> GetNormalizedDataForSimpleProperty(List<object> denormalizedList, PropertyInfo property, string propertyNamePrefix = null)
        {
            if (null == denormalizedList)
            {
                throw new ArgumentException(nameof(denormalizedList) + " must not be null.", nameof(denormalizedList));
            }

            if (null == property)
            {
                throw new ArgumentException(nameof(property) + " must not be null.", nameof(property));
            }

            List<object> normalizedPropertyData = new List<object>
            {
                string.Join(string.Empty, propertyNamePrefix, property.Name)
            };

            List<object> uniquePropertyValues = GetUniquePropertyValues(denormalizedList, property);

            if (uniquePropertyValues.Count < denormalizedList.Count)
            {
                normalizedPropertyData.AddRange(uniquePropertyValues);
            }

            return new List<List<object>>() { normalizedPropertyData };
        }

        internal virtual List<List<object>> CreateNormalizedData(List<object> denormalizedList, string dataName, List<object> uniqueValues)
        {
            List<object> normalizedPropertyData = new List<object>{ dataName };

            if (uniqueValues.Count < denormalizedList.Count)
            {
                normalizedPropertyData.AddRange(uniqueValues);
            }

            return new List<List<object>>() { normalizedPropertyData };
        }

        internal virtual bool IsSimpleType(Type type)
        {
            if (null == type)
            {
                throw new ArgumentException(nameof(type) + " must not be null.", nameof(type));
            }

            return Type.GetTypeCode(type) != TypeCode.Object;
        } 

        internal virtual List<object> GetUniquePropertyValues(List<object> objects, PropertyInfo property)
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

        internal virtual List<object> GetUniqueValues(List<object> objects)
        {
            if (null == objects)
            {
                throw new ArgumentException(nameof(objects) + " must not be null.", nameof(objects));
            }

            return objects.Distinct().ToList();
        }

        internal virtual List<List<object>> CreateNormalizedStructureList(List<object> denormalizedList
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

            int index = propertyName.IndexOfAny(new char[] {'.', '~'});

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
            string subPropName = string.Join("~", propertyNameParts.Skip(1));

            object list = denormalizedItem;

            if (!string.IsNullOrEmpty(propertyNameParts[0]))
            {
                PropertyInfo propInfo = denormalizedItem.GetType().GetProperty(propertyNameParts[0]);
                list = propInfo.GetValue(denormalizedItem, null); 
            }

            List<object> normalizedItemPropertyObject = new List<object>();

            foreach (var val in (IEnumerable)list)
            {
                normalizedItemPropertyObject.Add(GetNormalizedItemPropertyObject(val, normalizedPropertyData.Skip(1).ToList()));
            }

            return normalizedItemPropertyObject;
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
            string subPropName = string.Join(".", propertyNameParts.Skip(1));

            PropertyInfo propInfo = denormalizedItem.GetType().GetProperty(propertyNameParts[0]);
            object val = propInfo.GetValue(denormalizedItem, null);

            List<object> subPropertyNormalizedPropertyData = new List<object>() { subPropName };

            subPropertyNormalizedPropertyData.AddRange(normalizedPropertyData.Skip(1));

            return GetNormalizedItemPropertyObject(val, subPropertyNormalizedPropertyData);
        }
    }
}
