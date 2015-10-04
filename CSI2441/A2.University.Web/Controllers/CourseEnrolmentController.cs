using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using A2.University.Web.Models.Entities;

namespace A2.University.Web.Controllers
{
    public class CourseEnrolmentController : Controller
    {
        private UniversityEntities db = new UniversityEntities();

        // GET: CourseEnrolment
        public ActionResult Index()
        {
            var courseEnrolments = db.CourseEnrolments.Include(c => c.Course).Include(c => c.Student);
            return View(courseEnrolments.ToList());
        }

        // GET: CourseEnrolment/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseEnrolment courseEnrolment = db.CourseEnrolments.Find(id);
            if (courseEnrolment == null)
            {
                return HttpNotFound();
            }
            return View(courseEnrolment);
        }

        // GET: CourseEnrolment/Create
        public ActionResult Create()
        {
            ViewBag.course_id = new SelectList(db.Courses, "course_id", "title");
            ViewBag.student_id = new SelectList(db.Students, "student_id", "firstname");
            return View();
        }

        // POST: CourseEnrolment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "course_enrolment_id,student_id,course_id,course_status")] CourseEnrolment courseEnrolment)
        {
            if (ModelState.IsValid)
            {
                db.CourseEnrolments.Add(courseEnrolment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.course_id = new SelectList(db.Courses, "course_id", "title", courseEnrolment.course_id);
            ViewBag.student_id = new SelectList(db.Students, "student_id", "firstname", courseEnrolment.student_id);
            return View(courseEnrolment);
        }

        // GET: CourseEnrolment/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseEnrolment courseEnrolment = db.CourseEnrolments.Find(id);
            if (courseEnrolment == null)
            {
                return HttpNotFound();
            }
            ViewBag.course_id = new SelectList(db.Courses, "course_id", "title", courseEnrolment.course_id);
            ViewBag.student_id = new SelectList(db.Students, "student_id", "firstname", courseEnrolment.student_id);
            return View(courseEnrolment);
        }

        // POST: CourseEnrolment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "course_enrolment_id,student_id,course_id,course_status")] CourseEnrolment courseEnrolment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseEnrolment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.course_id = new SelectList(db.Courses, "course_id", "title", courseEnrolment.course_id);
            ViewBag.student_id = new SelectList(db.Students, "student_id", "firstname", courseEnrolment.student_id);
            return View(courseEnrolment);
        }

        // GET: CourseEnrolment/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseEnrolment courseEnrolment = db.CourseEnrolments.Find(id);
            if (courseEnrolment == null)
            {
                return HttpNotFound();
            }
            return View(courseEnrolment);
        }

        // POST: CourseEnrolment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            CourseEnrolment courseEnrolment = db.CourseEnrolments.Find(id);
            db.CourseEnrolments.Remove(courseEnrolment);
            db.SaveChanges();
            return RedirectToAction("Index");
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
