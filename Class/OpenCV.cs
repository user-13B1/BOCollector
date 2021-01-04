using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BOCollector
{
    class OpenCV
    {
        private readonly Writer console;
        private readonly AutoIt autoIt;
        Rectangle window;
        Bitmap gameScreenBitmap;

        public OpenCV(Writer console, AutoIt autoIt)
        {
            this.console = console;
            this.autoIt = autoIt;

            autoIt.UpdatePos(out window);
            gameScreenBitmap = new Bitmap(window.Width, window.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Graphics gameScreenGraphics = Graphics.FromImage(gameScreenBitmap);
            console.WriteLine("OpenCV loaded.");
        }

        internal bool SearchImageFromDict(Dictionary<string, Bitmap> buttonImages, out OpenCvSharp.Point f)
        {
            double threshold = 0.85;
            f = new OpenCvSharp.Point();
            autoIt.UpdatePos(out window);


            return true;
        }

        public bool SearchImg(Bitmap searchBitmap, out OpenCvSharp.Point f)
        {
            double threshold = 0.85;        //Пороговое значение SearchImg
            f = new OpenCvSharp.Point();
            autoIt.UpdatePos(out window);
            
            System.Drawing.Size size_region = new System.Drawing.Size(window.Width, window.Height);
            gameScreenGraphics.CopyFromScreen(window.X, window.Y, 0, 0, size_region);                         //делаем скрин экрана
           
            using Mat mat_result = new Mat();
            using Mat mat_region_desktop = OpenCvSharp.Extensions.BitmapConverter.ToMat(gameScreenBitmap);       //Сохраняем скрин экрана в mat
            using Mat mat_search = OpenCvSharp.Extensions.BitmapConverter.ToMat(searchBitmap);
            using Mat mat_region_desktop_gray = mat_region_desktop.CvtColor(ColorConversionCodes.BGR2GRAY);
            using Mat mat_search_gray = mat_search.CvtColor(ColorConversionCodes.BGR2GRAY);
            
            Cv2.MatchTemplate(mat_region_desktop_gray, mat_search_gray, mat_result, TemplateMatchModes.CCoeffNormed);        //Поиск шаблона
            Cv2.Threshold(mat_result, mat_result, threshold, 1.0, ThresholdTypes.Tozero);                                    //Оптимизация
            Cv2.MinMaxLoc(mat_result, out double minVal, out double maxVal, out OpenCvSharp.Point minLoc, out OpenCvSharp.Point maxLoc); //Поиск точки

            if (maxVal > threshold)
            {
                f = maxLoc;
                DrawToScreen.DrawRect(maxLoc.X + window.X, maxLoc.Y + window.Y, searchBitmap.Width, searchBitmap.Height);
            }
            else
                return false;
            
            return true;
        }


    }
}


//struct ImageButton
//{
//    public string name;
//    public int num;
//    Bitmap bitmap;

//    public ImageButton(string name, int num, Bitmap bitmap)
//    {
//        this.name = name;
//        this.num = num;
//        this.bitmap = bitmap;
//    }
//}

// ImageButton imageButton = new ImageButton(name,i,bitmap);
// buttons.Add(imageButton);