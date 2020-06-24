
using System;
using System.Collections.Generic;
using OpenCvSharp;
using OpenCvPoint = OpenCvSharp.Point;

namespace PCRBattleRecorder
{
    public class OpenCvTools
    {

        private static OpenCvTools instance;

        public static OpenCvTools GetInstance()
        {
            if (instance == null)
            {
                instance = new OpenCvTools();
            }
            return instance;
        }

        private OpenCvTools()
        {
        }

        public void ShowMat(string key, Mat mat)
        {

        }

        public OpenCvMatchImageResult MatchImage(Mat source, Mat search, double threshold)
        {
            if (source.Width < search.Width || source.Height < search.Height)
            {
                return new OpenCvMatchImageResult() { Success = false };
            }
            var res = new Mat();
            ShowMat("source", source);
            ShowMat("search", search);
            Cv2.MatchTemplate(source, search, res, TemplateMatchModes.CCoeffNormed);
            double minVal, maxVal;
            OpenCvPoint minLoc, maxLoc;
            Cv2.MinMaxLoc(res, out minVal, out maxVal, out minLoc, out maxLoc);
            if (maxVal < threshold)
            {
                return new OpenCvMatchImageResult() { Success = false, Maxval = maxVal };
            }
            Cv2.Circle(source, maxLoc.X + search.Width / 2, maxLoc.Y + search.Height / 2, 25, Scalar.Red);
            ShowMat("MatchImage", source);
            return new OpenCvMatchImageResult()
            {
                Success = true,
                MatchedRect = new RECT()
                {
                    x1 = maxLoc.X,
                    y1 = maxLoc.Y,
                    x2 = maxLoc.X + search.Width,
                    y2 = maxLoc.Y + search.Height,
                },
                Maxval = maxVal,
            };
        }

    }

    public struct OpenCvMatchImageResult
    {
        public bool Success;
        public RECT MatchedRect;
        public double Maxval;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewportRect">Mumu视口大小</param>
        /// <param name="matchSourceRectRate">采样区域相对于Mumu视口比率</param>
        /// <returns></returns>
        public RECT GetMatchedAbsoluteRect(RECT viewportRect, Vec4f matchSourceRectRate)
        {
            var matchSourceRelativeRect = viewportRect.GetChildRectByRate(matchSourceRectRate);
            return new RECT()
            {
                x1 = matchSourceRelativeRect.x1 + MatchedRect.x1,
                y1 = matchSourceRelativeRect.y1 + MatchedRect.y1,
                x2 = matchSourceRelativeRect.x1 + MatchedRect.x2,
                y2 = matchSourceRelativeRect.y1 + MatchedRect.y2,
            };
        }
    }
}
