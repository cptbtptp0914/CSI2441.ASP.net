using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using A2.University.Web.Models;
using A2.University.Web.Models.Entities;

namespace A2.University.Web.Controllers
{
    public class CourseEnrolmentController : Controller
    {
        private UniversityEntities db = new UniversityEntities();

        // GET: CourseEnrolment
        public ActionResult Index()
        {
            CourseEnrolmentIndexViewModel courseEnrolmentViewModel = new CourseEnrolmentIndexViewModel();
            courseEnrolmentViewModel.CourseEnrolments = db.CourseEnrolments.Include(c => c.Course).Include(c => c.Student).ToList();

            return View(courseEnrolmentViewModel.CourseEnrolments);
        }

        // GET: CourseEnrolment/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            CourseEnrolment courseEnrolmentEntityModel = db.CourseEnrolments.Find(id);
            // create viewmodel, pass values from entitymodel
            CourseEnrolmentDetailsViewModel courseEnrolmentViewModel = new CourseEnrolmentDetailsViewModel();
            SetCourseEnrolmentViewModel(courseEnrolmentViewModel, courseEnrolmentEntityModel);

            if (courseEnrolmentEntityModel == null)
            {
                return HttpNotFound();
            }
            return View(courseEnrolmentViewModel);
        }

        // GET: CourseEnrolment/Create
        public ActionResult Create()
        {
            // create viewmodel
            CourseEnrolmentCreateViewModel courseEnrolmentViewModel = new CourseEnrolmentCreateViewModel();
            courseEnrolmentViewModel.StudentDropDownList = new SelectList(db.Students.OrderBy(s => s.student_id), "student_id", "student_id_fullname");
            courseEnrolmentViewModel.CourseDropDownList = new SelectList(db.Courses.OrderBy(c => c.course_id), "course_id", "course_id_title");

            return View(courseEnrolmentViewModel);
        }

        // POST: CourseEnrolment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "course_enrolment_id,student_id,course_id,course_status")] CourseEnrolmentCreateViewModel courseEnrolmentViewModel)
        {
            // if input passes validation
            if (ModelState.IsValid)
            {
                // create entitymodel, pass values from viewmodel
                CourseEnrolment courseEnrolmentEntityModel = new CourseEnrolment();
                SetCourseEnrolmentEntityModel(courseEnrolmentViewModel, courseEnrolmentEntityModel);

                // TODO: add logic to check if student ENROLLED in another course, if so, make previous course DISCONTIN
                // create static class in Business.BusinessRulesModels

                // update db using entitymodel
                db.CourseEnrolments.Add(courseEnrolmentEntityModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // populate dropdownlists
            courseEnrolmentViewModel.StudentDropDownList = new SelectList(db.Students.OrderBy(s => s.student_id), "student_id", "student_id_fullname");
            courseEnrolmentViewModel.CourseDropDownList = new SelectList(db.Courses.OrderBy(c => c.course_id), "course_id", "course_id_title");

            return View(courseEnrolmentViewModel);
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

        /// <summary>
        /// Passes data from the view model to the entity model.
        /// </summary>
        /// <param name="viewModel">UnitBaseViewModel</param>
        /// <param name="entityModel">Unit</param>
        private void SetCourseEnrolmentEntityModel(CourseEnrolmentBaseViewModel viewModel, CourseEnrolment entityModel)
        {
            entityModel.course_enrolment_id = viewModel.course_enrolment_id;
            entityModel.student_id = viewModel.student_id;
            entityModel.course_id = viewModel.course_id;
            //entityModel.course_status = "ENROLLED"; //viewModel.course_status;
        }

        /// <summary>
        /// Passes data from the entity model to the view model.
        /// </summary>
        /// <param name="viewModel">UnitBaseViewModel</param>
        /// <param name="entityModel">Unit</param>
        private void SetCourseEnrolmentViewModel(CourseEnrolmentBaseViewModel viewModel, CourseEnrolment entityModel)
        {
            viewModel.course_enrolment_id = entityModel.course_enrolment_id;
            viewModel.student_id = entityModel.student_id;
            viewModel.course_id = entityModel.course_id;
            viewModel.course_status = entityModel.course_status;

            viewModel.fullname = entityModel.Student.firstname + " " + entityModel.Student.lastname;
            viewModel.title = entityModel.Course.title;
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
