using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace LabFormProject.Pages
{
    public class Utils
    {
        private static readonly Lazy<Utils> instance = new Lazy<Utils>(() => new Utils());

        public static Utils Instance => instance.Value;

        private Utils() { }

        public string ExportToJson<T>(List<T> data, List<string>? selectedProperties = null)
        {
            if (selectedProperties == null || selectedProperties.Count == 0)
            {
                return JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            }

            var result = data.Select(item =>
            {
                var dict = new Dictionary<string, object?>();

                foreach (var prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (selectedProperties.Contains(prop.Name))
                    {
                        dict[prop.Name] = prop.GetValue(item);
                    }
                }

                return dict;
            }).ToList();

            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
