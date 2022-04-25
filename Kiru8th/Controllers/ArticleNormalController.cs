using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Kiru8th.JWT;
using Kiru8th.Models;
using Kiru8th.Models.PutData;
using Kiru8th.Models.Swagger;
using Kiru8th.Swagger;

namespace Kiru8th.Controllers
{
    /// <summary>
    /// 一般文章的API
    /// </summary>
    public class ArticleNormalController : ApiController
    {
        KiruDb db = new KiruDb();
        /// <summary>
        /// 添加一般文章
        /// </summary>
        /// <param name="data">回傳給後端的格式</param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthFilter]
        public IHttpActionResult CreatArticleNormal(DataArticleNormal data)
        {
            var username = data.userName;
            var checkusername = db.Members.FirstOrDefault(m => m.UserName == username);
            if (checkusername == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此帳號"
                });
            }

            ArticleNormal article = new ArticleNormal();
            article.MemberId = checkusername.ID;
            article.Introduction = data.introduction;
            article.Title = data.title;
            article.Main = data.main;
            article.ArticlecategoryId = data.articlecategoryId;
            article.IsFree = data.isFree;
            article.IsPush = data.isPush;
            article.InitDate = DateTime.Now;
            db.ArticleNormals.Add(article);
            db.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "已新增文章",
                artId = article.ID
            });
        }
        /// <summary>
        /// 找到作者自己的所有一般文章
        /// </summary>
        /// <param name="isPush">是否發布(判斷是否在草稿)</param>
        /// <param name="nowpage">現在頁數(預設1)</param>
        /// <param name="showcount">一頁顯示幾筆資料</param>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthFilter]
        public IHttpActionResult GetUserAllArticleNormal(bool isPush, int nowpage, int showcount)
        {
            var username = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var memeberData = db.Members.FirstOrDefault(m => m.UserName == username);
            if (memeberData == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此作者"
                });
            }
            /*var data = db.ArticleNormals.Where(m => m.MemberId == memeberData.ID && m.IsPush == isPush)*/
            var data = memeberData.ArticleNormals.Where( m =>m.IsPush == isPush).Select(x =>
                new
                {
                    artId = x.ID,
                    username = x.Member.UserName,
                    author = x.Member.Name,
                    authorPic = x.Member.MemberPicName,
                    introduction = x.Introduction,
                    title = x.Title,
                    main = x.Main,
                    artArtlog = x.Articlecategory.Name,
                    articlecategoryId = x.ArticlecategoryId,
                    isFree = x.IsFree,
                    messageCount = x.MessageNormals.Count,
                    lovecount = x.Lovecount,
                    artInitDate = x.InitDate

                }).ToList();


            var total = data.Count;
            if (nowpage == 1)
            {
                var outPutData = data.OrderByDescending(x => x.artInitDate).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = outPutData
                });
            }
            else
            {
                var page = (nowpage - 1) * showcount;
                var outPutData = data.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = outPutData
                });
            }

        }
        /// <summary>
        /// 刪除作者的文章
        /// </summary>
        /// <param name="artId">要刪除的文章ID</param>
        /// <returns></returns>
        [Route("api/ArticleNormal/DeleteArticleNormal")]
        [HttpDelete]
        [JwtAuthFilter]
        public IHttpActionResult DeleteArticleNormal(int artId)
        {
            var username = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var data = db.ArticleNormals.FirstOrDefault(m => m.ID == artId);
            var dataUser = data.Member.UserName;
            if (dataUser != username)
            {
                return Ok(new
                {
                    success = false,
                    message = "你沒有權限"
                });
            }
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "文章ID錯誤"
                });
            }

            db.ArticleNormals.Remove(data);
            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "已刪除"
            });
        }
        /// <summary>
        /// 更改一般文章
        /// </summary>
        /// <param name="artId">文章的ID</param>
        /// <param name="data">更改過後的資料</param>
        /// <returns></returns>
        [HttpPut]
        [JwtAuthFilter]
        public IHttpActionResult EditArticleNormal(int artId, DataArticleNormal data)
        {
            var havedata = db.ArticleNormals.FirstOrDefault(m => m.ID == artId);
            if (havedata == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "文章ID錯誤"
                });
            }

            var checkuserName = havedata.Member.UserName;
            var jwtUserName = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            if (checkuserName != jwtUserName)
            {
                return Ok(new
                {
                    success = false,
                    message = "你沒有權限"
                });
            }

            var editdata = db.ArticleNormals.Where(x => x.ID == artId);
            foreach (var q in editdata)
            {
                q.Title = data.title;
                q.Introduction = data.introduction;
                q.Main = data.main;
                q.ArticlecategoryId = data.articlecategoryId;
                q.IsFree = data.isFree;
                q.IsPush = data.isPush;

            }

            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "已完成編輯"
            });
        }
        /// <summary>
        /// 一般文章按愛心
        /// </summary>
        /// <param name="artId">文章id</param>
        /// <param name="putlove">是否按愛心</param>
        /// <returns></returns>
        [HttpPut]
        [JwtAuthFilter]
        public IHttpActionResult AddLoveArticleNormal(int artId, bool putlove)
        {
            var data = db.ArticleNormals.FirstOrDefault(x => x.ID == artId);
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此文章"
                });
            }

            if (putlove == true)
            {
                data.Lovecount++;

                db.SaveChanges();
            }
            else
            {
                data.Lovecount--;
                db.SaveChanges();
            }

            return Ok(new
            {
                success = true,
                lovecount = data.Lovecount
            });
        }
        /// <summary>
        /// 取得一般文章頁面所需資訊
        /// </summary>
        /// <param name="artId">文章ID</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetArticleNormal(int? artId)
        {
            var data = db.ArticleNormals.Where(m => m.ID == artId).Select(x => new
            {
                artId = x.ID,
                username = x.Member.UserName,
                author = x.Member.Name,
                authorPic = x.Member.MemberPicName,
                introduction = x.Introduction,
                title = x.Title,
                articlecategoryId = x.ArticlecategoryId,
                artArtlog = x.Articlecategory.Name,
                main = x.Main,
                isFree = x.IsFree,
                isPush = x.IsPush,
                messageCount = x.MessageNormals.Count,
                lovecount = x.Lovecount,
                artInitDate = x.InitDate
            }).FirstOrDefault();

            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "查無此文章"
                });
            }

            return Ok(new
            {
                success = true,
                data = data
            });
        }
        /// <summary>
        /// 編輯用一般文章資訊
        /// </summary>
        /// <param name="artId">文章ID</param>
        /// <returns></returns>
        [Route("api/ArticleNormal/GetNormalArticle")]
        [ResponseType(typeof(NormalArticleOutPutForEdit))]
        [HttpGet]
        [JwtAuthFilter]
        public IHttpActionResult GetNormalArticleForEdit(int? artId)
        {
            var data = db.ArticleNormals.Where(m => m.ID == artId).Select(x => new
            {
                title = x.Title,
                introduction = x.Introduction,
                main = x.Main,
                articlecategoryId = x.ArticlecategoryId,
                artInitDate = x.InitDate,
                isFree = x.IsFree,
                isPush = x.IsPush,
            }).FirstOrDefault();

            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "查無此文章"
                });
            }

            var checkusername = db.ArticleNormals.FirstOrDefault(x => x.ID == artId).Member.UserName;
            var jwtUsrname = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            if (checkusername != jwtUsrname)
            {
                return Ok(new
                {
                    suceess = false,
                    message = "你沒有權限"
                });
            }


            return Ok(new
            {
                success = true,
                data = data

            });
        }
        /// <summary>
        /// 一般文章的留言
        /// </summary>
        /// <param name="artId">文章ID</param>
        /// <param name="main">留言內容</param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthFilter]
        public IHttpActionResult AddmessageArticleNrmal(int artId, string main)
        {
            var username = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var userId = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            var data = db.ArticleNormals.FirstOrDefault(m => m.ID == artId);
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "無此文章"
                });
            }
            MessageNormal message = new MessageNormal();
            message.UserName = username;
            message.ArticleNorId = artId;
            message.UserName = username;
            message.Main = main;
            message.InitDate = DateTime.Now;
            db.MessageNormals.Add(message);
            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "留言成功"
            });
        }
        /// <summary>
        /// 回覆一般文章留言
        /// </summary>
        /// <param name="messageId">留言ID</param>
        /// <param name="main">回覆留言的內容</param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthFilter]
        public IHttpActionResult Addremessage(int messageId, string main)
        {
            var username = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var data = db.MessageNormals.FirstOrDefault(m => m.Id == messageId);
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "無此留言"
                });
            }
            var dataArtID = data.ArticleNorId;
            var datausername = db.ArticleNormals.FirstOrDefault(x => x.ID == dataArtID).Member.UserName;
            if (username != datausername)
            {
                return Ok(new
                {
                    success = false,
                    message = "你沒有權限"
                });
            }

            ReMessageNormal rMessage = new ReMessageNormal();
            rMessage.MessageNorId = messageId;
            rMessage.Main = main;
            rMessage.InitDate = DateTime.Now;
            db.ReMessageNormals.Add(rMessage);
            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "已回覆"
            });
        }
        /// <summary>
        /// 找到一般文章所有的留言
        /// </summary>
        /// <param name="artId">文章ID</param>
        /// <param name="nowpage">現在頁數(預設1)</param>
        /// <param name="showcount">一頁顯示幾筆資料</param>
        /// <returns></returns>
        [Route("api/ArticleNormal/GetAllmessage")]
        [HttpGet]
        public IHttpActionResult Getmessage(int artId, int nowpage, int showcount)
        {
            var data = db.MessageNormals.Where(m => m.ArticleNorId == artId).Join(db.Members, a => a.UserName,
                b => b.UserName, (a, b) => new
                {
                    messageId = a.Id,
                    messageUserName = b.UserName,
                    messageMember = b.Name,
                    messageMemberPic = b.MemberPicName,
                    messageMain = a.Main,
                    messageInitDate = a.InitDate,
                    reMessageData = a.ReMessageNormals.Select(x=>new
                    {
                        reMessageId = x.Id,
                        userName = x.MessageNormals.ArticleNormals.Member.UserName,
                        author = x.MessageNormals.ArticleNormals.Member.Name,
                        authorPic = x.MessageNormals.ArticleNormals.Member.MemberPicName,
                        reMessageMain = x.Main,
                        reMessageInitDate = x.InitDate
                    }).OrderByDescending(x => x.reMessageInitDate)
                }).ToList();
          
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此文章"
                });
            }


            var total = data.Count;
            if (nowpage == 1)
            {
                var result = data.OrderByDescending(x => x.messageInitDate).Take(showcount);
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
                var result = data.OrderByDescending(x => x.messageInitDate).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = result
                });
            }
        }
        /// <summary>
        /// 取得一般文章一筆留言(含大頭貼))
        /// </summary>
        /// <param name="messageId">留言ID</param>
        /// <returns></returns>
        [Route("api/ArticleNormal/Getmessage")]
        [HttpGet]
        [ResponseType(typeof(OutPutMessage))]
        public IHttpActionResult GetOneMessage(int messageId)
        {
            var data = db.MessageNormals.Where(m => m.Id == messageId).
                Join(db.Members,a=>a.UserName,b=>b.UserName,(a,b)=>new
            {
                messageUserName = b.UserName,
                messageMember = b.Name,
                messageMemberPic = b.MemberPicName,
                messageMain = a.Main,
                messageInitDate = a.InitDate
            }).FirstOrDefault();
            
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此留言"
                });
            }


            return Ok(new
            {
                success = true,
                data = data
            });
        }
        /// <summary>
        /// 作者刪除留言
        /// </summary>
        /// <param name="messageId">留言ID</param>
        /// <returns></returns>
        [HttpDelete]
        [JwtAuthFilter]
        public IHttpActionResult DeleteMessage(int messageId)
        {
            var user = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var data = db.MessageNormals.FirstOrDefault(x => x.Id == messageId);
            var dataId = data.ArticleNorId;
            var artmember = db.ArticleNormals.FirstOrDefault(x => x.ID == dataId).Member.UserName;
            if (user != artmember)
            {
                return Ok(new
                {
                    success = false,
                    message = "你沒有權限刪除留言"
                });
            }

            db.MessageNormals.Remove(data);
            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "刪除留言"
            });

        }
        /// <summary>
        /// 作者刪除回覆
        /// </summary>
        /// <param name="reMessageId">回覆ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/ArticleNormal/DeleteReMessage")]
        [JwtAuthFilter]
        public IHttpActionResult DeleteReMessage(int reMessageId)
        {
            var user = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var data = db.ReMessageNormals.FirstOrDefault(x => x.Id == reMessageId);
            var messaageId = data.MessageNorId;
            var dataId = db.MessageNormals.FirstOrDefault(x => x.Id == messaageId).ArticleNorId;
            var artmember = db.ArticleNormals.FirstOrDefault(x => x.ID == dataId).Member.UserName;
            if (user != artmember)
            {
                return Ok(new
                {
                    success = false,
                    message = "你沒有權限"
                });
            }

            db.ReMessageNormals.Remove(data);
            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "刪除留言"
            });

        }
        /// <summary>
        /// 取得此留言回覆
        /// </summary>
        /// <param name="messageId">留言id</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(OutPutReMessage))]
        public IHttpActionResult Getrmessage(int messageId)
        {
            var data = db.ReMessageNormals.Where(x => x.Id == messageId).
                Select(x => new
            {
                reMessageId = x.Id,
                userName = x.MessageNormals.ArticleNormals.Member.UserName,
                author = x.MessageNormals.ArticleNormals.Member.Name,
                authorPic = x.MessageNormals.ArticleNormals.Member.MemberPicName,
                reMessageMain = x.Main,
                reMessageInitDate = x.InitDate
            }).ToList();
              
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "無此留言"
                });
            }



            return Ok(new
            {
                success = true,
                data = data
            });
        }
        /// <summary>
        /// 收藏一般文章
        /// </summary>
        /// <param name="artId">文章ID</param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthFilter]
        public IHttpActionResult Collectarticle(int artId)
        {
            var memberid = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            var datArticle = db.ArticleNormals.FirstOrDefault(x => x.ID == artId);

            if (datArticle == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此文章"
                });
            }

            CollectsNormal collectsNormal = new CollectsNormal();
            collectsNormal.ArticleNormalId = artId;
            collectsNormal.MemberId = memberid;
            db.CollectsNormals.Add(collectsNormal);
            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "以收藏"
            });

        }
        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="artId">文章ID</param>
        /// <returns></returns>
        [Route("api/ArticleNormal/Deletecollect")]
        [HttpDelete]
        [JwtAuthFilter]
        public IHttpActionResult Deletecollect(int artId)
        {
            var userid = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            var data = db.ArticleNormals.FirstOrDefault(x => x.ID == artId);
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此文章"
                });
            }

            var collect = db.CollectsNormals.FirstOrDefault(x => x.MemberId == userid && x.ArticleNormalId == artId);
            db.CollectsNormals.Remove(collect);
            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "以取消收藏"
            });
        }
        /// <summary>
        /// 取得會員收藏的一般文章
        /// </summary>
        /// <param name="nowpage">現在頁數(預設為1)</param>
        /// <param name="showcount">一頁顯示幾筆</param>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthFilter]
        public IHttpActionResult GetAllcollectart(int nowpage, int showcount)
        {
            var memberid = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            
            var data = db.CollectsNormals.Where(m => m.MemberId == memberid).Select(x => new
            {
                artId = x.NormalArticles.ID,
                author = x.NormalArticles.Member.Name,
                authorPic = x.NormalArticles.Member.MemberPicName,
                username = x.NormalArticles.Member.UserName,
                title = x.NormalArticles.Title,
                main = x.NormalArticles.Main,
                introduction = x.NormalArticles.Introduction,
                artArtlog = x.NormalArticles.Articlecategory.Name,
                articlecategoryId = x.NormalArticles.ArticlecategoryId,
                isFree = x.NormalArticles.IsFree,
                lovecount = x.NormalArticles.Lovecount,
                messageCount = x.NormalArticles.MessageNormals.Count,
                artInitDate = x.NormalArticles.InitDate
            }).ToList();
           

            int total = data.Count;
            if (nowpage == 1)
            {
                var result = data.OrderByDescending(x => x.artInitDate).Take(showcount);
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
                //排序依照日期
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
        /// 依照類別取得相關一般文章
        /// </summary>
        /// <param name="articlecategoryId">文章類別ID</param>
        /// <param name="nowpage">現在頁數(預設為1)</param>
        /// <param name="showcount">一頁顯示幾筆</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetArtlogNormalArticle(int articlecategoryId, int nowpage, int showcount)
        {
         
            var data = db.ArticleNormals.Where(m => m.ArticlecategoryId == articlecategoryId && m.IsPush == true).
                Select(x => new
                {
                    artId = x.ID,
                    author = x.Member.Name,
                    authorPic = x.Member.MemberPicName,
                    username = x.Member.UserName,
                    title = x.Title,
                    main = x.Main,
                    introduction = x.Introduction,
                    artArtlog = x.Articlecategory.Name,
                    articlecategoryId = x.ArticlecategoryId,
                    isFree = x.IsFree,
                    lovecount = x.Lovecount,
                    messageCount = x.MessageNormals.Count,
                    artInitDate = x.InitDate
                }).ToList();
           
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有相關文章"
                });
            }

            int total = data.Count;
            if (nowpage == 1)
            {
                var outPut = data.OrderByDescending(x => x.artInitDate).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = outPut
                });
            }
            else
            {
                int page = (nowpage - 1) * showcount;
                var outPut = data.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = outPut
                });
            }

        }

        /// <summary>
        /// 修改留言
        /// </summary>
        /// <param name="messageId">留言ID</param>
        /// <param name="main">修改後內容</param>
        /// <returns></returns>
        [Route("api/ArticleNormal/EditMessage")]
        [JwtAuthFilter]
        [HttpPut]
        public IHttpActionResult EditMessage(int messageId, string main)
        {
            var messageData = db.MessageNormals.FirstOrDefault(x => x.Id == messageId);
            if (messageData == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此留言"
                });
            }
            var memberUserName = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var checkUserName = messageData.UserName;
            if (checkUserName != memberUserName)
            {
                return Ok(new
                {
                    success = false,
                    message = "你沒有權限"
                });
            }

            messageData.Main = main;
            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "已修改留言"
            });
        }
        /// <summary>
        /// 修改回覆
        /// </summary>
        /// <param name="reMessageId">回覆的ID</param>
        /// <param name="main">修改的內容</param>
        /// <returns></returns>
        [Route("api/ArticleNormal/EditReMessage")]
        [JwtAuthFilter]
        [HttpPut]
        public IHttpActionResult EditReMessage(int reMessageId, string main)
        {
            var reData = db.ReMessageNormals.FirstOrDefault(x => x.Id == reMessageId);
            if (reData == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此回覆"
                });
            }

            var checkMessageId = reData.MessageNorId;
            var checkArticleId = db.MessageNormals.FirstOrDefault(x => x.Id == checkMessageId).ArticleNorId;
            var checkUserName = db.ArticleNormals.FirstOrDefault(x => x.ID == checkArticleId).Member.UserName;
            var memberUserName = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            if (checkUserName != memberUserName)
            {
                return Ok(new
                {
                    suceess = false,
                    message = "你沒有權限"
                });
            }

            reData.Main = main;

            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "已修改回覆"
            });
        }
    }
}
