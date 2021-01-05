using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace BOCollector
{
    public static class Key
    {
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        public const int RIGHT = 0x27;
        public const int LEFT = 0x25;
        public const int UP = 0x26;
        public const int DOWN = 0x28;
        public const int Q = 0x51;
        public const int W = 0x57;
        public const int E = 0x45;
        public const int A = 0x41;

        static public void Forward(int delay = 1000)
        {
                    
            keybd_event(RIGHT, 0, 0, 0);
            keybd_event(UP, 0, 0, 0);
            Thread.Sleep(delay);
            keybd_event(RIGHT, 0, 2, 0);
            keybd_event(UP, 0, 2, 0);

        }

        static public void Attack()
        {
            keybd_event(Q, 0, 0, 0);
            keybd_event(Q, 0, 2, 0);
            keybd_event(W, 0, 0, 0);
            keybd_event(W, 0, 2, 0);
            keybd_event(E, 0, 0, 0);
            keybd_event(E, 0, 2, 0);
            keybd_event(A, 0, 0, 0);
            keybd_event(A, 0, 2, 0);


        }



    }
}
