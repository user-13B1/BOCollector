using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace BOCollector
{
    public class Writer
    {
       
        private readonly Form form;
        private readonly TextWriter _writer;
        private readonly string dataPathDir;
        private readonly string pathMessageLogTxtFile;
        private readonly object lockObj;

        public Writer(object lockObj, Form form = null,TextBox textBox = null)
        {
            this.form = form;
            this.lockObj = lockObj;

            dataPathDir = Directory.GetCurrentDirectory();
            pathMessageLogTxtFile = dataPathDir + @"\log_message.txt";

            if (form != null && textBox != null)
            {
                _writer = new TextBoxStreamWriter(textBox);
                Console.SetOut(_writer);   // Перенаправляем выходной поток консоли   
            }
        }

        public void WriteLine(object textObj, bool log = true)
        {
            if (form != null)
            {
                if (form.InvokeRequired)
                    form.BeginInvoke(new Action(() => { Console.WriteLine(textObj); }));
                else
                    Console.WriteLine(textObj);
            }

            //Запись сообщения в лог файл
            if (log)
            {
                string textStr = String.Format("{0:F}  ", DateTime.Now) + String.Format($"{textObj}");
                WriteLog(textStr);
            }
        }

        private void WriteLog(string textStr)
        {
            if (String.IsNullOrWhiteSpace(textStr))
                return;
            lock (lockObj)
            {
                using StreamWriter writer = new StreamWriter(pathMessageLogTxtFile, true, Encoding.Default);
                writer.WriteLine(textStr);
                writer.Flush();
            }
        }
    }
}