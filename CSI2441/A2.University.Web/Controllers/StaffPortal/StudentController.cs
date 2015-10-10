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
    public class StudentController : Controller
    {
        private readonly UniversityEntities _db = new UniversityEntities();
        private int _emailMatchTally;
        private string _email;
        private string _tempEmail;

        // GET: Student
        public ActionResult Index()
        {
            // create viewmodel
            StudentIndexViewModel studentViewModel = new StudentIndexViewModel();
            // generate list from entity
            var studentsEntity = _db.Students.ToList();

            // transfer entity list to viewmodel list
            foreach (Student student in studentsEntity)
            {
                studentViewModel.Students.Add(new StudentIndexViewModel
                {
                    StudentId = student.student_id,
                    FirstName = student.firstname,
                    LastName = student.lastname,
                    Dob = student.dob.ToString("dd/MM/yyyy"),
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

        // GET: Student/Details/5
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

        // GET: Student/Create
        public ActionResult Create()
        {
            // create new viewmodel
            return View(new StudentCreateViewModel());
        }

        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                return RedirectToAction("Index");
            }

            return View(studentViewModel);
        }

        // GET: Student/Edit/5
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

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                return RedirectToAction("Index");
            }
            return View(studentViewModel);
        }

        // GET: Student/Delete/5
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

            if (studentEntityModel == null)
            {
                return HttpNotFound();
            }

            // show view using viewmodel
            return View(studentViewModel);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Student student = _db.Students.Find(id);
            _db.Students.Remove(student);
            _db.SaveChanges();
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
            entityModel.dob = Convert.ToDateTime(viewModel.Dob, CultureInfo.CurrentCulture).Date;
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
            viewModel.Dob = entityModel.dob.ToString("dd/MM/yyyy");
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
        /// Function initiates recursive search of generated emails to ensure no duplicates exist.
        /// Begins by reseting tallies and current Email fields, generates first version of Email, then passes to EmailRecursiveSearch.
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
        /// Recursive function to search each version of generated Email against existing emails to ensure no duplicates exist.
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

                // recursive call to search new version of generated Email
                EmailRecursiveSearch(student, _tempEmail);
            }
            // exit case, generated Email does not exist! let's use it
            else
            {
                _email = target;
            }
        }

        /// <summary>
        /// Function searches generated Email against existing emails in database.
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
