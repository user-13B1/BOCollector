using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BOCollector
{
    class BattleControl
    {
        private readonly Writer console;
        private readonly AutoIt autoIt;
        private readonly OpenCV openCV;
        private readonly Images images;

        string state;
       

        public BattleControl(Writer console, AutoIt autoIt, OpenCV openCV)
        {
            this.console = console;
            this.autoIt = autoIt;
            this.openCV = openCV;

            state = "Forward";
        }

        internal bool Battle()
        {
            AnalizeGame();
            Move();
            return true;
        }

        private void AnalizeGame()
        {
            Bitmap bitmap;
            OpenCvSharp.Point p;
            gameImages.TryGetValue("lifebar", out bitmap);
            if (openCV.SearchImageFromName(bitmap, out p))
            {
                // console.WriteLine(p);
            }
            else
                console.WriteLine("Not found lives");


            gameImages.TryGetValue("EnemyLife", out bitmap);
            if (openCV.SearchImageFromName(bitmap, out p))
            {
                state = "Attack";
            }
            else
            {
                state = "Forward";
            }

            void Move()
            {
                switch (state)
                {
                    case "Forward":
                        console.WriteLine("Forward.");
                        Key.Forward();
                        break;

                    case "Attack":
                        console.WriteLine("Attack");
                        Key.Attack();
                        break;


                    default:
                        console.WriteLine("Invalid State.");
                        break;
                }

            }

        }


    }
}
