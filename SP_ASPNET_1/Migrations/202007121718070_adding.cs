namespace SP_ASPNET_1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adding : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Visitor",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IPAdress = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.VisitorBlogPost",
                c => new
                    {
                        Visitor_ID = c.Int(nullable: false),
                        BlogPost_BlogPostID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Visitor_ID, t.BlogPost_BlogPostID })
                .ForeignKey("dbo.Visitor", t => t.Visitor_ID, cascadeDelete: true)
                .ForeignKey("dbo.BlogPost", t => t.BlogPost_BlogPostID, cascadeDelete: true)
                .Index(t => t.Visitor_ID)
                .Index(t => t.BlogPost_BlogPostID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VisitorBlogPost", "BlogPost_BlogPostID", "dbo.BlogPost");
            DropForeignKey("dbo.VisitorBlogPost", "Visitor_ID", "dbo.Visitor");
            DropIndex("dbo.VisitorBlogPost", new[] { "BlogPost_BlogPostID" });
            DropIndex("dbo.VisitorBlogPost", new[] { "Visitor_ID" });
            DropTable("dbo.VisitorBlogPost");
            DropTable("dbo.Visitor");
        }
    }
}
