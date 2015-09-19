using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSI2441.A2.Web.MVC.Models;
using CSI2441.A2.Web.MVC.Utilities;

namespace CSI2441.A2.Web.MVC.Controllers
{
    public class ManageStudentController : Controller, IEmailGenerator
    {

        /**
         * CRUD WebGrid here.
         */
        public ActionResult Index()
        {
            return View();
        }

        /**
         * Add Student here. Initial load.
         */
        [HttpGet]
        public ActionResult AddStudent()
        {
            return View();
        }

        /**
         * @Override
         * What happens when Add Student button clicked is called here.
         */
        [HttpPost]
        public ActionResult AddStudent(StudentModel model)
        {
            // if passed validation,
            if (ModelState.IsValid)
            {
                // generate email address
                GenerateEmail(model.FirstName, model.Surname);

                // insert student record
                InsertStudent(
                    model.FirstName,
                    model.Surname, 
                    model.Dob,
                    GenerateEmail(model.FirstName, model.Surname),
                    model.Landline,
                    model.Mobile,
                    model.Address,
                    model.City,
                    model.State,
                    model.Postcode
                    );

                // show success view
                return RedirectToAction("Success", "Home");
            }
            else
            {
                return View();
            }
        }

        /**
         * Generates email addresses, must be unique.
         */
        public string GenerateEmail(string firstName, string surname)
        {
            string finalEmail = "";
            string emailSuffix = "@our.ecu.edu.au";
            string firstEmail = firstName[0] + surname[0] + emailSuffix;

            int matchTally = 0;

            // pseudo:
            // get student emails from db, order by email
            // do binary search, target = firstEmail
            // if emails match,
                // matchTally++
            // else continue

            // pseudo:
            // if matchTally > 0
                // if matchTally > firstName.Length
                    // for i to firstName.Length - 1
                        // finalEmail += firstName[i]
                    // next
                // else
                    // for i to matchTally - 1
                        // finalEmail += firstName[i]
                    // next
                // end if
                // finalEmail += surname[0] + emailSuffix
                // return finalEmail
            // else
                // return firstEmail
            // end if

            return firstEmail;
        }

        /**
         * Retrieves email records from DB.
         */
        public void GetEmailRecordset()
        {
            // TODO: Get email records from DB.
        }

        /**
         * This function inserts new student record in DB.
         */
        public void InsertStudent(
            string firstName,
            string surname,
            DateTime dob,
            string email,
            string landline,
            string mobile,
            string address,
            string city,
            string state,
            int postcode)
        {
            // TODO: Insert new student record to DB.
        }

        public ActionResult EditStudent()
        {
            return View();
        }

        public ActionResult DeleteStudent()
        {
            return View();
        }
    }
}