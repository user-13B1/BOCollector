using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace BOCollector
{
    public partial class Form1 : Form
    {
        private readonly Writer console;
        Player player;

        public Form1()
        {
            InitializeComponent();
            console = new Writer(new object(), this, ConsoleBox);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            player = new Player(console);

        }

        private void Button1_Click(object sender, EventArgs e) => player.Start();

      
    }
}
