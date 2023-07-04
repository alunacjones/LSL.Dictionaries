using System.Collections;
using System.Collections.Generic;

namespace LSL.Dictionaries.Extensions
{
    /// <summary>
    /// ObjectExtensions for Dictionaries
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Converts an object into a dictionary
        /// </summary>
        /// <param name="source">Source object to convert to a dictionary</param>
        /// <returns>The dictionary version of the source object</returns>
        public static IDictionary<string, object> ToDictionary(this object source)
        {
            //TODO: cached compiled expressions
            if (source == null) return null;

            var result = new Dictionary<string, object>();

            foreach (var property in source.GetType().GetProperties())
            {
                if (property.PropertyType.IsClass && !property.PropertyType.IsAssignableFrom(typeof(string)) && !typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    result[property.Name] = property.GetValue(source).ToDictionary();
                }
                else
                {
                    result[property.Name] = property.GetValue(source);
                }
            }

            return result;
        }
    }
}