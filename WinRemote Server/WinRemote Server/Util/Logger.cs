using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinRemote_Server
{
    class Logger
    {
        private static bool errorOccured = false;
        private static LoggerInstance instance = null;
        static Logger()
        {
            string logFile = @"logs\";
            logFile += DateTime.Now.Year.ToString("0000");   //yyyy
            logFile += DateTime.Now.Day.ToString("00");    //yyyydd
            logFile += (DateTime.Now.Month + 1).ToString("00");    //yyyyddmm
            logFile += "-" + DateTime.Now.Hour.ToString("00"); //yyyyddmm-hh
            logFile += DateTime.Now.Minute.ToString("00"); //yyyyddmm-hhmm
            logFile += DateTime.Now.Second.ToString("00"); //yyyyddmm-hhmmss
            logFile += DateTime.Now.Millisecond.ToString("000"); //yyyyddmm-hhmmssmmm
            logFile += ".log";  //yyyyddmm-hhmmssmmm.log

            try
            {
                if (!Directory.Exists(@"logs"))
                {
                    Directory.CreateDirectory(@"logs");
                }
                File.Create(logFile).Close();
            }
            catch (IOException ex)
            {
                MessageBox.Show("The Log file couldn't be created.\nCheck the permissions and start the program as admin.\n" + ex.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            instance = new LoggerInstance(logFile, ref FormMain.logBox);
        }

        public static void Log(string prefix, string message)
        {
            if (errorOccured) return;
            if (instance != null)
            {
                instance.Log(prefix, message);
            }
            else   //LoggerInstance is null. Should never happen.
            {
                errorOccured = true;
                MessageBox.Show("The LoggerInstance instance is null!\n Please report this error.\nThe program will nevertheless work like always.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }


        private class LoggerInstance
        {
            private string fileName;
            private RichTextBox logBox = null;

            public LoggerInstance(string fileName, ref RichTextBox logBox)
            {
                this.fileName = fileName;
                this.logBox = logBox;
            }

            /// <summary>
            /// Creates a new instance of a StreamWriter with the given filePath.
            /// </summary>
            /// <param name="fileName">The file to log into</param>
            /// <returns>The new instance</returns>
            private static StreamWriter CreateWriter(string fileName)
            {
                StreamWriter writer = new StreamWriter(fileName, true);
                writer.NewLine = "\n";
                writer.AutoFlush = true;
                return writer;
            }

            public void Log(string prefix, string message)
            {
                StreamWriter writer = CreateWriter(fileName);
                string logLine = CreateTimeString() + ": " + prefix + "\t" + message + "\n";
                //write to log window
                logBox.Invoke((MethodInvoker)(() => logBox.AppendText(logLine)));
                //logBox.AppendText(logLine);
                //write to file
                writer.Write(logLine);
                writer.Close();
            }
        }

        private static string CreateTimeString()
        {
            return string.Format("{0:00}:{1:00}:{2:00}:{3:000}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
        }
    }

    
}
