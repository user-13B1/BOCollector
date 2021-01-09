using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;


using Point = OpenCvSharp.Point;

namespace BOCollector
{
    class BattleControl
    {
        private readonly Writer console;
        internal readonly AutoIt autoIt;
        private readonly OpenCV openCV;
        private readonly Images images;
        delegate bool Operation();
        internal delegate void DelegateValue(int value);
        internal event DelegateValue StatusHeroHealth;
        internal Key key;
        internal delegate void DelegateMessage(string message);
        internal event DelegateMessage StatusEnemyNearby;
        OverlayDX overlay;
        int health;
        bool enemyAttakedHero;
        bool enemy;
        bool enemyLowHealth;
        bool TeamHero;
        bool canMoveForward = true;
        bool TowerAhead;

        public BattleControl(Writer console, AutoIt autoIt, OpenCV openCV, Images images, OverlayDX overlay)
        {
            this.console = console;
            this.autoIt = autoIt;
            this.openCV = openCV;
            this.images = images;
            this.overlay = overlay;
            key = new Key();
           
        }

        

        internal bool Start()
        {
            UpdateData();
            //SetAction();
            //string state = UpdateState();
            //Operation action = GetAction(state);
            //if (!action())
            //    return false;
           
            return true;
        }


        private void UpdateData()
        {

            UpdateHeroHealth();
            UpdateEnemyAttackHero();
            UpdateEnemyOnScreen();
            UpdateTeamHero();
            UpdateTowerAhead();

        }

        private void UpdateTowerAhead()
        {

            if (autoIt.FindPixelColor(16764618, 1234, 550, 1280, 595))
                TowerAhead = true;
            else TowerAhead = false;
        }

        private void SetAction()
        {
            if (IsDeath())
            {
                DrawToScreen.String("Death", autoIt.window.X + 300, autoIt.window.Y + 700);
                ActionDelay();
                return;
            }

            if (IsTower())
            {
                DrawToScreen.String("Tower!Go back", autoIt.window.X+300,autoIt.window.Y + 700);

                ActionBack(3000);
                ActionHealth();
                ActionBaseAttack();
                Task.Run(() => BlockForwardMove(10000));
                return;
            }

            if (enemyLowHealth)
            {
                DrawToScreen.String("Attack weak enemy!", autoIt.window.X + 300, autoIt.window.Y + 700);
               
                ActionAttack();
                Thread.Sleep(1000);
                return;
            }



            if (TeamHero && enemy && health > 10)
            {
                DrawToScreen.String("Attack enemy!", autoIt.window.X + 300, autoIt.window.Y + 700);

                ActionAttack();
                return;
            }
          

            if (!enemyAttakedHero && enemy && !TeamHero && health < 40)
            {
                DrawToScreen.String("Back!", autoIt.window.X + 300, autoIt.window.Y + 700);

                ActionBack(1000);
                ActionAttack();
                return;
            }

            if (health <= 50 && enemyAttakedHero)
            {
                DrawToScreen.String("Low Health, Back!", autoIt.window.X + 300, autoIt.window.Y + 700);
                ActionBack(3000);
                return;
            }

            if (health <= 30 && enemyAttakedHero)
            {
                DrawToScreen.String("Low Health, Back!", autoIt.window.X + 300, autoIt.window.Y + 700);
                ActionBack(3000);
                return;
            }

            if (health > 50 && enemyAttakedHero)
            {
                DrawToScreen.String("Attack!", autoIt.window.X + 300, autoIt.window.Y + 700);
                ActionAttack();
                Thread.Sleep(500);
                ActionAttack();
                Thread.Sleep(500);
                return;
            }

            if (enemy)
            {
                DrawToScreen.String("Base Attack!", autoIt.window.X + 300, autoIt.window.Y + 700);
                ActionBaseAttack();
                Thread.Sleep(1500);

                Task.Run(()=>BlockForwardMove(5000));
                return;
            }

            if (canMoveForward)
            {
                DrawToScreen.String("Forward!", autoIt.window.X + 300, autoIt.window.Y + 700);
                ActionForward(3000);
                return;
            }

           
            Task.Run(() => ActionMoveAround());
            return;
        }

        private void ActionMoveAround()
        {
            console.WriteLine("Around.");
            key.Around();
            
        }

        private void BlockForwardMove(int v)
        {
            canMoveForward = false;
            Thread.Sleep(v);
            canMoveForward = true;
        }

        private void ActionHealth()
        {
            key.PreeHealth();
        }

        private bool IsDeath()
        {
            Bitmap bitmap;

            images.gameImages.TryGetValue("Death", out bitmap);
            if (openCV.SearchImageFromRegion(bitmap, out _, new Point(560, 100), new Point(720, 140)))
            {
                console.WriteLine("Hero is dead.");
                return true;
            }
            return false;
        }

        private void UpdateHeroHealth()   //Определяем количество жизней игрока
        {
            images.gameImages.TryGetValue("Lifebar", out Bitmap bitmap);
            if (openCV.SearchImageFromRegion(bitmap, out Point p, new Point(550, 260), new Point(630, 330)))
            {

                for (int i = 0; i <= 100; i += 4)
                {
                    int color1 = autoIt.GetPixelColor(p.X + 7 + i, p.Y + 7);
                    int color2 = autoIt.GetPixelColor(p.X + 11 + i, p.Y + 7);
                    //console.WriteLine($"color1 {color1} color2 {color2}");
                    if ((color1 < 6000000 || color1 > 7000000) && (color2 < 6000000 || color2 > 7000000))
                    {

                        break;
                    }
                    //DrawToScreen.DrawPoint(p.X + 7 + i + autoIt.window.X, p.Y + 7 + autoIt.window.Y);
                    health = i;
                }
           

                StatusHeroHealth?.Invoke(health);
            }
            else
                StatusHeroHealth?.Invoke(0);

        }

        private void UpdateEnemyOnScreen()
        {
            images.gameImages.TryGetValue("EnemyLife", out Bitmap bitmap);
            if(openCV.SearchImagesFromRegion(bitmap,out List<Point> points, new Point(100, 40), new Point(1200, 600)))
                foreach (var p in points)
                {
                    if (autoIt.IsHealthPixel(new Point(p.X + autoIt.window.X+ 12, p.Y + autoIt.window.Y+ 10)))
                    {
                        //DrawToScreen.DrawRect(p.X + autoIt.window.X, p.Y + autoIt.window.Y, 100, 200);
                        //console.WriteLine($"Enemy spotted!");
                        enemy = true;
                        return;
                    }
                }

        }

        private void UpdateEnemyAttackHero()
        {
            if (autoIt.GetPixelColor(600, 100) == 13112352)
            {
                enemyAttakedHero = true;

                if (autoIt.GetPixelColor(620, 100) != 13112352)
                {
                    enemyLowHealth = true;
                    console.WriteLine("Low health enemy.");
                }
                else
                    enemyLowHealth = false;
            }
            else
                enemyAttakedHero = false;

            StatusEnemyNearby?.Invoke(enemyAttakedHero.ToString());
        }

        private void UpdateTeamHero()
        {
            if (autoIt.FindPixelColorPos(29382, 600, 30, 1200, 500, out Point p))
            {
                if (autoIt.GetPixelColor(p.X + 5, p.Y) == 29382)
                {
                    console.WriteLine("Team Hero.");
                    //DrawToScreen.DrawRect(p.X + autoIt.window.X, p.Y + autoIt.window.Y, 80, 160);
                    TeamHero = true;
                    return;
                }

            }
            TeamHero = false;
        }

        private bool IsTower()
        {
            if (autoIt.FindPixelColor(13041664,300,150,1140,600))
            {
                console.WriteLine("Danger - Tower.");
                return true;
            }
            return false;

        }

        void ActionForward(int dist)
        {
            if(TowerAhead)
                return;
            console.WriteLine("Action Forward.");
            Task.Run(() => key.Forward(dist));
            
        }

        void ActionBack(int dist)
        {
            console.WriteLine("Action Back.");
            key.Back(dist);
        }

        bool ActionAttack()
        {
            console.WriteLine(" ActionAttack!");
            key.AttackAllSkills();
            key.BaseAttack();
           
            return true;
        }

        bool ActionBaseAttack()
        {
            console.WriteLine("ActionBaseAttack!");
            key.BaseAttack();
            return true;
        }

        bool ActionError()
        {
            console.WriteLine("Error State.");
            return true;
        }

        bool ActionDelay()
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(3000));
            return true;
        }

    }
}
