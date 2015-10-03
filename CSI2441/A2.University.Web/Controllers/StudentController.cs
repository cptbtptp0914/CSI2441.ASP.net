using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using A2.University.Web.Business.Services;
using A2.University.Web.Models;
using A2.University.Web.Models.Entities;

namespace A2.University.Web.Controllers
{
    public class StudentController : Controller
    {
        private UniversityEntities db = new UniversityEntities();
        private int _emailMatchTally;
        private string _email;
        private string _tempEmail;

        // GET: Student
        public ActionResult Index()
        {
            StudentIndexViewModel studentViewModel = new StudentIndexViewModel();
            studentViewModel.Students = db.Students.ToList();

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
            Student studentEntityModel = db.Students.Find(id);
            // create viewmodel, pass values from entitymodel
            StudentDetailsViewModel studentViewModel = new StudentDetailsViewModel();
            SetStudentViewModel(studentViewModel, studentEntityModel);

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
        public ActionResult Create([Bind(Include = "student_id,firstname,lastname,dob,gender,email,ph_landline,ph_mobile,adrs,adrs_city,adrs_state,adrs_postcode")] StudentCreateViewModel studentViewModel)
        {
            // if input passed validation
            if (ModelState.IsValid)
            {
                // generate email
                StartEmailRecursiveSearch(studentViewModel);

                // create entitymodel, pass values from viewmodel
                Student studentEntityModel = new Student();
                SetStudentEntityModel(studentViewModel, studentEntityModel);

                // update db using entitymodel
                db.Students.Add(studentEntityModel);
                db.SaveChanges();
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
            Student studentEntityModel = db.Students.Find(id);
            // create viewmodel, pass values from entitymodel
            StudentEditViewModel studentViewModel = new StudentEditViewModel();
            SetStudentViewModel(studentViewModel, studentEntityModel);

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
        public ActionResult Edit([Bind(Include = "student_id,firstname,lastname,dob,gender,email,ph_landline,ph_mobile,adrs,adrs_city,adrs_state,adrs_postcode")] Student studentEntityModel)
        {
            StudentEditViewModel studentViewModel = new StudentEditViewModel();
            SetStudentViewModel(studentViewModel, studentEntityModel);

            if (ModelState.IsValid)
            {
                // generate new email
                StartEmailRecursiveSearch(studentViewModel);
                studentEntityModel.email = _email;

                // update db using entitymodel
                db.Entry(studentEntityModel).State = EntityState.Modified;
                db.SaveChanges();
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
            Student studentEntityModel = db.Students.Find(id);
            // create viewmodel, pass values from entitymodel
            StudentDeleteViewModel studentViewModel = new StudentDeleteViewModel();
            SetStudentViewModel(studentViewModel, studentEntityModel);

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
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        /// <summary>
        /// Passes data from the view model to the entity model.
        /// </summary>
        /// <param name="viewModel">StudentBaseViewModel</param>
        /// <param name="entityModel">Student</param>
        private void SetStudentEntityModel(StudentBaseViewModel viewModel, Student entityModel)
        {
            // no student_id, generated by db
            entityModel.firstname = viewModel.firstname;
            entityModel.lastname = viewModel.lastname;
            entityModel.dob = viewModel.dob;
            entityModel.gender = viewModel.gender;
            // use generated email
            entityModel.email = _email;
            entityModel.ph_landline = viewModel.ph_landline;
            entityModel.ph_mobile = viewModel.ph_mobile;
            entityModel.adrs = viewModel.adrs;
            entityModel.adrs_city = viewModel.adrs_city.ToUpper();
            entityModel.adrs_state = viewModel.adrs_state;
            // cast postcode string to int
            entityModel.adrs_postcode = int.Parse(viewModel.adrs_postcode);
        }


        /// <summary>
        /// Passes data from the entity model to the view model.
        /// </summary>
        /// <param name="viewModel">StudentBaseViewModel</param>
        /// <param name="entityModel">Student</param>
        private void SetStudentViewModel(StudentBaseViewModel viewModel, Student entityModel)
        {
            viewModel.student_id = entityModel.student_id;
            viewModel.firstname = entityModel.firstname;
            viewModel.lastname = entityModel.lastname;
            viewModel.dob = entityModel.dob;
            viewModel.gender = entityModel.gender;
            viewModel.email = entityModel.email;
            viewModel.ph_landline = entityModel.ph_landline;
            viewModel.ph_mobile = entityModel.ph_mobile;
            viewModel.adrs = entityModel.adrs;
            viewModel.adrs_city = entityModel.adrs_city.ToUpper();
            viewModel.adrs_state = entityModel.adrs_state;
            // cast postcode int to string
            viewModel.adrs_postcode = entityModel.adrs_postcode.ToString();
        }

        /// <summary>
        /// Function initiates recursive search of generated emails to ensure no duplicates exist.
        /// Begins by reseting tallies and current email fields, generates first version of email, then passes to EmailRecursiveSearch.
        /// </summary>
        /// <param name="student">Student</param>
        private void StartEmailRecursiveSearch([Bind(Include = "student_id,firstname,lastname,dob,gender,email,ph_landline,ph_mobile,adrs,adrs_city,adrs_state,adrs_postcode")] StudentBaseViewModel student)
        {
            // reset fields for each new student instance
            _emailMatchTally = 0;
            _email = "";

            // generate initial standard email
            string target = student.firstname[0] + student.lastname + EmailGenerator.StudentEmailSuffix;
            // use email as target for search
            EmailRecursiveSearch(student, target.ToLower());
        }


        /// <summary>
        /// Recursive function to search each version of generated email against existing emails to ensure no duplicates exist.
        /// </summary>
        /// <param name="student">Student</param>
        /// <param name="target">string</param>
        private void EmailRecursiveSearch([Bind(Include = "student_id,firstname,lastname,dob,gender,email,ph_landline,ph_mobile,adrs,adrs_city,adrs_state,adrs_postcode")] StudentBaseViewModel student, string target)
        {
            // if current email is a match,
            if (SearchEmail(target))
            {
                // increment tally
                _emailMatchTally++;
                // make new temp email based on tally
                _tempEmail = EmailGenerator.GenerateEmail(
                    "student",
                    _emailMatchTally,
                    student.firstname.ToLower(),
                    student.lastname.ToLower()
                    );
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
            return db.Students.Any(e => e.email == target);
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
