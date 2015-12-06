namespace WebCookbook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecipeColumnInRating : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ratings", "Recipe_RecipeId", "dbo.Recipes");
            DropIndex("dbo.Ratings", new[] { "Recipe_RecipeId" });
            AlterColumn("dbo.Ratings", "Recipe_RecipeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Ratings", "Recipe_RecipeId");
            AddForeignKey("dbo.Ratings", "Recipe_RecipeId", "dbo.Recipes", "RecipeId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ratings", "Recipe_RecipeId", "dbo.Recipes");
            DropIndex("dbo.Ratings", new[] { "Recipe_RecipeId" });
            AlterColumn("dbo.Ratings", "Recipe_RecipeId", c => c.Int());
            CreateIndex("dbo.Ratings", "Recipe_RecipeId");
            AddForeignKey("dbo.Ratings", "Recipe_RecipeId", "dbo.Recipes", "RecipeId");
        }
    }
}
