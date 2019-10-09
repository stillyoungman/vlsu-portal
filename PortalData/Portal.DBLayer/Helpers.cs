using System.IO;
using System.Text;

namespace Portal.DBLayer
{
    internal static class Helpers
    {
        public static string GetQuery(string name, string queriesPath)
        {
            return string.Join(" ", File.ReadAllLines(Path.Combine(new[] { queriesPath, name }), Encoding.UTF8));
        }
    }
}
