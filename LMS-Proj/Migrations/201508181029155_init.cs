namespace LMS_Proj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Files", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Files", new[] { "ApplicationUserId" });
            AddColumn("dbo.Files", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Files", "Owner_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Files", "ApplicationUserId", c => c.String());
            CreateIndex("dbo.Files", "ApplicationUser_Id");
            CreateIndex("dbo.Files", "Owner_Id");
            AddForeignKey("dbo.Files", "Owner_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Files", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Files", "GroupId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Files", "GroupId", c => c.Int());
            DropForeignKey("dbo.Files", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Files", "Owner_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Files", new[] { "Owner_Id" });
            DropIndex("dbo.Files", new[] { "ApplicationUser_Id" });
            AlterColumn("dbo.Files", "ApplicationUserId", c => c.String(maxLength: 128));
            DropColumn("dbo.Files", "Owner_Id");
            DropColumn("dbo.Files", "ApplicationUser_Id");
            CreateIndex("dbo.Files", "ApplicationUserId");
            AddForeignKey("dbo.Files", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
