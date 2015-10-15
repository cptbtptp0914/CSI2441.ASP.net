using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using A2.University.Web.Models;
using A2.University.Web.Models.Entities;
using A2.University.Web.Models.StaffPortal;

namespace A2.University.Web.Controllers.StaffPortal
{

    /// <summary>
    /// Controller for Course
    /// </summary>
    public class CourseController : Controller
    {
        private readonly UniversityEntities _db = new UniversityEntities();

        /// <summary>
        /// GET: Course
        /// Displays Course/Index CRUD grid of all Courses in database.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            CourseIndexViewModel courseViewModel = new CourseIndexViewModel();

            var coursesEntity = _db.Courses
                .Include(c => 
                    c.CourseType)
                .Include(c => 
                    c.Staff)
                .ToList();

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
                    StaffFullName = 
                        $"{course.Staff.firstname} " +
                        $"{course.Staff.surname}",

                    CourseTypeTitle = course.CourseType.title,
                    CreditPoints = course.CourseType.credit_points,
                    Duration = course.CourseType.duration
                });
            }

            return View(courseViewModel.Courses);
        }

        /// <summary>
        /// GET: Course/Details/5
        /// Shows details of Course when "View" link clicked.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            Course courseEntityModel = _db.Courses.Find(id);
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


        /// <summary>
        /// GET: Course/Create
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            // crate viewmodel
            CourseCreateViewModel courseViewModel = new CourseCreateViewModel();
            // populate dropdownlists
            PopulateDropDownLists(courseViewModel);

            return View(courseViewModel);
        }

        /// <summary>
        /// POST: Course/Create
        /// Stores new Course in database if passes validation, defined by CourseBaseViewModelValidator.
        /// Shows feedback to user when successfully creates new Course.
        /// </summary>
        /// <param name="courseViewModel"></param>
        /// <returns></returns>
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
                _db.Courses.Add(courseEntityModel);
                _db.SaveChanges();

                // provide feedback to user
                TempData["notice"] = $"Course {courseEntityModel.course_id} {courseEntityModel.title} was successfully created";

                return RedirectToAction("Index");
            }

            // populate dropdownlists
            PopulateDropDownLists(courseViewModel);

            // render view using viewmodel
            return View(courseViewModel);
        }


        /// <summary>
        /// GET: Course/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            Course courseEntityModel = _db.Courses.Find(id);
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

        /// <summary>
        /// POST: Course/Edit/5
        /// Stores edited data if viewmodel passes validation.
        /// Shows feedback to user when successfully edits data.
        /// </summary>
        /// <param name="courseEntityModel"></param>
        /// <param name="courseViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseId,Title,CoordinatorId,CourseTypeId")] Course courseEntityModel, CourseEditViewModel courseViewModel)
        {
            if (ModelState.IsValid)
            {
                // populate entitymodel
                PopulateEntityModel(courseViewModel, courseEntityModel);

                // update db using entitymodel
                _db.Entry(courseEntityModel).State = EntityState.Modified;
                _db.SaveChanges();

                // provide feedback to user
                TempData["notice"] = $"Course {courseEntityModel.course_id} {courseEntityModel.title} was successfully edited";

                return RedirectToAction("Index");
            }

            // populate dropdownlists
            PopulateDropDownLists(courseViewModel);

            // render view using viewmodel
            return View(courseViewModel);
        }

        /// <summary>
        /// GET: Course/Delete/5
        /// Displays "Are you sure you want to delete" view.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            Course courseEntityModel = _db.Courses.Find(id);
            // create viewmodel, pass values from entitymodel
            CourseDeleteViewModel courseViewModel = new CourseDeleteViewModel();
            PopulateViewModel(courseViewModel, courseEntityModel);

            // get number of affected rows
            int rows = GetNumberOfAffectedRows(id);
            if (rows > 0)
            {
                // tell user how many rows this deletion will affect
                TempData["delete-notice"] = $"WARNING: Deleting this record will also delete {rows} other record/s in the database!";
            }

            if (courseEntityModel == null)
            {
                return HttpNotFound();
            }
            return View(courseViewModel);
        }

        /// <summary>
        /// POST: Course/Delete/5
        /// Deletes row from database.
        /// Shows feedback to user when successfully deletes.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Course course = _db.Courses.Find(id);

            // do own cascade on delete
            CascadeOnDelete(id);

            _db.Courses.Remove(course);
            _db.SaveChanges();

            // provide feedback to user
            TempData["notice"] = $"Course {course.course_id} {course.title} was successfully deleted";

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Populates dropdownlists for course view.
        /// </summary>
        /// <param name="viewModel">CourseDropDownListViewModel</param>
        private void PopulateDropDownLists(CourseDropDownListViewModel viewModel)
        {
            // get list of students/units from db
            var staffEntity = _db.Staff.ToList();
            var courseTypesEntity = _db.CourseTypes.ToList();

            // transfer relevant elements to viewmodel list
            foreach (Staff staff in staffEntity)
            {
                viewModel.Coordinators.Add(new CourseDropDownListViewModel
                {
                    CoordinatorId = staff.staff_id,
                    StaffIdFullName = 
                        $"{staff.staff_id} " +
                        $"{staff.firstname} " + 
                        $"{staff.surname}"
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

            viewModel.StaffFullName = 
                $"{entityModel.Staff.firstname} " + 
                $"{entityModel.Staff.surname}";

            viewModel.CourseTypeTitle = entityModel.CourseType.title;
            viewModel.CreditPoints = entityModel.CourseType.credit_points;
            viewModel.Duration = entityModel.CourseType.duration;
        }

        /// <summary>
        /// Returns number of affected rows.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private int GetNumberOfAffectedRows(string id)
        {
            return
                _db.CourseEnrolments.Count(su => su.course_id == id) +
                _db.UnitEnrolments.Count(ue => ue.CourseEnrolment.course_id == id);
        }

        /// <summary>
        /// Implemented own cascade on delete,
        /// database not performing it on its own.
        /// </summary>
        /// <param name="id"></param>
        private void CascadeOnDelete(string id)
        {
            var courseEnrolments = _db.CourseEnrolments
                .Where(ce => ce.course_id == id);
            var unitEnrolments = _db.UnitEnrolments
                .Where(ue => ue.CourseEnrolment.course_id == id);

            foreach (CourseEnrolment x in courseEnrolments)
            {
                _db.CourseEnrolments.Remove(x);
            }
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
