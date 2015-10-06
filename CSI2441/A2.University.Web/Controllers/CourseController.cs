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
            CourseIndexViewModel courseViewModel = new CourseIndexViewModel();
            var coursesEntity = db.Courses.Include(c => c.CourseType).Include(c => c.Staff).ToList();

            // transfer entity list to viewmodel list
            foreach (Course course in coursesEntity)
            {
                courseViewModel.Courses.Add(new CourseIndexViewModel
                {
                    // core fields
                    CourseId = course.course_id,
                    Title = course.title,
                    CoordinatorId = course.coordinator_id,
                    CourseTypeId = course.course_type_id,
                    // derived fields
                    StaffFullName = course.Staff.firstname + " " + course.Staff.surname,
                    CourseTypeTitle = course.CourseType.title,
                    CreditPoints = course.CourseType.credit_points,
                    Duration = course.CourseType.duration
                });
            }

            return View(courseViewModel.Courses);
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
            PopulateViewModel(courseViewModel, courseEntityModel);

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
            PopulateDropDownLists(courseViewModel);

            return View(courseViewModel);
        }

        // POST: Course/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseId,Title,CoordinatorId,CourseTypeId")] CourseCreateViewModel courseViewModel)
        {
            // if input passes validation
            if (ModelState.IsValid)
            {
                // create entity model, pass values from viewmodel
                Course courseEntityModel = new Course();
                PopulateEntityModel(courseViewModel, courseEntityModel);

                // update db using entitymodel
                db.Courses.Add(courseEntityModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // populate dropdownlists
            PopulateDropDownLists(courseViewModel);

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

            // create entitymodel, match id
            Course courseEntityModel = db.Courses.Find(id);
            // create viewmodel, pass values from entitymodel
            CourseEditViewModel courseViewModel =  new CourseEditViewModel();
            PopulateViewModel(courseViewModel, courseEntityModel);

            // populate dropdownlists
            PopulateDropDownLists(courseViewModel);

            if (courseEntityModel == null)
            {
                return HttpNotFound();
            }

            // render view using viewmodel
            return View(courseViewModel);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseId,Title,CoordinatorId,CourseTypeId")] Course courseEntityModel, CourseEditViewModel courseViewModel)
        {
            if (ModelState.IsValid)
            {
                // populate entitymodel
                PopulateEntityModel(courseViewModel, courseEntityModel);

                // update db using entitymodel
                db.Entry(courseEntityModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // populate dropdownlists
            PopulateDropDownLists(courseViewModel);

            // render view using viewmodel
            return View(courseViewModel);
        }

        // GET: Course/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            Course courseEntityModel = db.Courses.Find(id);
            // create viewmodel, pass values from entitymodel
            CourseDeleteViewModel courseViewModel = new CourseDeleteViewModel();
            PopulateViewModel(courseViewModel, courseEntityModel);

            if (courseEntityModel == null)
            {
                return HttpNotFound();
            }
            return View(courseViewModel);
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
        /// Populates dropdownlists for course view.
        /// </summary>
        /// <param name="viewModel">CourseDropDownListViewModel</param>
        private void PopulateDropDownLists(CourseDropDownListViewModel viewModel)
        {
            // get list of students/units from db
            var staffEntity = (from staff in db.Staff
                               select staff).ToList();
            var courseTypesEntity = (from courseType in db.CourseTypes
                                   select courseType).ToList();

            // transfer relevant elements to viewmodel list
            foreach (Staff staff in staffEntity)
            {
                viewModel.Coordinators.Add(new CourseDropDownListViewModel
                {
                    CoordinatorId = staff.staff_id,
                    StaffIdFullName = staff.staff_id + " " + staff.firstname + " " + staff.surname
                });
            }

            foreach (CourseType type in courseTypesEntity)
            {
                viewModel.CourseTypes.Add(new CourseDropDownListViewModel
                {
                    CourseTypeId = type.course_type_id,
                    CourseTypeTitle = type.title
                });
            }

            // populate dropdownlist from viewmodel list
            viewModel.CoordinatorDropDownList = new SelectList(viewModel.Coordinators.OrderBy(s => s.CoordinatorId), "CoordinatorId", "StaffIdFullName");
            viewModel.CourseTypeTitleDropDownList = new SelectList(viewModel.CourseTypes.OrderBy(u => u.CourseTypeId), "CourseTypeId", "CourseTypeTitle");
        }

        /// <summary>
        /// Passes data from the view model to the entity model.
        /// </summary>
        /// <param name="viewModel">UnitBaseViewModel</param>
        /// <param name="entityModel">Unit</param>
        private void PopulateEntityModel(CourseBaseViewModel viewModel, Course entityModel)
        {
            entityModel.course_id = viewModel.CourseId;
            entityModel.title = viewModel.Title;
            entityModel.coordinator_id = viewModel.CoordinatorId;
            entityModel.course_type_id = viewModel.CourseTypeId;
        }

        /// <summary>
        /// Passes data from the entity model to the view model.
        /// </summary>
        /// <param name="viewModel">UnitBaseViewModel</param>
        /// <param name="entityModel">Unit</param>
        private void PopulateViewModel(CourseBaseViewModel viewModel, Course entityModel)
        {
            viewModel.CourseId = entityModel.course_id;
            viewModel.Title = entityModel.title;
            viewModel.CoordinatorId = entityModel.coordinator_id;
            viewModel.CourseTypeId = entityModel.course_type_id;

            viewModel.StaffFullName = entityModel.Staff.firstname + " " + entityModel.Staff.surname;
            viewModel.CourseTypeTitle = entityModel.CourseType.title;
            viewModel.CreditPoints = entityModel.CourseType.credit_points;
            viewModel.Duration = entityModel.CourseType.duration;
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
