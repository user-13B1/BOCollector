using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoItX3Lib;
using System.Threading;
using System.Globalization;
using Point = OpenCvSharp.Point;


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

        internal void PrintMousePosColor()
        {
            while(true)
            {
                Thread.Sleep(100);

                int x = au3.MouseGetPosX();
                int y = au3.MouseGetPosY();
                if (IsHealthPixel(new Point(x, y)))
                    console.WriteLine("Is health bar");
            }
        }


        internal bool IsHealthPixel(Point p)
        {
            Color color = IntToColor(au3.PixelGetColor(p.X, p.Y));
            if(color.R>160 && color.G<80 && color.B<80)
                return true;
            return false;
        }

        internal Color IntToColor(int intColor)
        {
            Color color = Color.Empty;
            string hexColor = $"{intColor:X6}";
            if (hexColor.Length != 6)
                return Color.Empty;
            try
            {
                string r = hexColor.Substring(0, 2);
                string g = hexColor.Substring(2, 2);
                string b = hexColor.Substring(4, 2);

                int ri = Int32.Parse(r, NumberStyles.HexNumber);
                int gi = Int32.Parse(g, NumberStyles.HexNumber);
                int bi = Int32.Parse(b, NumberStyles.HexNumber);
                color = Color.FromArgb(ri, gi, bi);
            }
            catch
            {
                return Color.Empty;
            }
                return color;
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
                Thread.Sleep(TimeSpan.FromSeconds(3));
               
                int x = au3.MouseGetPosX();
                int y = au3.MouseGetPosY();

                console.WriteLine($"{x - window.X},{y - window.Y}");
                console.WriteLine($"Color - {au3.PixelGetColor(x, y)}");

            }

        }

        internal bool IsPixelColor(int x, int y, int color)
        {
            if(color == au3.PixelGetColor(x + window.X, y + window.Y))
                return true;
            return false;
        }

        internal int GetPixelColor(int x, int y)
        {
            return au3.PixelGetColor(x + window.X, y + window.Y);
        }

        internal int GetPixelColor(Point p)
        {
            return au3.PixelGetColor(p.X + window.X, p.Y + window.Y);
        }




        //internal bool FindPixelColor(int v)
        //{
        //    object search = au3.PixelSearch(window.X + 300, window.Y + 150, +window.X + window.Width - 300, window.Y + window.Height - 150, v);
            
        //    if (search.ToString() != "0")
        //        return true;
        //    return false;
        //}

        internal bool FindPixelColor(int v,int x1,int y1,int x2,int y2)
        {
            object search = au3.PixelSearch(window.X + x1, window.Y + y1, window.X + x2, window.Y + y2, v);

            if (search.ToString() != "0")
                return true;
            return false;
        }

        internal bool FindPixelColorOnScreen(int v)
        {
            object search = au3.PixelSearch(window.X, window.Y, window.X + window.Width, window.Y + window.Height, v);

            if (search.ToString() != "0")
                return true;
            return false;
        }

        internal bool FindPixelColorPos(int v, int x1, int y1, int x2, int y2, out Point p)
        {
            object search = au3.PixelSearch(window.X + x1, window.Y + y1, window.X + x2, window.Y + y2, v);

            if (search.ToString() != "0")
            {
                object[] pixelCoord = (object[])search;
               // au3.MouseMove((int)pixelCoord[0], (int)pixelCoord[1], 500);
                p = new Point((int)pixelCoord[0]- window.X, (int)pixelCoord[1]-window.Y);
                return true;
            }
            p = new Point(0, 0);
            return false;
        }





    }
}

