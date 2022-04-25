using Kiru8th.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kiru8th.Controllers
{
    /// <summary>
    /// 首頁切切的API
    /// </summary>
    public class HomepageController : ApiController
    {
        KiruDb db = new KiruDb();
        /// <summary>
        /// 依類別取切切文章(按照時間最新排列(有完成訂閱作者的話他的文章都會解鎖
        /// </summary>
        /// <param name="articlecategoryId">類別的數字值</param>
        /// <param name="nowpage">現在的頁數(一開始請填1)</param>
        /// <param name="showcount">要顯示幾筆資料</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult SeekArticle(int articlecategoryId, int nowpage, int showcount)
        {
            var artLog = db.Articlecategory.FirstOrDefault(x => x.Id == articlecategoryId);
            if (artLog == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "類別錯誤"
                });
            }
            var data = db.Articles.Where(m => m.ArticlecategoryId == articlecategoryId && m.IsPush == true).
                Select(x => new
            {
                artId = x.ID,
                author = x.Member.Name,
                authorPic = x.Member.MemberPicName,
                username = x.Member.UserName,
                title = x.Title,
                firstPhoto = x.FirstPicName ,
                introduction = x.Introduction,
                artArtlog = x.Articlecategory.Name,
                articlecategoryId = x.ArticlecategoryId,
                isFree = x.IsFree,
                lovecount = x.Lovecount,
                messageCount = x.Messages.Count,
                artInitDate = x.InitDate
            }).ToList();

            int pagecount = data.Count();

            if (nowpage == 1)
            {



                var result = data.OrderByDescending(x => x.artInitDate).Take(showcount);
                pagecount = data.Count();
                return Ok(new
                {
                    success = true,
                    total = pagecount,
                    data = result
                });
            }
            else
            {
                int page = (nowpage - 1) * showcount;
                //排序依照日期

                var result = data.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = pagecount,
                    data = result
                });
            }

        }

        /// <summary>
        /// 依照時間範圍搜尋切切文章
        /// </summary>
        /// <param name="datetime1">較早的時間(ex:2022.03.08)</param>
        /// <param name="datetime2">較晚的時間(ex:2022.03.10)</param>
        /// <param name="nowpage">現在頁數(一開始請直接傳1)</param>
        /// <param name="showcount">顯示幾筆資料</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult SeekTimeArticle(string datetime1, string datetime2, int nowpage, int showcount)
        {
            var datetime01 = DateTime.Parse(datetime1);
            var datetime02 = DateTime.Parse(datetime2).AddDays(1);
            var data = db.Articles.Where(m => m.InitDate >= datetime01 && m.InitDate < datetime02 && m.IsPush == true).
                Select(x => new
                {
                    artId = x.ID,
                    author = x.Member.Name,
                    authorPic = x.Member.MemberPicName,
                    username = x.Member.UserName,
                    title = x.Title,
                    firstPhoto = x.FirstPicName,
                    introduction = x.Introduction,
                    artArtlog = x.Articlecategory.Name,
                    articlecategoryId = x.ArticlecategoryId,
                    isFree = x.IsFree,
                    lovecount = x.Lovecount,
                    messageCount = x.Messages.Count,
                    artInitDate = x.InitDate
                }).ToList();

            int totalcount = data.Count();
        
            if (nowpage == 1)
            {
                //排序依照日期
                var result = data.OrderByDescending(x => x.artInitDate).Take(showcount);
               
                totalcount = data.Count();

                return Ok(new
                {
                    success = true,
                    total = totalcount,
                    data = result
                });
            }
            else
            {
                int page = (nowpage - 1) * showcount;
                //排序依照日期
                var result = data.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);

                return Ok(new
                {
                    success = true,
                    total = totalcount,
                    data = result
                });
            }
        }
        /// <summary>
        /// 最新的切切文章排列
        /// </summary>
        /// <param name="nowpage">現在頁數(預設1)</param>
        /// <param name="showcount">一頁顯示幾筆</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult NewArticle(int nowpage, int showcount)
        {
            var data = db.Articles.Where(m => m.IsPush == true).Select(x => new
            {
                artId = x.ID,
                author = x.Member.Name,
                authorPic = x.Member.MemberPicName,
                username = x.Member.UserName,
                title = x.Title,
                firstPhoto = x.FirstPicName,
                introduction = x.Introduction,
                artArtlog = x.Articlecategory.Name,
                articlecategoryId = x.ArticlecategoryId,
                isFree = x.IsFree,
                lovecount = x.Lovecount,
                messageCount = x.Messages.Count,
                artInitDate = x.InitDate
            }).ToList();

            var total = data.Count;
            if (nowpage == 1)
            {
                //排序依照日期 desending遞減
                //用Take表示拿取幾筆資料
                var result = data.OrderByDescending(x => x.artInitDate).Take(showcount);
                //另一種寫法
                //var result = from e in arrayList
                //    orderby e.InitDateTime descending 
                //    select e;
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = result
                });
            }
            else
            {
                var page = (nowpage - 1) * showcount;
                var result = data.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = result
                });
            }
        }
        /// <summary>
        /// 最熱門的切切文章排列
        /// </summary>
        /// <param name="nowpage">現在頁數(預設1)</param>
        /// <param name="showcount">一頁顯示幾筆</param>
        /// <returns></returns>
        [Route("api/Homepage/lovearticle")]
        [HttpGet]

        public IHttpActionResult Lovearticle(int nowpage, int showcount)
        {
            var data = db.Articles.Where(m => m.IsPush == true && m.Lovecount > 0).
                Select(x => new
            {
                artId = x.ID,
                author = x.Member.Name,
                authorPic = x.Member.MemberPicName,
                username = x.Member.UserName,
                title = x.Title,
                firstPhoto = x.FirstPicName,
                introduction = x.Introduction,
                artArtlog = x.Articlecategory.Name,
                articlecategoryId = x.ArticlecategoryId,
                isFree = x.IsFree,
                lovecount = x.Lovecount,
                messageCount = x.Messages.Count,
                artInitDate = x.InitDate
            }).ToList();

            int total = data.Count;
            if (nowpage == 1)
            {
                //排序依照日期 desending遞減
                var result = data.OrderByDescending(x => x.lovecount).Take(4);

                return Ok(new
                {
                    success = true,
                    total = total,
                    data = result
                });
            }
            else
            {
                int page = (nowpage - 1) * showcount;
                var result = data.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = result
                });
            }
        }

        /// <summary>
        /// 關鍵字搜尋切切文章標題
        /// </summary>
        /// <param name="keywords">關鍵字</param>
        /// <param name="nowpage">現在的頁數(一開始就是第一頁,請填1)</param>
        /// <param name="showcount">顯示筆數</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Seekstringarticle(string keywords, int nowpage, int showcount)
        {
            //多重查詢
            //var a = db.Articles.AsQueryable();
            //if (!string.IsNullOrWhiteSpace(keywords))
            //{
            //    a = a.Where(x => x.Title.Contains(keywords));

            //}
            //a = a.Where(x => x.InitDate >= DateTime.Today);
            //var b = a.ToList();
            if (string.IsNullOrWhiteSpace(keywords))
            {
                return Ok(new
                {
                    sucess = false,
                    message = "請輸入關鍵字"
                });
            }

            var dataarticle = db.Articles.Where(m => m.Title.Contains(keywords) && m.IsPush == true).
                Select(
                x => new
                {
                    artId = x.ID,
                    author = x.Member.Name,
                    authorPic = x.Member.MemberPicName,
                    username = x.Member.UserName,
                    title = x.Title,
                    firstPhoto = x.FirstPicName,
                    introduction = x.Introduction,
                    artArtlog = x.Articlecategory.Name,
                    articlecategoryId = x.ArticlecategoryId,
                    isFree = x.IsFree,
                    lovecount = x.Lovecount,
                    messageCount = x.Messages.Count,
                    artInitDate = x.InitDate
                }).ToList();

            if (nowpage == 1)
            {

                //排序依照日期 desending遞減
                var result = dataarticle.OrderByDescending(x => x.artInitDate).Take(showcount);

                return Ok(new
                {
                    success = true,
                    total = dataarticle.Count,
                    data = result
                });
            }
            else
            {
                int takepage = (nowpage - 1) * showcount;
                var resultdata = dataarticle.OrderByDescending(x => x.artInitDate).Skip(takepage).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = dataarticle.Count,
                    data = resultdata
                });
            }

        }
    }
}
