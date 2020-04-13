using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HospitalProject_Team4.Data;
using HospitalProject_Team4.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
//FAQ controller for Frequently asked Questions And Answers
namespace HospitalProject_Team4.Controllers
{
    public class FAQController : Controller
    {
        //not using Application user here for now just methods for CRUD of FAQS 
        private HospitalProjectContext db = new HospitalProjectContext();
        // GET: FAQ
        //List for showing all FAQs, in list view only showing questions.
        public ActionResult List()
        {
            string query = "Select * from FAQs";
            List<FAQ> faqs = db.FAQs.SqlQuery(query).ToList();

            return View(faqs);
        }
        //this method for opening admin view which will have edit and delete options, here in this method
        //its showing all the FAQS with questions and answers
        public ActionResult AdminViewFaq(int id)
        {
            string query = "Select * from FAQs where FAQId = @faqid";
            var parameter = new SqlParameter("@faqid", id);
            FAQ myfaq = db.FAQs.SqlQuery(query, parameter).FirstOrDefault();


            return View(myfaq);


        }
        //View method for showing questins with answers when question is clicked from list 
        //this method is designed for non-admin user who can only view but cannot edit or delete the entries.
        public ActionResult ViewFaq(int id)
        {
            string query = "Select * from FAQs where FAQId = @faqid";
            var parameter = new SqlParameter("@faqid", id);
            FAQ myfaq = db.FAQs.SqlQuery(query, parameter).FirstOrDefault();


            return View(myfaq); 

           
        }
        //this method calling Add view which has fields for questions and ANSWER TO be added by admin 
        public ActionResult Add()
        {

            return View();
        }
        //When add button is submitted by post method this methods is called which adds the entries entered by admin into the database
        [HttpPost]
        public ActionResult Add(string question, string answer)
        {
            string query = "insert into FAQs (question,answer) values (@question,@answer)";
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@question", question);
           sqlparams[1] = new SqlParameter("@answer", answer);
            //Debug.Write("Query--- " + query);
            db.Database.ExecuteSqlCommand(query,sqlparams);
            return RedirectToAction("List");
        }
        //update method for view for admin using LINQ, shows the update page where admin can enter new fields for question and answer and hit update button
        public ActionResult Update(int id)
        {
            //select * from FAQs where FAQId = @id

            //adding by LINQ
            FAQ faqs = db.FAQs.Find(id);

            return View(faqs);

        }
        //post method for admin to update the entries into the database and save in the database returning to updated list
        [HttpPost]
        public ActionResult Update(int id, string question, string answer)
        {
            //used LINQ 
            FAQ faqs = db.FAQs.Find(id);
            faqs.question = question;
            faqs.answer = answer;
           
            db.SaveChanges();

            return RedirectToAction("List");
        }

        //delete method for admin
        public ActionResult Delete(int id)
        {

            string query = "delete from FAQs where FAQId = @faqid";
            SqlParameter sqlparams = new SqlParameter("@faqid", id);



            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");
        }



    }
}