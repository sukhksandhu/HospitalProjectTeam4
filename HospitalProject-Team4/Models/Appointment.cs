using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HospitalProject_Team4.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HospitalProject_Team4.Models
{
    public class Appointment
    {
        //appointment id 
        [Key]
        [Column(Order = 0)]
        public int AppointmentID { get; set; }
        //patient--application user 

        [ForeignKey("ApplicationUser"), Column(Order = 1)]
        public string PatientID { get; set; }

        //doctor-- application user

        [Column(Order = 2)]
        public string DoctorID { get; set; }

        [ForeignKey("PatientID, DoctorID")]
        public virtual ApplicationUser ApplicationUser { get; set; }


        //date and time
        public DateTime appointmentdate { get; set; }
       
        //status
        public string appointmentstatus
        {
            get;
            set;
        }
    }
}