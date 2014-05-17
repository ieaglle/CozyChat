namespace CozyChat.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatRooms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        IsCurrent = c.Boolean(nullable: false),
                        CreatorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorId, cascadeDelete: true)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 50),
                        RegisteredDate = c.DateTime(nullable: false),
                        LastSeenDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        SentDate = c.DateTime(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        ChatRoomId = c.Int(),
                        ReceiverId = c.Int(),
                        SenderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChatRooms", t => t.ChatRoomId)
                .ForeignKey("dbo.Users", t => t.ReceiverId)
                .ForeignKey("dbo.Users", t => t.SenderId, cascadeDelete: true)
                .Index(t => t.ChatRoomId)
                .Index(t => t.ReceiverId)
                .Index(t => t.SenderId);
            
            CreateTable(
                "dbo.ChatRoomUserMappings",
                c => new
                    {
                        ChatRoomId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ChatRoomId, t.UserId })
                .ForeignKey("dbo.ChatRooms", t => t.ChatRoomId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.ChatRoomId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChatRoomUserMappings", "UserId", "dbo.Users");
            DropForeignKey("dbo.ChatRoomUserMappings", "ChatRoomId", "dbo.ChatRooms");
            DropForeignKey("dbo.ChatRooms", "CreatorId", "dbo.Users");
            DropForeignKey("dbo.Messages", "SenderId", "dbo.Users");
            DropForeignKey("dbo.Messages", "ReceiverId", "dbo.Users");
            DropForeignKey("dbo.Messages", "ChatRoomId", "dbo.ChatRooms");
            DropIndex("dbo.ChatRoomUserMappings", new[] { "UserId" });
            DropIndex("dbo.ChatRoomUserMappings", new[] { "ChatRoomId" });
            DropIndex("dbo.Messages", new[] { "SenderId" });
            DropIndex("dbo.Messages", new[] { "ReceiverId" });
            DropIndex("dbo.Messages", new[] { "ChatRoomId" });
            DropIndex("dbo.ChatRooms", new[] { "CreatorId" });
            DropTable("dbo.ChatRoomUserMappings");
            DropTable("dbo.Messages");
            DropTable("dbo.Users");
            DropTable("dbo.ChatRooms");
        }
    }
}
