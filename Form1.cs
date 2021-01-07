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
        GameControl game;
        

        public Form1()
        {
            InitializeComponent();
            console = new Writer(new object(), this, ConsoleBox);
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            game = new GameControl(console);
            game.StatusGame += SetStatusGame;
            game.battleControl.StatusHeroHealth += SetHealthBar;
            game.battleControl.StatusEnemyNearby += SetEnemyNearbyLabel;

            //SetStatusGame("Loaded");
            //SetHealthBar(100);
        }

        private void Button1_Click(object sender, EventArgs e) => game.Start();

        private void button2_Click(object sender, EventArgs e) => game.menuControl.autoIt.PrintMousePos();

        private void SetStatusGame(string message) => StatusLabel1.Text = message;

        private void SetHealthBar(int value) => HealthBar.Invoke((Action)(() => HealthBar.Value = value));

        private void SetEnemyNearbyLabel(string message) => HealthBar.Invoke((Action)(() => labelEnemyNearby.Text = message));

        private void button3_Click(object sender, EventArgs e) =>Task.Run(()=> game.menuControl.autoIt.PrintMousePosColor());

    }
}
