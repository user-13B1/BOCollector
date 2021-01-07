using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace BOCollector
{
    class GameControl
    {
        private readonly Writer console;
        internal readonly MenuControl menuControl;
        internal readonly BattleControl battleControl;
        internal readonly AutoIt autoIt;
        internal readonly OpenCV openCV;
        internal readonly Images images;
        internal delegate void DelegateMessage(string message);
        internal event DelegateMessage StatusGame;


        public GameControl(Writer console)
        {
            this.console = console;
            autoIt = new AutoIt(console, "LDPlayer-1");
            openCV = new OpenCV(console, autoIt);
            images = new Images(console, autoIt, openCV);

            menuControl = new MenuControl(console, autoIt, openCV, images);
            battleControl = new BattleControl(console, autoIt, openCV, images);
           
           
        }

        internal void Start() => Task.Run(() => GameCycle());

        private void GameCycle()
        {
           
            while (true)
            {
               // Stopwatch stopwatch = new Stopwatch();
               // stopwatch.Start();

                if (menuControl.IsBattle())
                {
                    StatusGame?.Invoke("Battle");
                    if (!battleControl.Start())
                        return;
                }
                else
                {
                    Thread.Sleep(1000);
                    StatusGame?.Invoke("Menu");
                    if (!menuControl.Start())
                        return;
                }

                //stopwatch.Stop();
               // console.WriteLine("ElapsedMilliseconds: " + stopwatch.ElapsedMilliseconds);
                //break;
            }
        }

    }
}

