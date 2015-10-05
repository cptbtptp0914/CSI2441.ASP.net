using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using A2.University.Web.Models;
using A2.University.Web.Models.Business;
using A2.University.Web.Models.Entities;

namespace A2.University.Web.Controllers
{
    public class CourseEnrolmentController : Controller
    {
        private UniversityEntities db = new UniversityEntities();
        private CourseRules courseRules = new CourseRules();

        // GET: CourseEnrolment
        public ActionResult Index()
        {
            CourseEnrolmentIndexViewModel courseEnrolmentViewModel = new CourseEnrolmentIndexViewModel();
            var courseEnrolmentsEntity = db.CourseEnrolments.Include(c => c.Course).Include(c => c.Student).ToList();
            
            // transfer entity list to viewmodel list
            foreach (CourseEnrolment courseEnrolment in courseEnrolmentsEntity)
            {
                courseEnrolmentViewModel.CourseEnrolments.Add(new CourseEnrolmentIndexViewModel
                {
                    course_enrolment_id = courseEnrolment.course_enrolment_id,
                    student_id = courseEnrolment.student_id,
                    firstname = courseEnrolment.Student.firstname,
                    lastname = courseEnrolment.Student.lastname,
                    course_id = courseEnrolment.course_id,
                    title = courseEnrolment.Course.title,
                    course_status = courseEnrolment.course_status
                });
            }

            // render view using viewmodel list
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
            PopulateViewModel(courseEnrolmentViewModel, courseEnrolmentEntityModel);

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
            PopulateDropDownLists(courseEnrolmentViewModel);

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
                PopulateEntityModel(courseEnrolmentViewModel, courseEnrolmentEntityModel);

                // TODO: add logic to check if student ENROLLED in another course, if so, make previous course DISCONTIN
                // create static class in Business.BusinessRulesModels
                DiscontinuePrevEnrolments(courseEnrolmentEntityModel.student_id);

                // update db using entitymodel
                db.CourseEnrolments.Add(courseEnrolmentEntityModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // populate dropdownlists
            PopulateDropDownLists(courseEnrolmentViewModel);

            return View(courseEnrolmentViewModel);
        }

        // GET: CourseEnrolment/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            CourseEnrolment courseEnrolmentEntityModel = db.CourseEnrolments.Find(id);
            // create viewmodel, pass values from entitymodel
            CourseEnrolmentEditViewModel courseEnrolmentViewModel = new CourseEnrolmentEditViewModel();
            PopulateViewModel(courseEnrolmentViewModel, courseEnrolmentEntityModel);

            // populate dropdownlists
            PopulateDropDownLists(courseEnrolmentViewModel);

            if (courseEnrolmentEntityModel == null)
            {
                return HttpNotFound();
            }

            return View(courseEnrolmentViewModel);
        }

        // POST: CourseEnrolment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "course_enrolment_id,student_id,course_id,course_status")] CourseEnrolment courseEnrolmentEntityModel, CourseEnrolmentEditViewModel courseEnrolmentViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseEnrolmentEntityModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // populate dropdownlists
            PopulateDropDownLists(courseEnrolmentViewModel);

            return View(courseEnrolmentViewModel);
        }

        // GET: CourseEnrolment/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            CourseEnrolment courseEnrolmentEntityModel = db.CourseEnrolments.Find(id);
            // create viewmodel, pass values from entitymodel
            CourseEnrolmentDeleteViewModel courseEnrolmentViewModel = new CourseEnrolmentDeleteViewModel();
            PopulateViewModel(courseEnrolmentViewModel, courseEnrolmentEntityModel);

            if (courseEnrolmentEntityModel == null)
            {
                return HttpNotFound();
            }
            return View(courseEnrolmentViewModel);
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
        /// Populates dropdownlists for unit enrolment view.
        /// </summary>
        /// <param name="viewModel">UnitEnrolmentDropDownListViewModel</param>
        private void PopulateDropDownLists(CourseEnrolmentDropDownListViewModel viewModel)
        {
            // get list of students/units from db
            var studentsEntity = (from student in db.Students
                                  select student).ToList();
            var coursesEntity = (from course in db.Courses
                               select course).ToList();

            // transfer relevant elements to viewmodel list
            foreach (Student student in studentsEntity)
            {
                viewModel.Students.Add(new CourseEnrolmentDropDownListViewModel
                {
                    student_id = student.student_id,
                    student_id_fullname = student.student_id + " " + student.firstname + " " + student.lastname
                });
            }

            foreach (Course course in coursesEntity)
            {
                viewModel.Courses.Add(new CourseEnrolmentDropDownListViewModel
                {
                    course_id = course.course_id,
                    course_id_title = course.course_id + " " + course.title
                });
            }

            // populate dropdownlist from viewmodel list
            viewModel.StudentDropDownList = new SelectList(viewModel.Students.OrderBy(s => s.student_id), "student_id", "student_id_fullname");
            viewModel.CourseDropDownList = new SelectList(viewModel.Courses.OrderBy(u => u.course_id), "course_id", "course_id_title");
        }

        /// <summary>
        /// Passes data from the view model to the entity model.
        /// </summary>
        /// <param name="viewModel">UnitBaseViewModel</param>
        /// <param name="entityModel">Unit</param>
        private void PopulateEntityModel(CourseEnrolmentBaseViewModel viewModel, CourseEnrolment entityModel)
        {
            entityModel.course_enrolment_id = viewModel.course_enrolment_id;
            entityModel.student_id = viewModel.student_id;
            entityModel.course_id = viewModel.course_id;
            // entityModel.course_status = "ENROLLED" is default
            // create and edit calls function to set course_status
        }

        /// <summary>
        /// Passes data from the entity model to the view model.
        /// </summary>
        /// <param name="viewModel">UnitBaseViewModel</param>
        /// <param name="entityModel">Unit</param>
        private void PopulateViewModel(CourseEnrolmentBaseViewModel viewModel, CourseEnrolment entityModel)
        {
            viewModel.course_enrolment_id = entityModel.course_enrolment_id;
            viewModel.student_id = entityModel.student_id;
            viewModel.course_id = entityModel.course_id;
            viewModel.course_status = entityModel.course_status;

            viewModel.fullname = entityModel.Student.firstname + " " + entityModel.Student.lastname;
            viewModel.title = entityModel.Course.title;
        }

        /// <summary>
        /// Sets previously ENROLLED units to DISCONTIN.
        /// </summary>
        /// <param name="studentId">long</param>
        private void DiscontinuePrevEnrolments (long studentId)
        {
            // if enrolled course for student not unique,
            if (courseRules.IsNotUniqueEnrolled(studentId))
            {
                // get list of student's course in ENROLLED status
                var enrolledCourses = (from ce in db.CourseEnrolments
                                       where ce.student_id == studentId &&
                                       ce.course_status == courseRules.CourseStates["Enrolled"]
                                       select ce).ToList();

                // set each course status to DISCONTIN
                foreach (var enrolled in enrolledCourses)
                {
                    enrolled.course_status = courseRules.CourseStates["Discontinued"];
                    db.Entry(enrolled).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        private void EnrolLatestCourse(long studentId)
        {
            // TODO: implement auto re-enrol latest course when deleting course enrolment
            // ie. course enrolment with highest id
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
