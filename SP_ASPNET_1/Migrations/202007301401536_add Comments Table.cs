namespace SP_ASPNET_1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCommentsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        body = c.String(),
                        dateTime = c.DateTime(nullable: false),
                        BlogPostID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BlogPost", t => t.BlogPostID, cascadeDelete: true)
                .Index(t => t.BlogPostID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comment", "BlogPostID", "dbo.BlogPost");
            DropIndex("dbo.Comment", new[] { "BlogPostID" });
            DropTable("dbo.Comment");
        }
    }
}
