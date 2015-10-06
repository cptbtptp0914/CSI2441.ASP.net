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
                    CourseEnrolmentId = courseEnrolment.course_enrolment_id,
                    StudentId = courseEnrolment.student_id,
                    StudentFirstName = courseEnrolment.Student.firstname,
                    StudentLastName = courseEnrolment.Student.lastname,
                    CourseId = courseEnrolment.course_id,
                    Title = courseEnrolment.Course.title,
                    CourseStatus = courseEnrolment.course_status
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
        public ActionResult Create([Bind(Include = "CourseEnrolmentId,StudentId,CourseId,CourseStatus")] CourseEnrolmentCreateViewModel courseEnrolmentViewModel)
        {
            // if input passes validation
            if (ModelState.IsValid)
            {
                // create entitymodel, pass values from viewmodel
                CourseEnrolment courseEnrolmentEntityModel = new CourseEnrolment();
                PopulateEntityModel(courseEnrolmentViewModel, courseEnrolmentEntityModel);

                // discontinue previous enrolled courses
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
            CourseEnrolmentEditViewModel courseEnrolmentViewModel = new CourseEnrolmentEditViewModel
            {
                // read only field in view
                StudentIdFullName =
                    $"{courseEnrolmentEntityModel.Student.student_id} " +
                    $"{courseEnrolmentEntityModel.Student.firstname} " +
                    $"{courseEnrolmentEntityModel.Student.lastname}"
            };

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
        public ActionResult Edit([Bind(Include = "CourseEnrolmentId,StudentId,CourseId,CourseStatus")] CourseEnrolment courseEnrolmentEntityModel, CourseEnrolmentEditViewModel courseEnrolmentViewModel)
        {
            if (ModelState.IsValid)
            {
                // populate the entitymodel
                PopulateEntityModel(courseEnrolmentViewModel, courseEnrolmentEntityModel);

                // update db using entitymodel
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

            // re-enrol last course
            ReEnrolLastCourse(courseEnrolment.student_id);

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
                    StudentId = student.student_id,
                    StudentIdFullName = student.student_id + " " + student.firstname + " " + student.lastname
                });
            }

            foreach (Course course in coursesEntity)
            {
                viewModel.Courses.Add(new CourseEnrolmentDropDownListViewModel
                {
                    CourseId = course.course_id,
                    CourseIdTitle = course.course_id + " " + course.title
                });
            }

            // populate dropdownlist from viewmodel list
            viewModel.StudentDropDownList = new SelectList(viewModel.Students.OrderBy(s => s.StudentId), "StudentId", "StudentIdFullName");
            viewModel.CourseDropDownList = new SelectList(viewModel.Courses.OrderBy(u => u.CourseId), "CourseId", "CourseIdTitle");
        }

        /// <summary>
        /// Passes data from the view model to the entity model.
        /// </summary>
        /// <param name="viewModel">UnitBaseViewModel</param>
        /// <param name="entityModel">Unit</param>
        private void PopulateEntityModel(CourseEnrolmentBaseViewModel viewModel, CourseEnrolment entityModel)
        {
            entityModel.course_enrolment_id = viewModel.CourseEnrolmentId;
            entityModel.student_id = viewModel.StudentId;
            entityModel.course_id = viewModel.CourseId;

            if (viewModel.CourseStatus != null)
            {
                entityModel.course_status = viewModel.CourseStatus;
            }
        }

        /// <summary>
        /// Passes data from the entity model to the view model.
        /// </summary>
        /// <param name="viewModel">UnitBaseViewModel</param>
        /// <param name="entityModel">Unit</param>
        private void PopulateViewModel(CourseEnrolmentBaseViewModel viewModel, CourseEnrolment entityModel)
        {
            viewModel.CourseEnrolmentId = entityModel.course_enrolment_id;
            viewModel.StudentId = entityModel.student_id;
            viewModel.CourseId = entityModel.course_id;
            viewModel.CourseStatus = entityModel.course_status;

            viewModel.StudentFullName = entityModel.Student.firstname + " " + entityModel.Student.lastname;
            viewModel.Title = entityModel.Course.title;
        }

        /// <summary>
        /// Sets previously ENROLLED course to DISCONTIN.
        /// </summary>
        /// <param name="studentId">long</param>
        private void DiscontinuePrevEnrolments (long studentId)
        {
            // if enrolled course for student not unique,
            if (courseRules.IsNotUniqueEnrolled(studentId))
            {
                // can't use dict in linq, substitute with string
                string state = new CourseRules().CourseStates["Enrolled"];

                // get list of student's courses in ENROLLED status
                var enrolledCourses = (
                    from ce in db.CourseEnrolments
                    where ce.student_id == studentId &&
                        ce.course_status == state
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

        /// <summary>
        /// Sets last DISCONTIN course to ENROLLED.
        /// Used when ActionResult Delete() called.
        /// </summary>
        /// <param name="studentId">long</param>
        private void ReEnrolLastCourse(long studentId)
        {
            // can't use dict in linq, substitute with string
            string state = new CourseRules().CourseStates["Discontinued"];

            // get list of student's courses in DISCONTIN status
            var discontinCourses = (
                from ce in db.CourseEnrolments
                where ce.student_id == studentId &&
                      ce.course_status == state
                select ce).ToList();

            if (discontinCourses.Count > 0)
            {
                // set last DISCONTIN (most recent) course to ENROLLED
                discontinCourses.Last().course_status = courseRules.CourseStates["Enrolled"];
                db.Entry(discontinCourses.Last()).State = EntityState.Modified;
                db.SaveChanges();
            }
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
