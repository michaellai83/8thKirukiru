using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;

namespace Kiru8th.Models
{
    public class Upload
    {
        #region"儲存上傳圖片"
        /// <summary>
        /// 儲存上傳圖片
        /// </summary>
        /// <param name="upfile">HttpPostedFile 物件</param>
        /// <returns>儲存檔名</returns>
        static public string SaveUpImage(HttpPostedFileBase upfile)
        {

            //取得副檔名
            string extension = upfile.FileName.Split('.')[upfile.FileName.Split('.').Length - 1];
            //新檔案名稱
            string fileName = String.Format("{0:yyyyMMddhhmmsss}.{1}", DateTime.Now, extension);
            string savedName = Path.Combine(HttpContext.Current.Server.MapPath("~/images"), fileName);
            upfile.SaveAs(savedName);
            return fileName;
        }
        #endregion

        #region "舉世無敵縮圖程式"
        /// <summary>
        /// 舉世無敵縮圖程式(多載)
        /// 1.會自動判斷是比較高還是比較寬，以比較大的那一方決定要縮的尺寸
        /// 2.指定寬度，等比例縮小
        /// 3.指定高度，等比例縮小
        /// </summary>
        /// <param name="name">原檔檔名</param>
        /// <param name="source">來源檔案的Stream,可接受上傳檔案</param>
        /// <param name="target">目的路徑</param>
        /// <param name="suffix">縮圖辯識符號</param>
        /// <param name="MaxWidth">指定要縮的寬度</param>
        /// <param name="MaxHight">指定要縮的高度</param>
        /// <remarks></remarks>
        static public void GenerateThumbnailImage(string name, System.IO.Stream source, string target, string suffix, int MaxWidth, int MaxHight)
        {
            System.Drawing.Image baseImage = System.Drawing.Image.FromStream(source);
            Single ratio = 0.0F; //存放縮圖比例
            Single h = baseImage.Height; //圖像原尺寸高度
            Single w = baseImage.Width;  //圖像原尺寸寬度
            int ht; //圖像縮圖後高度
            int wt;//圖像縮圖後寬度
            //寬固定算高
            ratio = MaxWidth / w; //計算寬度縮圖比例
            if (MaxWidth < w)
            {
                ht = Convert.ToInt32(ratio * h);
                wt = MaxWidth;

            }
            else
            {
                ht = Convert.ToInt32(baseImage.Height);
                wt = Convert.ToInt32(baseImage.Width);

            }
            if (MaxHight > ht || MaxWidth >= w)
            {
                ratio = MaxHight / h; //計算寬度縮圖比例
                if (MaxHight < h)
                {
                    ht = MaxHight;
                    wt = Convert.ToInt32(ratio * w);
                }
                else
                {
                    ht = Convert.ToInt32(baseImage.Height);
                    wt = Convert.ToInt32(baseImage.Width);
                }
            }
            string Newname = target + "\\" + suffix + name;
            System.Drawing.Bitmap img = new System.Drawing.Bitmap(wt, ht);
            System.Drawing.Graphics graphic = Graphics.FromImage(img);
            graphic.CompositingQuality = CompositingQuality.HighQuality;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.DrawImage(baseImage, 0, 0, wt, ht);
            img.Save(Newname);

            img.Dispose();
            graphic.Dispose();
            baseImage.Dispose();

        }
        #endregion
    }
}