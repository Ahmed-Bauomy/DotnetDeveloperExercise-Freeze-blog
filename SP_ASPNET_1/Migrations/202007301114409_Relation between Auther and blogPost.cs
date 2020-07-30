namespace SP_ASPNET_1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelationbetweenAutherandblogPost : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BlogPost", "Author_AuthorID", "dbo.Author");
            DropIndex("dbo.BlogPost", new[] { "Author_AuthorID" });
            RenameColumn(table: "dbo.BlogPost", name: "Author_AuthorID", newName: "AuthorID");
            AlterColumn("dbo.BlogPost", "AuthorID", c => c.Int(nullable: false));
            CreateIndex("dbo.BlogPost", "AuthorID");
            AddForeignKey("dbo.BlogPost", "AuthorID", "dbo.Author", "AuthorID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BlogPost", "AuthorID", "dbo.Author");
            DropIndex("dbo.BlogPost", new[] { "AuthorID" });
            AlterColumn("dbo.BlogPost", "AuthorID", c => c.Int());
            RenameColumn(table: "dbo.BlogPost", name: "AuthorID", newName: "Author_AuthorID");
            CreateIndex("dbo.BlogPost", "Author_AuthorID");
            AddForeignKey("dbo.BlogPost", "Author_AuthorID", "dbo.Author", "AuthorID");
        }
    }
}
