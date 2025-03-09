// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SquadNET.Test.Squad.Core
{
    public abstract class SquadTestBase : IDisposable
    {
        public static readonly string TestDataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Squad/TestData");
        protected readonly IServiceProvider ServiceProvider;
        protected readonly IServiceCollection Services;

        protected SquadTestBase()
        {
            Services = new ServiceCollection();
            Services.AddSquad();
            ServiceProvider = Services.BuildServiceProvider();
        }

        public void Dispose()
        {
            if (ServiceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        protected static string GetTestDataFilePath(string fileName)
        {
            return Path.Combine(TestDataDirectory, fileName);
        }

        /// <summary>
        /// Carga los datos de prueba desde un archivo JSON genérico.
        /// </summary>
        protected static IEnumerable<object[]> LoadTestData<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Test data file not found: {filePath}");
            }

            string json = File.ReadAllText(filePath);
            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true
            };
            List<T> testCases = JsonSerializer.Deserialize<List<T>>(json, options);

            if (testCases == null || !testCases.Any())
            {
                throw new InvalidOperationException($"Error al cargar datos de prueba desde {filePath}. Lista vacía o null.");
            }

            foreach (T testCase in testCases)
            {
                yield return ConvertToObjectArray(testCase);
            }
        }

        protected T GetService<T>() where T : notnull
        {
            T service = ServiceProvider.GetService<T>();
            if (service == null)
            {
                throw new InvalidOperationException($"No service for type '{typeof(T)}' has been registered.");
            }
            return service;
        }

        /// <summary>
        /// Convierte una instancia de prueba en un array de objetos para usar en `[Theory]`.
        /// </summary>
        private static object[] ConvertToObjectArray<T>(T testCase)
        {
            var properties = typeof(T)
                .GetProperties()
                .OrderBy(p => p.GetCustomAttributes(typeof(JsonPropertyOrderAttribute), false)
                               .Cast<JsonPropertyOrderAttribute>()
                               .FirstOrDefault()?.Order ?? int.MaxValue)
                .ThenBy(p => p.MetadataToken)
                .ToArray();

            object[] values = new object[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                values[i] = properties[i].GetValue(testCase);
            }

            return values;
        }
    }
}