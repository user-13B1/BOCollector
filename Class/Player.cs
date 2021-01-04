using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOCollector
{
    class Player
    {
        private readonly Writer console;
        private readonly LDmanager ldManager;

        public Player(Writer console)
        {
            this.console = console;
           
            ldManager = new LDmanager(console);
        }

        internal void Start()
        {
            Task.Run(() => GameCycle());
        }

        private void GameCycle()
        {

            if (IsBattle())
            {
                console.WriteLine("In battle.");
                // Battle();
            }
            else
            {
                console.WriteLine("Game menu.");
                if (!ldManager.GoBattle())
                    return;
            }
            
        }

        private bool IsBattle()
        {

            return false;
        }

        



    }
}

