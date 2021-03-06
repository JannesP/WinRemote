﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinRemote_Server
{
    class Logger
    {
        private static bool errorOccured = false;
        private static LoggerInstance instance = null;
        private static string logFile;
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
            Logger.logFile = Path.Combine(Environment.CurrentDirectory, logFile);
            try
            {
                if (!Directory.Exists(@"logs"))
                {
                    Directory.CreateDirectory(@"logs");
                }
                File.Create(Logger.logFile).Close();
                if (!CheckWriteAccess(Logger.logFile)) throw new Exception();
            }
            catch (IOException ex)
            {
                MessageBox.Show("The Log file couldn't be created.\nCheck the permissions or start the program as admin.\nWe need write access to the executable directory!\n" + ex.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Program.onShutdown = true;
                Application.Exit();
            }
            catch (Exception exception)
            {
                MessageBox.Show("The Log file couldn't be created.\nCheck the permissions or start the program as admin.\nWe need write access to the executable directory!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Program.onShutdown = true;
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
            private StreamWriter writer;

            public LoggerInstance(string fileName, ref RichTextBox logBox)
            {
                this.fileName = fileName;
                this.logBox = logBox;
                this.writer = CreateWriter(fileName);
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
                string logLine = CreateTimeString() + ": " + prefix + "\t" + message + "\n";
                //write to log window
                try
                {
                    logBox.Invoke((MethodInvoker)(() => 
                        {
                            logBox.AppendText(logLine);
                            logBox.SelectionStart = logBox.Text.Length;
                            logBox.ScrollToCaret();
                        }
                    ));
                    logBox.Invoke((MethodInvoker)(() => logBox.ScrollToCaret()));
                } catch { }
                //write to file
                Thread t = new Thread(() => writeToFile(logLine));
                t.Start();
            }

            private void writeToFile(string line)
            {
                if (writer == null)
                {
                    writer = CreateWriter(fileName);
                }
                writer.Write(line);
            }
        }

        

        private static string CreateTimeString()
        {
            return string.Format("{0:00}:{1:00}:{2:00}:{3:000}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
        }
        
        private static bool CheckWriteAccess(string path)
        {
            PermissionSet permissionSet = new PermissionSet(PermissionState.None);
            FileIOPermission writePermission = new FileIOPermission(FileIOPermissionAccess.Write | FileIOPermissionAccess.Append, path);
            permissionSet.AddPermission(writePermission);
            return permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet);
        }
    }

    
}
