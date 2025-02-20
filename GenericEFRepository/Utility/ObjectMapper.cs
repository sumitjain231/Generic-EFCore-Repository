using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class ObjectMapper<TSource>
    {
        private readonly object _source;

        public ObjectMapper(TSource source)
        {
            _source = source!;
        }

        public TDestination To<TDestination>(TDestination destination = null!) where TDestination : class
        {
            Type destinationType = typeof(TDestination);

            if (destination == null)
                destination = Activator.CreateInstance<TDestination>();

            EnumeratePropertiesAndSetValues(destinationType, destination);

            return destination;
        }

        public object PropertyToObject(Type destinationType, object destination = null!)
        {
            if (destination == null)
                destination = Activator.CreateInstance(destinationType)!;

            EnumeratePropertiesAndSetValues(destinationType, destination);

            return destination;
        }

        public TDestination MapProperties<TDestination>(TDestination destination) where TDestination : class
        {
            Type destinationType = typeof(TDestination);

            EnumeratePropertiesAndSetValues(destinationType, destination);

            return destination;
        }

        private static object SafeGetValue(PropertyInfo sourceProperty, object source)
        {
            object sourceValue = sourceProperty.GetValue(source)!;

            if (sourceValue != null)
            {
                return sourceValue;
            }

            Type sourceType = source.GetType();
            return (sourceType.IsValueType ? Activator.CreateInstance(sourceType) : null)!;
        }

        private void EnumeratePropertiesAndSetValues(Type destinationType, object destination)
        {
            Type sourceType = _source.GetType();

            PropertyInfo[] destinationProperties = destinationType.GetProperties();
            foreach (PropertyInfo destinationProperty in destinationProperties)
            {
                if (!PropertyExistsInSource(destinationProperty.Name, sourceType))
                {
                    continue;
                }

                Type destinationPropertyType = destinationProperty.PropertyType;

                if (destinationPropertyType.IsInterface || (destinationType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(destinationType)))
                {
                    continue;
                }

                object destinationValue = DetermineDestinationValue(destinationProperty.Name, destinationPropertyType);

                destinationProperty.SetValue(destination, destinationValue);
            }
        }

        private static bool PropertyExistsInSource(string propertyName, Type destinationType)
        {
            return destinationType.GetProperty(propertyName) != null;
        }

        private object DetermineDestinationValue(string sourcePropertyName, Type destinationPropertyType)
        {
            PropertyInfo sourceProperty = _source.GetType().GetProperty(sourcePropertyName)!;
            object sourceValue = SafeGetValue(sourceProperty, _source);
            if (sourceValue == null)
            {
                return null!;
            }

            Type sourcePropertyType = sourceProperty.PropertyType;

            if (CanAssignDirectly(sourcePropertyType, destinationPropertyType, sourceValue))
            {
                return sourceValue;
            }
            else
            {
                ObjectMapper<object> mapper = new ObjectMapper<object>(sourceValue);
                var destinationObject = mapper.PropertyToObject(destinationPropertyType);
                return destinationObject;
            }
        }

        private static bool CanAssignDirectly(Type sourceType, Type destinationPropertyType, object sourceValue)
        {
            return sourceValue.GetType() == destinationPropertyType || sourceType.IsAssignableFrom(destinationPropertyType);
        }
    }
}
