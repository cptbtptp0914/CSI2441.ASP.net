using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using A2.University.Web.Business.Services;
using A2.University.Web.Models.Entities;

namespace A2.University.Web.Controllers
{
    public class StudentController : Controller
    {
        private UniversityEntities db = new UniversityEntities();
        private int _emailMatchTally;
        private string _email;
        private string _tempEmail;

        // GET: Student
        public ActionResult Index()
        {
            return View(db.Students.ToList());
        }

        // GET: Student/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "student_id,firstname,lastname,dob,email,ph_landline,ph_mobile,adrs,adrs_city,adrs_state,adrs_postcode")] Student student)
        {
            // if input passed validation
            if (ModelState.IsValid)
            {
                // generate email
                StartEmailRecursiveSearch(student);
                student.email = _email;

                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Student/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "student_id,firstname,lastname,dob,email,ph_landline,ph_mobile,adrs,adrs_city,adrs_state,adrs_postcode")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Student/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        /// <summary>
        /// Function initiates recursive search of generated emails to ensure no duplicates exist.
        /// Begins by reseting tallies and current email fields, generates first version of email, then passes to EmailRecursiveSearch.
        /// </summary>
        /// <param name="student">Student</param>
        private void StartEmailRecursiveSearch([Bind(Include = "student_id,firstname,lastname,dob,email,ph_landline,ph_mobile,adrs,adrs_city,adrs_state,adrs_postcode")] Student student)
        {
            // reset fields for each new student instance
            _emailMatchTally = 0;
            _email = "";

            // generate initial standard email
            string target = student.firstname[0] + student.lastname + EmailGenerator.StudentEmailSuffix;
            // use email as target for search
            EmailRecursiveSearch(student, target.ToLower());
        }


        /// <summary>
        /// Recursive function to search each version of generated email against existing emails to ensure no duplicates exist.
        /// </summary>
        /// <param name="student">Student</param>
        /// <param name="target">string</param>
        private void EmailRecursiveSearch([Bind(Include = "student_id,firstname,lastname,dob,email,ph_landline,ph_mobile,adrs,adrs_city,adrs_state,adrs_postcode")] Student student, string target)
        {
            // if current email is a match,
            if (SearchEmail(target))
            {
                // increment tally
                _emailMatchTally++;
                // make new temp email based on tally
                _tempEmail = EmailGenerator.GenerateEmail(
                    "student",
                    _emailMatchTally,
                    student.firstname.ToLower(),
                    student.lastname.ToLower()
                    );
                // recursive call to search new version of generated email
                EmailRecursiveSearch(student, _tempEmail);
            }
            // exit case, generated email does not exist! let's use it
            else
            {
                _email = target;
            }
        }

        /// <summary>
        /// Function searches generated email against existing emails in database.
        /// </summary>
        /// <param name="target">string</param>
        /// <returns>bool</returns>
        private bool SearchEmail(string target)
        {
            return db.Students.Any(e => e.email == target);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
