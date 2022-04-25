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
    /// 首頁顯示一般文章
    /// </summary>
    public class HomepagenormalController : ApiController
    {
        KiruDb db = new KiruDb();
        /// <summary>
        /// 依類別搜尋一般文章(按照時間排列)
        /// </summary>
        /// <param name="articlecategoryId">類別ID</param>
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
            var datanormal = db.ArticleNormals.Where(m => m.ArticlecategoryId == articlecategoryId &&m.IsPush == true).
                Select(x => new
                {
                    artId = x.ID,
                    author = x.Member.Name,
                    authorPic = x.Member.MemberPicName,
                    username = x.Member.UserName,
                    introduction = x.Introduction,
                    title = x.Title,
                    articlecategoryId = x.ArticlecategoryId,
                    artArtlog = x.Articlecategory.Name,
                    main = x.Main,
                    isFree = x.IsFree,
                    messageCount = x.MessageNormals.Count,
                    lovecount = x.Lovecount,
                    artInitDate = x.InitDate
                }).ToList();
            
            int pagecount = datanormal.Count();
            
            if (nowpage == 1)
            {
                var result = datanormal.OrderByDescending(x => x.artInitDate).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = datanormal.Count,
                    data = result
                });
            }
            else
            {
                int page = (nowpage - 1) * showcount;
                var result = datanormal.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = datanormal.Count,
                    data = result
                });
            }
        }
        /// <summary>
        /// 依照時間搜尋一般文章
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

            var dataNormal = db.ArticleNormals.Where(m => m.InitDate >= datetime01 && m.InitDate < datetime02 && m.IsPush).
                Select(x => new
                {
                    artId = x.ID,
                    author = x.Member.Name,
                    authorPic = x.Member.MemberPicName,
                    username = x.Member.UserName,
                    introduction = x.Introduction,
                    title = x.Title,
                    articlecategoryId = x.ArticlecategoryId,
                    artArtlog = x.Articlecategory.Name,
                    main = x.Main,
                    isFree = x.IsFree,
                    messageCount = x.MessageNormals.Count,
                    lovecount = x.Lovecount,
                    artInitDate = x.InitDate
                }).ToList();
           
            if (nowpage == 1)
            {
                //排序依照日期
                var result = dataNormal.OrderByDescending(x => x.artInitDate).Take(showcount);
               
                return Ok(new
                {
                    success = true,
                    total = dataNormal.Count(),
                    data = result
                });
            }
            else
            {
                int page = (nowpage - 1) * showcount;
                //排序依照日期
                var result = dataNormal.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);

                return Ok(new
                {
                    success = true,
                    total = dataNormal.Count(),
                    data = result
                });
            }
        }
        /// <summary>
        /// 最新的一般文章
        /// </summary>
        /// <param name="showcount">一頁顯示幾筆</param>
        /// <param name="nowpage">現在頁數</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult NewArticle(int nowpage, int showcount)
        {

            var dataNormal = db.ArticleNormals.Where(x => x.IsPush == true).Select(x => new
            {
                artId = x.ID,
                author = x.Member.Name,
                authorPic = x.Member.MemberPicName,
                username = x.Member.UserName,
                introduction = x.Introduction,
                title = x.Title,
                articlecategoryId = x.ArticlecategoryId,
                artArtlog = x.Articlecategory.Name,
                main = x.Main,
                isFree = x.IsFree,
                messageCount = x.MessageNormals.Count,
                lovecount = x.Lovecount,
                artInitDate = x.InitDate
            }).ToList();
           
            if (nowpage == 1)
            {
                //排序依照日期 desending遞減
                var result = dataNormal.OrderByDescending(x => x.artInitDate).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = dataNormal.Count,
                    data = result
                });
            }
            else
            {
                int page = (nowpage - 1) * showcount;
                var result = dataNormal.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = dataNormal.Count,
                    data = result
                });
            }
        }
        /// <summary>
        /// 最熱門的一般文章
        /// </summary>
        /// <param name="nowpage">現在頁數(預設1)</param>
        /// <param name="showcount">一頁顯示幾筆</param>
        /// <returns></returns>
        [Route("api/Homepagenormal/lovenormalarticle")]
        [HttpGet]
        public IHttpActionResult Lovearticle(int nowpage, int showcount)
        {
            var dataNormal = db.ArticleNormals.Where(x => x.IsPush == true && x.Lovecount > 0).
                Select(x => new
            {
                artId = x.ID,
                author = x.Member.Name,
                authorPic = x.Member.MemberPicName,
                username = x.Member.UserName,
                introduction = x.Introduction,
                title = x.Title,
                articlecategoryId = x.ArticlecategoryId,
                artArtlog = x.Articlecategory.Name,
                main = x.Main,
                isFree = x.IsFree,
                messageCount = x.MessageNormals.Count,
                lovecount = x.Lovecount,
                artInitDate = x.InitDate
            }).ToList();
           
            if (nowpage == 1)
            {
                //排序依照日期 desending遞減
                var result = dataNormal.OrderByDescending(x => x.lovecount).Take(showcount);

                return Ok(new
                {
                    success = true,
                    total = dataNormal.Count,
                    data = result
                });
            }
            else
            {
                int page = (nowpage - 1) * showcount;
                var result = dataNormal.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = dataNormal.Count,
                    data = result
                });
            }
        }
        /// <summary>
        /// 依關鍵字搜尋一般文章
        /// </summary>
        /// <param name="keywords">關鍵字</param>
        /// <param name="nowpage">現在的頁數(一開始就是第一頁,請填1)</param>
        /// <param name="showcount">顯示筆數</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Seekstringarticle(string keywords, int nowpage, int showcount)
        {
            if (string.IsNullOrWhiteSpace(keywords))
            {
                return Ok(new
                {
                    sucess = false,
                    message = "請輸入關鍵字"
                });
            }
          
            var dataNormal = db.ArticleNormals.Where(x => x.Title.Contains(keywords) && x.IsPush == true)
                .Select(x => new
                {
                    artId = x.ID,
                    author = x.Member.Name,
                    authorPic = x.Member.MemberPicName,
                    username = x.Member.UserName,
                    introduction = x.Introduction,
                    title = x.Title,
                    articlecategoryId = x.ArticlecategoryId,
                    artArtlog = x.Articlecategory.Name,
                    main = x.Main,
                    isFree = x.IsFree,
                    messageCount = x.MessageNormals.Count,
                    lovecount = x.Lovecount,
                    artInitDate = x.InitDate
                }).ToList();

            if (nowpage == 1)
            {
                //排序依照日期 desending遞減
                var result = dataNormal.OrderByDescending(x => x.artInitDate).Take(showcount);

                return Ok(new
                {
                    success = true,
                    total = dataNormal.Count,
                    data = result
                });
            }
            else
            {
                int takepage = (nowpage - 1) * showcount;
                var resultdata = dataNormal.OrderByDescending(x => x.artInitDate).Skip(takepage).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = dataNormal.Count,
                    data = resultdata
                });
            }
        }
    }
}
