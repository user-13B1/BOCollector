using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenCvSharp;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace BOCollector
{
    class OpenCV
    {
        private readonly Writer console;
        private readonly AutoIt autoIt;
        Rectangle window;
        Bitmap gameScreen_bitmap;
        Graphics gameScreen_graphics;
        System.Drawing.Size size_region;
       

        public OpenCV(Writer console, AutoIt autoIt)
        {
            this.console = console;
            this.autoIt = autoIt;
            window = autoIt.window;
            if (window == null)
                return;
            gameScreen_bitmap = new Bitmap(window.Width, window.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            gameScreen_graphics = Graphics.FromImage(gameScreen_bitmap);
            size_region = new System.Drawing.Size(window.Width, window.Height);

            console.WriteLine("OpenCV loaded.");
        }

        internal bool SearchImageFromDict(Dictionary<string, Bitmap> buttonImages, out OpenCvSharp.Point centerPoint, out string name)
        {
            window = autoIt.window;
            double threshold = 0.85;
            name = null;
            centerPoint = new OpenCvSharp.Point();

            gameScreen_graphics.CopyFromScreen(window.X, window.Y, 0, 0, size_region);
            using Mat result = new Mat();
            using Mat gameScreen = OpenCvSharp.Extensions.BitmapConverter.ToMat(gameScreen_bitmap);       //Сохраняем скрин экрана в mat
            using Mat mat_region_desktop_gray = gameScreen.CvtColor(ColorConversionCodes.BGR2GRAY);

            foreach (KeyValuePair<string, Bitmap> buttonImage in buttonImages)
            {
                using Mat searchImg = OpenCvSharp.Extensions.BitmapConverter.ToMat(buttonImage.Value);
                using Mat searchImg_gray = searchImg.CvtColor(ColorConversionCodes.BGR2GRAY);

                Cv2.MatchTemplate(mat_region_desktop_gray, searchImg_gray, result, TemplateMatchModes.CCoeffNormed);        //Поиск шаблона
                Cv2.Threshold(result, result, threshold, 1.0, ThresholdTypes.Tozero);
                Cv2.MinMaxLoc(result, out double minVal, out double maxVal, out OpenCvSharp.Point minLoc, out OpenCvSharp.Point maxLoc); //Поиск точки
                if (maxVal > threshold)
                {
                    centerPoint = new OpenCvSharp.Point(maxLoc.X + buttonImage.Value.Width/2 , maxLoc.Y + buttonImage.Value.Height / 2);
                    DrawToScreen.DrawRect(maxLoc.X + window.X, maxLoc.Y + window.Y, buttonImage.Value.Width, buttonImage.Value.Height);
                    
                    name = buttonImage.Key;
                    return true;
                }
            }
            
            return false;
        }

        internal bool SearchImageFromName(Bitmap bitmap, out OpenCvSharp.Point f)
        {

            window = autoIt.window;
            double threshold = 0.9;        //Пороговое значение SearchImg
            f = new OpenCvSharp.Point();
            if(bitmap==null)
                return false;
            gameScreen_graphics.CopyFromScreen(window.X, window.Y, 0, 0, size_region);                         //делаем скрин экрана
            using Mat result = new Mat();
            using Mat gameScreen = OpenCvSharp.Extensions.BitmapConverter.ToMat(gameScreen_bitmap);       //Сохраняем скрин экрана в mat
            using Mat mat_region_desktop_gray = gameScreen.CvtColor(ColorConversionCodes.BGR2GRAY);
            using Mat searchImg = OpenCvSharp.Extensions.BitmapConverter.ToMat(bitmap);
            using Mat mat_search_gray = searchImg.CvtColor(ColorConversionCodes.BGR2GRAY);

            Cv2.MatchTemplate(mat_region_desktop_gray, mat_search_gray, result, TemplateMatchModes.CCoeffNormed);        //Поиск шаблона
            Cv2.Threshold(result, result, threshold, 1.0, ThresholdTypes.Tozero);
            Cv2.MinMaxLoc(result, out double minVal, out double maxVal, out OpenCvSharp.Point minLoc, out OpenCvSharp.Point maxLoc); //Поиск точки
            if (maxVal > threshold)
            {
                f = maxLoc;
                DrawToScreen.DrawRect(maxLoc.X + window.X, maxLoc.Y + window.Y, bitmap.Width, bitmap.Height);

                console.WriteLine(maxVal); 
            }
            else
                return false;

            return true;
        }

        public bool SearchImg(Bitmap searchBitmap, out OpenCvSharp.Point f)
        {
            window = autoIt.window;
            double threshold = 0.85;        //Пороговое значение SearchImg
            f = new OpenCvSharp.Point();
            gameScreen_graphics.CopyFromScreen(window.X, window.Y, 0, 0, size_region);                         //делаем скрин экрана
            
            using Mat result = new Mat();
            using Mat gameScreen = OpenCvSharp.Extensions.BitmapConverter.ToMat(gameScreen_bitmap);       //Сохраняем скрин экрана в mat
            using Mat searchImg = OpenCvSharp.Extensions.BitmapConverter.ToMat(searchBitmap);
            using Mat mat_region_desktop_gray = gameScreen.CvtColor(ColorConversionCodes.BGR2GRAY);
            using Mat mat_search_gray = searchImg.CvtColor(ColorConversionCodes.BGR2GRAY);
         
            Cv2.MatchTemplate(mat_region_desktop_gray, mat_search_gray, result, TemplateMatchModes.CCoeffNormed);        //Поиск шаблона
            Cv2.Threshold(result, result, threshold, 1.0, ThresholdTypes.Tozero);                                    //Оптимизация
            Cv2.MinMaxLoc(result, out double minVal, out double maxVal, out OpenCvSharp.Point minLoc, out OpenCvSharp.Point maxLoc); //Поиск точки

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