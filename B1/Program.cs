using log4net;
using System;
using System.Windows.Forms;

namespace B1
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            new MainFlow().start();
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs ex)
        {
            var logger = LogManager.GetLogger("InfoLogger");
            logger.Fatal(ex);
            MessageBox.Show("致命的な例外が発生しました。\n" + ((Exception)ex.ExceptionObject).Message);

            Environment.Exit(-1);
        }
    }
}
