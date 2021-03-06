﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace BOCollector
{
    class DrawToScreen
    {


        [DllImport("User32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("user32.dll")]
        internal static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        //internal static void DrawPoint(OpenCvSharp.Point p, WindowRegion win)
        //{
        //    IntPtr desktop = GetDC(IntPtr.Zero); // указатель для вывода изображения на экран
        //    Pen pen = new Pen(Color.IndianRed, 3);
        //    using (Graphics g_desctop = Graphics.FromHdc(desktop))
        //    {
        //        g_desctop.DrawEllipse(pen, p.X + win.x - 2, p.Y + win.y - 2, 4, 4);
        //    }
        //    ReleaseDC(IntPtr.Zero, desktop);
        //}


        static async internal void DrawRect(int x, int y, int Width, int Height)
        {
            await Task.Run(() =>
            {
                IntPtr desktop = GetDC(IntPtr.Zero); // указатель для вывода изображения на экран
                using (Graphics g_desctop = Graphics.FromHdc(desktop))
                {
                    for (int i = 0; i < 40; i++)
                    {
                        g_desctop.DrawRectangle(new Pen(Color.Crimson, 3), x-1, y-1, Width+1, Height+1);
                        Thread.Sleep(5);
                    }
                }
                ReleaseDC(IntPtr.Zero, desktop);
            });
        }


        static async internal void DrawPoint(int x, int y)
        {
            await Task.Run(() =>
            {
                IntPtr desktop = GetDC(IntPtr.Zero); // указатель для вывода изображения на экран
                using (Graphics g_desctop = Graphics.FromHdc(desktop))
                {
                    for (int i = 0; i < 40; i++)
                    {
                        g_desctop.DrawRectangle(new Pen(Color.Crimson, 3), x - 1, y - 1, 2,2);
                        Thread.Sleep(5);
                    }
                }
                ReleaseDC(IntPtr.Zero, desktop);
            });
        }



        //static public void DrawRect(Rectangle rec, int x = 0, int y = 0)
        //{
        //    IntPtr desktop = GetDC(IntPtr.Zero); // указатель для вывода изображения на экран
        //    using (Graphics g_desctop = Graphics.FromHdc(desktop))
        //        g_desctop.DrawRectangle(new Pen(Color.Green, 1), x + rec.X, y + rec.Y, rec.Width, rec.Height);
        //    ReleaseDC(IntPtr.Zero, desktop);
        //}

        //static public void DrawRect(Rectangle rec, Rectangle win, int z = 1)
        //{
        //    IntPtr desktop = GetDC(IntPtr.Zero); // указатель для вывода изображения на экран
        //    using (Graphics g_desctop = Graphics.FromHdc(desktop))
        //        g_desctop.DrawRectangle(new Pen(Color.GreenYellow, z), rec.X + win.X, rec.Y + win.Y, rec.Width, rec.Height);
        //    ReleaseDC(IntPtr.Zero, desktop);
        //}

        static public void String(string s, int x, int y)
        {
            Font font = new Font(FontFamily.GenericSansSerif, 20);
            IntPtr desktop = GetDC(IntPtr.Zero); // указатель для вывода изображения на экран
            using (Graphics g_desctop = Graphics.FromHdc(desktop))
            {
                for (int i = 0; i < 40; i++)
                {
                    g_desctop.DrawString(s, font, new SolidBrush(Color.White), x, y);
                    Thread.Sleep(5);
                }
            }
            ReleaseDC(IntPtr.Zero, desktop);

        }

    }
}
