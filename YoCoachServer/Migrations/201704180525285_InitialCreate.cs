namespace YoCoachServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Client",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AllowLoginWithCode = c.Boolean(),
                        CodeToAccess = c.String(),
                        CodeCreatedAt = c.DateTimeOffset(precision: 7),
                        CreatedAt = c.DateTimeOffset(precision: 7),
                        UpdateAt = c.DateTimeOffset(precision: 7),
                        RemoveAt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.ClientCoach",
                c => new
                    {
                        ClientId = c.String(nullable: false, maxLength: 128),
                        CoachId = c.String(nullable: false, maxLength: 128),
                        NickName = c.String(),
                        Email = c.String(),
                        Birthday = c.DateTimeOffset(precision: 7),
                        Code = c.String(),
                        IsExpired = c.Boolean(),
                        ClientType = c.Int(),
                    })
                .PrimaryKey(t => new { t.ClientId, t.CoachId })
                .ForeignKey("dbo.Client", t => t.ClientId, cascadeDelete: true)
                .ForeignKey("dbo.Coach", t => t.CoachId, cascadeDelete: true)
                .Index(t => t.ClientId)
                .Index(t => t.CoachId);
            
            CreateTable(
                "dbo.Coach",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TimeToCancel = c.Int(),
                        IsVisibleForClients = c.Boolean(),
                        HasPenality = c.Boolean(),
                        CreatedAt = c.DateTimeOffset(precision: 7),
                        UpdateAt = c.DateTimeOffset(precision: 7),
                        RemoveAt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Gym",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Address = c.String(),
                        CreatedAt = c.DateTimeOffset(precision: 7),
                        UpdateAt = c.DateTimeOffset(precision: 7),
                        RemoveAt = c.DateTimeOffset(precision: 7),
                        Coach_Id = c.String(maxLength: 128),
                        Credit_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Coach", t => t.Coach_Id)
                .ForeignKey("dbo.Credit", t => t.Credit_Id)
                .Index(t => t.Coach_Id)
                .Index(t => t.Credit_Id);
            
            CreateTable(
                "dbo.Credit",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CreditPolicy = c.Int(),
                        UnitValue = c.Double(),
                        Amount = c.Double(),
                        ExpiresAt = c.DateTimeOffset(precision: 7),
                        DayOfPayment = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Invoice",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        PaidAt = c.DateTimeOffset(precision: 7),
                        AmountExpend = c.Double(),
                        CreatedAt = c.DateTimeOffset(precision: 7),
                        UpdateAt = c.DateTimeOffset(precision: 7),
                        RemoveAt = c.DateTimeOffset(precision: 7),
                        Credit_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Credit", t => t.Credit_Id)
                .Index(t => t.Credit_Id);
            
            CreateTable(
                "dbo.Schedule",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        StartTime = c.DateTimeOffset(precision: 7),
                        EndTime = c.DateTimeOffset(precision: 7),
                        TotalValue = c.Double(),
                        IsConfirmed = c.Boolean(),
                        PaymentState = c.Int(),
                        ScheduleState = c.Int(),
                        CreditsQuantity = c.Double(),
                        CreatedAt = c.DateTimeOffset(precision: 7),
                        UpdateAt = c.DateTimeOffset(precision: 7),
                        RemoveAt = c.DateTimeOffset(precision: 7),
                        Coach_Id = c.String(maxLength: 128),
                        Gym_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Coach", t => t.Coach_Id)
                .ForeignKey("dbo.Gym", t => t.Gym_Id)
                .Index(t => t.Coach_Id)
                .Index(t => t.Gym_Id);
            
            CreateTable(
                "dbo.ClientDebit",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Balance_Id = c.String(maxLength: 128),
                        Client_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Credit", t => t.Balance_Id)
                .ForeignKey("dbo.Client", t => t.Client_Id)
                .ForeignKey("dbo.Schedule", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.Balance_Id)
                .Index(t => t.Client_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Type = c.String(),
                        Picture = c.Binary(),
                        Birthday = c.DateTimeOffset(precision: 7),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Installation",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DeviceType = c.Int(nullable: false),
                        ApplicationVersion = c.String(),
                        Badge = c.Int(),
                        DeviceToken = c.String(),
                        DeviceId = c.String(),
                        Enabled = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(precision: 7),
                        UpdateAt = c.DateTimeOffset(precision: 7),
                        RemoveAt = c.DateTimeOffset(precision: 7),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.ScheduleClient",
                c => new
                    {
                        Schedule_Id = c.String(nullable: false, maxLength: 128),
                        Client_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Schedule_Id, t.Client_Id })
                .ForeignKey("dbo.Schedule", t => t.Schedule_Id, cascadeDelete: true)
                .ForeignKey("dbo.Client", t => t.Client_Id, cascadeDelete: true)
                .Index(t => t.Schedule_Id)
                .Index(t => t.Client_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Client", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Coach", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Installation", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Schedule", "Gym_Id", "dbo.Gym");
            DropForeignKey("dbo.Schedule", "Coach_Id", "dbo.Coach");
            DropForeignKey("dbo.ScheduleClient", "Client_Id", "dbo.Client");
            DropForeignKey("dbo.ScheduleClient", "Schedule_Id", "dbo.Schedule");
            DropForeignKey("dbo.ClientDebit", "Id", "dbo.Schedule");
            DropForeignKey("dbo.ClientDebit", "Client_Id", "dbo.Client");
            DropForeignKey("dbo.ClientDebit", "Balance_Id", "dbo.Credit");
            DropForeignKey("dbo.Gym", "Credit_Id", "dbo.Credit");
            DropForeignKey("dbo.Invoice", "Credit_Id", "dbo.Credit");
            DropForeignKey("dbo.Gym", "Coach_Id", "dbo.Coach");
            DropForeignKey("dbo.ClientCoach", "CoachId", "dbo.Coach");
            DropForeignKey("dbo.ClientCoach", "ClientId", "dbo.Client");
            DropIndex("dbo.ScheduleClient", new[] { "Client_Id" });
            DropIndex("dbo.ScheduleClient", new[] { "Schedule_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Installation", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.ClientDebit", new[] { "Client_Id" });
            DropIndex("dbo.ClientDebit", new[] { "Balance_Id" });
            DropIndex("dbo.ClientDebit", new[] { "Id" });
            DropIndex("dbo.Schedule", new[] { "Gym_Id" });
            DropIndex("dbo.Schedule", new[] { "Coach_Id" });
            DropIndex("dbo.Invoice", new[] { "Credit_Id" });
            DropIndex("dbo.Gym", new[] { "Credit_Id" });
            DropIndex("dbo.Gym", new[] { "Coach_Id" });
            DropIndex("dbo.Coach", new[] { "Id" });
            DropIndex("dbo.ClientCoach", new[] { "CoachId" });
            DropIndex("dbo.ClientCoach", new[] { "ClientId" });
            DropIndex("dbo.Client", new[] { "Id" });
            DropTable("dbo.ScheduleClient");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Installation");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.ClientDebit");
            DropTable("dbo.Schedule");
            DropTable("dbo.Invoice");
            DropTable("dbo.Credit");
            DropTable("dbo.Gym");
            DropTable("dbo.Coach");
            DropTable("dbo.ClientCoach");
            DropTable("dbo.Client");
        }
    }
}
