using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZ.Framework.Drawing
{
    public class ValidateCoder
    {
        private static readonly Random random = new Random();

        public ValidateCoder()
        {
            FontNames = new List<string> { "Arial", "Batang", "Buxton Sketch", "David", "SketchFlow Print" };
            FontNamesForChinese = new List<string> { "宋体", "幼圆", "楷体", "仿宋", "隶书", "黑体" };
            FontSize=FontWidth = 20;
            BgColor = Color.FromArgb(240, 240, 240);
            RandomPointPercent = 0;
            
        }

        #region 属性

        /// <summary>
        ///字体名称集合 
        /// </summary>
        public List<string> FontNames { get; set; }

        /// <summary>
        /// 中文字体名称集合
        /// </summary>
        public List<string> FontNamesForChinese { get; set; }

        /// <summary>
        /// 字体大小
        /// </summary>
        public int FontSize { get; set; }

        /// <summary>
        /// 字体宽度
        /// </summary>
        public int FontWidth { get; set; }

        /// <summary>
        /// 图片高度
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 背景颜色
        /// </summary>
        public Color BgColor { get; set; }

        /// <summary>
        /// 是否有边框
        /// </summary>
        public bool HasBorder { get; set; }

        /// <summary>
        ///是否随机位置 
        /// </summary>
        public bool RandomPosition { get; set; }

        /// <summary>
        ///  是否随机字体颜色
        /// </summary>
        public bool RandomColor { get; set; }

        /// <summary>
        /// 是否随机倾斜字体
        /// </summary>
        public bool RandomItalic { get; set; }

        /// <summary>
        /// 随机干扰点百分比（百分数形式）
        /// </summary>
        public double RandomPointPercent { get; set; }

        /// <summary>
        /// 随机干扰线数量
        /// </summary>
        public int RandomLineCount { get; set; }
        
        #endregion


        public string GetCode(int lenght, ValidateCodeType codeType = ValidateCodeType.NumberAndLetter)
        {
            lenght.CheckGreaterThan("lenght", 0);

            switch (codeType)
            {
                case ValidateCodeType.Number:
                    GetRandomNums(lenght);
                    break;
                case ValidateCodeType.NumberAndLetter:
                    break;
                default:
                    break;

            }
            return "";
        }

        /// <summary>
        /// 获取指定字符串的验证码图片
        /// </summary>
        /// <param name="code"></param>
        /// <param name="codeType"></param>
        /// <returns></returns>
        public Bitmap CreateImage(string code, ValidateCodeType codeType)
        {
            code.CheckNotNullOrEmpty("code");

            int width = FontWidth * code.Length + FontWidth;
            int height = FontSize + FontSize / 2;
            const int flag = 255 / 2;
            bool isBgLight = (BgColor.R + BgColor.G + BgColor.B) / 3 > flag;
            Bitmap image = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(image);
            Brush brush = new SolidBrush(Color.FromArgb(255 - BgColor.R, 255 - BgColor.G, 255 - BgColor.B));
            int x, y = 3;
            if (HasBorder)
            {
                graphics.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
            }

            Random rnd = new Random();

            //绘制干扰线
            for (int i = 0; i < RandomLineCount; i++)
            {
                x = rnd.Next(image.Width);
                y = rnd.Next(image.Height);
                int m = rnd.Next(image.Width);
                int n = rnd.Next(image.Height);
                Color lineColor = !RandomColor
                    ? Color.FromArgb(90, 90, 90)
                    : isBgLight
                    ? Color.FromArgb(rnd.Next(130, 200), rnd.Next(130, 200), rnd.Next(130, 220))
                    : Color.FromArgb(rnd.Next(70, 150), rnd.Next(70, 150), rnd.Next(70, 150));
                Pen pen = new Pen(lineColor, 2);
                graphics.DrawLine(pen, x, y, m, n);
            }

            //绘制干扰点
            int pointCount = (int)(image.Width * image.Height * RandomPointPercent / 100);
            for (int i = 0; i <pointCount; i++)
            {
                x = rnd.Next(image.Width);
                y = rnd.Next(image.Height);
                Color pointColor = isBgLight
                    ? Color.FromArgb(rnd.Next(30, 80), rnd.Next(30, 80), rnd.Next(30, 80))
                    : Color.FromArgb(rnd.Next(150, 200), rnd.Next(150, 200), rnd.Next(150, 200));
                image.SetPixel(x, y, pointColor);
            }

            //绘制文字
            for (int i = 0; i < code.Length; i++)
            {
                rnd = random;
                x = FontWidth / 4 + FontWidth * i;
                if (RandomPosition)
                {
                    x = rnd.Next(FontWidth / 4) + FontWidth * i;
                    y = rnd.Next(image.Height / 5);
                }
                PointF point = new PointF(x, y);
                if (RandomColor)
                {
                    int r, g, b;
                    if (!isBgLight)
                    {
                        r = rnd.Next(255 - BgColor.R);
                        g = rnd.Next(255 - BgColor.G);
                        b = rnd.Next(255 - BgColor.B);
                        if ((r + g + b) / 3 < flag)
                        {
                            r = 255 - r;
                            g = 255 - g;
                            b = 255 - b;
                        }
                    }
                    else
                    {
                        r = rnd.Next(BgColor.R);
                        g = rnd.Next(BgColor.G);
                        b = rnd.Next(BgColor.B);
                        if ((r + g + b) / 3 > flag)
                        {
                            r = 255 - r;
                            g = 255 - g;
                            b = 255 - b;
                        }
                    }
                    brush = new SolidBrush(Color.FromArgb(r, g, b));
                }
                string fontName = codeType == ValidateCodeType.Chinese
                    ? FontNamesForChinese[rnd.Next(FontNamesForChinese.Count)]
                    : FontNames[rnd.Next(FontNames.Count)];
                Font font = new Font(fontName, FontSize, FontStyle.Bold);
                if (RandomItalic)
                {
                    graphics.TranslateTransform(0, 0);
                    Matrix transform = graphics.Transform;
                    transform.Shear(Convert.ToSingle(rnd.Next(2, 9) / 10d - 0.5), 0.001f);
                    graphics.Transform = transform;
                }
                graphics.DrawString(code.Substring(i,1),font,brush,point);
                graphics.ResetTransform();
            }

            return image;
        }


        /// <summary>
        /// 获取指定长度的验证码图片
        /// </summary>
        /// <param name="lenght"></param>
        /// <param name="code"></param>
        /// <param name="codeType"></param>
        /// <returns></returns>
        public Bitmap CreateImage(int lenght, out string code, ValidateCodeType codeType = ValidateCodeType.NumberAndLetter)
        {
            lenght.CheckGreaterThan("lenght", 0);

            lenght = lenght < 1 ? 1 : lenght;
            switch (codeType)
            { 
                case ValidateCodeType.Number:
                    code = "";
                    break;
                case ValidateCodeType.NumberAndLetter:
                    code = "";
                    break;
                default:
                    code = "";
                    break;

            }
            if (code.Length > lenght) code = code.Substring(0, lenght);

            return CreateImage(code, codeType);
        }


        private static string GetRandomNums(int length)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < length; i++)
            {
                list.Add(random.Next(0, 9));
            }
            return string.Empty;//list.ExpandAndToString("");
        }

        private static string GetRandomNumsAndLetters(int length)
        {
            const string allChar = "2,3,4,5,6,7,8,9," +
                "A,B,C,D,E,F,G,H,J,K,M,N,P,Q,R,S,T,U,V,W,X,Y,Z," +
                "a,b,c,d,e,f,g,h,k,m,n,p,q,r,s,t,u,v,w,x,y,z";
            string[] allChars = allChar.Split(',');
            List<string> result = new List<string>();
            while (result.Count < length)
            {
                int index = random.Next(allChars.Length);
                string c = allChars[index];
                result.Add(c);
            }
            return string.Empty;//result.ExpandAndToString("");
        }


        private static string GetRandomChinese(int length)
        {
            string[] baseStr = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f".Split(',');
            Encoding encoding = Encoding.GetEncoding("GB2312");
            string result = string.Empty;

            for (int i = 0; i < length; i++)
            {
                Random rnd = random;
                int index1 = rnd.Next(11,14);
                string str1 = baseStr[index1];

                int index2 = index1 == 13 ? rnd.Next(0, 7) : rnd.Next(0, 16);
                string str2 = baseStr[index2];

                int index3 = rnd.Next(10,16);
                string str3 = baseStr[index3];

                int index4 = index3 == 10 ? rnd.Next(1, 16) : (index3 == 15 ? rnd.Next(0, 15) : rnd.Next(0,16));
                string str4 = baseStr[index4];

                byte b1 = Convert.ToByte(str1 + str2, 16);
                byte b2 = Convert.ToByte(str3 + str4, 16);
                byte[] bs = { b1, b2 };

                result +=encoding.GetString(bs);
            }

            return result;
        }

    }
}
