namespace WebCookbook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rating : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        RatingId = c.Int(nullable: false, identity: true),
                        Like = c.Boolean(nullable: false),
                        User_Id = c.String(maxLength: 128),
                        Recipe_RecipeId = c.Int(),
                    })
                .PrimaryKey(t => t.RatingId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .ForeignKey("dbo.Recipes", t => t.Recipe_RecipeId)
                .Index(t => t.User_Id)
                .Index(t => t.Recipe_RecipeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ratings", "Recipe_RecipeId", "dbo.Recipes");
            DropForeignKey("dbo.Ratings", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Ratings", new[] { "Recipe_RecipeId" });
            DropIndex("dbo.Ratings", new[] { "User_Id" });
            DropTable("dbo.Ratings");
        }
    }
}
