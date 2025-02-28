using System;
using System.Collections.Generic;
using System.Reflection;
using SquadNET.Core;

public static class DictionaryModelConverter
{
    /// <summary>
    /// Convierte un diccionario de valores en una instancia del modelo T,
    /// asignando cada valor al atributo correspondiente del modelo si coincide el nombre de la propiedad.
    /// </summary>
    /// <typeparam name="T">Tipo del modelo de salida (por ejemplo, PlayerConnectedInfo).</typeparam>
    /// <param name="parsedValues">Diccionario con los valores a parsear.</param>
    /// <returns>Instancia de T con las propiedades asignadas desde el diccionario.</returns>
    /// <exception cref="InvalidOperationException">Se lanza si ocurren errores al parsear un valor.</exception>
    public static T ConvertDictionaryToModel<T>(Dictionary<string, string> parsedValues) where T : new()
    {
        // Se crea una instancia vacía del modelo
        T model = new T();

        // Obtenemos todas las propiedades públicas del tipo T
        PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (PropertyInfo property in properties)
        {
            // Verificamos si el diccionario contiene una clave igual al nombre de la propiedad
            if (!parsedValues.ContainsKey(property.Name))
            {
                continue;
            }

            // Valor bruto proveniente del diccionario
            // Se hace un Trim() por si hay espacios extra
            string rawValue = parsedValues[property.Name]?.Trim();

            // Obtenemos el tipo de la propiedad para parsear correctamente
            Type propertyType = property.PropertyType;

            try
            {
                // Para simplificar, controlamos conversiones básicas con los métodos de extensión TryParse<T>().
                if (propertyType == typeof(string))
                {
                    // Simplemente se asigna la cadena al modelo
                    property.SetValue(model, rawValue);
                }
                else if (propertyType == typeof(int) || propertyType == typeof(int?))
                {
                    // Se utiliza nuestro método genérico de parseo
                    if (rawValue.TryParse<int>(out int intResult))
                    {
                        property.SetValue(model, intResult);
                    }
                    else if (propertyType == typeof(int?))
                    {
                        // Si es tipo nullable, podemos asignar null
                        property.SetValue(model, null);
                    }
                    else
                    {
                        throw new InvalidOperationException($"No se puede convertir '{rawValue}' a int para la propiedad '{property.Name}'.");
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
                        throw new InvalidOperationException($"No se puede convertir '{rawValue}' a ulong para la propiedad '{property.Name}'.");
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
                        throw new InvalidOperationException($"No se puede convertir '{rawValue}' a bool para la propiedad '{property.Name}'.");
                    }
                }
                else if (propertyType.IsEnum)
                {
                    // Para enumeraciones (ej. TeamId) puede ser necesario convertir el valor numérico o la cadena
                    // al tipo enum esperado.
                    // Primero probamos si es numérico...
                    if (int.TryParse(rawValue, out int enumValueAsInt))
                    {
                        object enumValue = Enum.ToObject(propertyType, enumValueAsInt);
                        property.SetValue(model, enumValue);
                    }
                    else
                    {
                        // Si no es numérico, probamos parseo por nombre del enum
                        object enumValue = Enum.Parse(propertyType, rawValue, ignoreCase: true);
                        property.SetValue(model, enumValue);
                    }
                }
                // Caso para tipos int? con N/A o similares
                // (Se puede insertar más lógica específica según las necesidades)
                else if (propertyType == typeof(int?))
                {
                    property.SetValue(model, rawValue == "N/A"
                        ? null
                        : rawValue.TryParseOrNull());
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
                        throw new InvalidOperationException($"No se puede convertir '{rawValue}' a float para la propiedad '{property.Name}'.");
                    }
                }
                else
                {
                    // Aquí se podrían agregar más manejos personalizados o lanzar excepción si no se contempla el tipo
                    throw new NotSupportedException(
                        $"El tipo '{propertyType.Name}' no está soportado por el convertidor genérico."
                    );
                }
            }
            catch (Exception ex)
            {
                // Control de excepción genérico por si algo falla
                throw new InvalidOperationException(
                    $"Error al asignar valor a la propiedad '{property.Name}' en el modelo '{typeof(T).Name}': {ex.Message}", ex
                );
            }
        }

        return model;
    }
}
