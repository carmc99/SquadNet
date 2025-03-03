// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using System.Globalization;
using System.Reflection;

public static class DictionaryModelConverter
{
    public static T ConvertDictionaryToModel<T>(Dictionary<string, string> parsedValues) where T : new()
    {
        T model = new T();
        PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (PropertyInfo property in properties)
        {
            if (!parsedValues.ContainsKey(property.Name))
            {
                continue;
            }

            string rawValue = parsedValues[property.Name]?.Trim();
            Type propertyType = property.PropertyType;

            try
            {
                if (propertyType == typeof(string))
                {
                    property.SetValue(model, rawValue);
                }
                else if (propertyType == typeof(int) || propertyType == typeof(int?))
                {
                    if (rawValue.TryParse<int>(out int intResult))
                    {
                        property.SetValue(model, intResult);
                    }
                    else if (propertyType == typeof(int?))
                    {
                        property.SetValue(model, null);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Cannot convert '{rawValue}' to int for property '{property.Name}'.");
                    }
                }
                else if (propertyType == typeof(ulong) || propertyType == typeof(ulong?))
                {
                    if (rawValue.TryParse<ulong>(out ulong ulongResult))
                    {
                        property.SetValue(model, ulongResult);
                    }
                    else if (propertyType == typeof(ulong?))
                    {
                        property.SetValue(model, null);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Cannot convert '{rawValue}' to ulong for property '{property.Name}'.");
                    }
                }
                else if (propertyType == typeof(bool) || propertyType == typeof(bool?))
                {
                    if (rawValue.TryParse<bool>(out bool boolResult))
                    {
                        property.SetValue(model, boolResult);
                    }
                    else if (propertyType == typeof(bool?))
                    {
                        property.SetValue(model, null);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Cannot convert '{rawValue}' to bool for property '{property.Name}'.");
                    }
                }
                else if (propertyType.IsEnum)
                {
                    if (int.TryParse(rawValue, out int enumValueAsInt))
                    {
                        object enumValue = Enum.ToObject(propertyType, enumValueAsInt);
                        property.SetValue(model, enumValue);
                    }
                    else
                    {
                        object enumValue = Enum.Parse(propertyType, rawValue, ignoreCase: true);
                        property.SetValue(model, enumValue);
                    }
                }
                else if (propertyType == typeof(int?))
                {
                    property.SetValue(model, rawValue == "N/A" ? null : rawValue.TryParseOrNull());
                }
                else if (propertyType == typeof(float) || propertyType == typeof(float?))
                {
                    if (rawValue.TryParse<float>(out float floatResult))
                    {
                        property.SetValue(model, floatResult);
                    }
                    else if (propertyType == typeof(float?))
                    {
                        property.SetValue(model, null);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Cannot convert '{rawValue}' to float for property '{property.Name}'.");
                    }
                }
                else if (propertyType == typeof(double) || propertyType == typeof(double?))
                {
                    if (double.TryParse(rawValue, NumberStyles.Float, CultureInfo.InvariantCulture, out double doubleResult))
                    {
                        property.SetValue(model, doubleResult);
                    }
                    else if (propertyType == typeof(double?))
                    {
                        property.SetValue(model, null);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Cannot convert '{rawValue}' to double for property '{property.Name}'.");
                    }
                }
                else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
                {
                    if (DateTime.TryParse(rawValue, out DateTime dtResult))
                    {
                        property.SetValue(model, dtResult);
                    }
                    else if (propertyType == typeof(DateTime?))
                    {
                        property.SetValue(model, null);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Cannot convert '{rawValue}' to DateTime for property '{property.Name}'.");
                    }
                }
                else
                {
                    throw new NotSupportedException($"Type '{propertyType.Name}' is not supported by the converter.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error assigning value to property '{property.Name}' in model '{typeof(T).Name}': {ex.Message}", ex);
            }
        }

        return model;
    }
}