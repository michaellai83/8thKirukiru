namespace Kiru8th.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNew : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articlecategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ArticleNormals",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MemberId = c.Int(nullable: false),
                        Title = c.String(),
                        Introduction = c.String(),
                        Main = c.String(),
                        ArticlecategoryId = c.Int(nullable: false),
                        IsFree = c.Boolean(nullable: false),
                        IsPush = c.Boolean(nullable: false),
                        InitDate = c.DateTime(nullable: false),
                        Lovecount = c.Int(nullable: false),
                        IsManangerStop = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Articlecategories", t => t.ArticlecategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Members", t => t.MemberId, cascadeDelete: true)
                .Index(t => t.MemberId)
                .Index(t => t.ArticlecategoryId);
            
            CreateTable(
                "dbo.CollectsNormals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MemberId = c.Int(nullable: false),
                        ArticleNormalId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ArticleNormals", t => t.ArticleNormalId, cascadeDelete: true)
                .Index(t => t.ArticleNormalId);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 200),
                        PassWord = c.String(nullable: false),
                        PasswordSalt = c.String(nullable: false),
                        Name = c.String(nullable: false, maxLength: 200),
                        Email = c.String(nullable: false, maxLength: 200),
                        ArticlecategoryId = c.Int(nullable: false),
                        Isidentify = c.Boolean(nullable: false),
                        Emailidentify = c.String(),
                        initDate = c.DateTime(nullable: false),
                        MemberPicName = c.String(),
                        Introduction = c.String(),
                        Opencollectarticles = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MemberId = c.Int(nullable: false),
                        FirstPicName = c.String(),
                        Title = c.String(),
                        IsFree = c.Boolean(nullable: false),
                        Introduction = c.String(),
                        ArticlecategoryId = c.Int(nullable: false),
                        IsPush = c.Boolean(nullable: false),
                        InitDate = c.DateTime(nullable: false),
                        Lovecount = c.Int(nullable: false),
                        FinalText = c.String(),
                        IsManangerStop = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Articlecategories", t => t.ArticlecategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Members", t => t.MemberId, cascadeDelete: true)
                .Index(t => t.MemberId)
                .Index(t => t.ArticlecategoryId);
            
            CreateTable(
                "dbo.ArticleMains",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PicName = c.String(),
                        Main = c.String(),
                        ArticleId = c.Int(nullable: false),
                        InDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .Index(t => t.ArticleId);
            
            CreateTable(
                "dbo.Collects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MemberId = c.Int(nullable: false),
                        ArticleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .Index(t => t.ArticleId);
            
            CreateTable(
                "dbo.FinalMissions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ArticleId = c.Int(nullable: false),
                        Title = c.String(),
                        Main = c.String(),
                        InitDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .Index(t => t.ArticleId);
            
            CreateTable(
                "dbo.Firstmissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PicName = c.String(),
                        FirstItem = c.String(),
                        ItemNumber = c.String(),
                        ArticleId = c.Int(nullable: false),
                        InitDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .Index(t => t.ArticleId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        ArticleId = c.Int(nullable: false),
                        Main = c.String(),
                        InitDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .Index(t => t.ArticleId);
            
            CreateTable(
                "dbo.ReMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageId = c.Int(nullable: false),
                        Main = c.String(),
                        InitDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Messages", t => t.MessageId, cascadeDelete: true)
                .Index(t => t.MessageId);
            
            CreateTable(
                "dbo.Orderlists",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Ordernumber = c.String(),
                        MemberID = c.Int(nullable: false),
                        AuthorName = c.String(),
                        Amount = c.Int(nullable: false),
                        Issuccess = c.Boolean(nullable: false),
                        InitDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Members", t => t.MemberID, cascadeDelete: true)
                .Index(t => t.MemberID);
            
            CreateTable(
                "dbo.Subscriptionplans",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MemberID = c.Int(nullable: false),
                        Introduction = c.String(),
                        Amount = c.String(),
                        InitDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Members", t => t.MemberID, cascadeDelete: true)
                .Index(t => t.MemberID);
            
            CreateTable(
                "dbo.MessageNormals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        ArticleNorId = c.Int(nullable: false),
                        Main = c.String(),
                        InitDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ArticleNormals", t => t.ArticleNorId, cascadeDelete: true)
                .Index(t => t.ArticleNorId);
            
            CreateTable(
                "dbo.ReMessageNormals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageNorId = c.Int(nullable: false),
                        Main = c.String(),
                        InitDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MessageNormals", t => t.MessageNorId, cascadeDelete: true)
                .Index(t => t.MessageNorId);
            
            CreateTable(
                "dbo.BackArticles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BackmemberID = c.Int(nullable: false),
                        BackMemberPic = c.String(),
                        Title = c.String(),
                        Titlepic = c.String(),
                        Main = c.String(),
                        IniDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Backmembers", t => t.BackmemberID, cascadeDelete: true)
                .Index(t => t.BackmemberID);
            
            CreateTable(
                "dbo.Backmembers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Salt = c.String(),
                        Name = c.String(),
                        Email = c.String(),
                        Photo = c.String(),
                        IniDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.BackQAs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Answer = c.String(),
                        InitDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BackArticles", "BackmemberID", "dbo.Backmembers");
            DropForeignKey("dbo.ReMessageNormals", "MessageNorId", "dbo.MessageNormals");
            DropForeignKey("dbo.MessageNormals", "ArticleNorId", "dbo.ArticleNormals");
            DropForeignKey("dbo.Subscriptionplans", "MemberID", "dbo.Members");
            DropForeignKey("dbo.Orderlists", "MemberID", "dbo.Members");
            DropForeignKey("dbo.ReMessages", "MessageId", "dbo.Messages");
            DropForeignKey("dbo.Messages", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.Articles", "MemberId", "dbo.Members");
            DropForeignKey("dbo.Firstmissions", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.FinalMissions", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.Collects", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.ArticleMains", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.Articles", "ArticlecategoryId", "dbo.Articlecategories");
            DropForeignKey("dbo.ArticleNormals", "MemberId", "dbo.Members");
            DropForeignKey("dbo.CollectsNormals", "ArticleNormalId", "dbo.ArticleNormals");
            DropForeignKey("dbo.ArticleNormals", "ArticlecategoryId", "dbo.Articlecategories");
            DropIndex("dbo.BackArticles", new[] { "BackmemberID" });
            DropIndex("dbo.ReMessageNormals", new[] { "MessageNorId" });
            DropIndex("dbo.MessageNormals", new[] { "ArticleNorId" });
            DropIndex("dbo.Subscriptionplans", new[] { "MemberID" });
            DropIndex("dbo.Orderlists", new[] { "MemberID" });
            DropIndex("dbo.ReMessages", new[] { "MessageId" });
            DropIndex("dbo.Messages", new[] { "ArticleId" });
            DropIndex("dbo.Firstmissions", new[] { "ArticleId" });
            DropIndex("dbo.FinalMissions", new[] { "ArticleId" });
            DropIndex("dbo.Collects", new[] { "ArticleId" });
            DropIndex("dbo.ArticleMains", new[] { "ArticleId" });
            DropIndex("dbo.Articles", new[] { "ArticlecategoryId" });
            DropIndex("dbo.Articles", new[] { "MemberId" });
            DropIndex("dbo.CollectsNormals", new[] { "ArticleNormalId" });
            DropIndex("dbo.ArticleNormals", new[] { "ArticlecategoryId" });
            DropIndex("dbo.ArticleNormals", new[] { "MemberId" });
            DropTable("dbo.BackQAs");
            DropTable("dbo.Backmembers");
            DropTable("dbo.BackArticles");
            DropTable("dbo.ReMessageNormals");
            DropTable("dbo.MessageNormals");
            DropTable("dbo.Subscriptionplans");
            DropTable("dbo.Orderlists");
            DropTable("dbo.ReMessages");
            DropTable("dbo.Messages");
            DropTable("dbo.Firstmissions");
            DropTable("dbo.FinalMissions");
            DropTable("dbo.Collects");
            DropTable("dbo.ArticleMains");
            DropTable("dbo.Articles");
            DropTable("dbo.Members");
            DropTable("dbo.CollectsNormals");
            DropTable("dbo.ArticleNormals");
            DropTable("dbo.Articlecategories");
        }
    }
}
