namespace HospitalProject_Team4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appointmenttabledoctor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "DoctorID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointments", "DoctorID");
        }
    }
}
