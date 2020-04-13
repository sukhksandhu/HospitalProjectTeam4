using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HospitalProject_Team4.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HospitalProject_Team4.Models
{
    public class FAQ
    {
       
        [Key]
        public int FAQId { get; set; }

        public string question { get; set; }
        public string answer { get; set; }
       

    }
}