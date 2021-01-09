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
using Point = OpenCvSharp.Point;


namespace BOCollector
{
    class OpenCV
    {
        private readonly Writer console;
        private readonly AutoIt autoIt;
        private readonly OverlayDX overlay;
        Rectangle window;
        Bitmap gameScreen_bitmap;
        Graphics gameScreen_graphics;
        System.Drawing.Size size_region;
        

        public OpenCV(Writer console, AutoIt autoIt, OverlayDX overlay)
        {
            this.console = console;
            this.autoIt = autoIt;
            this.overlay = overlay;
            window = autoIt.window;
            if (window == null)
                return;
            gameScreen_bitmap = new Bitmap(window.Width, window.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            gameScreen_graphics = Graphics.FromImage(gameScreen_bitmap);
            size_region = new System.Drawing.Size(window.Width, window.Height);

            console.WriteLine("OpenCV loaded.");
        }

        internal bool SearchImageFromDict(Dictionary<string, Bitmap> buttonImages, out Point centerPoint, out string name)
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
                    overlay.DrawRect(maxLoc.X, maxLoc.Y, buttonImage.Value.Width, buttonImage.Value.Height);
                    name = buttonImage.Key;
                    return true;
                }
            }
            
            return false;
        }

        internal bool SearchImageFromRegion(Bitmap bitmap, out Point f, Point start, Point end )
        {
            window = autoIt.window;
            double threshold = 0.8;        //Пороговое значение SearchImg
            f = new OpenCvSharp.Point();

            if (bitmap == null)
            {
                console.WriteLine("Error. Null bitmap.");
                return false; 
            }

            if(start.X>=end.X || start.Y>=end.Y || end.X > window.Width || end.Y > window.Height)
            {
                console.WriteLine("Error region.");
                return false;
            }

            gameScreen_graphics.CopyFromScreen(window.X, window.Y, 0, 0, size_region);                    //делаем скрин экрана
            using Mat gameScreen = OpenCvSharp.Extensions.BitmapConverter.ToMat(gameScreen_bitmap);  //Сохраняем скрин экрана в mat
            using Mat gameScreenGray = gameScreen.SubMat(start.Y, end.Y, start.X, end.X);            //Вырезаем область
            using Mat gameScreenGrayRegion = gameScreenGray.CvtColor(ColorConversionCodes.BGR2GRAY); //Конвертируем в ЧБ   

            using Mat searchImg = OpenCvSharp.Extensions.BitmapConverter.ToMat(bitmap);            
            using Mat searchImgGray = searchImg.CvtColor(ColorConversionCodes.BGR2GRAY);

            using Mat result = new Mat();

            Cv2.MatchTemplate(gameScreenGrayRegion, searchImgGray, result, TemplateMatchModes.CCoeffNormed);        //Поиск шаблона
            Cv2.Threshold(result, result, threshold, 1.0, ThresholdTypes.Tozero);
            Cv2.MinMaxLoc(result, out double minVal, out double maxVal, out OpenCvSharp.Point minLoc, out OpenCvSharp.Point maxLoc); //Поиск точки

            if (maxVal > threshold)
            {
                f = new OpenCvSharp.Point(maxLoc.X + start.X, maxLoc.Y + start.Y);
                overlay.DrawRect(maxLoc.X + start.X, maxLoc.Y+ start.Y, bitmap.Width, bitmap.Height);
            }
            else
                return false;


            return true;
        }

        public bool SearchBitmapPos(Bitmap bitmap, out Point sourccePoint,out Point centerPoint)
        {
            window = autoIt.window;
            double threshold = 0.85;        //Пороговое значение SearchImg
            sourccePoint = new Point();
            centerPoint = new Point();
            if (bitmap == null)
                return false;

            gameScreen_graphics.CopyFromScreen(window.X, window.Y, 0, 0, size_region);                         //делаем скрин экрана
            
            using Mat resultImg = new Mat();
            using Mat gameScreen = OpenCvSharp.Extensions.BitmapConverter.ToMat(gameScreen_bitmap);       //Сохраняем скрин экрана в mat
            using Mat searchImg = OpenCvSharp.Extensions.BitmapConverter.ToMat(bitmap);
            using Mat gameScreenGray = gameScreen.CvtColor(ColorConversionCodes.BGR2GRAY);
            using Mat searchImgGray = searchImg.CvtColor(ColorConversionCodes.BGR2GRAY);
         
            Cv2.MatchTemplate(gameScreenGray, searchImgGray, resultImg, TemplateMatchModes.CCoeffNormed);        //Поиск шаблона
            Cv2.Threshold(resultImg, resultImg, threshold, 1.0, ThresholdTypes.Tozero);                                    //Оптимизация
            Cv2.MinMaxLoc(resultImg, out double minVal, out double maxVal, out OpenCvSharp.Point minLoc, out OpenCvSharp.Point maxLoc); //Поиск точки

            if (maxVal > threshold)
            {
                sourccePoint = maxLoc;
                centerPoint = new Point(maxLoc.X + bitmap.Width / 2, maxLoc.Y + bitmap.Height / 2);
                overlay.DrawRect(maxLoc.X, maxLoc.Y, bitmap.Width, bitmap.Height);
            }
            else
                return false;
           
            return true;
        }

        internal bool SearchImagesFromRegion(Bitmap bitmap, out List<Point> points, Point start, Point end)
        {
            window = autoIt.window;
            double threshold = 0.77;  //Пороговое значение SearchImg
            points = new List<Point>();

            if (bitmap == null)
            {
                console.WriteLine("Error. Null bitmap.");
                return false;
            }

            if (start.X >= end.X || start.Y >= end.Y || end.X > window.Width || end.Y > window.Height)
            {
                console.WriteLine("Error region.");
                return false;
            }

            gameScreen_graphics.CopyFromScreen(window.X, window.Y, 0, 0, size_region);               //делаем скрин экрана
            using Mat gameScreen = OpenCvSharp.Extensions.BitmapConverter.ToMat(gameScreen_bitmap);  //Сохраняем скрин экрана в mat
            using Mat gameScreenGray = gameScreen.SubMat(start.Y, end.Y, start.X, end.X);            //Вырезаем область
            using Mat gameScreenGrayRegion = gameScreenGray.CvtColor(ColorConversionCodes.BGR2GRAY); //Конвертируем в ЧБ  

            using Mat searchImg = OpenCvSharp.Extensions.BitmapConverter.ToMat(bitmap);
            using Mat searchImgGray = searchImg.CvtColor(ColorConversionCodes.BGR2GRAY);

            using Mat result = new Mat();

            Cv2.MatchTemplate(gameScreenGrayRegion, searchImgGray, result, TemplateMatchModes.CCoeffNormed);
            Cv2.Threshold(result, result, threshold, 1.0, ThresholdTypes.Tozero);
           
            using Mat resultPoints = new Mat();
            Cv2.FindNonZero(result, resultPoints);

            for (int i = 0; i < resultPoints.Total(); i++)
            {
                points.Add(new Point(resultPoints.At<Point>(i).X + start.X, resultPoints.At<Point>(i).Y + start.Y));
            }
            
            //Cv2.ImShow("Matches", result);
            //Cv2.WaitKey();

            return true;
        }
    }
}



// ImageButton imageButton = new ImageButton(name,i,bitmap);
// buttons.Add(imageButton);

//Cv2.ImShow("Matches", gameScreenGrayRegion);
//Cv2.WaitKey();