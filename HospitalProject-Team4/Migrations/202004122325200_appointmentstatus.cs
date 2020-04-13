namespace HospitalProject_Team4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appointmentstatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "appointmentstatus", c => c.String());
            DropColumn("dbo.Appointments", "status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Appointments", "status", c => c.String());
            DropColumn("dbo.Appointments", "appointmentstatus");
        }
    }
}
