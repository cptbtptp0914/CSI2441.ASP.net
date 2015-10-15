using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using A2.University.Web.Models;
using A2.University.Web.Models.Business;
using A2.University.Web.Models.Entities;
using A2.University.Web.Models.StaffPortal;

namespace A2.University.Web.Controllers.StaffPortal
{

    /// <summary>
    /// Controller for Course
    /// </summary>
    public class CourseEnrolmentController : Controller
    {
        private readonly UniversityEntities _db = new UniversityEntities();
        private readonly CourseRules _courseRules = new CourseRules();

        /// <summary>
        /// GET: CourseEnrolment
        /// Displays CourseEnrolment/Index CRUD grid of all CourseEnrolments in database.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            CourseEnrolmentIndexViewModel courseEnrolmentViewModel = new CourseEnrolmentIndexViewModel();

            // get list of course enrolments in db
            var courseEnrolmentsEntity = _db.CourseEnrolments
                .OrderByDescending(c => c.course_enrolment_id)
                .Include(c => c.Course)
                .Include(c => c.Student)
                .ToList();
            
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

        /// <summary>
        /// GET: CourseEnrolment/Details/5
        /// Shows details of CourseEnrolment when "View" link clicked.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            CourseEnrolment courseEnrolmentEntityModel = _db.CourseEnrolments.Find(id);
            // create viewmodel, pass values from entitymodel
            CourseEnrolmentDetailsViewModel courseEnrolmentViewModel = new CourseEnrolmentDetailsViewModel();
            PopulateViewModel(courseEnrolmentViewModel, courseEnrolmentEntityModel);

            if (courseEnrolmentEntityModel == null)
            {
                return HttpNotFound();
            }
            return View(courseEnrolmentViewModel);
        }

        /// <summary>
        /// GET: CourseEnrolment/Create
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            // create viewmodel
            CourseEnrolmentCreateViewModel courseEnrolmentViewModel = new CourseEnrolmentCreateViewModel();
            PopulateDropDownLists(courseEnrolmentViewModel);

            return View(courseEnrolmentViewModel);
        }

        /// <summary>
        /// POST: CourseEnrolment/Create
        /// Stores new CourseEnrolment in database if passes validation, defined by CourseEnrolmentBaseViewModelValidator.
        /// Shows feedback to user when successfully creates new CourseEnrolment.
        /// </summary>
        /// <param name="courseEnrolmentViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseEnrolmentId,StudentId,CourseId,CourseStatus")] CourseEnrolmentCreateViewModel courseEnrolmentViewModel)
        {
            // if input passes validation
            if (ModelState.IsValid)
            {
                // ASSUMPTION: Student with existing EXCLUDED course is unable to re-enrol until approved
                // SEE: UnitEnrolmentController.PopulateEntityModel()
                // SEE: https://www.ecu.edu.au/__data/assets/pdf_file/0005/378320/Admission-Enrolment-and-Academic-Progress-Rules.pdf, rule 26.7, page 22.
                // OUT OF SCOPE: Implementation of automated PROBATION status,
                // CURRENT WORKAROUND: Manually delete Student's EXCLUDED course enrolment to allow future re-enrolment

                // IMPORTANT: If Student has existing EXCLUDED status, deny enrolment to future courses,
                // this check is implemented to ensure that UnitEnrolmentController.PopulateEntityModel() does not throw exception.
                if (_courseRules.IsAlreadyExcluded(courseEnrolmentViewModel.StudentId))
                {
                    // in lieu of automated PROBATION status (out of scope), show error message to user
                    ModelState.AddModelError("StudentId", "* Student is EXCLUDED awaiting re-enrolment approval");
                    // populate dropdownlists
                    PopulateDropDownLists(courseEnrolmentViewModel);
                    return View(courseEnrolmentViewModel);
                }

                // create entitymodel, pass values from viewmodel
                CourseEnrolment courseEnrolmentEntityModel = new CourseEnrolment();
                PopulateEntityModel(courseEnrolmentViewModel, courseEnrolmentEntityModel);

                // get course
                var course = _db.Courses
                    .FirstOrDefault(c => 
                        c.course_id == courseEnrolmentEntityModel.course_id);
                // get student
                var student = _db.Students
                    .FirstOrDefault(s => 
                        s.student_id == courseEnrolmentEntityModel.student_id);

                // provide feedback to user
                TempData["notice"] = $"Course Enrolment {courseEnrolmentEntityModel.course_id} {course?.title} for {student?.firstname} {student?.lastname} was successfully created";

                // discontinue previous enrolled courses
                DiscontinuePrevEnrolments(courseEnrolmentEntityModel.student_id);

                // update db using entitymodel
                _db.CourseEnrolments.Add(courseEnrolmentEntityModel);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            // populate dropdownlists
            PopulateDropDownLists(courseEnrolmentViewModel);

            return View(courseEnrolmentViewModel);
        }

        /// <summary>
        /// GET: CourseEnrolment/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            CourseEnrolment courseEnrolmentEntityModel = _db.CourseEnrolments.Find(id);
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

        /// <summary>
        /// POST: CourseEnrolment/Edit/5
        /// Stores edited data if viewmodel passes validation.
        /// Shows feedback to user when successfully edits data.
        /// </summary>
        /// <param name="courseEnrolmentEntityModel"></param>
        /// <param name="courseEnrolmentViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseEnrolmentId,StudentId,CourseId,CourseStatus")] CourseEnrolment courseEnrolmentEntityModel, CourseEnrolmentEditViewModel courseEnrolmentViewModel)
        {
            if (ModelState.IsValid)
            {
                // populate the entitymodel
                PopulateEntityModel(courseEnrolmentViewModel, courseEnrolmentEntityModel);

                // update db using entitymodel
                _db.Entry(courseEnrolmentEntityModel).State = EntityState.Modified;
                _db.SaveChanges();

                // get course
                var course = _db.Courses
                    .FirstOrDefault(c =>
                        c.course_id == courseEnrolmentEntityModel.course_id);
                // get student
                var student = _db.Students
                    .FirstOrDefault(s =>
                        s.student_id == courseEnrolmentEntityModel.student_id);

                // provide feedback to user
                TempData["notice"] = $"Course Enrolment {courseEnrolmentEntityModel.course_id} {course?.title} for {student?.firstname} {student?.lastname} was successfully created";

                return RedirectToAction("Index");
            }

            // populate dropdownlists
            PopulateDropDownLists(courseEnrolmentViewModel);

            return View(courseEnrolmentViewModel);
        }

        /// <summary>
        /// GET: CourseEnrolment/Delete/5
        /// Displays "Are you sure you want to delete" view.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            CourseEnrolment courseEnrolmentEntityModel = _db.CourseEnrolments.Find(id);
            // create viewmodel, pass values from entitymodel
            CourseEnrolmentDeleteViewModel courseEnrolmentViewModel = new CourseEnrolmentDeleteViewModel();
            PopulateViewModel(courseEnrolmentViewModel, courseEnrolmentEntityModel);

            // get number of affected rows
            int rows = GetNumberOfAffectedRows((long) id);
            if (rows > 0)
            {
                // tell user how many rows this deletion will affect
                TempData["delete-notice"] = $"WARNING: Deleting this record will also delete {rows} other record/s in the database!";
            }

            if (courseEnrolmentEntityModel == null)
            {
                return HttpNotFound();
            }
            return View(courseEnrolmentViewModel);
        }

        /// <summary>
        /// POST: CourseEnrolment/Delete/5
        /// Deletes row from database.
        /// Shows feedback to user when successfully deletes.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            CourseEnrolment courseEnrolment = _db.CourseEnrolments.Find(id);

            // provide feedback to user
            TempData["notice"] = $"Course Enrolment {courseEnrolment.course_id} {courseEnrolment.Course.title} for {courseEnrolment.Student.firstname} {courseEnrolment.Student.lastname} was successfully deleted";

            _db.CourseEnrolments.Remove(courseEnrolment);

            // re-enrol last course
            ReEnrolLastCourse(courseEnrolment.student_id, courseEnrolment.course_enrolment_id);

            // do own cascade on delete
            CascadeOnDelete(id);

            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Populates dropdownlists for unit enrolment view.
        /// </summary>
        /// <param name="viewModel"></param>
        private void PopulateDropDownLists(CourseEnrolmentDropDownListViewModel viewModel)
        {
            // get list of students/units from db
            var studentsEntity = _db.Students.ToList();
            var coursesEntity = _db.Courses.ToList();

            // transfer relevant elements to viewmodel list
            foreach (Student student in studentsEntity)
            {
                viewModel.Students.Add(new CourseEnrolmentDropDownListViewModel
                {
                    StudentId = student.student_id,
                    StudentIdFullName =
                        $"{student.student_id} " + 
                        $"{student.firstname} " +
                        $"{student.lastname}"
                });
            }

            foreach (Course course in coursesEntity)
            {
                viewModel.Courses.Add(new CourseEnrolmentDropDownListViewModel
                {
                    CourseId = course.course_id,
                    CourseIdTitle =
                        $"{course.course_id} " +
                        $"{course.title}"
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

            viewModel.StudentFullName = 
                $"{entityModel.Student.firstname} " +
                $"{entityModel.Student.lastname}";

            viewModel.Title = entityModel.Course.title;
        }

        /// <summary>
        /// Sets previously ENROLLED course to DISCONTIN.
        /// </summary>
        /// <param name="studentId">long</param>
        private void DiscontinuePrevEnrolments (long studentId)
        {
            // if enrolled course for student not unique,
            if (_courseRules.IsNotUniqueEnrolled(studentId))
            {
                // can't use dict in linq, substitute with string
                string state = _courseRules.CourseStates["Enrolled"];

                // get list of student's courses in ENROLLED status
                var enrolledCourses = _db.CourseEnrolments
                    .Where(ce =>
                        ce.student_id == studentId &&
                        ce.course_status == state)
                    .ToList();

                // set each course status to DISCONTIN
                foreach (var enrolled in enrolledCourses)
                {
                    enrolled.course_status = _courseRules.CourseStates["Discontinued"];
                    _db.Entry(enrolled).State = EntityState.Modified;
                    _db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Sets last DISCONTIN course to ENROLLED.
        /// Used when ActionResult Delete() called.
        /// </summary>
        /// <param name="studentId">long</param>
        /// <param name="courseEnrolmentId"></param>
        private void ReEnrolLastCourse(long studentId, long courseEnrolmentId)
        {
            // can't use dict in linq, substitute with string
            string state = _courseRules.CourseStates["Discontinued"];

            // get list of student's courses in DISCONTIN status
            var discontinCourses = _db.CourseEnrolments
                .Where(ce => 
                    ce.student_id == studentId &&
                    ce.course_status == state)
                .ToList();

            // change status to enrolled if list contains at least one item AND is not current course enrolment
            if (discontinCourses.Count > 0 && discontinCourses.All(dc => dc.course_enrolment_id != courseEnrolmentId))
            {
                // set last DISCONTIN (most recent) course to ENROLLED
                discontinCourses.Last().course_status = _courseRules.CourseStates["Enrolled"];
                _db.Entry(discontinCourses.Last()).State = EntityState.Modified;
                _db.SaveChanges();
            }
        }

        /// <summary>
        /// Returns number of affected rows.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private int GetNumberOfAffectedRows(long id)
        {
            return _db.UnitEnrolments.Count(ue => ue.course_enrolment_id == id);

        }

        /// <summary>
        /// Implemented own cascade on delete,
        /// database not performing it on its own.
        /// </summary>
        /// <param name="id"></param>
        private void CascadeOnDelete(long id)
        {
            var unitEnrolments = _db.UnitEnrolments
                .Where(ue => ue.course_enrolment_id == id);

            foreach (UnitEnrolment x in unitEnrolments)
            {
                _db.UnitEnrolments.Remove(x);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
