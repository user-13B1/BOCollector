using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BOCollector
{
    class Images
    {
        private Writer console;
        private AutoIt autoIt;
        private OpenCV openCV;
        private Dictionary<string, Bitmap> gameImages;
        private Dictionary<string, Bitmap> buttonImages;
        private Dictionary<string, Bitmap> stateImages;


        public Images(Writer console, AutoIt autoIt, OpenCV openCV)
        {
            this.console = console;
            this.autoIt = autoIt;
            this.openCV = openCV;

            Task.Run(() => LoadImages(@"\button", out buttonImages));
            Task.Run(() => LoadImages(@"\state", out stateImages));
            Task.Run(() => LoadImages(@"\game_image", out gameImages));
        }

        private bool LoadImages(string folderName, out Dictionary<string, Bitmap> images)
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


    }
}
