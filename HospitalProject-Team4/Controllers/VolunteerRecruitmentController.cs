using HospitalProject_Team4.Data;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using HospitalProject_Team4.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data.SqlClient;
using System.IO;

namespace HospitalProject_Team4.Controllers
{
    public class VolunteerRecruitmentController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private HospitalProjectContext db = new HospitalProjectContext();
        // GET: VolunteerRecruitment
        public ActionResult Index(){
            return View();
        }

        public ActionResult Add(){
            return View();
        }
        //add volunteer 
        [HttpPost]
        public async Task<ActionResult> Add(string Username, string phoneNumber, string volunteerSpecialization, string Useremail, string Userpass)
        {
            Debug.WriteLine("Inside Post method add");
            //christine pet groomer for reference
            ApplicationUser NewUser = new ApplicationUser();
            NewUser.UserName = Username;
            NewUser.Email = Useremail;
            NewUser.PhoneNumber = phoneNumber;
            //code interpreted from AccountController.cs Register Method
            IdentityResult result = await UserManager.CreateAsync(NewUser, Userpass);

            if (result.Succeeded)
            {
                //we need to find the register user we just created -- get the ID of particular user
                string Id = NewUser.Id; //what was the id of the new account?
                //link this id to the Volunteer
                string volunteer_id = Id;
                string volunteer_status = "Waiting";

                VolunteerRecruitment NewVol = new VolunteerRecruitment();
                NewVol.volunteer_id = Id;
                NewVol.volunteer_specialization = volunteerSpecialization;
                NewVol.volunteer_status = volunteer_status;
               //LINQ
                //SQL equivalent : INSERT INTO volunteerRecruitment (volunteer_id, .. ) VALUES (@id..)
                db.volunteerRecruitment.Add(NewVol);

                db.SaveChanges();
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
        
        public ActionResult List(string volunteersearchkey)
        {

            Debug.WriteLine("search keyword is " + volunteersearchkey);

            if (volunteersearchkey != null && volunteersearchkey != "")
            {
                Debug.WriteLine("Finding in whole table : " + volunteersearchkey);
                List<VolunteerRecruitment> volunteers = db.volunteerRecruitment
                    .Where(volunteer =>
                        volunteer.volunteer_specialization.Contains(volunteersearchkey) ||
                        volunteer.volunteer_FileExtension.Contains(volunteersearchkey)
                    )
                    .ToList();
                return View(volunteers);
            }
            else
            {
                string query = "select * from volunteerRecruitments inner join AspNetUsers on volunteerRecruitments.volunteer_id= AspNetUsers.id";
                List<VolunteerRecruitment> volunteers = db.volunteerRecruitment.SqlQuery(query).ToList();
                

                return View(volunteers);
            }
        }
        public ActionResult Show(string id)
        {
            string query = "select * from volunteerRecruitments inner join AspNetUsers on volunteerRecruitments.volunteer_id= AspNetUsers.id where AspNetUsers.id = @id";
            var parameter = new SqlParameter("@id", id);
            Debug.WriteLine("--------------  "+ query);

            VolunteerRecruitment volunteer = db.volunteerRecruitment.SqlQuery(query,parameter).FirstOrDefault();
            return View(volunteer);
        }
        public ActionResult Update(string id)
        {
            string query = "select * from volunteerRecruitments inner join AspNetUsers on volunteerRecruitments.volunteer_id= AspNetUsers.id where AspNetUsers.id = @id";
            var parameter = new SqlParameter("@id", id);
            VolunteerRecruitment volunteer = db.volunteerRecruitment.SqlQuery(query, parameter).FirstOrDefault();

            return View(volunteer);

        }
        [HttpPost]
        public ActionResult Update(string id, string phoneNumber, string volunteerspecialization, HttpPostedFileBase VolunteerCV)
        {
            //Christine pet groomer for reference
            //assume that file is empty keeping the int value 0
            int hasFileTemp = 0;
            string volunteerFileExtension = "";
            //checking to see if some data is there
            if (VolunteerCV != null)
            {
                Debug.WriteLine("File found...");
                //checking to see if the file size is greater than 0bytes
                if (VolunteerCV.ContentLength > 0)
                {
                    Debug.WriteLine("file has some content");
                    //file extensioncheck taken from https://www.c-sharpcorner.com/article/file-upload-extension-validation-in-asp-net-mvc-and-javascript/
                    var valtypes = new[] { "pdf", "word", "doc", "docx" };  //accept only this type of files
                    var extension = Path.GetExtension(VolunteerCV.FileName).Substring(1);

                    if (valtypes.Contains(extension))
                    {
                        try
                        {
                            //file name is the id of the file
                            //we can change and provide user name
                            string fn = id + "." + extension;
                            //get a direct file path to ~/Content/VolunteerFiles/{id}.{extension}
                            //saving on certain path
                            string path = Path.Combine(Server.MapPath("~/Content/VolunteerFiles/"), fn);
                            //save the file
                            VolunteerCV.SaveAs(path);
                            //if these are all successful then we can set these fields
                            hasFileTemp = 1;
                            volunteerFileExtension = extension;

                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("CV file was not saved successfully.");
                            Debug.WriteLine("Exception:" + ex);
                        }

                    }
                }
            }

            string query = "update VolunteerRecruitments set volunteer_specialization=@volunteer_specialization, HasFile=@hasFile, volunteer_FileExtension=@volunteer_FileExtension where volunteer_id=@v_id";
            SqlParameter[] sqlparams = new SqlParameter[4];
            sqlparams[0] = new SqlParameter("@volunteer_FileExtension", volunteerFileExtension);
            sqlparams[1] = new SqlParameter("@hasFile", hasFileTemp);
            sqlparams[2] = new SqlParameter("@volunteer_specialization", volunteerspecialization);
            sqlparams[3] =  new SqlParameter("@v_id", id);

            string query_aspnet = "update AspNetUsers set phoneNumber=@phoneNumber where Id=@aspnet_id";
            SqlParameter[] sqlparams_aspusers = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@phoneNumber", phoneNumber);
            sqlparams[1] = new SqlParameter("@aspnet_id", id);

            Debug.WriteLine("Query " + query);
            Debug.WriteLine("paramm " + sqlparams);
            Debug.WriteLine("VOlunteerr Specialization ---- " + volunteerspecialization);

            db.Database.ExecuteSqlCommand(query, sqlparams); 

            db.Database.ExecuteSqlCommand(query_aspnet, sqlparams_aspusers);

            //logic for updating the pet in the database goes here
            return RedirectToAction("List");
        }
        public ActionResult DeleteConfirm(string id)
        {
            string query = "select * from volunteerRecruitments inner join AspNetUsers on volunteerRecruitments.volunteer_id= AspNetUsers.id where AspNetUsers.id = @id";
            SqlParameter param = new SqlParameter("@id", id);
            VolunteerRecruitment volunteer = db.volunteerRecruitment.SqlQuery(query, param).FirstOrDefault();

            return View(volunteer);
        }
        [HttpPost]
        public ActionResult Delete(string id)
        {
            string query = "delete from VolunteerRecruitments where volunteer_id = @id";
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
            get{
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set{
                _signInManager = value;
            }
        }
     
        public ApplicationUserManager UserManager
        {
            get{
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set{
                _userManager = value;
            }
        }
       // [HttpPost]
        //public async Task<ActionResult> Add(string Username, string Useremail, string Userpass, string GroomerFName, string GroomerLName, string GroomerDOB, decimal GroomerRate)
        //{
        //    return View();
        //}
    }
}