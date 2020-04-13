using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HospitalProject_Team4.Data;
using HospitalProject_Team4.Migrations;
using HospitalProject_Team4.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace HospitalProject_Team4.Controllers
{
    public class AppointmentController : Controller
    {
        //appointment controller to add appointments by admin and edit and delete by admin
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private HospitalProjectContext db = new HospitalProjectContext();
        // GET: Appointment
        public ActionResult Index()
        {
            return View();
        }
        //this method select all appointments from the table and display, from the view edit and delete button is attached with this list
        public ActionResult List()
        {
            string query = "Select * from Appointments";
            List<Appointment> aps = db.Appointments.SqlQuery(query).ToList();

            return View(aps);
        }
        //add view to add the appointment view
        public ActionResult Add()
        {
            return View();
        }
        //post method form update view reaches here on submit update appointment button
        [HttpPost]
        public async Task<ActionResult> AddAppointment(string Username, string Userpass, string Useremail, string phoneNumber, string DoctorID, string PatientID, DateTime appointmentdate, string appointmentstatus)
        {
            Debug.WriteLine("reached inside add appointment method");
            ApplicationUser NewUser = new ApplicationUser();
            NewUser.UserName = Username;
            NewUser.Email = Useremail;
            NewUser.PhoneNumber = phoneNumber;
           
           
                string Id = NewUser.Id; 
                string query = "INSERT INTO Appointments (appointmentdate, appointmentstatus) VALUES (@appointmentdate, @appointmentstatus)";
                SqlParameter[] sqlparams = new SqlParameter[2];
                sqlparams[0] = new SqlParameter("@appointmentdate", appointmentdate);
                sqlparams[1] = new SqlParameter("@appointmentstatus", appointmentstatus);

                db.Database.ExecuteSqlCommand(query, sqlparams);
                db.SaveChanges();
                return RedirectToAction("List");
            }

        //update method for admin for editing appointment date and appointment status
        public ActionResult Update(int id)
        {
            //select * from Appointments where AppointmentID = @id

            //adding by LINQ
            Appointment aps = db.Appointments.Find(id);

            return View(aps);

        }
        //post method for admin to update the entries into the database and save in the database returning to updated list
        [HttpPost]
        public ActionResult Update(int id, DateTime appointmentdate, string appointmentstatus)
        {
            //used LINQ 
            Appointment aps = db.Appointments.Find(id);
            aps.appointmentdate = appointmentdate;
            aps.appointmentstatus = appointmentstatus;

           // Debug.Write("inside update appointment reached");
            db.SaveChanges();

            return RedirectToAction("List");
        }

        //delete method for deleting appointment by admin
        
        public ActionResult Delete(int id)
        {

            string query = "delete from Appointments where AppointmentID = @appointmentID";
            SqlParameter sqlparams = new SqlParameter("@appointmentID", id);



            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

    }
}