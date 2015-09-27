using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using A2.University.Web.Business.Services;
using A2.University.Web.Models.Entities;

namespace A2.University.Web.Controllers
{
    public class StaffController : Controller
    {
        private UniversityEntities db = new UniversityEntities();
        private int _emailMatchTally;
        private string _email;
        private string _tempEmail;

        // GET: Staff
        public ActionResult Index()
        {
            return View(db.Staff.ToList());
        }

        // GET: Staff/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staff.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // GET: Staff/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Staff/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "staff_id,firstname,surname,email")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                db.Staff.Add(staff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(staff);
        }

        // GET: Staff/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staff.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // POST: Staff/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "staff_id,firstname,surname,email")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staff);
        }

        // GET: Staff/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staff.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // POST: Staff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Staff staff = db.Staff.Find(id);
            db.Staff.Remove(staff);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        /// <summary>
        /// Function initiates recursive search of generated emails to ensure no duplicates exist.
        /// Begins by reseting tallies and current email fields, generates first version of email, then passes to EmailRecursiveSearch.
        /// TODO: Try to implement this in GenerateEmail class, code duplication between Student/UnitControllers
        /// </summary>
        /// <param name="staff">Staff</param>
        private void StartEmailRecursiveSearch([Bind(Include = "staff_id,firstname,surname,email")] Staff staff)
        {
            // reset fields for each new student instance
            _emailMatchTally = 0;
            _email = "";

            // generate initial standard email
            string target = staff.firstname[0] + "." + staff.surname + EmailGenerator.StaffEmailSuffix;
            // us email as target for search
            EmailRecursiveSearch(staff, target.ToLower());
        }


        /// <summary>
        /// Recursive function to search each version of generated email against existing emails to ensure no duplicates exist.
        /// </summary>
        /// <param name="staff">Staff</param>
        /// <param name="target">string</param>
        private void EmailRecursiveSearch([Bind(Include = "staff_id,firstname,surname,email")] Staff staff,
            string target)
        {
            if (SearchEmail(target))
            {
                // increment tally
                _emailMatchTally++;
                // make new temp email based on tally
                _tempEmail = EmailGenerator.GenerateEmail(
                    "staff",
                    _emailMatchTally,
                    staff.firstname.ToLower(),
                    staff.surname.ToLower()
                    );
                // recursive call to search new version of generated email
                EmailRecursiveSearch(staff, _tempEmail);
            }
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
            return db.Staff.Any(e => e.email == target);
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
