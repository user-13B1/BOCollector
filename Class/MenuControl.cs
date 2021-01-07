using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenCvSharp;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace BOCollector
{
    class MenuControl
    {
        internal readonly AutoIt autoIt;
        private readonly OpenCV openCV;
        private readonly Images images;
        private readonly Writer console;
        delegate bool Operation();
       

        public MenuControl(Writer console, AutoIt autoIt, OpenCV openCV, Images images)
        {
            this.console = console;
            this.autoIt = autoIt;
            this.openCV = openCV;
            this.images = images;
        }

        internal bool IsBattle()
        {
            autoIt.UpdateWindowPos();
            if (!autoIt.IsPixelColor(244, 49, 125697))
                return false;
            return true;
        }

        public bool Start()
        {
           
            CloseFrameOnScreen();
            string state = StateSearch();
            Operation action = GetAction(state);
            if(!action())
                return false;

            return true;
        }

        Operation GetAction(string state) => state switch
        {
            "Classic" => ActionClassic,
            "VsAI" => ActionVsAI,
            "ReturnLobby" => ActionDelay,
            "VS" => ActionDelay,
            { } => ActionError,
            null => ActionError
        };

        private string StateSearch()
        {
            if (openCV.SearchImageFromDict(images.stateImages, out _, out string name))
            {
                console.WriteLine($"State found: {name}");
                return name;
            }

            return null;
        }

        private void CloseFrameOnScreen()
        {
            if (openCV.SearchImageFromDict(images.buttonImages, out OpenCvSharp.Point centerPoint, out string name))
            {
                console.WriteLine($"Button found: {name}");
                autoIt.ClickMouseToWindow(centerPoint.X,centerPoint.Y);
            }
        }

        bool ActionClassic()
        {
           
            autoIt.ClickMouseToWindow(405, 614, 1000);
            autoIt.ClickMouseToWindow(405, 614, 1000);
            ActionVsAI();
            return true;
        }

        bool ActionVsAI()
        {
            autoIt.ClickMouseToWindow(650, 600, 1000);
            autoIt.ClickMouseToWindow(480, 645, 1000);
            autoIt.ClickMouseToWindow(430, 340, 1000);
            autoIt.ClickMouseToWindow(520, 640, 1000);
            return true;
        }

        bool ActionDelay()
        {

            Thread.Sleep(TimeSpan.FromMilliseconds(3000));
            return true;
        }

        bool ActionError()
        {
            console.WriteLine("Invalid State.");
            return true;
        }


    }
}


//if (openCV.Search("StartGame", out _))
//{
//    console.WriteLine("State:Game Lobby.");
//    return true;
//}

//if (!buttonImages.TryGetValue(nameBtn, out Bitmap buffer))
//{
//    console.WriteLine("Error. Invalid image name.");
//    return false;
//}

//x = 244
//y = 49
//Color - 125697

//keybd_event('A', 0, 0, 0);
//keybd_event('A', 0, KEYEVENTF_KEYUP, 0);