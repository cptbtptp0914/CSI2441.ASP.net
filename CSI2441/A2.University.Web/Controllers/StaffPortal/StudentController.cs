using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using A2.University.Web.Models;
using A2.University.Web.Models.Business.Services;
using A2.University.Web.Models.Entities;
using A2.University.Web.Models.StaffPortal;

namespace A2.University.Web.Controllers.StaffPortal
{

    /// <summary>
    /// Controller for Student
    /// </summary>
    public class StudentController : Controller
    {
        private readonly UniversityEntities _db = new UniversityEntities();
        private int _emailMatchTally;
        private string _email;
        private string _tempEmail;

        /// <summary>
        /// GET: Student
        /// Displays Student/Index CRUD grid of all students in database.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // create viewmodel
            StudentIndexViewModel studentViewModel = new StudentIndexViewModel();
            // generate list from entity
            var studentsEntity = _db.Students.OrderByDescending(s => s.student_id).ToList();

            // transfer entity list to viewmodel list
            foreach (Student student in studentsEntity)
            {
                studentViewModel.Students.Add(new StudentIndexViewModel
                {
                    StudentId = student.student_id,
                    FirstName = student.firstname,
                    LastName = student.lastname,
                    Dob = student.dob,
                    Gender = student.gender,
                    Email = student.email,
                    LandLine = student.ph_landline,
                    Mobile = student.ph_mobile,
                    Adrs = student.adrs,
                    AdrsCity = student.adrs_city,
                    AdrsState = student.adrs_state,
                    AdrsPostcode = student.adrs_postcode.ToString()
                });
            }

            // render view using viewmodel list
            return View(studentViewModel.Students);
        }

        /// <summary>
        /// GET: Student/Details/5
        /// Shows details of Student when "View" link clicked.
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
            Student studentEntityModel = _db.Students.Find(id);
            // create viewmodel, pass values from entitymodel
            StudentDetailsViewModel studentViewModel = new StudentDetailsViewModel();
            PopulateViewModel(studentViewModel, studentEntityModel);

            if (studentEntityModel == null)
            {
                return HttpNotFound();
            }

            // show view using viewmodel
            return View(studentViewModel);
        }

        /// <summary>
        /// GET: Student/Create
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            // create new viewmodel
            return View(new StudentCreateViewModel());
        }

        /// <summary>
        /// POST: Student/Create
        /// Stores new Student in database if passes validation, defined by StudentBaseViewModelValidator.
        /// Shows feedback to user when successfully creates new student.
        /// </summary>
        /// <param name="studentViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentId,FirstName,LastName,Dob,Gender,Email,LandLine,Mobile,Adrs,AdrsCity,AdrsState,AdrsPostcode")] StudentCreateViewModel studentViewModel)
        {
            // if input passed validation
            if (ModelState.IsValid)
            {
                // create entitymodel, pass values from viewmodel
                Student studentEntityModel = new Student();
                PopulateEntityModel(studentViewModel, studentEntityModel);

                // update db using entitymodel
                _db.Students.Add(studentEntityModel);
                _db.SaveChanges();

                // provide feedback to user
                TempData["notice"] = $"Student {studentEntityModel.student_id} {studentEntityModel.firstname} {studentEntityModel.lastname} was successfully created";

                return RedirectToAction("Index");
            }

            return View(studentViewModel);
        }

        /// <summary>
        /// GET: Student/Edit/5
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
            Student studentEntityModel = _db.Students.Find(id);
            // create viewmodel, pass values from entitymodel
            StudentEditViewModel studentViewModel = new StudentEditViewModel();
            PopulateViewModel(studentViewModel, studentEntityModel);

            if (studentEntityModel == null)
            {
                return HttpNotFound();
            }

            // show view using viewmodel
            return View(studentViewModel);
        }

        /// <summary>
        /// POST: Student/Edit/5
        /// Stores edited data if viewmodel passes validation.
        /// Shows feedback to user when successfully edits data.
        /// New email is also generated, and StudentUser is kept in sync, if Student has registered.
        /// TODO: Implement a way to stop new email generation if input name is same as name in db.
        /// </summary>
        /// <param name="studentEntityModel"></param>
        /// <param name="studentViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentId,FirstName,LastName,Dob,Gender,Email,LandLine,Mobile,Adrs,AdrsCity,AdrsState,AdrsPostcode")] Student studentEntityModel, StudentEditViewModel studentViewModel)
        {
            if (ModelState.IsValid)
            {
                // populate the entitymodel
                PopulateEntityModel(studentViewModel, studentEntityModel);

                // update db using entitymodel
                _db.Entry(studentEntityModel).State = EntityState.Modified;
                _db.SaveChanges();

                // keep StudentUser in sync, update with new email if StudentUser exists
                var studentUser = _db.StudentUsers.FirstOrDefault(su => su.student_id == studentEntityModel.student_id);
                if (studentUser != null)
                {
                    studentUser.email = studentEntityModel.email;
                    _db.Entry(studentUser).State = EntityState.Modified;
                    _db.SaveChanges();
                }

                // provide feedback to user
                TempData["notice"] = $"Student {studentEntityModel.student_id} {studentEntityModel.firstname} {studentEntityModel.lastname} was successfully edited";

                return RedirectToAction("Index");
            }
            return View(studentViewModel);
        }

        /// <summary>
        /// GET: Student/Delete/5
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

            // create entity model, match id
            Student studentEntityModel = _db.Students.Find(id);
            // create viewmodel, pass values from entitymodel
            StudentDeleteViewModel studentViewModel = new StudentDeleteViewModel();
            PopulateViewModel(studentViewModel, studentEntityModel);

            // get number of affected rows
            int rows = GetNumberOfAffectedRows((long) id);
            if (rows > 0)
            {
                // tell user how many rows this deletion will affect
                TempData["delete-notice"] = $"WARNING: Deleting this record will also delete {rows} other record/s in the database!";
            }

            if (studentEntityModel == null)
            {
                return HttpNotFound();
            }

            // show view using viewmodel
            return View(studentViewModel);
        }

        /// <summary>
        /// POST: Student/Delete/5
        /// Deletes row from database.
        /// Shows feedback to user when successfully deletes.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Student student = _db.Students.Find(id);

            // do own cascade on delete
            CascadeOnDelete(id);

            _db.Students.Remove(student);
            _db.SaveChanges();

            // provide feedback to user
            TempData["notice"] = $"Student {student.student_id} {student.firstname} {student.lastname} was successfully deleted";

            return RedirectToAction("Index");
        }


        /// <summary>
        /// Passes data from the view model to the entity model.
        /// </summary>
        /// <param name="viewModel">StudentBaseViewModel</param>
        /// <param name="entityModel">Student</param>
        private void PopulateEntityModel(StudentBaseViewModel viewModel, Student entityModel)
        {
            entityModel.student_id = viewModel.StudentId;
            entityModel.firstname = viewModel.FirstName;
            entityModel.lastname = viewModel.LastName;
            entityModel.dob = viewModel.Dob;
            entityModel.gender = viewModel.Gender;
            entityModel.ph_landline = viewModel.LandLine;
            entityModel.ph_mobile = viewModel.Mobile;
            entityModel.adrs = viewModel.Adrs;
            entityModel.adrs_city = viewModel.AdrsCity.ToUpper();
            entityModel.adrs_state = viewModel.AdrsState;
            // cast postcode string to int
            entityModel.adrs_postcode = int.Parse(viewModel.AdrsPostcode);

            // generate email
            StartEmailRecursiveSearch(viewModel);
            // could pass directly to entity, but may need in view
            viewModel.Email = _email;
            entityModel.email = viewModel.Email;
        }


        /// <summary>
        /// Passes data from the entity model to the view model.
        /// </summary>
        /// <param name="viewModel">StudentBaseViewModel</param>
        /// <param name="entityModel">Student</param>
        private void PopulateViewModel(StudentBaseViewModel viewModel, Student entityModel)
        {
            viewModel.StudentId = entityModel.student_id;
            viewModel.FirstName = entityModel.firstname;
            viewModel.LastName = entityModel.lastname;
            viewModel.Dob = entityModel.dob.Date;
            viewModel.Gender = entityModel.gender;
            viewModel.Email = entityModel.email;
            viewModel.LandLine = entityModel.ph_landline;
            viewModel.Mobile = entityModel.ph_mobile;
            viewModel.Adrs = entityModel.adrs;
            viewModel.AdrsCity = entityModel.adrs_city.ToUpper();
            viewModel.AdrsState = entityModel.adrs_state;
            // cast postcode int to string
            viewModel.AdrsPostcode = entityModel.adrs_postcode.ToString();
        }

        /// <summary>
        /// Returns number of affected rows.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private int GetNumberOfAffectedRows(long id)
        {
            return
                _db.StudentUsers.Count(su => su.student_id == id) +
                _db.UnitEnrolments.Count(ue => ue.student_id == id) +
                _db.CourseEnrolments.Count(ce => ce.student_id == id);

        }

        /// <summary>
        /// Implemented own cascade on delete,
        /// database not performing it on its own.
        /// </summary>
        /// <param name="id"></param>
        private void CascadeOnDelete(long id)
        {
            var users = _db.StudentUsers
                .Where(su => su.student_id == id);
            var unitEnrolments = _db.UnitEnrolments
                .Where(ue => ue.student_id == id);
            var courseEnrolments = _db.CourseEnrolments
                .Where(ce => ce.student_id == id);

            foreach (StudentUser x in users)
            {
                _db.StudentUsers.Remove(x);
            }
            foreach (UnitEnrolment x in unitEnrolments)
            {
                _db.UnitEnrolments.Remove(x);
            }
            foreach (CourseEnrolment x in courseEnrolments)
            {
                _db.CourseEnrolments.Remove(x);
            }
        }

        /// <summary>
        /// Function initiates recursive search of generated emails to ensure no duplicates exist.
        /// Begins by reseting tallies and current email fields, generates first version of Email, then passes to EmailRecursiveSearch.
        /// </summary>
        /// <param name="student">Student</param>
        private void StartEmailRecursiveSearch([Bind(Include = "StudentId,FirstName,LastName,Dob,Gender,Email,LandLine,Mobile,Adrs,AdrsCity,AdrsState,AdrsPostcode")] StudentBaseViewModel student)
        {
            // reset fields for each new student instance
            _emailMatchTally = 0;
            _email = "";

            // generate initial standard Email
            string target = student.FirstName[0] + student.LastName + EmailGenerator.StudentEmailSuffix;
            // use email as target for search
            EmailRecursiveSearch(student, target.ToLower());
        }


        /// <summary>
        /// Recursive function to search each version of generated email against existing emails to ensure no duplicates exist.
        /// </summary>
        /// <param name="student">Student</param>
        /// <param name="target">string</param>
        private void EmailRecursiveSearch([Bind(Include = "StudentId,FirstName,LastName,Dob,Gender,Email,LandLine,Mobile,Adrs,AdrsCity,AdrsState,AdrsPostcode")] StudentBaseViewModel student, string target)
        {
            // if current Email is a match,
            if (SearchEmail(target))
            {
                // increment tally
                _emailMatchTally++;
                // make new temp Email based on tally
                _tempEmail = EmailGenerator.GenerateEmail(
                    "student",
                    _emailMatchTally,
                    student.FirstName.ToLower(),
                    student.LastName.ToLower());

                // recursive call to search new version of generated email
                EmailRecursiveSearch(student, _tempEmail);
            }
            // exit case, generated email does not exist! let's use it
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
            return _db.Students.Any(e => e.email == target);
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
