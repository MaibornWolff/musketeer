using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace Musketeer.Utils
{
    public class TestData
    {
        public static string GetTestDataPath() =>
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data");

        public static string GetTestFilePath(string testdataFile) =>
            Path.Combine(GetTestDataPath(), testdataFile);

        public static T ReadJsonFile<T>(string testdataFile)
        {
            using (StreamReader r = new StreamReader(GetTestFilePath(testdataFile)))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(json);
            }

        }
    }
}