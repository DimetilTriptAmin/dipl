namespace dipl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Player : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        AccountId = c.Int(nullable: false, identity: true),
                        UserType = c.Int(nullable: false),
                        Image = c.Binary(),
                        User_Username = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AccountId)
                .ForeignKey("dbo.Users", t => t.User_Username)
                .Index(t => t.User_Username);
            
            CreateTable(
                "dbo.Playlists",
                c => new
                    {
                        PlaylistId = c.Int(nullable: false, identity: true),
                        AccountId = c.Int(nullable: false),
                        Name = c.String(),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => new { t.AccountId, t.PlaylistId })
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.Audios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsLiked = c.Boolean(nullable: false),
                        Image = c.Binary(),
                        Name = c.String(),
                        SourceUrl = c.String(),
                        Playlist_AccountId = c.Int(),
                        Playlist_PlaylistId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Playlists", t => new { t.Playlist_AccountId, t.Playlist_PlaylistId })
                .Index(t => new { t.Playlist_AccountId, t.Playlist_PlaylistId });
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 128),
                        PasswordHash = c.String(),
                    })
                .PrimaryKey(t => t.Username);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "User_Username", "dbo.Users");
            DropForeignKey("dbo.Playlists", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.Audios", new[] { "Playlist_AccountId", "Playlist_PlaylistId" }, "dbo.Playlists");
            DropIndex("dbo.Audios", new[] { "Playlist_AccountId", "Playlist_PlaylistId" });
            DropIndex("dbo.Playlists", new[] { "AccountId" });
            DropIndex("dbo.Accounts", new[] { "User_Username" });
            DropTable("dbo.Users");
            DropTable("dbo.Audios");
            DropTable("dbo.Playlists");
            DropTable("dbo.Accounts");
        }
    }
}
