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
    public class Key
    {
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        public const byte RIGHT = 0x27;
        public const byte LEFT = 0x25;
        public const byte UP = 0x26;
        public const byte DOWN = 0x28;
        public const byte Q = 0x51;
        public const byte W = 0x57;
        public const byte E = 0x45;
        public const byte A = 0x41;
        public const byte C = 0x43;
        byte oldKey1, oldKey2;
        private string state = "Stop";
        private int timer;



        public void Forward(int delay = 1000)
        {
            Move("Forward", RIGHT, UP, delay);
        }

        public void Back(int delay = 1000)
        {
            Move("Back", LEFT, DOWN, delay);
        }

        private void Move(string newState, byte key1, byte key2, int delay)
        {
            if (state == "Stop")
            {
                state = newState;
                oldKey1 = key1;
                oldKey2 = key2;
               
                timer = delay / 200;
                keybd_event(key1, 0, 0, 0);
                keybd_event(key2, 0, 0, 0);

                while (timer > 0) 
                {
                    timer--;
                    if (timer % 5 == 0)
                    {
                        keybd_event(key1, 0, 2, 0);
                        Thread.Sleep(360);
                        keybd_event(key1, 0, 0, 0);
                    }
                    Thread.Sleep(200);
                }

                if (state == "Stop")
                    return;

                keybd_event(key1, 0, 2, 0);
                keybd_event(key2, 0, 2, 0);
                state = "Stop";
                return;
            }
          
            if (state == newState)
            {
                timer = delay / 200;
            }
            else
            {
                timer = 0;
                state = "Stop";
                keybd_event(oldKey1, 0, 2, 0);
                keybd_event(oldKey2, 0, 2, 0);
                Move(newState, key1, key2, delay);
            }


        }

        public void Around()
        {
            keybd_event(LEFT, 0, 0, 0);
            Thread.Sleep(400);
            keybd_event(DOWN, 0, 0, 0);
            Thread.Sleep(50);
            keybd_event(LEFT, 0, 2, 0);
           
            Thread.Sleep(400);
            keybd_event(RIGHT, 0, 0, 0);
            Thread.Sleep(50);
            keybd_event(DOWN, 0, 2, 0);

            
            Thread.Sleep(400);
            keybd_event(UP, 0, 0, 0);
            Thread.Sleep(50);
            keybd_event(RIGHT, 0, 2, 0);
           
            Thread.Sleep(400);
            keybd_event(UP, 0, 2, 0);

        }

        public void AttackAllSkills()
        {
            keybd_event(A, 0, 0, 0);
            keybd_event(A, 0, 2, 0);
            Thread.Sleep(30);
            keybd_event(Q, 0, 0, 0);
            keybd_event(Q, 0, 2, 0);
            Thread.Sleep(30);
            keybd_event(W, 0, 0, 0);
            keybd_event(W, 0, 2, 0);
            Thread.Sleep(30);
            keybd_event(E, 0, 0, 0);
            keybd_event(E, 0, 2, 0);
            Thread.Sleep(30);
            keybd_event(A, 0, 0, 0);
            keybd_event(A, 0, 2, 0);
        }

        public void PreeHealth()
        {
            keybd_event(C, 0, 0, 0);
            keybd_event(C, 0, 2, 0);

        }

        
        public void BaseAttack()
        {
            keybd_event(A, 0, 0, 0);
            keybd_event(A, 0, 2, 0);

        }

    }

    
}
