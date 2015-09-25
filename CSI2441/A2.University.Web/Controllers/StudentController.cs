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
        private int MatchTally;
        private string Email;
        private string TempEmail;

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
                SetEmail(student);

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

        private void SetEmail([Bind(Include = "student_id,firstname,lastname,dob,email,ph_landline,ph_mobile,adrs,adrs_city,adrs_state,adrs_postcode")] Student student)
        {
            // reset fields for new student instance
            MatchTally = 0;
            Email = "";

            // get target email string to search
            string target = student.firstname[0] + student.lastname + EmailGenerator.StudentEmailSuffix;
            // get match tally
            var matchTally = db.Students.Where(e => e.email == target.ToLower()).ToList().Count();
            var test = db.Students.Where(e => e.email == target.ToLower());

            if (SearchEmail(target))
            {
                matchTally++;
                target = EmailGenerator.GenerateEmail(
                    "student"
                    )
            }

            // need to recursively test new email until no matches found, then store
            string tempEmail = EmailGenerator.GenerateEmail(
                "student",
                matchTally,
                student.firstname.ToLower(),
                student.lastname.ToLower()
            );
            // generate email
            student.email = EmailGenerator.GenerateEmail(
                "student",
                matchTally,
                student.firstname.ToLower(),
                student.lastname.ToLower()
            );
        }

        private void GetEmail([Bind(Include = "student_id,firstname,lastname,dob,email,ph_landline,ph_mobile,adrs,adrs_city,adrs_state,adrs_postcode")] Student student, string target)
        {
            // if current email is a match,
            if (SearchEmail(target))
            {
                // increment tally
                MatchTally++;
                // make new temp email based on tally
                TempEmail = EmailGenerator.GenerateEmail(
                    "student",
                    MatchTally,
                    student.firstname.ToLower(),
                    student.lastname.ToLower()
                    );
                // recursive call to search same email again
                GetEmail(student, TempEmail);
            }
            // end case, if no emails match
            else
            {
                Email = TempEmail;
            }
        }

        private Boolean SearchEmail(string target)
        {
            var test = (from e in db.Students
                where e.email == target
                select e.email).SingleOrDefault();
            return Convert.ToBoolean(test);
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
