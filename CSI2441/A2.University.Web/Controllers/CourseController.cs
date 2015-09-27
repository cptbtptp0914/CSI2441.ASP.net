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
    public class CourseController : Controller
    {
        private UniversityEntities db = new UniversityEntities();

        // GET: Course
        public ActionResult Index()
        {
            var courses = db.Courses.Include(c => c.CourseType).Include(c => c.Staff);
            return View(courses.ToList());
        }

        // GET: Course/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            ViewBag.course_type_id = new SelectList(db.CourseTypes, "course_type_id", "title");
            ViewBag.coordinator_id = new SelectList(db.Staff, "staff_id", "firstname");
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "course_id,title,coordinator_id,course_type_id")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.course_type_id = new SelectList(db.CourseTypes, "course_type_id", "title", course.course_type_id);
            ViewBag.coordinator_id = new SelectList(db.Staff, "staff_id", "firstname", course.coordinator_id);
            return View(course);
        }

        // GET: Course/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.course_type_id = new SelectList(db.CourseTypes, "course_type_id", "title", course.course_type_id);
            ViewBag.coordinator_id = new SelectList(db.Staff, "staff_id", "firstname", course.coordinator_id);
            return View(course);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "course_id,title,coordinator_id,course_type_id")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.course_type_id = new SelectList(db.CourseTypes, "course_type_id", "title", course.course_type_id);
            ViewBag.coordinator_id = new SelectList(db.Staff, "staff_id", "firstname", course.coordinator_id);
            return View(course);
        }

        // GET: Course/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
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
