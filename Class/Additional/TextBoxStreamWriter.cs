using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BOCollector
{
    internal class TextBoxStreamWriter : TextWriter
    {
        
        readonly TextBox _output = null;


        public TextBoxStreamWriter(TextBox output)
        {
            _output = output;
        }

        public override void Write(char value)
        {
            base.Write(value);
            _output.AppendText(value.ToString()); // Когда символ записывается в поток, добавляем его в textbox.
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}