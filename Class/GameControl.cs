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
       
        internal readonly MenuControl menuControl;
        internal readonly BattleControl battleControl;
        internal readonly AutoIt autoIt;
        internal readonly OpenCV openCV;
        internal readonly Images images;
        internal delegate void DelegateMessage(string message);
        internal event DelegateMessage StatusGame;
        internal readonly OverlayDX overlay;

        public GameControl(Writer console)
        {
            autoIt = new AutoIt(console, "LDPlayer-1");
          
            images = new Images(console, autoIt, openCV);
            overlay = new OverlayDX(autoIt);
            openCV = new OpenCV(console, autoIt, overlay);

            menuControl = new MenuControl(console, autoIt, openCV, images);
            battleControl = new BattleControl(console, autoIt, openCV, images, overlay);
        }

        internal void Start()
        {
            autoIt.UpdateWindowPos();
            overlay.Load();
            Task.Run(() => GameCycle());
        }

        private void GameCycle()
        {
            while (true)
            {

                if (menuControl.IsBattle())
                {
                    StatusGame?.Invoke("Battle");
                    if (!battleControl.Start())
                        return;
                }
                else
                {
                    StatusGame?.Invoke("Menu");
                    if (!menuControl.Start())
                        return;
                    Thread.Sleep(1000);
                }

                overlay.UpdateFrame();
                overlay.ClearElements();
            }
        }

    }
}

