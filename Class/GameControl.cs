using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

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
        internal delegate void DelegateEvent(string message);
        internal event DelegateEvent Status;
        

        public GameControl(Writer console)
        {
            this.console = console;
            autoIt = new AutoIt(console, "LDPlayer-1");
            openCV = new OpenCV(console, autoIt);
            images = new Images(console, autoIt, openCV);

            menuControl = new MenuControl(console, autoIt, openCV, images);
            battleControl = new BattleControl(console, autoIt, openCV, images);
        }

        internal void Start()
        {
            Task.Run(() => GameCycle());
        }

        private void GameCycle()
        {
            while (true)
            {
                if (menuControl.IsBattle())
                {
                    Status?.Invoke("Battle");
                    if (!menuControl.Battle())
                        return;
                }
                else
                {
                    Status?.Invoke("Menu");
                    if (!menuControl.StartBattle())
                        return;
                }
                Thread.Sleep(1000);
            }
        }

    }
}

