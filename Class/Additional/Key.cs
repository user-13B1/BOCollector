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
        byte currentPressKey1, curentPressKey2;
        public string newState = "Stop";
        public string oldState = "Stop";
        Writer console;

        public Key(Writer console)
        {
            this.console = console;
            Task.Run(()=>MoveHero());

        }

        public void MoveHero()
        {
            while (true)
            {
                Thread.Sleep(500);
                if (oldState == newState)
                   continue;
               
                switch (newState)
                {
                    case "Stop":
                        Stop();
                        break;

                    case "Forward":
                        console.WriteLine("Forward go");
                        Forward();
                        break;

                    case "Back":
                        Back();
                        break;
                   
                    case "Right":
                        Right();
                        break;

                    case "Left":
                        Left();
                        break;

                    default:
                        console.WriteLine("Error move");
                        break;
                }
                oldState = newState;
              
            }

        }

        public void Stop()
        {
          
            keybd_event(currentPressKey1, 0, 2, 0);
            keybd_event(curentPressKey2, 0, 2, 0);
            currentPressKey1 = 0;
            curentPressKey2 = 0;
           
        }


        public void Forward()
        {
            if(currentPressKey1!=0)
                keybd_event(currentPressKey1, 0, 2, 0);
            if(curentPressKey2!= 0)
                keybd_event(curentPressKey2, 0, 2, 0);

            keybd_event(UP, 0, 0, 0);
            keybd_event(RIGHT, 0, 0, 0);
           
            currentPressKey1 = RIGHT;
            curentPressKey2 = UP;
        }

        public void Right()
        {
            if (currentPressKey1 != 0)
                keybd_event(currentPressKey1, 0, 2, 0);
            if (curentPressKey2 != 0)
                keybd_event(curentPressKey2, 0, 2, 0);

            keybd_event(DOWN, 0, 0, 0);
            keybd_event(RIGHT, 0, 0, 0);

            currentPressKey1 = RIGHT;
            curentPressKey2 = DOWN;
        }

        public void Left()
        {
            if (currentPressKey1 != 0)
                keybd_event(currentPressKey1, 0, 2, 0);
            if (curentPressKey2 != 0)
                keybd_event(curentPressKey2, 0, 2, 0);

            keybd_event(UP, 0, 0, 0);
            keybd_event(LEFT, 0, 0, 0);

            currentPressKey1 = LEFT;
            curentPressKey2 = UP;
        }


        public void Back()
        {
            if (currentPressKey1 != 0)
                keybd_event(currentPressKey1, 0, 2, 0);
            if (curentPressKey2 != 0)
                keybd_event(curentPressKey2, 0, 2, 0);
            
            keybd_event(DOWN, 0, 0, 0);
            keybd_event(LEFT, 0, 0, 0);

            currentPressKey1 = LEFT;
            curentPressKey2 = DOWN;
            
        }


        public void AttackAllSkills()
        {
            keybd_event(Q, 0, 0, 0);
            keybd_event(Q, 0, 2, 0);
            Thread.Sleep(30);
            keybd_event(W, 0, 0, 0);
            keybd_event(W, 0, 2, 0);
            Thread.Sleep(30);
            keybd_event(E, 0, 0, 0);
            keybd_event(E, 0, 2, 0);
           
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
