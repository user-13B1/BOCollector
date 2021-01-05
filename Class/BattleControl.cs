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
        internal readonly AutoIt autoIt;
        private readonly OpenCV openCV;
        private readonly Images images;
        delegate bool Operation();

        public BattleControl(Writer console, AutoIt autoIt, OpenCV openCV, Images images)
        {
            this.console = console;
            this.autoIt = autoIt;
            this.openCV = openCV;
            this.images = images;
        }

        internal bool Start()
        {
            string state = StateSearch();
            Operation action = GetAction(state);
            if (!action())
                return false;

            return true;
        }

        Operation GetAction(string state) => state switch
        {
            "Forward" => ActionForward,
            "Attack" => ActionAttack,
            
            { } => ActionError,
            null => ActionError
        };

        private string StateSearch()
        {
            Bitmap bitmap;
            OpenCvSharp.Point p;

            //Определяем количество жизней игрока
            images.gameImages.TryGetValue("lifebar", out bitmap);
            if (openCV.SearchImageFromRegion(bitmap, out p,new Rectangle(100, 200, 200,300)))
            {
                console.WriteLine(p);
            }
            else
                console.WriteLine("Not found lives");


            //Поиск цели
            images.gameImages.TryGetValue("EnemyLife", out bitmap);
            if (openCV.SearchImageFromName(bitmap, out p))
            {
                return "Attack";
            }
            else
            {
                return "Forward";
            }

        }

        bool ActionForward()
        {
            console.WriteLine("Forward.");
            Key.Forward();
            return true;
        }

        bool ActionAttack()
        {
            console.WriteLine("Attack");
            Key.Attack();
            return true;
        }

        bool ActionError()
        {
            console.WriteLine("Invalid State.");
            return true;
        }


    }
}
