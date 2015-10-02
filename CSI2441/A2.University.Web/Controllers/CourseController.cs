using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using A2.University.Web.Models;
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

            // create entitymodel, match id
            Course courseEntityModel = db.Courses.Find(id);
            // create viewmodel, pass values from entitymodel
            CourseDetailsViewModel courseViewModel = new CourseDetailsViewModel();
            SetCourseViewModel(courseViewModel, courseEntityModel);

            if (courseEntityModel == null)
            {
                return HttpNotFound();
            }

            // render view using viewmodel
            return View(courseViewModel);
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            // crate viewmodel
            CourseCreateViewModel courseViewModel = new CourseCreateViewModel();
            // populate dropdownlists
            courseViewModel.CoordinatorDropDownList = new SelectList(db.Staff.OrderBy(s => s.firstname), "staff_id", "fullname");
            courseViewModel.CourseTypeTitleDropDownList = new SelectList(db.CourseTypes, "course_type_id", "title");

//            ViewBag.course_type_id = new SelectList(db.CourseTypes, "course_type_id", "title");
//            ViewBag.coordinator_id = new SelectList(db.Staff.OrderBy(s => s.firstname), "staff_id", "fullname");
            return View(courseViewModel);
        }

        // POST: Course/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "course_id,title,coordinator_id,course_type_id")] CourseCreateViewModel courseViewModel)
        {
            // if input passes validation
            if (ModelState.IsValid)
            {
                // create entity model, pass values from viewmodel
                Course courseEntityModel = new Course();
                SetCourseEntityModel(courseViewModel, courseEntityModel);

                // update db using entitymodel
                db.Courses.Add(courseEntityModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // populate dropdownlists
            courseViewModel.CoordinatorDropDownList = new SelectList(db.Staff.OrderBy(s => s.firstname), "staff_id", "fullname", courseViewModel.course_type_id);
            courseViewModel.CourseTypeTitleDropDownList = new SelectList(db.CourseTypes, "course_type_id", "title", courseViewModel.course_type_id);

            // render view using viewmodel
            return View(courseViewModel);
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
            ViewBag.coordinator_id = new SelectList(db.Staff.OrderBy(s => s.firstname), "staff_id", "fullname", course.coordinator_id);

            // ViewData in conjunction with ViewBag above, guarantees default selection value from db
            List<Staff> coordinatorsList = new List<Staff>(db.Staff.OrderBy(s => s.firstname).ToList());
            ViewData["coordinatorsList"] = coordinatorsList;
            List<CourseType> courseTypesList = new List<CourseType>(db.CourseTypes.ToList());
            ViewData["courseTypesList"] = courseTypesList;

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

        /// <summary>
        /// Passes data from the view model to the entity model.
        /// </summary>
        /// <param name="viewModel">UnitBaseViewModel</param>
        /// <param name="entityModel">Unit</param>
        private void SetCourseEntityModel(CourseBaseViewModel viewModel, Course entityModel)
        {
            entityModel.course_id = viewModel.course_id;
            entityModel.title = viewModel.title;
            entityModel.coordinator_id = viewModel.coordinator_id;
            entityModel.course_type_id = viewModel.course_type_id;
        }

        /// <summary>
        /// Passes data from the entity model to the view model.
        /// </summary>
        /// <param name="viewModel">UnitBaseViewModel</param>
        /// <param name="entityModel">Unit</param>
        private void SetCourseViewModel(CourseBaseViewModel viewModel, Course entityModel)
        {
            viewModel.course_id = entityModel.course_id;
            viewModel.title = entityModel.title;
            viewModel.coordinator_id = entityModel.coordinator_id;
            viewModel.course_type_id = entityModel.course_type_id;

            viewModel.coordinator_name = GetCoordinatorFullName(entityModel.coordinator_id);
            viewModel.course_type_title = GetCourseTypeTitle(entityModel.course_type_id);
            viewModel.credit_points = GetCourseCreditPoints(entityModel.course_type_id);
            viewModel.duration = GetCourseDuration(entityModel.course_type_id);
        }

        /// <summary>
        /// SQL statement returns coordinator's full name.
        /// </summary>
        /// <param name="staff_id">long</param>
        /// <returns>string</returns>
        private string GetCoordinatorFullName(long staff_id)
        {
            var query = (
                from c in db.Staff
                where c.staff_id == staff_id
                select c.firstname + " " + c.surname
            ).FirstOrDefault();

            return query;
        }

        /// <summary>
        /// SQL statement returns course type title.
        /// </summary>
        /// <param name="course_type_id">long</param>
        /// <returns>string</returns>
        private string GetCourseTypeTitle(long course_type_id)
        {
            var query = (
                from ct in db.CourseTypes
                where ct.course_type_id == course_type_id
                select ct.title
            ).FirstOrDefault();

            return query;
        }

        /// <summary>
        /// SQL statement returns course credit points.
        /// </summary>
        /// <param name="course_type_id">long</param>
        /// <returns>string</returns>
        private int GetCourseCreditPoints(long course_type_id)
        {
            var query = (
                from ct in db.CourseTypes
                where ct.course_type_id == course_type_id
                select ct.credit_points
            ).FirstOrDefault();

            return query;
        }

        /// <summary>
        /// SQL statement returns course duration.
        /// </summary>
        /// <param name="course_type_id">long</param>
        /// <returns>string</returns>
        private int GetCourseDuration(long course_type_id)
        {
            var query = (
                from ct in db.CourseTypes
                where ct.course_type_id == course_type_id
                select ct.duration
            ).FirstOrDefault();

            return query;
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
