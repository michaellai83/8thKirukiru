using System;
using System.Data.Entity;
using System.Linq;

namespace Kiru8th.Models
{
    public class KiruDb : DbContext
    {
        // 您的內容已設定為使用應用程式組態檔 (App.config 或 Web.config)
        // 中的 'KiruDb' 連接字串。根據預設，這個連接字串的目標是
        // 您的 LocalDb 執行個體上的 'Kiru8th.Models.KiruDb' 資料庫。
        // 
        // 如果您的目標是其他資料庫和 (或) 提供者，請修改
        // 應用程式組態檔中的 'KiruDb' 連接字串。
        public KiruDb()
            : base("name=KiruDb")
        {
        }

        // 針對您要包含在模型中的每種實體類型新增 DbSet。如需有關設定和使用
        // Code First 模型的詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=390109。

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<ArticleNormal> ArticleNormals { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Articlecategory> Articlecategory { get; set; }
        public virtual DbSet<Firstmission> Firstmissions { get; set; }
        public virtual DbSet<ArticleMain> ArticleMains { get; set; }
        public virtual DbSet<FinalMission> FinalMissions { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<MessageNormal> MessageNormals { get; set; }
        public virtual DbSet<ReMessage> ReMessages { get; set; }
        public virtual DbSet<ReMessageNormal> ReMessageNormals { get; set; }
        public virtual DbSet<Subscriptionplan> Subscriptionplans { get; set; }
        public virtual DbSet<Orderlist> Orderlists { get; set; }
        public virtual DbSet<Collect> Collects { get; set; }
        public virtual DbSet<CollectsNormal> CollectsNormals { get; set; }
        public virtual DbSet<Backmember> Backmembers { get; set; }
        public virtual DbSet<BackArticle> BackArticles { get; set; }
        public virtual DbSet<BackQA> BackQas { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}