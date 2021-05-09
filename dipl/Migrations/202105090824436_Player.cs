﻿namespace dipl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Player : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Audios", "DurationTime", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Audios", "DurationTime", c => c.Time(nullable: false, precision: 7));
        }
    }
}
