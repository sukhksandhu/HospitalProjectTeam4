namespace HospitalProject_Team4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appointmenttable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentID = c.Int(nullable: false, identity: true),
                        PatientID = c.String(maxLength: 128),
                        appointmentdate = c.DateTime(nullable: false),
                        status = c.String(),
                    })
                .PrimaryKey(t => t.AppointmentID)
                .ForeignKey("dbo.AspNetUsers", t => t.PatientID)
                .Index(t => t.PatientID);
            
            AddColumn("dbo.AspNetUsers", "doctor_AppointmentID", c => c.Int());
            AddColumn("dbo.AspNetUsers", "patient_AppointmentID", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "doctor_AppointmentID");
            CreateIndex("dbo.AspNetUsers", "patient_AppointmentID");
            AddForeignKey("dbo.AspNetUsers", "doctor_AppointmentID", "dbo.Appointments", "AppointmentID");
            AddForeignKey("dbo.AspNetUsers", "patient_AppointmentID", "dbo.Appointments", "AppointmentID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "PatientID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "patient_AppointmentID", "dbo.Appointments");
            DropForeignKey("dbo.AspNetUsers", "doctor_AppointmentID", "dbo.Appointments");
            DropIndex("dbo.AspNetUsers", new[] { "patient_AppointmentID" });
            DropIndex("dbo.AspNetUsers", new[] { "doctor_AppointmentID" });
            DropIndex("dbo.Appointments", new[] { "PatientID" });
            DropColumn("dbo.AspNetUsers", "patient_AppointmentID");
            DropColumn("dbo.AspNetUsers", "doctor_AppointmentID");
            DropTable("dbo.Appointments");
        }
    }
}
