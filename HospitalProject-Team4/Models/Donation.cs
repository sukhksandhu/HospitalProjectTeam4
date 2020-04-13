using HospitalProject_Team4.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospitalProject_Team4.Models
{
    public class Donation
    {
        [Key, ForeignKey("ApplicationUser")]
        public string DonationID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        

        public string donor_mail { get; set; }
        public int donation_amount { get; set; }
        public DateTime donated_on { get; set; }
    }
}