using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using A2.University.Web.Business.Services;
using A2.University.Web.Models;
using A2.University.Web.Models.Entities;

namespace A2.University.Web.Controllers
{
    public class StaffController : Controller
    {
        private UniversityEntities db = new UniversityEntities();
        private int _emailMatchTally;
        private string _email;
        private string _tempEmail;

        // GET: Staff
        public ActionResult Index()
        {
            StaffIndexViewModel staffViewModel = new StaffIndexViewModel();
            staffViewModel.StaffList = db.Staff.ToList();

            return View(staffViewModel.StaffList);
        }

        // GET: Staff/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            Staff staffEntityModel = db.Staff.Find(id);
            // create viewmodel, pass values from entitymodel
            StaffDetailsViewModel staffViewModel = new StaffDetailsViewModel();
            SetStaffViewModel(staffViewModel, staffEntityModel);

            if (staffEntityModel == null)
            {
                return HttpNotFound();
            }
            
            // render view using viewmodel
            return View(staffViewModel);
        }

        // GET: Staff/Create
        public ActionResult Create()
        {
            // render view using viewmodel
            return View(new StaffCreateViewModel());
        }

        // POST: Staff/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "staff_id,firstname,surname,email")] StaffCreateViewModel staffViewModel)
        {
            // if input passes validation
            if (ModelState.IsValid)
            {
                // generate email
                StartEmailRecursiveSearch(staffViewModel);
                
                // create entitymodel, pass values from viewmodel
                Staff staffEntityModel = new Staff();
                SetStaffEntityModel(staffViewModel, staffEntityModel);

                // update db using entitymodel
                db.Staff.Add(staffEntityModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(staffViewModel);
        }

        // GET: Staff/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            Staff staffEntityModel = db.Staff.Find(id);
            // create viewmodel, pass values from entity model
            StaffEditViewModel staffViewModel = new StaffEditViewModel();
            SetStaffViewModel(staffViewModel, staffEntityModel);

            if (staffEntityModel == null)
            {
                return HttpNotFound();
            }

            // render view using viewmodel
            return View(staffViewModel);
        }

        // POST: Staff/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "staff_id,firstname,surname,email")] Staff staffEntityModel)
        {
            StaffEditViewModel staffViewModel = new StaffEditViewModel();
            SetStaffViewModel(staffViewModel, staffEntityModel);

            if (ModelState.IsValid)
            {
                // generate new email
                StartEmailRecursiveSearch(staffViewModel);
                staffEntityModel.email = _email;

                // update db using entitymodel
                db.Entry(staffEntityModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staffViewModel);
        }

        // GET: Staff/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entity model, match id
            Staff staffEntityModel = db.Staff.Find(id);
            // create viewmodel, pass values from entitymodel
            StaffDeleteViewModel staffViewModel = new StaffDeleteViewModel();
            SetStaffViewModel(staffViewModel, staffEntityModel);

            if (staffEntityModel == null)
            {
                return HttpNotFound();
            }

            // render view using viewmodel
            return View(staffViewModel);
        }

        // POST: Staff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Staff staff = db.Staff.Find(id);
            db.Staff.Remove(staff);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Passes data from the view model to the entity model.
        /// </summary>
        /// <param name="viewModel">StaffBaseViewModel</param>
        /// <param name="entityModel">Staff</param>
        private void SetStaffEntityModel(StaffBaseViewModel viewModel, Staff entityModel)
        {
            // no staff_id, generated by db
            entityModel.firstname = viewModel.firstname;
            entityModel.surname = viewModel.surname;
            // use generated email
            entityModel.email = _email;
        }


        /// <summary>
        /// Passes data from the entity model to the view model.
        /// </summary>
        /// <param name="viewModel">StaffBaseViewModel</param>
        /// <param name="entityModel">Staff</param>
        private void SetStaffViewModel(StaffBaseViewModel viewModel, Staff entityModel)
        {
            viewModel.staff_id = entityModel.staff_id;
            viewModel.firstname = entityModel.firstname;
            viewModel.surname = entityModel.surname;
            viewModel.email = entityModel.email;
        }

        /// <summary>
        /// Function initiates recursive search of generated emails to ensure no duplicates exist.
        /// Begins by reseting tallies and current email fields, generates first version of email, then passes to EmailRecursiveSearch.
        /// TODO: Try to implement this in GenerateEmail class, code duplication between Student/UnitControllers
        /// </summary>
        /// <param name="staff">Staff</param>
        private void StartEmailRecursiveSearch([Bind(Include = "staff_id,firstname,surname,email")] StaffBaseViewModel staff)
        {
            // reset fields for each new student instance
            _emailMatchTally = 0;
            _email = "";

            // generate initial standard email
            string target = staff.firstname[0] + "." + staff.surname + EmailGenerator.StaffEmailSuffix;
            // us email as target for search
            EmailRecursiveSearch(staff, target.ToLower());
        }
        
        /// <summary>
        /// Recursive function to search each version of generated email against existing emails to ensure no duplicates exist.
        /// </summary>
        /// <param name="staff">Staff</param>
        /// <param name="target">string</param>
        private void EmailRecursiveSearch([Bind(Include = "staff_id,firstname,surname,email")] StaffBaseViewModel staff,
            string target)
        {
            if (SearchEmail(target))
            {
                // increment tally
                _emailMatchTally++;
                // make new temp email based on tally
                _tempEmail = EmailGenerator.GenerateEmail(
                    "staff",
                    _emailMatchTally,
                    staff.firstname.ToLower(),
                    staff.surname.ToLower()
                    );
                // recursive call to search new version of generated email
                EmailRecursiveSearch(staff, _tempEmail);
            }
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
            return db.Staff.Any(e => e.email == target);
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
