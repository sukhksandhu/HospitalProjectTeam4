using HospitalProject_Team4.Data;
using HospitalProject_Team4.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HospitalProject_Team4.Controllers
{
    public class DonationController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private HospitalProjectContext db = new HospitalProjectContext();
        // GET: Donation
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }
        //add donations
        [HttpPost]
        public async Task<ActionResult> Add(string Username, string phoneNumber, decimal donation_amount,  string Useremail, string Userpass)
        {
            Debug.WriteLine("Inside Post method add");

            ApplicationUser NewUser = new ApplicationUser();
            NewUser.UserName = Username;
            NewUser.Email = Useremail;
            NewUser.PhoneNumber = phoneNumber;
            //code interpreted from AccountController.cs Register Method
            IdentityResult result = await UserManager.CreateAsync(NewUser, Userpass);

            if (result.Succeeded)
            {
                string Id = NewUser.Id; //what was the id of the new account?
                string query = "INSERT INTO Donations (DonationID, donor_mail, donation_amount, donated_on) VALUES (@DonationID, @donor_mail, @donation_amount, @donated_on)";
                SqlParameter[] sqlparams = new SqlParameter[4];
                sqlparams[0] = new SqlParameter("@donor_mail", Useremail);
                sqlparams[1] = new SqlParameter("@donation_amount", (int)(donation_amount));
                sqlparams[2] = new SqlParameter("@donated_on",DateTime.Now);
                sqlparams[3] = new SqlParameter("@DonationID", Id);

                db.Database.ExecuteSqlCommand(query, sqlparams);
                db.SaveChanges();
                return RedirectToAction("List");
                /*

                Donation newbooking = new Donation();
                newbooking.DonationID = Id;
                newbooking.donated_on = DateTime.Now;
                newbooking.donation_amount = ;

                //first add booking
                db.Donation.Add(newbooking);*/

            }
            else
            {
                //Simple way of displaying errors
                ViewBag.ErrorMessage = "Something Went Wrong!";
                ViewBag.Errors = new List<string>();
                foreach (var error in result.Errors)
                {
                    ViewBag.Errors.Add(error);
                }
            }
            return View();
        }

        public ActionResult List(string searchkey)
        {

            Debug.WriteLine("search keyword is " + searchkey);
            
            if (searchkey != null && searchkey != "")
            {
                Debug.WriteLine("Finding in whole table : " + searchkey);
                Donation donation = new Donation();
                string query = "Select * from Donatons WHERE donor_mail = @donor_mail";
                 var parameter = new SqlParameter("@donor_mail", searchkey);

                db.Database.ExecuteSqlCommand(query, parameter);
                return View(donation);
            }
            else
            {
                string query = "select * from Donations inner join AspNetUsers on Donations.DonationId = AspNetUsers.Id";
                //SqlParameter param = new SqlParameter("@id", id);
                List<Donation> donation = db.Donation.SqlQuery(query).ToList();

                return View(donation);
            }
        }
        public ActionResult Show(string id)
        {
           
            string query = "select * from Donations inner join AspNetUsers on Donations.DonationID = AspNetUsers.id where AspNetUsers.id = @id";
            var parameter = new SqlParameter("@id", id);
            Debug.WriteLine("Query : " + query);

            Donation donation= db.Donation.SqlQuery(query, parameter).FirstOrDefault();


            return View(donation);
        }
        public ActionResult Update(string id)
        {
            string query = "select * from Donations inner join AspNetUsers on Donations.DonationID= AspNetUsers.id where AspNetUsers.id = @id";
            var parameter = new SqlParameter("@id", id);
            Donation donation= db.Donation.SqlQuery(query, parameter).FirstOrDefault();
            //VolunteerRecruitment volunteer = db.volunteerRecruitment.Find(id);

            return View(donation);

        }
        /*
        [HttpPost]
        public ActionResult Update(string id, string donat, string volunteerspecialization)
        {
            
            string query = "update Donations set volunteer_specialization=@volunteer_specialization, HasFile=@hasFile, volunteer_FileExtension=@volunteer_FileExtension where volunteer_id=@v_id";
            SqlParameter[] sqlparams = new SqlParameter[4];
            sqlparams[0] = new SqlParameter("@volunteer_FileExtension", volunteerFileExtension);
            sqlparams[1] = new SqlParameter("@hasFile", hasFileTemp);
            sqlparams[2] = new SqlParameter("@volunteer_specialization", volunteerspecialization);
            sqlparams[3] = new SqlParameter("@v_id", id);

            string query_aspnet = "update AspNetUsers set phoneNumber=@phoneNumber where Id=@aspnet_id";
            SqlParameter[] sqlparams_aspusers = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@phoneNumber", phoneNumber);
            sqlparams[1] = new SqlParameter("@aspnet_id", id);

            db.Database.ExecuteSqlCommand(query, sqlparams);

            //logic for updating the pet in the database goes here
            return RedirectToAction("List");
        }*/
        public ActionResult DeleteConfirm(string id)
        {
            string query = "select * from Donations inner join AspNetUsers on Donations.DonationId = AspNetUsers.id where AspNetUsers.id = @id";
            SqlParameter param = new SqlParameter("@id", id);
            Donation donation = db.Donation.SqlQuery(query, param).FirstOrDefault();

            return View(donation);
        }
        [HttpPost]
        public ActionResult Delete(string id)
        {
            string query = "delete from Donations where DonationID= @id";
            SqlParameter param = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, param);

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