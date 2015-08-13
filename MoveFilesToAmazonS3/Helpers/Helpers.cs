using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Markup;

namespace MoveFilesToAmazonS3.Helpers
{
    public class Helpers
    {
        public static T Load<T>(string xamlPath)
        {
            using (var stream = File.OpenRead(xamlPath))
            {
                var returnValue = (T)XamlReader.Load(stream);
                return returnValue;
            }
        }

        public static IEnumerable<string> GetSubdirectoriesContainingOnlyFiles(string path)
        {
            return from subdirectory in Directory.GetDirectories(path, "*", SearchOption.AllDirectories)
                   where Directory.GetFiles(subdirectory).Length > 0
                   select subdirectory;
        }

        private static readonly object _syncObject = new object();

        public static void LogExceptions(string errorMessgae)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("*** Exception Begins ***");
            stringBuilder.AppendLine("Message = " + errorMessgae);
            stringBuilder.AppendLine("*** Exception Ends ***");
            lock (_syncObject)
            {
                using (StreamWriter sw = File.AppendText("ErrorLog.txt"))
                {
                    sw.WriteLine(stringBuilder.ToString());
                }
            }
        }
    }
}
