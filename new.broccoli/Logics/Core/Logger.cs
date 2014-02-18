using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroccoliTrade.Logics.Core
{
    public static class Logger
    {
        public static void Log(Exception exception)
        {
            // Write the string to a file.append mode is enabled so that the log
            // lines get appended to  test.txt than wiping content and writing the log

            var file = new System.IO.StreamWriter("c:\\broccoli_temp\\log.txt", true);
            file.WriteLine(DateTime.Now.ToString(), Environment.NewLine, exception.Message, Environment.NewLine,
                           exception.StackTrace, Environment.NewLine, Environment.NewLine);

            file.Close();
        }
    }
}
