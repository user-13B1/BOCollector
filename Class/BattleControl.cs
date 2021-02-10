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
        readonly OverlayDX overlay;
        int health;
        bool IsBattle;
        bool enemy;
        bool enemyLowHealth;
        bool TeamHero;
        bool canMoveForward = true;
        bool TowerZone,TowerAhead;
        bool HeroAboveLine,HeroBelowLine;

        public BattleControl(Writer console, AutoIt autoIt, OpenCV openCV, Images images, OverlayDX overlay)
        {
            this.console = console;
            this.autoIt = autoIt;
            this.openCV = openCV;
            this.images = images;
            this.overlay = overlay;
            key = new Key(console);
            HeroAboveLine = HeroBelowLine = false;

        }

        

        internal bool Start()
        {
            UpdateState();
            SetAction();
            return true;
        }
       

        private void UpdateState()
        {
            UpdateHeroHealth();
            UpdateIsBattle();
            UpdateEnemyOnScreen();
            UpdateTeamHero();
            UpdateTowerZone();
            UpdateTowerAhead();
            CheckPos();
        }

        private void CheckPos()
        {
            if(openCV.IsHeroCameOutLine(out bool isAboveLine))
            {
                if (isAboveLine)
                {
                    console.WriteLine("Hero is above line");
                    HeroAboveLine = true;
                }
                else
                {
                    console.WriteLine("Hero is below line");
                    HeroBelowLine = true;
                }
            }
        }

        private void UpdateTowerZone()
        {
            images.gameImages.TryGetValue("Tower", out Bitmap bitmap);
            if (openCV.SearchImageFromRegion(bitmap, out Point p, new Point(300, 40), new Point(1220, 600)))
            {
                Color color = autoIt.IntToColor(autoIt.au3.PixelGetColor(p.X+13+ autoIt.window.X, p.Y+10+ autoIt.window.Y));
                if (color.R > 150 && color.G > 150 && color.B < 150)
                {
                    overlay.DrawText("TowerZone!", 40, 680);
                    overlay.DrawRect(p.X - 10, p.Y - 10, 46, 46);
                    TowerZone = true;
                }
            }
         
            else TowerZone = false;

        }

        private void UpdateTowerAhead()
        {
            Color color = autoIt.IntToColor(autoIt.au3.PixelGetColor(1246 + autoIt.window.X, 565 + autoIt.window.Y));
            if (color.R > 170 && color.G > 170 && color.B > 170)
            {
                overlay.DrawText("TowerAhead!", 40, 680);
                TowerAhead = true;
            }

            else TowerAhead = false;

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
                    if ((color1 < 6000000 || color1 > 7000000) && (color2 < 6000000 || color2 > 7000000))
                    {
                        break;
                    }
                    if (i > 10)
                        health = i;
                }

                overlay.DrawRect(p.X - 20, p.Y - 20, 150, 240);
                overlay.DrawText(health.ToString(), 20, 600);
                StatusHeroHealth?.Invoke(health);
            }
            else
                StatusHeroHealth?.Invoke(0);

        }

        private void UpdateEnemyOnScreen()
        {
            images.gameImages.TryGetValue("EnemyLife", out Bitmap bitmap);
            if (openCV.SearchImagesFromRegion(bitmap, out List<Point> points, new Point(100, 40), new Point(1200, 600)))
                foreach (var p in points)
                {
                    if (autoIt.IsHealthPixel(new Point(p.X + autoIt.window.X + 12, p.Y + autoIt.window.Y + 10)))
                    {
                        overlay.DrawRect(p.X - 20, p.Y - 20, 150, 240);
                        overlay.DrawText("Enemy spotted!", 20, 640);
                        enemy = true;
                        return;
                    }
                }
           
            enemy = false;
        }

        private void UpdateIsBattle()
        {
            if (autoIt.GetPixelColor(600, 100) == 13112352)
            {
                IsBattle = true;
                overlay.DrawText("In battle.", 20, 620);

                if (autoIt.GetPixelColor(620, 100) != 13112352)
                {
                    enemyLowHealth = true;
                    overlay.DrawText("Low health enemy.", 80, 620);
                }
                else
                    enemyLowHealth = false;
            }
            else
                IsBattle = false;

        }

        private void UpdateTeamHero()
        {
            if (autoIt.FindPixelColorPos(29382, 600, 30, 1200, 500, out Point p))
            {
                if (autoIt.GetPixelColor(p.X + 5, p.Y) == 29382)
                {
                    overlay.DrawText("Team Hero.", 20, 660);
                    overlay.DrawRect(p.X - 20, p.Y - 20, 150, 240);
                    TeamHero = true;
                    return;
                }

            }
            TeamHero = false;
        }




        private void SetAction()
        {
            if(HeroAboveLine)
            {
                key.newState = "Right";
                Thread.Sleep(TimeSpan.FromMilliseconds(2000));
                key.newState = "Stop";
                HeroAboveLine = false;
            }

            if (HeroBelowLine)
            {
                key.newState = "Left";
                Thread.Sleep(TimeSpan.FromMilliseconds(2000));
                key.newState = "Stop";
                HeroBelowLine = false;
            }


            if (IsDeath())
            {
                overlay.DrawText("Death", 80, 700);
                key.newState = "Stop";
                ActionDelay();
                return;
            }

            if (TowerZone)
            {
                overlay.DrawText("Tower.Back", 50, 260);
                key.newState = "Back";
                ActionHealth();
                ActionAttack();

                ActionBaseAttack();
                Task.Run(() => BlockForwardMove(4000));
                return;
            }

            if (TowerAhead && enemy)
            {
                overlay.DrawText("Stop - Attack - Back", 50, 280);
                key.newState = "Stop";
                ActionAttack();
                key.newState = "Back";
                return;
            }

            if (TowerAhead)
            {
                overlay.DrawText("Tower ahead - Back.", 50, 280);
                key.newState = "Back";
                ActionBaseAttack();
                Task.Run(() => BlockForwardMove(1000));
                return;
            }


            if (enemy)
            {
                overlay.DrawText("Attack enemy!", 50, 260);
                ActionAttack();
                ActionBaseAttack();
                return;
            }


            if (enemy && IsBattle)
            {
                overlay.DrawText("Attack enemy!", 50, 260);
                key.newState = "Stop";
                ActionAttack();
                return;
            }


            if (canMoveForward)
            {
                overlay.DrawText("Forward!", 50, 260);
                key.newState = "Forward";
                return;
            }

            overlay.DrawText("BaseAttack.", 50, 260);
            ActionBaseAttack();
            return;
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
            images.gameImages.TryGetValue("Death", out Bitmap bitmap);
            if (openCV.SearchImageFromRegion(bitmap, out _, new Point(560, 100), new Point(720, 140)))
            {
                console.WriteLine("Hero is dead.");
                return true;
            }
            return false;
        }


        bool ActionAttack()
        {
            console.WriteLine(" ActionAttack!");
            key.AttackAllSkills();
           
            return true;
        }

        bool ActionBaseAttack()
        {
            console.WriteLine("ActionBaseAttack!");
            key.BaseAttack();
            return true;
        }

        bool ActionDelay()
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(3000));
            return true;
        }

    }
}
