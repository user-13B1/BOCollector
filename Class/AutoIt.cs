using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoItX3Lib;
using System.Threading;


namespace BOCollector
{
    class AutoIt
    {
        internal readonly AutoItX3 au3;
        private readonly Writer console;
        private readonly string appName;
        internal Rectangle window;

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
            UpdateWindowPos();
        }

        public bool UpdateWindowPos()
        {
            au3.WinActivate(appName);
            if (au3.WinExists(appName) != 1)
            {
                console.WriteLine("Game not loaded. Please load game.");
                return false;
            }
           
            window = new Rectangle(au3.WinGetPosX(appName), au3.WinGetPosY(appName), au3.WinGetPosWidth(appName), au3.WinGetPosHeight(appName));
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
            au3.WinActivate(appName);
            au3.WinSetOnTop(appName, "", 1);

            //if (au3.WinMove(appName, "", 2000, 10, 1322,756) != 1)
            //{
            //    console.WriteLine("Error. Window not founded");
            //    return false;
            //}

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

        public void ClickMouseToWindow(int x,int y, int delayAfter = 0)
        {
            au3.MouseClick("left", x + window.X, y + window.Y);
            if (au3.error != 0)
                console.WriteLine("Error: mouse not control.");

            if (delayAfter > 0)
                Thread.Sleep(TimeSpan.FromMilliseconds(delayAfter));

        }

        public void PrintMousePos()
        {
            UpdateWindowPos();
            for (int i = 0; i < 2; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));
               
                int x = au3.MouseGetPosX();
                int y = au3.MouseGetPosY();

                console.WriteLine($"x = {x - window.X}\r\ny = {y - window.Y}");
                console.WriteLine($"Color - {au3.PixelGetColor(x, y)}");

            }

        }

        internal bool IsPixelColor(int x, int y, int color)
        {
            if(color == au3.PixelGetColor(x + window.X, y + window.Y))
                return true;
            return false;
        }
    }
}

