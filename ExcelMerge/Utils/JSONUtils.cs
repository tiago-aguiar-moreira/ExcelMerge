using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ExcelMerge.Utils
{
    public static class JSONUtils
    {
        public static T LoadFromFile<T>(string path) => JObject.Parse(File.ReadAllText(path)).ToObject<T>();

        public static void SaveToFile<T>(T obj, string path) => File.WriteAllText(path, JsonConvert.SerializeObject(obj));
    }
}
