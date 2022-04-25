namespace Kiru8th.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditBackMembers : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BackArticles", "Title", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.BackArticles", "Titlepic", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Backmembers", "Username", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Backmembers", "Password", c => c.String(nullable: false));
            AlterColumn("dbo.Backmembers", "Email", c => c.String(nullable: false, maxLength: 200));
            DropColumn("dbo.BackArticles", "BackMemberPic");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BackArticles", "BackMemberPic", c => c.String());
            AlterColumn("dbo.Backmembers", "Email", c => c.String());
            AlterColumn("dbo.Backmembers", "Password", c => c.String());
            AlterColumn("dbo.Backmembers", "Username", c => c.String());
            AlterColumn("dbo.BackArticles", "Titlepic", c => c.String());
            AlterColumn("dbo.BackArticles", "Title", c => c.String());
        }
    }
}
