using Kiru8th.JWT;
using Kiru8th.Models;
using Kiru8th.Models.Swagger;
using Kiru8th.PutData;
using Kiru8th.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Kiru8th.Controllers
{
    /// <summary>
    /// 切切的文章
    /// </summary>
    public class ArticleController : ApiController
    {
        KiruDb db = new KiruDb();
        /// <summary>
        /// 添加切切文章
        /// </summary>
        /// <param name="dataArticle ">切切前端傳進來資料</param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthFilter]
        public IHttpActionResult AddArticle([FromBody] DataArticle dataArticle)
        {
            var userId = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            var username = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            //var username = dataArticle.memberUserName;
            if (string.IsNullOrWhiteSpace(dataArticle.firstPhoto) || string.IsNullOrWhiteSpace(dataArticle.title) || dataArticle.articlecategoryId == 0)
            {
                return Ok(new
                {
                    success = false,
                    message = "請上傳首圖和填寫標題和填寫分類"
                });
            }
            var memberdata = db.Members.FirstOrDefault(x => x.UserName == username);

            Article article = new Article();
            article.MemberId = userId;
            article.Title = dataArticle.title;
            article.FirstPicName = dataArticle.firstPhoto;
            article.Introduction = dataArticle.introduction;
            article.ArticlecategoryId = dataArticle.articlecategoryId;
            article.InitDate = DateTime.Now;
            article.IsFree = dataArticle.isFree;
            article.IsPush = dataArticle.isPush;
            article.Lovecount = 0;
            article.FinalText = dataArticle.final;
            db.Articles.Add(article);
            db.SaveChanges();
            //存完直接回傳ID
            var artId = article.ID;

            foreach (var data in dataArticle.fArrayList)
            {
                Firstmission firstmission = new Firstmission();
                firstmission.ArticleId = artId;
                firstmission.PicName = data.secPhoto;
                firstmission.FirstItem = data.mission;
                firstmission.InitDate = DateTime.Now;
                db.Firstmissions.Add(firstmission);

            }

            foreach (var data in dataArticle.mArrayList)
            {
                ArticleMain articleMain = new ArticleMain();
                articleMain.ArticleId = artId;
                articleMain.PicName = data.thirdPhoto;
                articleMain.Main = data.main;
                articleMain.InDateTime = DateTime.Now;
                db.ArticleMains.Add(articleMain);
            }

            foreach (var data in dataArticle.fMissionList)
            {
                FinalMission finalmission = new FinalMission();
                finalmission.Title = data.auxiliary;
                finalmission.Main = data.auxiliarymain;
                finalmission.ArticleId = artId;
                finalmission.InitDateTime = DateTime.Now;
                db.FinalMissions.Add(finalmission);
            }

            db.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "新增成功",
                artId = artId
            });

        }
        /// <summary>
        /// 訪客留言
        /// </summary>
        /// <param name="Main">留言內容</param>
        /// <param name="artId">文章ID</param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthFilter]
        public IHttpActionResult AddMessage(string Main, int artId)
        {
            var UserName = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);

            var UserID = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            var data = db.Articles.FirstOrDefault(x => x.ID == artId);
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "查無此文章"
                });
            }
            else
            {
                Message message = new Message();
                message.UserName = UserName;
                message.ArticleId = artId;
                message.Main = Main;
                message.InitDate = DateTime.Now;
                db.Messages.Add(message);
                db.SaveChanges();
                return Ok(new
                {
                    success = true,
                    message = "已留言"
                });
            }

        }

        /// <summary>
        /// 取得此篇文章所有留言資料
        /// </summary>
        /// <param name="artId">文章Id</param>
        /// <param name="nowpage">現在頁數(預設1)</param>
        /// <param name="showcount">一頁顯示幾筆</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAllMessage(int artId, int nowpage, int showcount)
        {

            var data = db.Messages.Where(m => m.ArticleId == artId).
                Join(db.Members, a => a.UserName, b => b.UserName, (a, b) => new
                {
                    messageId = a.Id,
                    messageUserName = a.UserName,
                    messageMember = b.Name,
                    messageMemberPic = b.MemberPicName,
                    messageMain = a.Main,
                    messageInitDate = a.InitDate,
                    reMessageData = a.ReMessages.Select(x => new
                    {
                        reMessageId = x.Id,
                        userName = x.Messages.Articles.Member.UserName,
                        author = x.Messages.Articles.Member.Name,
                        authorPic = x.Messages.Articles.Member.MemberPicName,
                        reMessageMain = x.Main,
                        reMessageInitDate = x.InitDate
                    })

                }).ToList();
            if (data != null)
            {
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
            else
            {
                return Ok(new
                {
                    success = false,
                    message = "查無此留言"
                });

            }

        }
        /// <summary>
        /// 取得留言資料(單筆)(含大頭貼)
        /// </summary>
        /// <param name="messageId">留言的ID</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(OutPutMessage))]
        public IHttpActionResult Getmessage(int messageId)
        {
            var data = db.Messages.Where(x => x.Id == messageId).
                Join(db.Members, a => a.UserName, b => b.UserName, (a, b) => new
                {
                    messageUserName = a.UserName,
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
        /// 回覆留言
        /// </summary>
        /// <param name="messageId">留言的ID</param>
        /// <param name="main">留言內容</param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthFilter]
        public IHttpActionResult AddReMessage(int messageId, string main)
        {
            var data = db.Messages.FirstOrDefault(x => x.Id == messageId);
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "查無此留言"
                });

            }
            int checkartId = data.ArticleId;
            var artData = db.Articles.FirstOrDefault(x => x.ID == checkartId).Member.UserName;
            var memberUsername = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);

            if (artData != memberUsername)
            {
                return Ok(new
                {
                    success = false,
                    message = "你沒有權限"
                });
            }

            ReMessage rMessage = new ReMessage();
            rMessage.MessageId = messageId;
            rMessage.Main = main;
            rMessage.InitDate = DateTime.Now;
            db.ReMessages.Add(rMessage);
            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "已回覆留言"
            });

        }

        /// <summary>
        /// 取得留言回覆的內容
        /// </summary>
        /// <param name="messageId">留言的ID</param>
        /// <returns></returns>
        [Route("api/Article/GetReMessage")]
        [ResponseType(typeof(OutPutReMessage))]
        [HttpGet]
        public IHttpActionResult GetReMessage(int messageId)
        {
            var data = db.ReMessages.Where(m => m.MessageId == messageId).Select(x => new
            {
                reMessageId = x.Id,
                userName = x.Messages.Articles.Member.UserName,
                author = x.Messages.Articles.Member.Name,
                authorPic = x.Messages.Articles.Member.MemberPicName,
                reMessageMain = x.Main,
                reMessageInitDate = x.InitDate
            }).ToList();

            if (data.Count > 0)
            {
                return Ok(new
                {
                    success = true,
                    data = data
                });
            }
            else
            {
                return Ok(new
                {
                    success = false,
                    message = "查無此留言"
                });
            }

        }
        /// <summary>
        /// 取得文章資料
        /// </summary>
        /// <param name="artId">文章ID</param>
        /// <returns></returns>

        [HttpGet]
        [Route("api/Article/intoArticle")]
        [ResponseType(typeof(Kirukiruoutput))]
        public IHttpActionResult GetArctile(int artId)
        {

            var havdata = db.Articles.Where(x => x.ID == artId).Select(a => new
            {
                artId = a.ID,
                username = a.Member.UserName,
                authorPic = a.Member.MemberPicName,
                author = a.Member.Name,
                title = a.Title,
                firstPhoto = a.FirstPicName,
                introduction = a.Introduction,
                articlecategoryId = a.ArticlecategoryId,
                artArtlog = a.Articlecategory.Name,
                isFree = a.IsFree,
                isPush = a.IsPush,
                lovecount = a.Lovecount,
                artInitDate = a.InitDate,
                fArrayList = a.Firstmissions.Select(y => new
                {
                    fId = y.Id,
                    secPhoto = y.PicName,
                    mission = y.FirstItem,
                }),
                mArrayList = a.ArticleMains.Select(y => new
                {
                    mId = y.Id,
                    thirdPhoto = y.PicName,
                    main = y.Main
                }),
                fMissionList = a.FinalMissions.Select(y => new
                {
                    fId = y.ID,
                    auxiliary = y.Title,
                    auxiliarymain = y.Main
                }),
                final = a.FinalText
            }).FirstOrDefault();

            if (havdata == null)
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
                data = havdata
            });
        }
        /// <summary>
        /// 編輯文章
        /// </summary>
        /// <param name="editkirukiru">編輯文章前端回傳的檔案</param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthFilter]
        [Route("EditArticle")]
        public IHttpActionResult EditArticle([FromBody] Editkirukiru editkirukiru)
        {
            var artId = editkirukiru.artId;
            var otpicdata = db.Articles.FirstOrDefault(m => m.ID == artId);
            if (otpicdata == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "無此篇文章"
                });
            }

            var checkUserName = otpicdata.Member.UserName;
            var jwtusername = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            if (checkUserName != jwtusername)
            {
                return Ok(new
                {
                    success = false,
                    message = "你沒有權限"
                });
            }

            string picpath = "~/Pic/";
            //刪除封面照片


            //刪除資料夾的圖片
            string picname = otpicdata.FirstPicName;
            string savpath = System.Web.HttpContext.Current.Server.MapPath($"~/Pic/{picname}");

            File.Delete(savpath);
            //db.FirstPics.Remove(otpicdata);
            //刪除前置步驟
            var OFpicary = db.Firstmissions.Where(m => m.ArticleId == artId).ToList();
            foreach (var pic in OFpicary)
            {
                int picid = pic.Id;
                var fdata = db.Firstmissions.FirstOrDefault(m => m.Id == picid);
                picname = fdata.PicName;
                if (picname.Equals("."))
                {
                    db.Firstmissions.Remove(fdata);
                }
                else
                {
                    savpath = System.Web.HttpContext.Current.Server.MapPath($"~/Pic/{picname}");
                    File.Delete(savpath);
                    db.Firstmissions.Remove(fdata);
                }

            }
            //刪除切切
            var OMpicary = db.ArticleMains.Where(m => m.ArticleId == artId).ToList();
            foreach (var str in OMpicary)
            {
                int picid = str.Id;
                var mdata = db.ArticleMains.FirstOrDefault(m => m.Id == picid);
                picname = mdata.PicName;
                if (picname.Equals("."))
                {
                    db.ArticleMains.Remove(mdata);
                }
                else
                {
                    savpath = System.Web.HttpContext.Current.Server.MapPath($"~/Pic/{picname}");
                    File.Delete(savpath);
                    db.ArticleMains.Remove(mdata);
                }

            }
            //刪除附屬任務
            var oFinalMission = db.FinalMissions.Where(m => m.ArticleId == artId).ToList();
            foreach (var str in oFinalMission)
            {
                int oFinalId = str.ID;
                var odata = db.FinalMissions.FirstOrDefault(x => x.ID == oFinalId);
                db.FinalMissions.Remove(odata);
            }

            db.SaveChanges();
            var username = editkirukiru.userName;
            if (string.IsNullOrWhiteSpace(editkirukiru.firstPhoto) || string.IsNullOrWhiteSpace(editkirukiru.title))
            {
                return Ok(new
                {
                    success = false,
                    message = "請上傳首圖和填寫標題"
                });
            }
            var artTitlePic = editkirukiru.firstPhoto.Split('.');
            string titlePicName = artTitlePic[0];
            string titleFileName = artTitlePic[1];

            var q = db.Articles.Where(p => p.ID == artId);
            //var q = from p in db.Articles where p.ID == artId select p;
            foreach (var p in q)
            {
                p.Title = editkirukiru.title;
                p.FirstPicName = titlePicName;
                p.Introduction = editkirukiru.introduction;
                p.ArticlecategoryId = editkirukiru.articlecategoryId;
                p.IsFree = editkirukiru.isFree;
                p.IsPush = editkirukiru.isPush;
                p.FinalText = editkirukiru.final;
            }

            db.SaveChanges();


            var firstMission = editkirukiru.fArrayList.ToList();
            var kiruMain = editkirukiru.mArrayList.ToList();
            var finalMission = editkirukiru.fMissionList.ToList();

            foreach (var data in firstMission)
            {
                string picName = "";
                string picFileName = "";
                if (string.IsNullOrWhiteSpace(data.secPhoto))
                {
                    Firstmission firstmission = new Firstmission();
                    firstmission.ArticleId = artId;
                    firstmission.PicName = picName;
                    firstmission.FirstItem = data.mission;
                    firstmission.InitDate = DateTime.Now;
                    db.Firstmissions.Add(firstmission);

                }
                else
                {
                    var PicName = data.secPhoto.Split('.');
                    picName = PicName[0];
                    picFileName = PicName[1];
                    Firstmission firstmission = new Firstmission();
                    firstmission.ArticleId = artId;
                    firstmission.PicName = picName;
                    firstmission.FirstItem = data.mission;
                    firstmission.InitDate = DateTime.Now;
                    db.Firstmissions.Add(firstmission);
                }

            }

            foreach (var data in kiruMain)
            {
                string picName = "";
                string picFileName = "";
                if (string.IsNullOrWhiteSpace(data.thirdPhoto))
                {
                    ArticleMain articleMain = new ArticleMain();
                    articleMain.ArticleId = artId;
                    articleMain.PicName = picName;
                    articleMain.Main = data.main;
                    articleMain.InDateTime = DateTime.Now;
                    db.ArticleMains.Add(articleMain);
                }
                else
                {
                    var kiruPhoto = data.thirdPhoto.Split('.');
                    picName = kiruPhoto[0];
                    picFileName = kiruPhoto[1];
                    ArticleMain articleMain = new ArticleMain();
                    articleMain.ArticleId = artId;
                    articleMain.PicName = picName;
                    articleMain.Main = data.main;
                    articleMain.InDateTime = DateTime.Now;
                    db.ArticleMains.Add(articleMain);
                }



            }

            foreach (var data in finalMission)
            {
                FinalMission finalmission = new FinalMission();
                finalmission.Title = data.auxiliary;
                finalmission.Main = data.auxiliarymain;
                finalmission.ArticleId = artId;
                finalmission.InitDateTime = DateTime.Now;
                db.FinalMissions.Add(finalmission);
            }


            db.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "修改成功"
            });
        }
        /// <summary>
        /// 編輯用回傳文章資料
        /// </summary>
        /// <param name="artId">文章ID</param>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthFilter]
        [Route("api/Article/Geteditarticle")]
        [ResponseType(typeof(KiruOutPutForEdit))]
        public IHttpActionResult GetEditArticle(int artId)
        {
            var jwtusername = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var havdata = db.Articles.Where(m => m.ID == artId).Select(x => new
            {
                artId = x.ID,
                title = x.Title,
                firstPhoto = x.FirstPicName,
                introduction = x.Introduction,
                articlecategoryId = x.ArticlecategoryId,
                artArtlog = x.Articlecategory.Name,
                fArrayList = x.Firstmissions.Select(y => new
                {
                    fId = y.Id,
                    secPhoto = y.PicName,
                    mission = y.FirstItem,
                }),
                mArrayList = x.ArticleMains.Select(y => new
                {
                    mId = y.Id,
                    thirdPhoto = y.PicName,
                    main = y.Main
                }),
                fMissionList = x.FinalMissions.Select(y => new
                {
                    fId = y.ID,
                    auxiliary = y.Title,
                    auxiliarymain = y.Main
                }),
                isFree = x.IsFree,
                isPush = x.IsPush,
                artInitDate = x.InitDate,
                final = x.FinalText
            }).FirstOrDefault();
            if (havdata == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此文章"
                });
            }

            var checkusername = db.Articles.FirstOrDefault(m => m.ID == artId).Member.UserName;

            if (checkusername != jwtusername)
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
                data = havdata
            });

        }
        /// <summary>
        /// 刪除文章
        /// </summary>
        /// <param name="artId">文章ID</param>
        /// <returns></returns>
        [Route("api/Article/DeleteArticle")]
        [HttpDelete]
        [JwtAuthFilter]
        public IHttpActionResult DeleteActile(int artId)
        {
            var ArtData = db.Articles.FirstOrDefault(m => m.ID == artId);
            if (ArtData == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "查無此文章"
                });
            }

            var checkUsername = ArtData.Member.UserName;
            var jwtUsername = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            if (checkUsername != jwtUsername)
            {
                return Ok(new
                {
                    success = false,
                    message = "你沒有權限"
                });
            }

            var ArtFirstMission = ArtData.Firstmissions.ToList();
            if (ArtFirstMission != null)
            {
                foreach (var str in ArtFirstMission)
                {
                    int FirstID = str.Id;
                    var FirstPic = db.Firstmissions.FirstOrDefault(m => m.Id == FirstID);
                    db.Firstmissions.Remove(FirstPic);

                }
            }

            var ArtMain = ArtData.ArticleMains.ToList();
            if (ArtMain != null)
            {
                foreach (var str in ArtMain)
                {
                    int MainPicID = str.Id;
                    var MainPic = db.ArticleMains.FirstOrDefault(m => m.Id == MainPicID);
                    db.ArticleMains.Remove(MainPic);
                }
            }

            var ArtMessage = ArtData.Messages.ToList();
            if (ArtMessage.Count > 0)
            {
                foreach (var message in ArtMessage)
                {
                    int id = message.Id;
                    var Rmessage = db.ReMessages.FirstOrDefault(m => m.MessageId == id);
                    var result = db.Messages.FirstOrDefault(m => m.Id == id);
                    db.ReMessages.Remove(Rmessage);
                    db.Messages.Remove(result);
                }
            }

            db.Articles.Remove(ArtData);
            db.SaveChanges();
         
            return Ok(new
            {
                success = true,
                message = "已刪除文章"
            });


        }
        /// <summary>
        /// 找到作者的所有切切
        /// </summary>
        /// <param name="ispush">是否發布(用來查詢是否在草稿</param>
        /// <param name="nowpage">現在頁數(預設1)</param>
        /// <param name="showcount">每頁顯示幾筆資料</param>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthFilter]
        public IHttpActionResult GetUserArticle(bool ispush, int nowpage, int showcount)
        {
            var username = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);

            var memberData = db.Members.FirstOrDefault(m => m.UserName == username);
            if (memberData == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此作者"
                });
            }

            var havedata = memberData.Articles.Where(x=>x.IsPush == ispush).Select(x => new
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
               


            int pagecount = havedata.Count;
            if (nowpage == 1)
            {
                var result = havedata.OrderByDescending(x => x.artInitDate).Take(showcount);

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

                var result = havedata.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = pagecount,
                    data = result
                });
            }

        }
        /// <summary>
        /// 按愛心
        /// </summary>
        /// <param name="artId">文章ID</param>
        /// <param name="putlove">是否按愛心</param>
        /// <returns></returns>
        [HttpPut]
        [JwtAuthFilter]
        public IHttpActionResult AddLoveArticle(int artId, bool putlove)
        {
            var data = db.Articles.FirstOrDefault(x => x.ID == artId);

            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "查無此文章"
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
        /// 收藏切切文章
        /// </summary>
        /// <param name="artId">文章ID</param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthFilter]
        public IHttpActionResult Collectarticle(int artId)
        {
            var memberid = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            var datArticle = db.Articles.FirstOrDefault(x => x.ID == artId);

            if (datArticle == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "查無此文章"
                });
            }

            Collect collect = new Collect();
            collect.ArticleId = artId;
            collect.MemberId = memberid;
            db.Collects.Add(collect);
            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "已收藏"
            });

        }
        /// <summary>
        /// 取消收藏文章
        /// </summary>
        /// <param name="artId">收藏文章的ID</param>
        /// <returns></returns>
        [Route("api/Article/Deletecollect")]
        [HttpDelete]
        [JwtAuthFilter]
        public IHttpActionResult Deletecollect(int artId)
        {
            var userid = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            var data = db.Articles.FirstOrDefault(x => x.ID == artId);
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "查無此文章"
                });
            }

            var collect = db.Collects.FirstOrDefault(x => x.MemberId == userid && x.ArticleId == artId);
            db.Collects.Remove(collect);
            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "已取消收藏"
            });
        }
        /// <summary>
        /// 取得會員收藏的切切文章
        /// </summary>
        /// <param name="nowpage">現在頁數(預設為1)</param>
        /// <param name="showcount">一頁顯示幾筆</param>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthFilter]
        public IHttpActionResult GetAllcollectart(int nowpage, int showcount)
        {
            var memberid = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            var data = db.Collects.Where(m => m.MemberId == memberid).Select(x => new
            {
                artId = x.Articles.ID,
                author = x.Articles.Member.Name,
                authorPic = x.Articles.Member.MemberPicName,
                username = x.Articles.Member.UserName,
                title = x.Articles.Title,
                firstPhoto = x.Articles.FirstPicName,
                introduction = x.Articles.Introduction,
                artArtlog = x.Articles.Articlecategory.Name,
                articlecategoryId = x.Articles.ArticlecategoryId,
                isFree = x.Articles.IsFree,
                lovecount = x.Articles.Lovecount,
                messageCount = x.Articles.Messages.Count,
                artInitDate = x.Articles.InitDate
            }).ToList();
           

            int total = data.Count;
            if (nowpage == 1)
            {
                var newArticles = data.OrderByDescending(x => x.artInitDate).Take(showcount);

                return Ok(new
                {
                    success = true,
                    total = total,
                    data = newArticles
                });
            }
            else
            {
                var page = (nowpage - 1) * showcount;
                var newArticles = data.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = newArticles
                });
            }

        }
        /// <summary>
        /// 依類別取得切切文章
        /// </summary>
        /// <param name="articlecategoryId">類別ID</param>
        /// <param name="nowpage">現在頁數(預設1)</param>
        /// <param name="showcount">一頁顯示幾筆</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(KiruArtLogFourOutPut))]
        public IHttpActionResult GetArtlogArticle(int articlecategoryId, int nowpage, int showcount)
        {
            var data = db.Articles.Where(x => x.ArticlecategoryId == articlecategoryId && x.IsPush == true)
                .Select(x => new
                {
                    artId = x.ID,
                    username = x.Member.UserName,
                    author = x.Member.Name,
                    authorPic = x.Member.MemberPicName,
                    title = x.Title,
                    firstPhoto = x.FirstPicName ,
                    introduction = x.Introduction,
                    artArtlog = x.Articlecategory.Name,
                    articlecategoryId = x.ArticlecategoryId,
                    isFree = x.IsFree,
                    lovecount = x.Lovecount,
                    messageCount = x.Messages.Count,
                    artInitDate = x.InitDate,

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
                var dataOutput = data.OrderByDescending(x => x.artInitDate).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = dataOutput
                });
            }
            else
            {
                int page = (nowpage - 1) * showcount;
                var dataOutput = data.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = dataOutput
                });
            }

        }
        /// <summary>
        /// 修改留言
        /// </summary>
        /// <param name="messageId">留言ID</param>
        /// <param name="main">修改後內容</param>
        /// <returns></returns>
        [Route("api/Article/EditMessage")]
        [JwtAuthFilter]
        [HttpPut]
        public IHttpActionResult EditMessage(int messageId, string main)
        {
            var messageData = db.Messages.FirstOrDefault(x => x.Id == messageId);
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
        [Route("api/Article/EditReMessage")]
        [JwtAuthFilter]
        [HttpPut]
        public IHttpActionResult EditReMessage(int reMessageId, string main)
        {
            var reData = db.ReMessages.FirstOrDefault(x => x.Id == reMessageId);
            if (reData == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此回覆"
                });
            }

            var checkMessageId = reData.MessageId;
            var checkArticleId = db.Messages.FirstOrDefault(x => x.Id == checkMessageId).ArticleId;
            var checkUserName = db.Articles.FirstOrDefault(x => x.ID == checkArticleId).Member.UserName;
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
        /// <summary>
        /// 作者刪除留言
        /// </summary>
        /// <param name="messageId">留言ID</param>
        /// <returns></returns>
        [Route("api/Article/DeleteMessage")]
        [HttpDelete]
        [JwtAuthFilter]
        public IHttpActionResult DeleteMessage(int messageId)
        {
            var user = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var data = db.Messages.FirstOrDefault(x => x.Id == messageId);
            var dataId = data.ArticleId;
            var artmember = db.Articles.FirstOrDefault(x => x.ID == dataId).Member.UserName;
            if (user != artmember)
            {
                return Ok(new
                {
                    success = false,
                    message = "你沒有權限刪除留言"
                });
            }

            db.Messages.Remove(data);
            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "刪除留言"
            });

        }
        /// <summary>
        /// 刪除切切作者回覆
        /// </summary>
        /// <param name="reMessageId">回覆ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/Article/DeleteReMessage")]
        [JwtAuthFilter]
        public IHttpActionResult DeleteReMessage(int reMessageId)
        {
            var user = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var data = db.ReMessages.FirstOrDefault(x => x.Id == reMessageId);
            var messaageId = data.MessageId;
            var dataId = db.Messages.FirstOrDefault(x => x.Id == messaageId).ArticleId;
            var artmember = db.Articles.FirstOrDefault(x => x.ID == dataId).Member.UserName;
            if (user != artmember)
            {
                return Ok(new
                {
                    success = false,
                    message = "你沒有權限"
                });
            }

            db.ReMessages.Remove(data);
            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "刪除留言"
            });

        }
    }
}
