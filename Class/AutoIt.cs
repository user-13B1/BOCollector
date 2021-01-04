using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoItX3Lib;

namespace BOCollector
{
    class AutoIt
    {
        private readonly AutoItX3 au3;
        private readonly Writer console;
        private readonly string appName;

        public AutoIt(Writer console,string appName)
        {
            this.console = console;
            this.appName = appName;

            try
            {
                au3 = new AutoItX3();
                SetWindow();
                console.WriteLine("AutoIt loaded.");
            }
            catch (Exception e)
            {
                console.WriteLine("AutoItX3 Error!");
                console.WriteLine($"IBotArm error = { e.Message}");
            }
            
          
        }

        internal bool UpdatePos(out Rectangle rect)
        {
            if (au3.WinExists(appName) != 1)
            {
                console.WriteLine("Game not loaded.Please load game.");
                rect = new Rectangle(0,0,0,0);
                return false;
            }

            rect = new Rectangle(au3.WinGetPosX(appName), au3.WinGetPosY(appName), au3.WinGetPosWidth(appName), au3.WinGetPosHeight(appName));
            return true;
        }

        public bool SetWindow()
        {
            if (au3.WinExists(appName) != 1)
            {
                console.WriteLine("Game not loaded.Please load game.");
                return false;
            }

            au3.WinSetState(appName, "", 1);
            au3.WinSetOnTop(appName, "", 1);

            if (au3.WinMove(appName, "", 2000, 10, 1322,756) != 1)
            {
                console.WriteLine("Error. Window not founded");
                return false;
            }

            if (au3.WinGetPosHeight(appName) != 756)
            {
                console.WriteLine("Window height not correct");
                return false;
            }

            if (au3.WinGetPosWidth(appName) != 1322)
            {
                console.WriteLine("Window width not correct");
                return false;
            }

            return true;
        }




    }
}


//console.WriteLine(au3.WinGetPosX(appName));
//console.WriteLine(au3.WinGetPosY(appName));