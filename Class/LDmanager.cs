using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenCvSharp;
using System.IO;
using System.Windows.Forms;

namespace BOCollector
{
    class LDmanager
    {
        private readonly AutoIt autoIt;
        private readonly OpenCV openCV;
        private Dictionary<string, Bitmap> buttonImages;
        private Dictionary<string, Bitmap> stateImages;
        private readonly Writer console;

        public LDmanager(Writer console)
        {
            this.console = console;
            Task.Run(() => LoadImages(@"\button", buttonImages));
            Task.Run(() => LoadImages(@"\state", stateImages));

            autoIt = new AutoIt(console, "LDPlayer-1");
            openCV = new OpenCV(console, autoIt);

        }

        private bool LoadImages(string folderName, Dictionary<string, Bitmap> images)
        {
            string imgFolderDirPath = Directory.GetCurrentDirectory() + folderName;
            images = new Dictionary<string, Bitmap>();

            if (!Directory.Exists(imgFolderDirPath))
            {
                console.WriteLine("Error. Image directory not founded.");
                return false;
            }

            string[] imgPaths = Directory.GetFiles(imgFolderDirPath, "*.png");
            for (int i = 0; i < imgPaths.Length; i++)
            {
                if (!File.Exists(imgPaths[i]))
                {
                    MessageBox.Show($"Error load image: {imgPaths[i]}", "Error.");
                    return false;
                }

                string name = imgPaths[i];
                name = name.Replace(imgFolderDirPath, "").Replace("\\", "").Replace(".png", "");

                images.Add(name, new Bitmap(imgPaths[i]));
            }

            return true;
        }

        public bool GoBattle()
        {
            PressButton();


            return true;
        }

        private void PressButton()
        {
            openCV.SearchImageFromDict(buttonImages, out OpenCvSharp.Point F);


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